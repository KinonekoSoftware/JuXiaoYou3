namespace Acorisoft.FutureGL.MigaStudio
{
    partial class SubSystemString
    {
        public static string ModuleFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "Module File|*.png",
                CultureArea.French   => "Fichier de module|*.png",
                CultureArea.Japanese => "モジュールファイル|*.png",
                CultureArea.Korean   => "Module 파일|*.png",
                CultureArea.Russian  => "Файл модуля|*.png",
                _                    => "模组文件|*.png",
            };
        }
        
        public static string JsonFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "Json File|*.json",
                CultureArea.French   => "Fichier Json|*.json",
                CultureArea.Japanese => "Jsonファイル|*.json",
                CultureArea.Korean   => "Json 파일|*.json",
                CultureArea.Russian  => "Файл Json|*.json",
                _                    => "模板数据|*.json",
            };
        }
        
        public static string AllFileFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "All File|*.*",
                CultureArea.French   => "Tous les fichiers|*.*",
                CultureArea.Japanese => "すべてのファイル|*.*",
                CultureArea.Korean   => "모든 파일|*.*",
                CultureArea.Russian  => "Все файлы|*.*",
                _                    => "所有文件|*.*",
            };
        }
        
        
        public static string PdfFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "PDF File|*.pdf",
                CultureArea.French   => "Fichier PDF|*.pdf",
                CultureArea.Japanese => "PDFファイルはこちら|*.pdf",
                CultureArea.Korean   => "PDF 파일|*.pdf",
                CultureArea.Russian  => "файл Pdf|*.pdf",
                _                    => "PDF文件|*.pdf",
            };
        }
        public static string MarkdownFilter
        {
            get => Language.Culture switch
            {
                CultureArea.Chinese  => "Markdown文件|*.md",
                CultureArea.French   => "Fichier Markdown|*.md",
                CultureArea.Japanese => "マークダウンファイル|*.md",
                CultureArea.Korean   => "Markdown 파일|*.md",
                CultureArea.Russian  => "Файл Markdown|*.md",
                _                    => "Markdown File|*.md",
            };
        }
        
        public static string MusicFilter
        {
            get => Language.Culture switch
            {
                CultureArea.Chinese  => "Music File|*.wav;*.mp3",
                CultureArea.French   => "Fichier de musique|*.wav;*.mp3",
                CultureArea.Japanese => "音楽ファイル|*.wav;*.mp3",
                CultureArea.Korean   => "Music 파일|*.wav;*.mp3",
                CultureArea.Russian  => "Музыкальный файл|*.pdf",
                _                    => "音乐文件|*.wav;*.mp3",
            };
        }

        public static string ImageFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.French   => "Fichier image|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "画像ファイル|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean   => "Image 파일|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian  => "Файл изображения|*.png;*.jpg;*.bmp;*.jpeg",
                _                    => "图片文件|*.png;*.jpg;*.bmp;*.jpeg",
            };
        }
        
        public static string AudioFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "Audio File|*.mp3;*.wma;*.wav;*.m4a",
                CultureArea.French   => "Fichier audio|*.mp3;*.wma;*.wav;*.m4a",
                CultureArea.Japanese => "オーディオファイル|*.mp3;*.wma;*.wav;*.m4a",
                CultureArea.Korean   => "Audio 파일|*.mp3;*.wma;*.wav;*.m4a",
                CultureArea.Russian  => "Аудио файл|*.mp3;*.wma;*.wav;*.m4a",
                _                    => "音频文件|*.mp3;*.wma;*.wav;*.m4a",
            };
        }
        
        public static string VideoFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "Video File|*.mp4;*.avi;*.mkv;*.wmv;*.mpeg;*.mpg",
                CultureArea.French   => "Fichier vidéo|*.mp4;*.avi;*.mkv;*.wmv;*.mpeg;*.mpg",
                CultureArea.Japanese => "動画ファイル|*.mp4;*.avi;*.mkv;*.wmv;*.mpeg;*.mpg",
                CultureArea.Korean   => "Image 파일|*.mp4;*.avi;*.mkv;*.wmv;*.mpeg;*.mpg",
                CultureArea.Russian  => "Видеофайл|*.mp4;*.avi;*.mkv;*.wmv;*.mpeg;*.mpg",
                _                    => "视频文件|*.mp4;*.avi;*.mkv;*.wmv;*.mpeg;*.mpg",
            };
        }
        
        public static string PngFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "PNG File|*.png",
                CultureArea.French   => "Fichier PNG|*.png",
                CultureArea.Japanese => "PNG ファイル|*.png",
                CultureArea.Korean   => "PNG 파일|*.png",
                CultureArea.Russian  => "Файл PNG|*.png",
                _                    => "图片文件|*.png",
            };
        }
    }
}