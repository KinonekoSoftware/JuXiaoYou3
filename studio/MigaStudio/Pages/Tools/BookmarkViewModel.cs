using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Others;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;
using ImTools;
using DragDropEffects = System.Windows.DragDropEffects;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class BookmarkViewModel : EntityTabViewModel<Bookmark>
    {
        private static readonly HttpClient _client = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });

        public BookmarkViewModel()
        {
            Engine = Studio.Engine<EntityEngine>();
        }
        
        
        protected override async void OnDragDrop(WindowDragDropArgs e)
        {
            var args = e.Args;
            args.Handled = true;
            if (e.State == DragDropState.DragStart)
            {
                if (args.Data
                        .GetDataPresent(DataFormats.Text))
                    args.Effects = DragDropEffects.Link;
                else
                    args.Effects = DragDropEffects.None;
            }
            else
            {
                var link = args.Data
                                .GetData(DataFormats.Text)
                                ?.ToString();
                
                if (!Uri.TryCreate(link, UriKind.Absolute, out _))
                {
                    return;
                }
                
                var bookmark = new Bookmark
                {
                    Id   = ID.Get(),
                    Link = link
                };

                Engine.AddBookmark(bookmark);
                Collection.Add(bookmark);

                if (!await UpdatingBookmark(bookmark))
                {
                    var length = bookmark.Link
                                         ?.Length ?? 0;
                    bookmark.Title = bookmark.Link?[..Math.Clamp(length, length, 50)];
                }
            }
        }

        private static async Task<bool> UpdatingBookmark(Bookmark bookmark)
        {
            if (bookmark is null ||
                string.IsNullOrEmpty(bookmark.Link))
            {
                return false;
            }
            
            try
            {
                var text = await _client.GetStringAsync(bookmark.Link);
                var document = new HtmlDocument
                {
                    OptionReadEncoding = true
                };
                document.LoadHtml(text);

                var titleNode = document.DocumentNode
                                        .SelectNodes("//title")
                                        .FirstOrDefault();

                var title = titleNode?.InnerText;
                bookmark.Title = title;
                return true;
            }
            catch (Exception _)
            {
                Debug.WriteLine(bookmark.Link);
                return false;
            }
        }

        protected override bool NeedDataSourceSynchronize()
        {
            if (Version != Engine.Version)
            {
                Version = Engine.Version;
                return true;
            }

            return false;
        }

        protected override void OnRequestDataSourceSynchronize(ICollection<Bookmark> dataSource)
        {
            dataSource.AddMany(Engine.GetBookmarks(), true);
        }

        protected override void Save()
        {
        }

        protected override async Task<Op<Bookmark>> Add()
        {
            var r = await SingleLineViewModel.String(SR.EditNameTitle);

            if (!r.IsFinished)
            {
                return Op<Bookmark>.Failed(Language.GetEnum(EngineFailedReason.Cancelled));
            }

            if (!Uri.TryCreate(r.Value, UriKind.RelativeOrAbsolute, out _))
            {
                return Op<Bookmark>.Failed(Language.GetEnum(EngineFailedReason.InputDataError));
            }

            var bookmark = new Bookmark
            {
                Id   = ID.Get(),
                Link = r.Value
            };

            if (!await UpdatingBookmark(bookmark))
            {
                bookmark.Title = bookmark.Link?[20..];
            }

            Engine.AddBookmark(bookmark);
            return Op<Bookmark>.Success(bookmark);
        }

        protected override Task Edit(Bookmark entity)
        {
            return Task.Run(() => Engine.UpdateBookmark(entity));
        }

        protected override void Remove(Bookmark entity)
        {
            Engine.RemoveBookmark(entity);
        }

        protected override void ShiftUp(Bookmark entity, int oldIndex, int newIndex)
        {
        }

        protected override void ShiftDown(Bookmark entity, int oldIndex, int newIndex)
        {
        }

        protected override void ClearEntity(Bookmark[] entities)
        {
        }
        
        public EntityEngine Engine { get; }
        public sealed override bool Uniqueness => true;
    }
}