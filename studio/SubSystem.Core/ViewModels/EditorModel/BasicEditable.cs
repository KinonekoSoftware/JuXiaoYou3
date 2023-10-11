using System.Runtime.CompilerServices;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using static Acorisoft.FutureGL.MigaStudio.Utilities.MetadataUtilities;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class BasicEditable : HierarchicalViewModel
    {
 
        
        public string GetOrAddMetadata(string name)
        {
            var dict = BasicPart.Buckets;
            if (dict.TryGetValue(name, out var value))
            {
                return value;
            }

            if (MetadataTrackerByName.TryGetValue(name, out var metadata))
            {
                if (!string.IsNullOrEmpty(metadata.Value))
                {
                    BasicPart.Buckets.TryAdd(name, metadata.Value);
                    return metadata.Value;
                }
            }

            value = string.Empty;
            dict.TryAdd(name, value);
            return value;
        }

        public bool GetOrAddBoolMetadata(string name)
        {
            var v = GetOrAddMetadata(name);
            return bool.TryParse(v, out var n) && n;
        }

        public string GetOrAddMetadata(string name, string defaultValue)
        {
            var dict = BasicPart.Buckets;
            if (dict.TryGetValue(name, out var value))
            {
                return value;
            }

            value = defaultValue;
            dict.TryAdd(name, value);
            return value;
        }

        public void UpsertMetadata(string name, string value, [CallerMemberName] string propName = "")
        {
            var dict = BasicPart.Buckets;
            dict[name] = value;

            AddMetadata(new Metadata
            {
                Name  = name,
                Value = value,
                Type  = MetadataKind.Text,
            });

            RaiseUpdated(propName);
            SetDirtyState();
        }

        public string Rarity
        {
            get => GetOrAddMetadata(MetaNameOfRarity);
            set
            {
                UpsertMetadata(MetaNameOfRarity, value);
            }
        }
        
        public string Price
        {
            get => GetOrAddMetadata(MetaNameOfPrice);
            set
            {
                UpsertMetadata(MetaNameOfPrice, value);
            }
        }
        
        public string ProduceArea
        {
            get => GetOrAddMetadata(MetaNameOfProduceArea);
            set
            {
                UpsertMetadata(MetaNameOfProduceArea, value);
            }
        }
        

        public string NickName
        {
            get => GetOrAddMetadata(MetaNameOfNickName);
            set
            {
                UpsertMetadata(MetaNameOfNickName, value);
            }
        }


        public string Age
        {
            get => GetOrAddMetadata(MetaNameOfAge);
            set
            {
                UpsertMetadata(MetaNameOfAge, value);
            }
        }


        public string Race
        {
            get => GetOrAddMetadata(MetaNameOfRace);
            set
            {
                UpsertMetadata(MetaNameOfRace, value);
            }
        }

        public string Country
        {
            get => GetOrAddMetadata(MetaNameOfCountry);
            set
            {
                UpsertMetadata(MetaNameOfCountry, value);
            }
        }

        public string Weight
        {
            get => GetOrAddMetadata(MetaNameOfWeight);
            set
            {
                UpsertMetadata(MetaNameOfWeight, value);
            }
        }

        public bool IsDeath
        {
            get => GetOrAddBoolMetadata(MetaNameOfIsDeath);
            set
            {
                UpsertMetadata(MetaNameOfIsDeath, value.ToString());
            }
        }
        
        public string Height
        {
            get => GetOrAddMetadata(MetaNameOfHeight);
            set
            {
                UpsertMetadata(MetaNameOfHeight, value);
            }
        }


        public string Birth
        {
            get => GetOrAddMetadata(MetaNameOfBirth);
            set
            {
                UpsertMetadata(MetaNameOfBirth, value);
            }
        }


        public string Gender
        {
            get => GetOrAddMetadata(MetaNameOfGender);
            set => UpsertMetadata(MetaNameOfGender, value);
        }
        
        public string Intro
        {
            get => GetOrAddMetadata(MetaNameOfIntro);
            set
            {
                UpsertMetadata(MetaNameOfIntro, value);
                Document.Intro = value;
                Cache.Intro    = value;
            }
        }

        public string Weakness
        {
            get => GetOrAddMetadata(MetaNameOfWeakness);
            set
            {
                UpsertMetadata(MetaNameOfWeakness, value);
            }
        }

        public bool IsMainPictureOfSquareEmpty => string.IsNullOrEmpty(MainPictureOfSquare);
        public bool IsMainPictureOfVerticalEmpty => string.IsNullOrEmpty(MainPictureOfVertical);
        public bool IsMainPictureOfHorizontalEmpty => string.IsNullOrEmpty(MainPictureOfHorizontal);
        public bool IsMainPictureEmpty => string.IsNullOrEmpty(MainPicture);
        
        public string MainPicture
        {
            get => GetOrAddMetadata(MetaNameOfMainPicture);
            set
            {
                UpsertMetadata(MetaNameOfMainPicture, value);
                RaiseUpdated(nameof(IsMainPictureEmpty));
            }
        }
        
        public string MainPictureOfSquare
        {
            get => GetOrAddMetadata(MetaNameOfMainPictureOf4x3);
            set
            {
                UpsertMetadata(MetaNameOfMainPictureOf4x3, value);
                RaiseUpdated(nameof(IsMainPictureOfSquareEmpty));
            }
        }
        
        public string MainPictureOfVertical
        {
            get => GetOrAddMetadata(MetaNameOfMainPictureOf9x16);
            set
            {
                UpsertMetadata(MetaNameOfMainPictureOf9x16, value);
                RaiseUpdated(nameof(IsMainPictureOfVerticalEmpty));
            }
        }
        
        public string MainPictureOfHorizontal
        {
            get => GetOrAddMetadata(MetaNameOfMainPictureOf16x9);
            set
            {
                UpsertMetadata(MetaNameOfMainPictureOf16x9, value);
                RaiseUpdated(nameof(IsMainPictureOfHorizontalEmpty));
            }
        }


        public string Avatar
        {
            get => GetOrAddMetadata(MetaNameOfAvatar);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                Document.Avatar = value;
                Cache.Avatar    = value;
                UpsertMetadata(MetaNameOfAvatar, value);
            }
        }

        public string Name
        {
            get => GetOrAddMetadata(MetaNameOfName, Language.GetText("global.unNamed"));
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                Document.Name = value;
                Cache.Name    = value;
                UpsertMetadata(MetaNameOfName, value);
            }
        }

        public PartOfBasic BasicPart { get; protected set; }
    }
}