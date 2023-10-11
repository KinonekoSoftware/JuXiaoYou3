using CommunityToolkit.Mvvm.Input;
using System.Reactive.Linq;
using GongSolutions.Wpf.DragDrop;
using NAudio.Wave;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Services;
using Acorisoft.FutureGL.MigaStudio.Utilities;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Services
{
    
    public class MusicViewModel : TabViewModel, IDropTarget
    {
         private readonly MusicService _service;

        private bool     _isMute;
        private double   _volume;
        private double   _lastVolume;
        private string   _background;
        private bool     _isPlaying;
        private int      _index;
        private Music    _current;
        private PlayMode _mode;
        private TimeSpan _position;
        private TimeSpan _duration;
        private bool     _isVolumePaneOpen;
        private bool     _isPlaylistPaneOpen;

        private Playlist _playlist;

        public MusicViewModel()
        {
            _service = Xaml.Get<MusicService>();
            MusicEngine = Studio.DatabaseManager()
                              .GetEngine<MusicEngine>();
            _service.State
                    .ObserveOn(Scheduler)
                    .Subscribe(x => { HandleStateChanged(x.Item1, x.Item2, x.Item3, x.Item4); })
                    .DisposeWith(Collector);
            _service.Position
                    .ObserveOn(Scheduler)
                    .Subscribe(x => { Position = x; })
                    .DisposeWith(Collector);

            _service.Duration
                    .ObserveOn(Scheduler)
                    .Subscribe(x => { Duration = x; })
                    .DisposeWith(Collector);

            Playlist = _service.Playlist
                               .CurrentValue ?? new Playlist
            {
                Name  = "新建播放列表",
                Items = new ObservableCollection<Music>()
            };

            Background = null;
            Volume     = 0.5d;

            AddMusicToPlaylistCommand      = AsyncCommand(AddMusicToPlaylistImpl, HasPlaylist);
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

            _index     = index;
            Duration   = duration;
            IsPlaying  = state == PlaybackState.Playing;
            Position   = TimeSpan.Zero;
            Current    = item;
            Background = item?.Cover;
            PlayNextCommand?.NotifyCanExecuteChanged();
            PlayPreviousCommand?.NotifyCanExecuteChanged();
        }

        private bool HasPlaylist() => Playlist is not null;

        private bool HasMusicItem(Music item) => item is not null && Playlist is not null;
        private bool WasLastItem() => Playlist is not null && (Mode == PlayMode.Shuffle || _index < Playlist.Items.Count - 1);
        private bool WasFirstItem() => Playlist is not null && (Mode == PlayMode.Shuffle || _index > 0);

        private void ChangePlayModeImpl()
        {
            Mode = Mode switch
            {
                PlayMode.Loop    => PlayMode.Repeat,
                PlayMode.Repeat  => PlayMode.Shuffle,
                PlayMode.Shuffle => PlayMode.Sequence,
                _                => PlayMode.Loop
            };

            PlayNextCommand?.NotifyCanExecuteChanged();
            PlayPreviousCommand?.NotifyCanExecuteChanged();
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
                if (_current is null &&
                    _playlist is not null)
                {
                    Play(Playlist.Items
                                 .First());
                }
                _service.Play();
                IsPlaying = true;
            }
        }

        private async Task AddMusicToPlaylistImpl()
        {
            var opendlg = Studio.Open(SubSystemString.MusicFilter, true);
            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            await MusicUtilities.AddMusic(opendlg.FileNames, MusicEngine, x => Playlist.Items.Add(x));
            PlayMusicCommand.NotifyCanExecuteChanged();
            PlayNextCommand.NotifyCanExecuteChanged();
            PlayPreviousCommand.NotifyCanExecuteChanged();
            PlayOrPauseCommand.NotifyCanExecuteChanged();
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

            if (_current is null)
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
            var targetPlayList = _service.Playlist.CurrentValue;

            if (targetPlayList is null ||
                !ReferenceEquals(_service.Playlist.CurrentValue, Playlist))
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
            if (item is null ||
                !File.Exists(item.Path))
            {
                this.Obsoleted(string.Format(Language.GetText("text.FileNotFound"), item?.Name));
                RemoveMusicFromPlaylistImpl(item);
                return;
            }
            
            IsPlaying = true;

            //
            _service.Play(item);

            //
            //
            if (!string.IsNullOrEmpty(item.Cover))
            {
                Background = item.Cover;
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
        /// 获取或设置 <see cref="IsPlaylistPaneOpen"/> 属性。
        /// </summary>
        public bool IsPlaylistPaneOpen
        {
            get => _isPlaylistPaneOpen;
            set => SetValue(ref _isPlaylistPaneOpen, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsVolumePaneOpen"/> 属性。
        /// </summary>
        public bool IsVolumePaneOpen
        {
            get => _isVolumePaneOpen;
            set => SetValue(ref _isVolumePaneOpen, value);
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
        public string Background
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
            set { SetValue(ref _isPlaying, value); }
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

        public MusicEngine MusicEngine { get; }

        public RelayCommand PlayPreviousCommand { get; }
        public RelayCommand PlayNextCommand { get; }
        public RelayCommand MuteOrUnMuteCommand { get; }
        public AsyncRelayCommand AddMusicToPlaylistCommand { get; }
        public RelayCommand ChangePlayModeCommand { get; }
        public RelayCommand PlayOrPauseCommand { get; }
        public RelayCommand<Music> PlayMusicCommand { get; }
        public RelayCommand<Music> RemoveMusicFromPlaylistCommand { get; }
    }
}