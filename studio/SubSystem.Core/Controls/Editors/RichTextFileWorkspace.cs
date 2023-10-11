using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Acorisoft.FutureGL.MigaStudio.Controls.Editors.Utils;
using Acorisoft.FutureGL.MigaStudio.Editors.Models;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public class RichTextFileWorkspace : Workspace<RichTextBox>
    {
        
        public PartOfRtf Part { get; init; }

        public override void ScrollTo(IOutlineModel outlineModel)
        {
            
        }

        public override bool CanUndo() => Control.CanUndo;

        public override bool CanRedo()=> Control.CanRedo;

        public override void Undo()
        {
            if (Control is null)
            {
                return;
            }

            if (!CanUndo())
            {
                return;
            }

            Control.Undo();
        }

        public override void Redo()
        {
            if (Control is null)
            {
                return;
            }

            if (!CanRedo())
            {
                return;
            }

            Control.Redo();
        }

        public override void Inactive()
        {
            Control.Visibility = Visibility.Collapsed;
        }

        public override void Active()
        {
            Control.Visibility = Visibility.Visible;
            UpdateDocumentState();
        }
        
        private void UpdateDocumentState()
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Selection, this);
            WorkspaceChanged?.Invoke(StateChangedEventSource.Caret, this);
        }

        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(Part?.Content))
            {
                Control.SetText(Part.Content);
            }
            
            if (IsWorking)
            {
                return;
            }

            IsWorking = true;
            Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                          added => Control.TextChanged   += added,
                          removed => Control.TextChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(200))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x =>
                      {
                          OnTextChanged(x.Sender, x.EventArgs);
                      })
                      .DisposeWith(Disposable);
            Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                          added => Control.SelectionChanged   += added,
                          removed => Control.SelectionChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(200))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x =>
                      {
                          OnSelectionChanged(x.Sender, x.EventArgs);
                      })
                      .DisposeWith(Disposable);
            
        }
        
        public sealed override void UpdateCaretState()
        {
            if (Control is null)
            {
                LineNumber = -1;
                LineColumn = -1;
                return;
            }

        }

        public override IEnumerable<IOutlineModel> GetOutlineModels()
        {
            // TODO:
            return null;
        }

        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Selection, this);
            WorkspaceChanged?.Invoke(StateChangedEventSource.Caret, this);
            
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Part.Content = Control.GetText();
            WorkspaceChanged?.Invoke(StateChangedEventSource.TextSource, this);
        }

        public override string Content =>  Part is null ? string.Empty : Part.Content;
    }
}