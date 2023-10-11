using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Acorisoft.FutureGL.MigaStudio.Resources.Converters;
using Slider = Acorisoft.FutureGL.Forest.Controls.Slider;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Acorisoft.FutureGL.MigaStudio.Pages.Services
{

    public partial class MusicPlayerView
    {
        public MusicPlayerView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            ViewModel.Volume = 0.5d;
        }

        private void Button_OpenVolume(object sender, RoutedEventArgs e)
        {
            ViewModel.IsVolumePaneOpen = true;
        }

        private void Button_ClosePlaylist(object sender, RoutedEventArgs e)
        {
            ViewModel.IsPlaylistPaneOpen = false;
        }


        private void Button_OpenPlaylist(object sender, RoutedEventArgs e)
        {
            ViewModel.IsPlaylistPaneOpen = true;
        }


        private void OnVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ViewModel.IsMute = ViewModel.Volume == 0;
        }

        public MusicPlayerViewModel ViewModel => ViewModel<MusicPlayerViewModel>();

        private bool _isDragging;

        private void Slider_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var slider = (Slider)sender;

            if (ViewModel.Current is null)
            {
                return;
            }

            _isDragging = true;
            
            //
            // 清除绑定
            slider.ClearValue(RangeBase.ValueProperty);
            slider.ValueChanged += Slider_OnValueChanged;
        }

        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = (Slider)sender;

            var time = TimeSpan.FromSeconds(slider.Value);
            ViewModel.SetPosition(time);
        }

        private void Slider_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var slider = (Slider)sender;

            if (ViewModel.Current is null)
            {
                return;
            }
            
            if (_isDragging)
            {
                _isDragging = false;
                
                //
                // 恢复绑定
                slider.ValueChanged -= Slider_OnValueChanged;
                slider.SetBinding(RangeBase.ValueProperty, new Binding
                {
                    Source    = ViewModel,
                    Converter = (TimeSpanConverter)FindResource("TimeSpanConverter"),
                    Path      = new PropertyPath("Position"),
                    Mode      = BindingMode.OneWay
                });
            }
        }
    }
}