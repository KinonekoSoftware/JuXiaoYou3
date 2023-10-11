using System.Windows;
using Acorisoft.FutureGL.Forest.Models;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(BookmarkPage), ViewModel = typeof(BookmarkViewModel))]
    public partial class BookmarkPage
    {
        public BookmarkPage()
        {
            InitializeComponent();
            this.DragEnter += OnDragEnter;
            this.Drop      += OnDragOver;
        }
        
        
        private void OnDragOver(object sender, DragEventArgs e)
        {
            ViewModel<BookmarkViewModel>().SetWindowEvent(new WindowDragDropArgs
            {
                State = DragDropState.Dropped,
                Args  = e
            });
            e.Handled = true;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            ViewModel<BookmarkViewModel>().SetWindowEvent(new WindowDragDropArgs
            {
                State = DragDropState.DragStart,
                Args  = e
            });
            e.Handled = true;
        }
    }
}