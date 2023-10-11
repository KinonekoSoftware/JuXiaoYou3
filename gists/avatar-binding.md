# Xaml 部分

``` xaml
<Border.Background>
     <ImageBrush>
            <ImageBrush.ImageSource>
                <MultiBinding Converter="{x:Static thisConverters:Converters.Avatar}">
                    <Binding Path="Avatar" Mode="OneWay" />
                    <Binding Path="Type" />
            </MultiBinding>
        </ImageBrush.ImageSource>
    </ImageBrush>
</Border.Background>
```

# 转换器声明

``` xaml            
 xmlns:thisConverters="clr-namespace:Acorisoft.FutureGL.MigaStudio.Resources.Converters"
 <thisConverters:AvatarConverter x:Key="AvatarConverter" />
```

# 编辑头像

``` C#

        /// <summary>
        /// 
        /// </summary>
        private async Task ChangeAvatarImpl()
        {
            var r = await ImageUtilities.Avatar();
            
            if (!r.IsFinished)
            {
                return;
            }
            
            if (!r.IsFinished)
            {
                return;
            }

            var    buffer = r.Buffer;
            var    raw    = await Pool.MD5.ComputeHashAsync(buffer);
            var    md5    = Convert.ToBase64String(raw);
            string avatar;

            if (ImageEngine.HasFile(md5))
            {
                var fr = ImageEngine.Records.FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = $"avatar_{ID.Get()}.png";
                buffer.Seek(0, SeekOrigin.Begin);
                ImageEngine.SetAvatar(buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Type = ResourceType.Image
                };

                ImageEngine.AddFile(record);
            }

            Avatar = avatar;
        }
```