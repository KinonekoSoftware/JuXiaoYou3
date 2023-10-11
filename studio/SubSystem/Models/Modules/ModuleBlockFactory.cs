using System.Linq;
using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;
using Acorisoft.Miga.Doc.Documents;
using Acorisoft.Miga.Doc.Parts;

namespace Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels
{
    public static class ModuleBlockFactory
    {
        public static IEnumerable<ModuleBlock> CreateBlocks()
        {
            var b = CreateBlocksImpl();
            b.ForEach(x =>
            {
                x.ClearValue();
            });
            return b;
        }

        #region BlockFactory

        public static GroupBlock CreateGroup()
        {
            return new GroupBlock
            {
                Id       = ID.Get(),
                Name     = SubSystemString.GetGroupName(),
                ToolTips = SubSystemString.GetToolTipsField(),
                Items    = new List<ModuleBlock>()
            };
        }

        public static SequenceBlock CreateSequence()
        {
            var b = new SequenceBlock
            {
                Id       = ID.Get(),
                Name     = SubSystemString.GetSequenceName(),
                Fallback = "Item1",
                Value    = null,
                Items    = new List<OptionItem>(),
                ToolTips = SubSystemString.GetToolTipsField(),
            };
            b.Items.Add(new OptionItem { Name = "Item1", Value = "Item1" });
            b.Items.Add(new OptionItem { Name = "Item2", Value = "Item2" });
            b.Items.Add(new OptionItem { Name = "Item3", Value = "Item3" });
            b.Items.Add(new OptionItem { Name = "Item4", Value = "ItemA" });
            return b;
        }


        private static SingleLineBlock CreateSingleLine() => new SingleLineBlock
        {
            Id       = ID.Get(),
            Value    = null,
            Name     = SubSystemString.GetSingleLineName(),
            Suffix   = SubSystemString.GetSuffixField(),
            Fallback = SubSystemString.GetFallbackField(),
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static MultiLineBlock CreateMultiLine() => new MultiLineBlock
        {
            Id       = ID.Get(),
            Value    = null,
            Name     = SubSystemString.GetMultiLineName(),
            Fallback = SubSystemString.GetFallbackField(),
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static ColorBlock CreateColor() => new ColorBlock
        {
            Id       = ID.Get(),
            Value    = null,
            Name     = SubSystemString.GetColorName(),
            Fallback = "#007ACC",
            ToolTips = SubSystemString.GetToolTipsField(),
        };


        private static NumberBlock CreateNumber() => new NumberBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetNumberName(),
            Fallback = 12,
            Maximum  = 30,
            Minimum  = 0,
            Value    = -1,
            Suffix   = SubSystemString.GetNumberName(),
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static SliderBlock CreateSlider() => new SliderBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetSliderName(),
            Fallback = 12,
            Maximum  = 30,
            Minimum  = 0,
            Value    = -1,
            Suffix   = SubSystemString.GetSliderName(),
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static DegreeBlock CreateDegree() => new DegreeBlock
        {
            Id         = ID.Get(),
            Name       = SubSystemString.GetDegreeName(),
            Fallback   = 4,
            Maximum    = 10,
            Minimum    = 0,
            DivideLine = 10,
            Value      = -1,
            Negative   = SubSystemString.GetNegativeField(),
            Positive   = SubSystemString.GetPositiveField(),
            ToolTips   = SubSystemString.GetToolTipsField(),
        };


        private static SwitchBlock CreateSwitch() => new SwitchBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetSwitchName(),
            Fallback = false,
            ToolTips = SubSystemString.GetToolTipsField(),
        };


        private static BinaryBlock CreateBinary() => new BinaryBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetBinaryName(),
            Fallback = false,
            ToolTips = SubSystemString.GetToolTipsField(),

            Negative = SubSystemString.GetNegativeField(),
            Positive = SubSystemString.GetPositiveField(),
        };


        private static HeartBlock CreateHeart() => new HeartBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetHeartName(),
            Fallback = false,
            ToolTips = SubSystemString.GetToolTipsField(),
        };


        private static StarBlock CreateStar() => new StarBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetStarName(),
            Fallback = false,
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static LikabilityBlock CreateLikability() => new LikabilityBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetLikabilityName(),
            Fallback = 6,
            Maximum  = 10,
            Minimum  = 0,
            Value    = -1,
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static RateBlock CreateRate() => new RateBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetRateName(),
            Fallback = 6,
            Maximum  = 10,
            Value    = -1,
            Minimum  = 0,
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static RadarBlock CreateRadar() => new RadarBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetRadarName(),
            Maximum  = 10,
            Value    = null,
            Minimum  = 0,
            ToolTips = SubSystemString.GetToolTipsField(),

            Axis     = new[] { "Axis1", "Axis2", "Axis3", "Axis4" },
            Fallback = new[] { 3, 4, 5, 6 },
            Color    = "#007ACC"
        };


        private static HistogramBlock CreateHistogram() => new HistogramBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetHistogramName(),
            Maximum  = 10,
            Value    = null,
            Minimum  = 0,
            ToolTips = SubSystemString.GetToolTipsField(),

            Axis     = new[] { "Axis1", "Axis2", "Axis3", "Axis4" },
            Fallback = new[] { 3, 4, 5, 6 },
            Color    = "#007ACC"
        };

        private static AudioBlock CreateAudio() => new AudioBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetAudioName(),
            TargetName      = SubSystemString.GetAudioName(),
            TargetSource    = SubSystemString.GetAudioName(),
            TargetThumbnail = SubSystemString.GetAudioName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static FileBlock CreateFile() => new FileBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetFileName(),
            TargetName      = SubSystemString.GetFileName(),
            TargetSource    = SubSystemString.GetFileName(),
            TargetThumbnail = SubSystemString.GetFileName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static VideoBlock CreateVideo() => new VideoBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetVideoName(),
            TargetName      = SubSystemString.GetVideoName(),
            TargetSource    = SubSystemString.GetVideoName(),
            TargetThumbnail = SubSystemString.GetVideoName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static MusicBlock CreateMusic() => new MusicBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetMusicName(),
            TargetName      = SubSystemString.GetMusicName(),
            TargetSource    = SubSystemString.GetMusicName(),
            TargetThumbnail = SubSystemString.GetMusicName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static ImageBlock CreateImage() => new ImageBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetImageName(),
            TargetName      = SubSystemString.GetImageName(),
            TargetSource    = SubSystemString.GetImageName(),
            TargetThumbnail = SubSystemString.GetImageName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static ReferenceBlock CreateReference() => new ReferenceBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetReferenceName(),
            DataSource      = ReferenceSource.Compose,
            TargetName      = SubSystemString.GetReferenceName(),
            TargetSource    = SubSystemString.GetReferenceName(),
            TargetThumbnail = SubSystemString.GetReferenceName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        #endregion

        public static ModuleBlock[] CreateBlocksImpl()
        {
            return new ModuleBlock[]
            {
                CreateSingleLine(),
                CreateMultiLine(),
                CreateColor(),
                CreateNumber(),
                CreateSlider(),
                CreateDegree(),
                CreateSwitch(),
                CreateBinary(),
                CreateHeart(),
                CreateStar(),


                CreateSequence(),
                CreateGroup(),

                CreateLikability(),
                CreateRate(),

                CreateRadar(),
                CreateHistogram(),

                CreateAudio(),
                CreateFile(),
                CreateVideo(),
                CreateMusic(),
                CreateImage(),
                CreateReference(),
            };
        }


        public static ModuleBlockDataUI GetDataUI(ModuleBlock block)
        {
            return block switch
            {
                SingleLineBlock SingleLineBlock => new SingleLineBlockDataUI(SingleLineBlock),
                MultiLineBlock MultiLineBlock   => new MultiLineBlockDataUI(MultiLineBlock),
                ColorBlock ColorBlock           => new ColorBlockDataUI(ColorBlock),
                NumberBlock NumberBlock         => new NumberBlockDataUI(NumberBlock),
                SliderBlock SliderBlock         => new SliderBlockDataUI(SliderBlock),

                DegreeBlock DegreeBlock => new DegreeBlockDataUI(DegreeBlock),
                SwitchBlock SwitchBlock => new SwitchBlockDataUI(SwitchBlock),
                BinaryBlock BinaryBlock => new BinaryBlockDataUI(BinaryBlock),
                HeartBlock HeartBlock   => new HeartBlockDataUI(HeartBlock),
                StarBlock StarBlock     => new StarBlockDataUI(StarBlock),

                SequenceBlock SequenceBlock => new SequenceBlockDataUI(SequenceBlock),
                GroupBlock GroupBlock       => new GroupBlockDataUI(GroupBlock),

                LikabilityBlock LikabilityBlock => new LikabilityBlockDataUI(LikabilityBlock),
                RateBlock RateBlock             => new RateBlockDataUI(RateBlock),

                RadarBlock RadarBlock         => new RadarBlockDataUI(RadarBlock),
                HistogramBlock HistogramBlock => new HistogramBlockDataUI(HistogramBlock),

                AudioBlock AudioBlock         => new AudioBlockDataUI(AudioBlock),
                FileBlock FileBlock           => new FileBlockDataUI(FileBlock),
                VideoBlock VideoBlock         => new VideoBlockDataUI(VideoBlock),
                MusicBlock MusicBlock         => new MusicBlockDataUI(MusicBlock),
                ImageBlock ImageBlock         => new ImageBlockDataUI(ImageBlock),
                ReferenceBlock ReferenceBlock => new ReferenceBlockDataUI(ReferenceBlock),
                _                             => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
        }

        public static ModuleBlockDataUI GetDataUI(ModuleBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler)
        {
            return block switch
            {
                SingleLineBlock SingleLineBlock => new SingleLineBlockDataUI(SingleLineBlock, handler),
                MultiLineBlock MultiLineBlock   => new MultiLineBlockDataUI(MultiLineBlock, handler),
                ColorBlock ColorBlock           => new ColorBlockDataUI(ColorBlock, handler),
                NumberBlock NumberBlock         => new NumberBlockDataUI(NumberBlock, handler),
                SliderBlock SliderBlock         => new SliderBlockDataUI(SliderBlock, handler),

                DegreeBlock DegreeBlock => new DegreeBlockDataUI(DegreeBlock, handler),
                SwitchBlock SwitchBlock => new SwitchBlockDataUI(SwitchBlock, handler),
                BinaryBlock BinaryBlock => new BinaryBlockDataUI(BinaryBlock, handler),
                HeartBlock HeartBlock   => new HeartBlockDataUI(HeartBlock, handler),
                StarBlock StarBlock     => new StarBlockDataUI(StarBlock, handler),

                SequenceBlock SequenceBlock => new SequenceBlockDataUI(SequenceBlock, handler),
                GroupBlock GroupBlock       => new GroupBlockDataUI(GroupBlock, handler),

                LikabilityBlock LikabilityBlock => new LikabilityBlockDataUI(LikabilityBlock, handler),
                RateBlock RateBlock             => new RateBlockDataUI(RateBlock, handler),

                RadarBlock RadarBlock         => new RadarBlockDataUI(RadarBlock, handler),
                HistogramBlock HistogramBlock => new HistogramBlockDataUI(HistogramBlock, handler),

                AudioBlock AudioBlock         => new AudioBlockDataUI(AudioBlock, handler),
                FileBlock FileBlock           => new FileBlockDataUI(FileBlock, handler),
                VideoBlock VideoBlock         => new VideoBlockDataUI(VideoBlock, handler),
                MusicBlock MusicBlock         => new MusicBlockDataUI(MusicBlock, handler),
                ImageBlock ImageBlock         => new ImageBlockDataUI(ImageBlock, handler),
                ReferenceBlock ReferenceBlock => new ReferenceBlockDataUI(ReferenceBlock, handler),
                _                             => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
        }
        
        
        public static ModuleBlock GetBlock(BlockType kind)
        {
            ModuleBlock b = kind switch
            {
                BlockType.SingleLine => CreateSingleLine(),
                BlockType.MultiLine  => CreateMultiLine(),
                BlockType.Color      => CreateColor(),
                BlockType.Number     => CreateNumber(),
                BlockType.Slider     => CreateSlider(),

                BlockType.Degree => CreateDegree(),
                BlockType.Switch => CreateSwitch(),
                BlockType.Binary => CreateBinary(),
                BlockType.Heart  => CreateHeart(),
                BlockType.Star   => CreateStar(),


                BlockType.Sequence => CreateSequence(),
                BlockType.Group    => CreateGroup(),

                BlockType.Likability => CreateLikability(),
                BlockType.Rate       => CreateRate(),

                BlockType.Radar     => CreateRadar(),
                BlockType.Histogram => CreateHistogram(),

                BlockType.Audio     => CreateAudio(),
                BlockType.File      => CreateFile(),
                BlockType.Video     => CreateVideo(),
                BlockType.Music     => CreateMusic(),
                BlockType.Image     => CreateImage(),
                BlockType.Reference => CreateReference(),
                _                      => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            if (b is SequenceBlock s)
            {
                s.Items.Add(new OptionItem
                {
                    Name = "选项1（Item1）",
                    Value = "选项1（Item1）"
                });
                
                s.Items.Add(new OptionItem
                {
                    Name  = "选项2（Item2）",
                    Value = "选项2（Item2）"
                });
            }

            if (b is GroupBlock g)
            {
                g.Items.Add(CreateSingleLine());
                g.Items.Add(CreateSingleLine());
            }

            return b;
        }

        public static Presentation GetPresentation(MetadataKind kind)
        {
            return kind switch
            {
                MetadataKind.Rarity  => new RarityPresentation
                {
                    Id = ID.Get(),
                    Type = kind,
                },
                MetadataKind.HistogramChart     => new ChartPresentation
                {
                    Id        = ID.Get(),
                    Type      = kind,
                    ChartType = ChartType.Histogram
                },
                MetadataKind.RadarChart => new ChartPresentation
                {
                    Id    = ID.Get(),
                    Type  = kind,
                    ChartType = ChartType.Radar
                },
                MetadataKind.String => new StringPresentation
                {
                    Id = ID.Get(),
                },
                MetadataKind.Audio     => null,
                MetadataKind.File      => null,
                MetadataKind.Video     => null,
                MetadataKind.Music     => null,
                MetadataKind.Image     => null,
                MetadataKind.Reference => null,
                _                      => new GroupingPresentation
                {
                    Id        = ID.Get(),
                    Type      = kind,
                    DataLists = new ObservableCollection<IPresentationData>()
                }
            };
        }

        public static ModuleBlockEditUI GetEditUI(BlockType kind)
        {
            ModuleBlock b = kind switch
            {
                BlockType.SingleLine => CreateSingleLine(),
                BlockType.MultiLine  => CreateMultiLine(),
                BlockType.Color      => CreateColor(),
                BlockType.Number     => CreateNumber(),
                BlockType.Slider     => CreateSlider(),

                BlockType.Degree => CreateDegree(),
                BlockType.Switch => CreateSwitch(),
                BlockType.Binary => CreateBinary(),
                BlockType.Heart  => CreateHeart(),
                BlockType.Star   => CreateStar(),


                BlockType.Sequence => CreateSequence(),
                BlockType.Group    => CreateGroup(),

                BlockType.Likability => CreateLikability(),
                BlockType.Rate       => CreateRate(),

                BlockType.Radar     => CreateRadar(),
                BlockType.Histogram => CreateHistogram(),

                BlockType.Audio     => CreateAudio(),
                BlockType.File      => CreateFile(),
                BlockType.Video     => CreateVideo(),
                BlockType.Music     => CreateMusic(),
                BlockType.Image     => CreateImage(),
                BlockType.Reference => CreateReference(),
                _                      => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
            return GetEditUI(b);
        }

        public static ModuleBlockEditUI GetEditUI(ModuleBlock block)
        {
            return block switch
            {
                SingleLineBlock SingleLineBlock => new SingleLineBlockEditUI(SingleLineBlock),
                MultiLineBlock MultiLineBlock   => new MultiLineBlockEditUI(MultiLineBlock),
                ColorBlock ColorBlock           => new ColorBlockEditUI(ColorBlock),
                NumberBlock NumberBlock         => new NumberBlockEditUI(NumberBlock),
                SliderBlock SliderBlock         => new SliderBlockEditUI(SliderBlock),

                DegreeBlock DegreeBlock => new DegreeBlockEditUI(DegreeBlock),
                SwitchBlock SwitchBlock => new SwitchBlockEditUI(SwitchBlock),
                BinaryBlock BinaryBlock => new BinaryBlockEditUI(BinaryBlock),
                HeartBlock HeartBlock   => new HeartBlockEditUI(HeartBlock),
                StarBlock StarBlock     => new StarBlockEditUI(StarBlock),


                SequenceBlock SequenceBlock => new SequenceBlockEditUI(SequenceBlock),
                GroupBlock GroupBlock       => new GroupBlockEditUI(GroupBlock),

                LikabilityBlock LikabilityBlock => new LikabilityBlockEditUI(LikabilityBlock),
                RateBlock RateBlock             => new RateBlockEditUI(RateBlock),

                RadarBlock RadarBlock         => new RadarBlockEditUI(RadarBlock),
                HistogramBlock HistogramBlock => new HistogramBlockEditUI(HistogramBlock),

                AudioBlock AudioBlock         => new AudioBlockEditUI(AudioBlock),
                FileBlock FileBlock           => new FileBlockEditUI(FileBlock),
                VideoBlock VideoBlock         => new VideoBlockEditUI(VideoBlock),
                MusicBlock MusicBlock         => new MusicBlockEditUI(MusicBlock),
                ImageBlock ImageBlock         => new ImageBlockEditUI(ImageBlock),
                ReferenceBlock ReferenceBlock => new ReferenceBlockEditUI(ReferenceBlock),
                _                             => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
        }

        public static readonly BlockType[] GroupElementKinds = new[]
        {
            BlockType.Color,
            BlockType.Number,
            BlockType.SingleLine,
            BlockType.Slider,
            BlockType.Heart,
            BlockType.Degree,
            BlockType.MultiLine,
            BlockType.Likability,
            BlockType.Rate,
            BlockType.Reference,
            BlockType.Image,
            BlockType.Video,
            BlockType.Music,
            BlockType.Audio,
            BlockType.File,
        };

        // (\BlockType.w,
        // BlockType.$1,
        public static readonly BlockType[] BasicBlockKinds = new[]
        {
            BlockType.Color,
            BlockType.Number,
            BlockType.SingleLine,
            BlockType.MultiLine,
            BlockType.Likability,
            BlockType.Rate,
            BlockType.Slider,
            BlockType.Heart,
            BlockType.Degree,
        };

        public static readonly BlockType[] AdvancedBlockKinds = new[]
        {
            BlockType.Reference,
            BlockType.Image,
            BlockType.Video,
            BlockType.Music,
            BlockType.Audio,
            BlockType.File,
        };

        public static readonly BlockType[] OptionBlockKinds = new[]
        {
            BlockType.Binary,
            BlockType.Switch,
            BlockType.Sequence,
            BlockType.Group,
        };

        public static readonly BlockType[] ChartBlockKinds = new[]
        {
            BlockType.Histogram,
            BlockType.Radar,
        };
        
        public static readonly MetadataKind[] BasicMetadataKinds = new[]
        {
            MetadataKind.Text,
            MetadataKind.Color,
            MetadataKind.Degree,
            MetadataKind.Heart,
            MetadataKind.Star,
            MetadataKind.Switch,
            MetadataKind.Progress,
            MetadataKind.Likability,
            MetadataKind.Rarity,
        };

        public static readonly MetadataKind[] AdvancedMetadataKinds = new[]
        {
            MetadataKind.String,
            /* MetadataKind.Service, */
            MetadataKind.HistogramChart,
            MetadataKind.RadarChart,
        };


        internal static void EmptyHandler(ModuleBlockDataUI ui, ModuleBlock block)
        {
        }

        private static DocumentType GetType(DocumentKind kind)
        {
            return kind switch
            {
                DocumentKind.Assets    => DocumentType.Item,
                DocumentKind.Character => DocumentType.Character,
                DocumentKind.Skills    => DocumentType.Skill,
                DocumentKind.Materials => DocumentType.Item,
                DocumentKind.Map       => DocumentType.Geography,
                _                      => DocumentType.Other,
            };
        }

        private static ModuleBlock Upgrade(GroupProperty2 property)
        {
            var group = new GroupBlock
            {
                Id       = ID.Get(),
                Name     = property.Name,
                Metadata = property.Metadata,
                ToolTips = property.ToolTips,
                Items    = property.Values.Select(Upgrade).ToList()
            };
            return group;
        }
        
        private static ModuleBlock Upgrade(OptionProperty2 property)
        {
            if (property.Kind == OptionKind.Opposite)
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
            
            if (property.Kind == OptionKind.Favorite)
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

        public static ModuleBlock Upgrade(InputProperty2 property)
        {
            return property switch
            {
                TextProperty2 t => new SingleLineBlock
                {
                    Id       = ID.Get(),
                    Name     = t.Name,
                    Metadata = t.Metadata,
                    Suffix   = t.Unit,
                    Fallback = t.Fallback,
                    ToolTips = t.ToolTips
                },
                PageProperty2 p => new MultiLineBlock
                {
                    Id       = ID.Get(),
                    Name     = p.Name,
                    Metadata = p.Metadata,
                    Fallback = p.Fallback,
                    ToolTips = p.ToolTips,
                    CharacterLimited = 800,
                    EnableExpression = false,
                },
                NumberProperty2 n => new NumberBlock
                {
                    Id       = ID.Get(),
                    Name     = n.Name,
                    Metadata = n.Metadata,
                    Fallback = int.TryParse(n.Fallback, out var n_f) ? n_f : 10,
                    ToolTips = n.ToolTips,
                    Maximum = 10,
                    Minimum = 0,
                    Suffix = n.Unit,
                },
                SequenceProperty2 s => new SequenceBlock
                {
                    Id       = ID.Get(),
                    Name     = s.Name,
                    Metadata = s.Metadata,
                    Fallback = s.Fallback,
                    ToolTips = s.ToolTips,
                    Items = s.Values.Select(x => new OptionItem
                    {
                        Name = x.Text,
                        Value = x.Text
                    }).ToList(),
                },
                ColorProperty2 c => new ColorBlock
                {
                    Id       = ID.Get(),
                    Name     = c.Name,
                    Metadata = c.Metadata,
                    Fallback = c.Fallback,
                    ToolTips = c.ToolTips,
                },
                GroupProperty2 g => Upgrade(g),
                OptionProperty2 o => Upgrade(o),
                _ => null,
            };
        }

        public static ModuleTemplate Upgrade(Module oldTemplate)
        {
            if(oldTemplate is null)
            {
                return null;
            }

            var template = new ModuleTemplate
            {
                Id            = oldTemplate.Id,
                Name          = oldTemplate.Name,
                AuthorList    = oldTemplate.Author,
                ContractList  = oldTemplate.Contract,
                Organizations = oldTemplate.Organization,
                Version       = 1,
                Intro         = oldTemplate.Name,
                MetadataList  = new List<MetadataCache>(),
                Blocks        = new List<ModuleBlock>(oldTemplate.Items.Count),
                ForType       = GetType(oldTemplate.Type)
            };

            var metadatas = new Dictionary<string, MetadataCache>();

            foreach (var property in oldTemplate.Items)
            {
                if (!string.IsNullOrEmpty(property.Metadata) &&
                    !metadatas.ContainsKey(property.Metadata))
                {
                    var meta = new MetadataCache
                    {
                        Id           = ID.Get(),
                        Name         = property.Name,
                        MetadataName = property.Metadata,
                        RefCount     = 1
                    };
                    metadatas.Add(property.Metadata, meta);
                    template.MetadataList.Add(meta);
                }

                var block = Upgrade(property);
                
                if (block is null)
                {
                    continue; 
                }
                
                template.Blocks.Add(block);
            }

            return template;
        }
    }
}