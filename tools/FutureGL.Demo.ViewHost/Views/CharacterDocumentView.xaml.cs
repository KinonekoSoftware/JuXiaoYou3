using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Adorners;
using Acorisoft.FutureGL.Forest.Controls;
using ColorPicker.Models;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;

namespace ViewHost.Views
{
    [Connected(View = typeof(CharacterDocumentView), ViewModel = typeof(CharacterDocumentViewModel))]
    public partial class CharacterDocumentView:ForestUserControl 
    {
        public CharacterDocumentView()
        {
            InitializeComponent();
        }
    }
}