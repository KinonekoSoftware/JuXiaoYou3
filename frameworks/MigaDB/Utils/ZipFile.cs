using Acorisoft.FutureGL.MigaUtils.Collections;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;

namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public class ZipFile
    {
        private const char ZipEntryFolderCharacter = '/';

        public static void AddFolder(ZipOutputStream zip, string pattern, string dir)
        {
            
        }

        public static void AddFile(ZipOutputStream zip, FileInfo fileName)
        {
            
        }

        private static string ProcessDirectory(string value, string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            
            var diffIndex = 0;
            var minLength = Math.Min(pattern.Length, value.Length);
            var sb        = Pool.GetStringBuilder();
            
            for(var i =0; i< minLength;i++)
            {
                diffIndex = i;
                if (pattern[i] != value[i])
                {
                    break;
                }
            }

            var span = value.AsSpan()[diffIndex..];
            sb.Append(span);
            sb.Append(ZipEntryFolderCharacter);

            var s = sb.ToString();
            Pool.ReturnStringBuilder(sb);
            return s;
        }

        public static Task Zip(string dir, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return Task.FromException(new ArgumentNullException(nameof(fileName)));
            }
            
            if (string.IsNullOrEmpty(dir))
            {
                return Task.FromException(new ArgumentNullException(nameof(dir)));
            }

            var queue = new Queue<string>();
            return Task.Run(() =>
            {
                var files = Directory.GetFiles(dir)
                                     .Select(x => new FileInfo(x))
                                     .ToArray();
                var       dirs = Directory.GetDirectories(dir);
                using var fs   = new FileStream(fileName, FileMode.Create);
                using var zip  = new ZipOutputStream(fs);
                var       crc  = new Crc32();

                string entryName;

                zip.SetLevel(3);
                if (files.Length != 0)
                {
                    foreach (var file in files)
                    {
                        if (!file.Exists)
                        {
                            continue;
                        }
                        var buffer = File.ReadAllBytes(file.FullName);
                        crc.Reset();
                        crc.Update(buffer);
                        entryName = Path.GetFileName(file.Name);
                        zip.PutNextEntry(new ZipEntry(entryName)
                        {
                            Crc      = crc.Value,
                            DateTime = file.LastWriteTime,
                        });

                        zip.Write(buffer);
                        zip.CloseEntry();
                    }
                }

                queue.AddMany(dirs);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    files = Directory.GetFiles(current)
                                     .Select(x => new FileInfo(x))
                                     .ToArray();
                    dirs = Directory.GetDirectories(current);
                    queue.AddMany(dirs);
                    zip.PutNextEntry(new ZipEntry(ProcessDirectory(current, dir)));
                    zip.CloseEntry();
                    
                    foreach (var file in files)
                    {
                        if (!file.Exists)
                        {
                            continue;
                        }

                        var buffer = File.ReadAllBytes(file.FullName);
                        crc.Reset();
                        crc.Update(buffer);
                        entryName = Path.GetFileName(file.Name);
                        zip.PutNextEntry(new ZipEntry(entryName)
                        {
                            Crc      = crc.Value,
                            DateTime = file.LastWriteTime,
                        });

                        zip.Write(buffer);
                        zip.CloseEntry();
                    }
                }
                
                zip.Close();
                zip.Flush();
                
            });
        }
    }
}