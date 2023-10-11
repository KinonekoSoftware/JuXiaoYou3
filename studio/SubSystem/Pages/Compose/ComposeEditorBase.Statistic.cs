using System.Reactive.Concurrency;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Editors;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    partial class ComposeEditorBase
    {
        private int _totalCharacterCount;
        private int _nonCharacterCount;
        private int _characterCount;
        private int _whitespaceCount;
        private int _lineNumber;
        private int _columnNumber;
        private int _selectionCount;
        private int _digitCount;
        private int _controlCount;

        protected void Statistic(IWorkspace workspace)
        {
            if (workspace is null)
            {
                return;
            }

            var textSnapshot = workspace.Content;

            if (textSnapshot is not null)
            {
                TotalCharacterCount = textSnapshot.Length;
                Cache.Length        = TotalCharacterCount;
                RaiseUpdated(nameof(TotalCharacterCount));
            }


            Task.Run(() =>
            {
                var asyncOutlineModel = workspace.GetOutlineModels();
                var asyncDocumentModel = Xaml.Get<ITokenizerService>()
                                             .Tokenize(textSnapshot);
                
                Scheduler.Schedule(() =>
                {
                    Outlines.AddMany(asyncOutlineModel, true);
                    ReferenceDocuments.AddMany(asyncDocumentModel, true);
                });
            });

            // Task.Run(() =>
            // {
            //     return;
            //     foreach (var ch in textSnapshot)
            //     {
            //         if (char.IsDigit(ch))
            //         {
            //             _digitCount++;
            //             _characterCount++;
            //             continue;
            //         }
            //         
            //         if (char.IsControl(ch))
            //         {
            //             _controlCount++;
            //             _nonCharacterCount++;
            //             continue;
            //         }
            //
            //         if (char.IsWhiteSpace(ch))
            //         {
            //             _whitespaceCount++;
            //             _nonCharacterCount++;
            //             continue;
            //         }
            //         
            //         _characterCount++;
            //     }
            //     
            //     Scheduler.Schedule(() =>
            //     {
            //         RaiseUpdated(nameof(ControlCount));
            //         RaiseUpdated(nameof(DigitCount));
            //         RaiseUpdated(nameof(WhitespaceCount));
            //         RaiseUpdated(nameof(CharacterCount));
            //         RaiseUpdated(nameof(NonCharacterCount));
            //     });
            // });
        }

        /// <summary>
        /// 获取或设置 <see cref="DigitCount"/> 属性。
        /// </summary>
        public int DigitCount
        {
            get => _digitCount;
            set => SetValue(ref _digitCount, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectionCount"/> 属性。
        /// </summary>
        public int SelectionCount
        {
            get => _selectionCount;
            set => SetValue(ref _selectionCount, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ColumnNumber"/> 属性。
        /// </summary>
        public int ColumnNumber
        {
            get => _columnNumber;
            set => SetValue(ref _columnNumber, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="LineNumber"/> 属性。
        /// </summary>
        public int LineNumber
        {
            get => _lineNumber;
            set => SetValue(ref _lineNumber, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="WhitespaceCount"/> 属性。
        /// </summary>
        public int WhitespaceCount
        {
            get => _whitespaceCount;
            set => SetValue(ref _whitespaceCount, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="CharacterCount"/> 属性。
        /// </summary>
        public int CharacterCount
        {
            get => _characterCount;
            set => SetValue(ref _characterCount, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="ControlCount"/> 属性。
        /// </summary>
        public int ControlCount
        {
            get => _controlCount;
            set => SetValue(ref _controlCount, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="NonCharacterCount"/> 属性。
        /// </summary>
        public int NonCharacterCount
        {
            get => _nonCharacterCount;
            set => SetValue(ref _nonCharacterCount, value);
        }


        /// <summary>
        /// 获取或设置 <see cref="TotalCharacterCount"/> 属性。
        /// </summary>
        public int TotalCharacterCount
        {
            get => _totalCharacterCount;
            set => SetValue(ref _totalCharacterCount, value);
        }
    }
}