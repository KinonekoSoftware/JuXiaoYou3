using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Editors.Completion;
using Acorisoft.FutureGL.MigaStudio.Editors.Models;
using DynamicData;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using Markdig;
using Markdig.Syntax;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public class MarkdownOutlineModel : IOutlineModel
    {
        public string Name { get; init; }
        public int Level { get; init; }
        public int CharacterOffset { get; init; }
        public int CharacterLength { get; init; }
        public List<IOutlineModel> Children { get; init; }
    }

    public class MarkdownWorkspace : Workspace<TextEditor>
    {
        private ConceptCompletionWindow _window;
        private bool                    _isShow;

        private record struct ParsingNode
        {
            public HeadingBlock Block { get; init; }
            public int ParentLevel { get; init; }
        }

        public override bool CanUndo() => Control.CanUndo;

        public override bool CanRedo() => Control.CanRedo;

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

        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(Part?.Content))
            {
                Control.Text = Part.Content;
            }


            if (IsWorking)
            {
                return;
            }

            IsWorking = true;
            Observable.FromEventPattern<EventHandler, EventArgs>(
                          added => Control.TextChanged   += added,
                          removed => Control.TextChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(300))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x => { OnTextChanged(x.Sender, x.EventArgs); })
                      .DisposeWith(Disposable);
            Observable.FromEventPattern<EventHandler, EventArgs>(
                          added => Control.TextArea.SelectionChanged   += added,
                          removed => Control.TextArea.SelectionChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(300))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x => { OnSelectionChanged(x.Sender, x.EventArgs); })
                      .DisposeWith(Disposable);
            Observable.FromEventPattern<EventHandler, EventArgs>(
                          added => Control.TextArea
                                          .Caret
                                          .PositionChanged += added,
                          removed => Control.TextArea
                                            .Caret
                                            .PositionChanged -= removed)
                      .Throttle(TimeSpan.FromMilliseconds(300))
                      .ObserveOn(Xaml.Get<IScheduler>())
                      .Subscribe(x => { OnPositionChanged(x.Sender, x.EventArgs); })
                      .DisposeWith(Disposable);
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

        #region GetOutlineModels

        private static MarkdownOutlineModel FromBlock(HeadingBlock block)
        {
            return new MarkdownOutlineModel
            {
                Name            = block.GetString(),
                Level           = block.Level,
                CharacterLength = block.Span.Length,
                CharacterOffset = block.Span.Start,
                Children        = new List<IOutlineModel>(8)
            };
        }


        public override IEnumerable<IOutlineModel> GetOutlineModels()
        {
            if (string.IsNullOrEmpty(Content))
            {
                return Array.Empty<IOutlineModel>();
            }

            var markdown = Markdown.Parse(Content);

            if (markdown is null)
            {
                return null;
            }

            var headingBlocks = markdown.OfType<HeadingBlock>();
            var root          = new List<MarkdownOutlineModel>(8);
            var stack         = new Stack<MarkdownOutlineModel>();
            var rootBlock     = (MarkdownOutlineModel)null;
            var parentBlock   = (MarkdownOutlineModel)null;
            var currentBlock  = (MarkdownOutlineModel)null;

            /*
             * --- L3
             * --- L3
             * ---- L4
             * ---- L4
             * - L1
             * -- L2
             * --- L3
             * - L1
             * -- L2
             * - L1
             */
            foreach (var block in headingBlocks)
            {
                //
                // 当前的块
                var current = FromBlock(block);

                //
                // 第一次添加，做初始化
                if (rootBlock is null)
                {
                    currentBlock = current;
                    rootBlock    = currentBlock;
                    root.Add(rootBlock);
                    continue;
                }

                if (currentBlock.Level == current.Level)
                {
                    if (parentBlock is null)
                    {
                        currentBlock = current;
                        rootBlock    = currentBlock;
                        root.Add(rootBlock);
                    }
                    else
                    {
                        parentBlock.Children.Add(current);
                        currentBlock = current;
                    }
                }
                else if (currentBlock.Level > current.Level)
                {
                    //
                    // 遇到上级
                    if (stack.Count == 0)
                    {
                    }
                    else
                    {
                        var grandBlock = stack.Peek();

                        if (grandBlock.Level < current.Level)
                        {
                            //
                            // 虽然目前的块是上一个块的父级，但是祖父块依然是当前块的父级
                            grandBlock.Children
                                      .Add(current);

                            currentBlock = current;
                            parentBlock  = currentBlock;
                        }
                        else
                        {
                            /*
                             * 1
                             * 2
                             * 3
                             * 4                        
                             * 5
                             * 2            stack: 1 2 3 
                             */
                            //
                            // 找到祖父块
                            while (stack.Count > 0)
                            {
                                grandBlock = stack.Pop();

                                if (grandBlock.Level < current.Level)
                                {
                                    break;
                                }
                            }

                            //
                            //
                            if (grandBlock.Level < current.Level)
                            {
                                grandBlock.Children
                                          .Add(current);

                                parentBlock  = grandBlock;
                                currentBlock = current;
                            }
                            else
                            {
                                /*
                                 * 3
                                 * 4                        
                                 * 5
                                 * 2            stack: 3 
                                 */
                                rootBlock    = current;
                                parentBlock  = null;
                                currentBlock = current;
                                root.Add(currentBlock);
                            }
                        }
                    }
                }
                else
                {
                    /*
                     * --- L3
                     * ----- L5
                     * ---- L4 
                     */
                    //
                    // 遇到子级，就先压栈
                    if (parentBlock is not null) stack.Push(parentBlock);
                    parentBlock  = currentBlock;
                    currentBlock = current;
                    parentBlock.Children
                               .Add(current);
                }
            }

            //
            // 返回
            return root;
        }

        #endregion
        
        
        public override void ScrollTo(IOutlineModel outlineModel)
        {
            if (outlineModel is not MarkdownOutlineModel mom)
            {
                return;
            }

            var document = Control.TextArea
                                  .Document;
            var location = document.GetLocation(mom.CharacterOffset);
            var location2 = document.GetLocation(mom.CharacterOffset+ mom.CharacterLength);
            Control.TextArea
                   .Selection = new RectangleSelection(
                Control.TextArea,
                new TextViewPosition(location),
                new TextViewPosition(location2));
            Control.ScrollTo(location.Line, location.Column);
        }

        private void UpdateDocumentState()
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Selection, this);
            WorkspaceChanged?.Invoke(StateChangedEventSource.Caret, this);
        }

        private void OnPositionChanged(object sender, EventArgs e)
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Caret, this);
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            WorkspaceChanged?.Invoke(StateChangedEventSource.Selection, this);
        }

        private void OnWindowClose(object sender, EventArgs e)
        {
            _window.Closed -= OnWindowClose;
            _window        =  null;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
#if NEXT_VER
            if (_window is null)
            {
                _isShow = false;
                _window = new ConceptCompletionWindow(Control.TextArea);
                _window.Closed += OnWindowClose;
            }

            var data = _window.CompletionList
                              .CompletionData;
            var engine = Studio.Engine<DocumentEngine>();
            var concepts = engine.GetConcepts()
                                 .Select(x => new ConceptCompletionData
                                 {
                                     Text = x.Name,
                                     Content = x.Name
                                 });
            data.AddMany(concepts, true);
            
            //
            //
            if (_isShow)
            {
                _window.Close();
            }

            if (_window is null)
            {
                _isShow = false;
                return;
            }

            _isShow = true;
            _window.Show();
#endif
            Part.Content = Control.Text;
            WorkspaceChanged?.Invoke(StateChangedEventSource.TextSource, this);
        }

        public sealed override void UpdateCaretState()
        {
            if (Control is null)
            {
                LineNumber = -1;
                LineColumn = -1;
                return;
            }

            var caret = Control.TextArea
                               .Caret;

            LineNumber = caret.Line;
            LineColumn = caret.Column;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Content => Part is null ? string.Empty : Part.Content;

        /// <summary>
        /// 
        /// </summary>
        public PartOfMarkdown Part { get; init; }
    }
}