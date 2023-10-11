using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Tools.MusicPlayer.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using GongSolutions.Wpf.DragDrop;
using NAudio.Wave;
using TagLib;
using TagLib.Mpeg;
using File = TagLib.File;
using System.Diagnostics;
using static System.Windows.Forms.AxHost;

namespace Acorisoft.FutureGL.Tools.MusicPlayer.ViewModels
{
    public sealed class MusicPlayerViewModel : ForestObject, IDropTarget
    {
        private readonly MusicService    _service;
        private readonly HashSet<string> _hash;
        private readonly IScheduler      _scheduler;

        private bool        _isMute;
        private double      _volume;
        private double      _lastVolume;
        private ImageSource _cover;
        private ImageSource _background;
        private bool        _isPlaying;
        private int         _index;
        private Playlist    _playlist;
        private Music       _current;
        private PlayMode    _mode;
        private TimeSpan    _position;
        private TimeSpan    _duration;


        public MusicPlayerViewModel()
        {
            _hash      = new HashSet<string>();
            _scheduler = new SynchronizationContextScheduler(SynchronizationContext.Current!);
            _service   = new MusicService();
            _service.StateUpdatedHandler = HandleStateChanged;
            _service.Position
                .ObserveOn(_scheduler)
                .Subscribe(x =>
                {
                    Position  = x;
                });
            
            _service.Duration
                .ObserveOn(_scheduler)
                .Subscribe(x =>
                {
                    Duration = x;
                });
            _service.Playlist.Observable
                .ObserveOn(_scheduler)
                .Where(x => x != null)
                .Subscribe(x =>
            {
                _hash.Clear();
                foreach (var item in x.Items)
                {
                    _hash.Add(item.Id);
                }
            });

            Background = new BitmapImage(new Uri("E:\\1.jpg"));
            Cover      = Background;
            Playlist = new Playlist
            {
                Name  = "新建播放列表",
                Items = new ObservableCollection<Music>()
            };
            Volume = 0.5d;

            AddMusicToPlaylistCommand      = new RelayCommand(AddMusicToPlaylistImpl, HasPlaylist);
            RemoveMusicFromPlaylistCommand = new RelayCommand<Music>(RemoveMusicFromPlaylistImpl, HasMusicItem);
            PlayMusicCommand               = new RelayCommand<Music>(PlayMusicImpl, HasMusicItem);
            PlayPreviousCommand            = new RelayCommand(() => _service.PlayLast(), WasFirstItem);
            PlayNextCommand                = new RelayCommand(() => _service.PlayNext(), WasLastItem);
            PlayOrPauseCommand             = new RelayCommand(PlayOrPauseImpl, HasPlaylist);
            MuteOrUnMuteCommand            = new RelayCommand(MuteOrUnmuteImpl);
            ChangePlayModeCommand          = new RelayCommand(ChangePlayModeImpl);
        }

        private void HandleStateChanged(TimeSpan duration, PlaybackState state, Music item, int index)
        {
            if (state == PlaybackState.Playing && item is null)
            {
                _index = index;
                PlayNextCommand?.NotifyCanExecuteChanged();
                PlayPreviousCommand?.NotifyCanExecuteChanged();
                return;
            }
            
            _index = index;
            Duration  = duration;
            IsPlaying = state == PlaybackState.Playing;
            Position  = TimeSpan.Zero;
            Current = item;
            PlayNextCommand?.NotifyCanExecuteChanged();
            PlayPreviousCommand?.NotifyCanExecuteChanged();
        }

        private bool HasPlaylist() => Playlist is not null;
        
        private bool HasMusicItem(Music item) => item is not null && Playlist is not null;
        private bool WasLastItem() => Playlist is not null && _index < Playlist.Items.Count - 1;
        private bool WasFirstItem() => Playlist is not null && _index > 0;

        private void ChangePlayModeImpl()
        {
            Mode = Mode switch
            {
                PlayMode.Loop => PlayMode.Repeat,
                PlayMode.Repeat => PlayMode.Shuffle,
                PlayMode.Shuffle => PlayMode.Sequence,
                _=> PlayMode.Sequence
            };
        }

        private void MuteOrUnmuteImpl()
        {
            if (Volume == 0)
            {
                Volume = _lastVolume;
            }
            else
            {
                {
                    _lastVolume = Volume;
                    Volume      = 0d;
                }
            }
        }
        
        private void PlayOrPauseImpl()
        {
            if (IsPlaying)
            {
                _service.Pause();
                IsPlaying = false;
            }
            else
            {
                _service.Play();
                IsPlaying = true;
            }
        }

        private void AddMusicToPlaylistImpl()
        {
            var opendlg = new OpenFileDialog
            {
                Filter = "音乐文件|*.wav;*.mp3",
                Multiselect = true
            };

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            foreach (var fileName in opendlg.FileNames)
            {
                if (!_hash.Add(fileName))
                {
                    // TODO: duplicated
                    return;
                }

                var file = File.Create(fileName);
                var musicFile = (AudioFile)file;
                var tag = musicFile.GetTag(TagTypes.Id3v2);
                var cover = string.Empty;

                if (tag.Pictures is not null)
                {
                    var pic = tag.Pictures.First();
                    cover = Path.Combine(Path.GetDirectoryName(fileName)!, Path.GetFileNameWithoutExtension(fileName) + ".png");

                    if (!System.IO.File.Exists(cover))
                    {
                        System.IO.File.WriteAllBytes(cover, pic.Data.Data);
                    }
                }

                var music = new Music
                {
                    Id     = fileName,
                    Path   = fileName,
                    Name   = tag.Title,
                    Author = tag.FirstPerformer,
                    Cover  = cover
                };

                //
                //
                Playlist.Items.Add(music);
            }
        }

        private void RemoveMusicFromPlaylistImpl(Music item)
        {
            if (item is null)
            {
                return;
            }

            var index = _playlist.Items.IndexOf(item);
            
            if (index == -1)
            {
                return;
            }

            var removedItem = _playlist.Items[index];
            _playlist.Items.RemoveAt(index);
            _hash.Remove(item.Id);

            if(_current is null)
            {
                return;
            }

            if (_playlist.Items.Count == 0)
            {
                _service.Stop();
            }
            else if (_current.Id == removedItem.Id)
            {
                _service.Stop();
                _service.Play(_playlist.Items[index % _playlist.Items.Count]);
            }
        }

        private void PlayMusicImpl(Music item)
        {
            if (_service.Playlist.CurrentValue is null)
            {
                _service.SetPlaylist(Playlist);
            }

            if (_service.Music.CurrentValue is not null &&
                _service.Music.CurrentValue.Id == item.Id &&
                IsPlaying)
            {
                _service.Pause();
            }
            else
            {
                Play(item);
            }
        }

        public void SetPosition(TimeSpan time) => _service.SetPosition(time);
        
        void Play(Music item)
        {
            IsPlaying = true;

            //
            _service.Play(item);

            //
            //
            if (!string.IsNullOrEmpty(item.Cover))
            {
                Cover      = new BitmapImage(new Uri(item.Cover));
                Background = Cover;
            }
        }
        
        
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            _service.DragOver(dropInfo);
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            _service.Drop(dropInfo);
        }

        /// <summary>
        /// 获取或设置 <see cref="Current"/> 属性。
        /// </summary>
        public Music Current
        {
            get => _current;
            set
            {
                SetValue(ref _current, value);
                Debug.WriteLine(value?.Path);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Playlist"/> 属性。
        /// </summary>
        public Playlist Playlist
        {
            get => _playlist;
            set
            {
                SetValue(ref _playlist, value);
                _service.SetPlaylist(value);
            } 
        }

        /// <summary>
        /// 获取或设置 <see cref="Cover"/> 属性。
        /// </summary>
        public ImageSource Cover
        {
            get => _cover;
            set => SetValue(ref _cover, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Volume"/> 属性。
        /// </summary>
        public double Volume
        {
            get => _volume;
            set
            {
                SetValue(ref _volume, value);
                IsMute          = value == 0;
                _service.Volume = (float)value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="IsMute"/> 属性。
        /// </summary>
        public bool IsMute
        {
            get => _isMute;
            set => SetValue(ref _isMute, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Background"/> 属性。
        /// </summary>
        public ImageSource Background
        {
            get => _background;
            set => SetValue(ref _background, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsPlaying"/> 属性。
        /// </summary>
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                SetValue(ref _isPlaying, value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Mode"/> 属性。
        /// </summary>
        public PlayMode Mode
        {
            get => _mode;
            set
            {
                SetValue(ref _mode, value);
                _service.Mode = value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Duration"/> 属性。
        /// </summary>
        public TimeSpan Duration
        {
            get => _duration;
            set => SetValue(ref _duration, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Position"/> 属性。
        /// </summary>
        public TimeSpan Position
        {
            get => _position;
            set => SetValue(ref _position, value);
        }

        public RelayCommand PlayPreviousCommand { get; }
        public RelayCommand PlayNextCommand { get; }
        public RelayCommand MuteOrUnMuteCommand { get; }
        public RelayCommand AddMusicToPlaylistCommand { get; }
        public RelayCommand ChangePlayModeCommand { get; }
        public RelayCommand PlayOrPauseCommand { get; }
        public RelayCommand<Music> PlayMusicCommand { get; }
        public RelayCommand<Music> RemoveMusicFromPlaylistCommand { get; }
    }
}