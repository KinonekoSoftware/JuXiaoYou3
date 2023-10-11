using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Threading;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Models;
using GongSolutions.Wpf.DragDrop;
using NAudio.Wave;

namespace Acorisoft.FutureGL.MigaStudio.Services
{

    
    public class ObservableProperty<T> : ObservableObject, IObservableProperty<T>
    {
        private readonly BehaviorSubject<T> _stream;
        private          T                  _value;

        public ObservableProperty()
        {
            _value  = default(T);
            _stream = new BehaviorSubject<T>(_value);
        }

        public ObservableProperty(T defaultValue)
        {
            _value  = defaultValue;
            _stream = new BehaviorSubject<T>(defaultValue);
        }

        public void SetValue(T value)
        {
            _value = value;
            _stream.OnNext(value);
            ValueChanged?.Invoke(value);
        }

        protected override void ReleaseManagedResources()
        {
            // 释放
            (_value as IDisposable)?.Dispose();
        }

        /// <summary>
        /// 当前值
        /// </summary>
        public T CurrentValue => _value;
        
        /// <summary>
        /// 可观测对象
        /// </summary>
        public IObservable<T> Observable => _stream;
        
        /// <summary>
        /// 值改变事件。
        /// </summary>
        public event Action<T> ValueChanged;
    }
    
    public class MusicService : Disposable, IDropTarget
    {
        private readonly WaveOutEvent _device;
        private readonly Timer        _timer;
        private readonly BehaviorSubject<Tuple<TimeSpan, PlaybackState, Music, int>> _handler;

        private AudioFileReader                             _reader;
        private bool                                        _manualStop;
        private int                                         _currentIndex;
        private TimeSpan                                    _currentTime;
        private PlaybackState                               _currentState;
        private Music                                       _item;
        private int                                         _currentIndex2;

        private readonly ObservableProperty<TimeSpan> _positionStream;
        private readonly ObservableProperty<TimeSpan> _durationStream;
        private readonly ObservableProperty<Music>    _targetStream;
        private readonly ObservableProperty<Playlist> _playlistStream;
        private readonly ObservableProperty<bool>     _stateStream;

        public MusicService()
        {
            _handler = new BehaviorSubject<Tuple<TimeSpan, PlaybackState, Music, int>>(
                new Tuple<TimeSpan, PlaybackState, Music, int>(TimeSpan.Zero, PlaybackState.Stopped, null, -1));
            _durationStream         =  new ObservableProperty<TimeSpan>();
            _positionStream         =  new ObservableProperty<TimeSpan>(TimeSpan.Zero);
            _targetStream           =  new ObservableProperty<Music>(null);
            _playlistStream         =  new ObservableProperty<Playlist>(null);
            _stateStream            =  new ObservableProperty<bool>();
            _timer                  =  new Timer(DurationPushHandler, null, 0, 1000);
            _device                 =  new WaveOutEvent();
            _device.PlaybackStopped += OnDevicePlayStopped;
            Mode                    =  PlayMode.Sequence;
        }

        void Update(TimeSpan time, PlaybackState state, Music item, int index)
        {
            _currentTime   = time;
            _currentState  = state;
            _item          = item;
            _currentIndex2 = index;
            _handler.OnNext(new Tuple<TimeSpan, PlaybackState, Music, int>(_currentTime, _currentState, _item, _currentIndex2));
            _stateStream.SetValue(state == PlaybackState.Playing);
        }

        private void DurationPushHandler(object state)
        {
            if (_device.PlaybackState == PlaybackState.Playing)
            {
                _positionStream.SetValue(_reader.CurrentTime);
            }
        }

        private void OnDevicePlayStopped(object sender, StoppedEventArgs e)
        {
            //
            // 检测手动停止
            if (_manualStop)
            {
                _manualStop = false;
                return;
            }

            _positionStream.SetValue(_reader.TotalTime);
            Update(TimeSpan.Zero, PlaybackState.Stopped, null, -1);
            _reader.Dispose();
            _reader = null;

            if(Mode == PlayMode.Repeat)
            {
                Play(_playlistStream.CurrentValue.Items[_currentIndex]);
            }
            else
            {
                //
                // 播放下一个
                PlayNext();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayLast()
        {
            var maxIndex = _playlistStream.CurrentValue.Items.Count - 1;
            Music music;

            if (Mode == PlayMode.Sequence &&
                _currentIndex == maxIndex)
            {
                return;
            }

            if (Mode == PlayMode.Shuffle)
            {
                _currentIndex = Random.Shared.Next(0, maxIndex);
                music         = _playlistStream.CurrentValue.Items[_currentIndex];
                _targetStream.SetValue(music);
            }
            else
            {
                _currentIndex = --_currentIndex % maxIndex;
                music         = _playlistStream.CurrentValue.Items[_currentIndex];
                _targetStream.SetValue(music);
            }

            //
            //
            Play(music);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayNext()
        {
            var oldIndex = _currentIndex;
            var maxIndex = _playlistStream.CurrentValue.Items.Count;
            Music music;

            if (Mode == PlayMode.Sequence &&
                _currentIndex - 1 == maxIndex)
            {
                return;
            }

            if (Mode == PlayMode.Shuffle)
            {
                while(oldIndex == _currentIndex)
                {
                    _currentIndex = Random.Shared.Next(0, maxIndex - 1);
                }
                music         = _playlistStream.CurrentValue.Items[_currentIndex];
                _targetStream.SetValue(music);
            }
            else
            {
                _currentIndex = ++_currentIndex % maxIndex;
                music = _playlistStream.CurrentValue.Items[_currentIndex];
                _targetStream.SetValue(music);
            }

            //
            //
            Play(music);
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            if (_device.PlaybackState == PlaybackState.Paused)
            {
                _device.Play();
            }
        }

        public void PlayOrPause()
        {
            if (_device.PlaybackState == PlaybackState.Playing)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="music"></param>
        public void Play(Music music)
        {
            if (music is null)
            {
                return;
            }


            //
            // 没有播放列表就创建
            if ( _currentIndex == -1 || _playlistStream.CurrentValue is null)
            {
                SetPlaylist(new Playlist
                {
                    Items = new ObservableCollection<Music>(new[] { music })
                }, true);
            }
            else
            {
                _currentIndex = Playlist.CurrentValue.Items.IndexOf(music);

                if (_currentIndex == -1)
                {
                    SetPlaylist(new Playlist
                    {
                        Items = new ObservableCollection<Music>(new[] { music })
                    }, true);
                    return;
                }
                
                //
                // 停止播放
                Stop();

                _reader = new AudioFileReader(music.Path);
                _device.Init(_reader);
                _device.Play();
                _durationStream.SetValue(_reader.TotalTime);
                _targetStream.SetValue(music);
                Update(_reader.TotalTime, PlaybackState.Playing, music, _currentIndex);
            }
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        public void Pause()
        {
            if (_device.PlaybackState == PlaybackState.Playing)
            {
                _device.Pause();
                var music = _playlistStream.CurrentValue.Items[_currentIndex];
                Update(_reader.TotalTime, PlaybackState.Paused, music, _currentIndex);
            }
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        public void Stop()
        {
            if (_device.PlaybackState != PlaybackState.Stopped)
            {
                _manualStop = true;
                _device.Stop();
                _reader.Dispose();
                _reader = null;
            }
        }

        /// <summary>
        /// 设置播放列表
        /// </summary>
        /// <param name="playlist">播放列表</param>
        /// <param name="autoPlay">是否自动播放</param>
        public void SetPlaylist(Playlist playlist, bool autoPlay = false)
        {
            if (playlist is null)
            {
                return;
            }

            _playlistStream.SetValue(playlist);

            if (autoPlay)
            {
                var maxIndex = playlist.Items.Count - 1;
                Music music;

                if (Mode == PlayMode.Shuffle)
                {
                    var index = Random.Shared.Next(0, maxIndex);
                    music = _playlistStream.CurrentValue.Items[index];

                    _currentIndex = index;
                    _targetStream.SetValue(music);
                }
                else
                {
                    music = _playlistStream.CurrentValue.Items[0];

                    _currentIndex = 0;
                    _targetStream.SetValue(music);
                }

                //
                //
                Play(music);
            }
        }

        /// <summary>
        /// 设置播放位置
        /// </summary>
        /// <param name="time"></param>
        public void SetPosition(TimeSpan time)
        {
            if (_reader is null)
            {
                return;
            }

            _reader.CurrentTime = time;

            if (_device.PlaybackState == PlaybackState.Paused)
            {
                _device.Play();
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is Music &&
                dropInfo.TargetItem is Music) {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects           = DragDropEffects.Move;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is Music source&&
                dropInfo.TargetItem is Music target)
            {
                var items = _playlistStream.CurrentValue.Items;
                var sourceIndex = items.IndexOf(source);
                var targetIndex = items.IndexOf(target);
                var current = _targetStream.CurrentValue;
                items.Move(sourceIndex, targetIndex);

                if (target is not null)
                {
                    if (current.Id == source.Id)
                    {
                        _currentIndex = targetIndex;
                        Update(_reader.TotalTime, PlaybackState.Playing, null, _currentIndex);
                    }
                    else if(current.Id == target.Id)
                    {
                        _currentIndex = sourceIndex;
                        Update(_reader.TotalTime, PlaybackState.Playing, null, _currentIndex);
                    }
                }
                
            }
        }

        protected override void ReleaseManagedResources()
        {
            Stop();
            _stateStream.Dispose();
            _timer.Dispose();
            _playlistStream.Dispose();
            _positionStream.Dispose();
            _targetStream.Dispose();
            _device.Dispose();
            _reader?.Dispose();
        }


        public IObservable<TimeSpan> Duration => _durationStream.Observable;
        public IObservable<TimeSpan> Position => _positionStream.Observable;
        public float Volume
        {
            get => _device.Volume;
            set => _device.Volume = value;
        }

        public IObservableProperty<Music> Music => _targetStream;
        public IObservableProperty<Playlist> Playlist => _playlistStream;
        public IObservableProperty<bool> IsPlaying => _stateStream;

        public IObservable<Tuple<TimeSpan, PlaybackState, Music, int>> State => _handler;

        public PlayMode Mode { get; set; }
    }
}