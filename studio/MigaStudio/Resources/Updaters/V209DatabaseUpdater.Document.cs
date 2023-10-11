using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Relatives;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels;
using Acorisoft.Miga.Doc;
using Acorisoft.Miga.Doc.Documents;
using Acorisoft.Miga.Doc.Parts;
using DataPartCollection = Acorisoft.FutureGL.MigaDB.Data.DataParts.DataPartCollection;
using OldDocument = Acorisoft.Miga.Doc.Documents.Document;
using NewDocument = Acorisoft.FutureGL.MigaDB.Documents.Document;
using OldDataPart = Acorisoft.Miga.Doc.Parts.DataPart;
using NewDataPart = Acorisoft.FutureGL.MigaDB.Data.DataParts.DataPart;
using StickyNote = Acorisoft.FutureGL.MigaDB.Data.DataParts.StickyNote;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Updaters
{
    partial class V209DatabaseUpdater
    {
        public static DocumentCache Transform(string avatar, DocumentIndex oldCache)
        {
            //
            var newCache = new DocumentCache
            {
                Id             = oldCache.Id,
                Avatar         = avatar,
                Name           = oldCache.Name,
                Type           = Transform(oldCache.DocumentType),
                Removable      = false,
                IsDeleted      = oldCache.IsDelete,
                IsLocked       = oldCache.IsLocking,
                TimeOfCreated  = oldCache.CreatedDateTime,
                TimeOfModified = oldCache.ModifiedDateTime,
            };

            return newCache;
        }

        public static NewDocument Transform(OldDocument oldDocument)
        {
            if (oldDocument is null)
            {
                return null;
            }

            var newDocument = new NewDocument
            {
                Id    = oldDocument.Id,
                Name  = oldDocument.Name,
                Metas = new MetadataCollection(),
                Parts = new DataPartCollection(),
                Type  = Transform(oldDocument.Type),
            };

            if (oldDocument.Parts is not null)
            {
                newDocument.Parts
                           .AddMany(oldDocument.Parts
                                               .Select(Transform)
                                               .Where(newPart => newPart is not null));
            }

            return newDocument;
        }
        
        private static NewDataPart Transform(OldDataPart part)
        {
            if (part is WritingPart wp)
            {
                var sn = new PartOfStickyNote
                {
                    Items = new List<StickyNote>()
                };
                foreach (var oldSN in wp.Composes)
                {
                    sn.Items
                      .Add(new StickyNote
                      {
                          TimeOfCreated  = DateTime.Now,
                          TimeOfModified = DateTime.Now,
                          Id             = ID.Get(),
                          Content        = oldSN.Content,
                          Title          = oldSN.Name,
                          Intro          = oldSN.Summary,
                          
                      });
                }
                return sn;
            }
            if (part is CustomDataPart cp)
            {
                var mp = new PartOfModule
                {
                    Id   = cp.Id.ToString("N"),
                    Name = cp.Name,
                    Blocks = new List<ModuleBlock>(cp.Properties
                                                     .Select(Transform)),
                    Version = 1
                };
                return mp;
            }
            return null;
        }

        public static ModuleBlock Transform(InputProperty property)
        {
            return property switch
            {
                TextProperty t => new SingleLineBlock
                {
                    Id       = ID.Get(),
                    Name     = t.Name,
                    Metadata = t.Metadata,
                    Suffix   = t.Unit,
                    Fallback = t.Fallback,
                    ToolTips = t.ToolTips
                },
                PageProperty p => new MultiLineBlock
                {
                    Id               = ID.Get(),
                    Name             = p.Name,
                    Metadata         = p.Metadata,
                    Fallback         = p.Fallback,
                    ToolTips         = p.ToolTips,
                    CharacterLimited = 800,
                    EnableExpression = false,
                },
                NumberProperty n => new NumberBlock
                {
                    Id       = ID.Get(),
                    Name     = n.Name,
                    Metadata = n.Metadata,
                    Fallback = int.TryParse(n.Fallback, out var n_f) ? n_f : 10,
                    ToolTips = n.ToolTips,
                    Maximum  = 10,
                    Minimum  = 0,
                    Suffix   = n.Unit,
                },
                SequenceProperty s => new SequenceBlock
                {
                    Id       = ID.Get(),
                    Name     = s.Name,
                    Metadata = s.Metadata,
                    Fallback = s.Fallback,
                    ToolTips = s.ToolTips,
                    Items = s.Values.Select(x => new OptionItem
                    {
                        Name  = x.Text,
                        Value = x.Text
                    }).ToList(),
                },
                ColorProperty c => new ColorBlock
                {
                    Id       = ID.Get(),
                    Name     = c.Name,
                    Metadata = c.Metadata,
                    Fallback = c.Fallback,
                    ToolTips = c.ToolTips,
                },
                GroupProperty g  => Transform(g),
                OptionProperty o => Transform(o),
                _                => null,
            };
        }
        private static ModuleBlock Transform(GroupProperty property)
        {
            var group = new GroupBlock
            {
                Id       = ID.Get(),
                Name     = property.Name,
                Metadata = property.Metadata,
                ToolTips = property.ToolTips,
                Items    = property.Values.Select(Transform).ToList()
            };
            return group;
        }
        
        private static ModuleBlock Transform(OptionProperty property)
        {
            if (property.Kind == OptionKind2.Opposite)
            {
                return new BinaryBlock
                {
                    Id       = ID.Get(),
                    Name     = property.Name,
                    Metadata = property.Metadata,
                    ToolTips = property.ToolTips,
                    Negative = property.NegativeValue,
                    Positive = property.PositiveValue,
                    Fallback = false
                };
            }
            
            if (property.Kind == OptionKind2.Favorite)
            {
                return new HeartBlock
                {
                    Id       = ID.Get(),
                    Name     = property.Name,
                    Metadata = property.Metadata,
                    ToolTips = property.ToolTips,
                    Fallback = false
                };
            }
            
            return new SwitchBlock
            {
                Id       = ID.Get(),
                Name     = property.Name,
                Metadata = property.Metadata,
                ToolTips = property.ToolTips,
                Fallback = false
            };
        }

        public static DocumentType Transform(OldDocumentKind type)
        {
            return type switch
            {
                OldDocumentKind.Assets    => DocumentType.Item,
                OldDocumentKind.Character => DocumentType.Character,
                OldDocumentKind.Skills    => DocumentType.Skill,
                OldDocumentKind.Materials => DocumentType.Item,
                OldDocumentKind.Map       => DocumentType.Geography,
                _                         => DocumentType.Other,
            };
        }
    }


    
    
}