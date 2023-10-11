// See https://aka.ms/new-console-template for more information

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;
using Colorful;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using Console = Colorful.Console;


[assembly: AssemblyCompany("罗易斯（Luoyisi）")]
[assembly: AssemblyFileVersion("3.0.0.*")]
[assembly: AssemblyProduct("橘小柚")]
[assembly: AssemblyTitle("橘小柚")]
[assembly: AssemblyVersion("3.0.0.*")]
[assembly: AssemblyInformationalVersion("3.0.0-Preview7")]

namespace Acorisoft.FutureGL.MigaStudio.Tools.BugReporter
{
    public class Program
    {
        private static readonly Color PrimaryColor   = Color.FromArgb(0x98, 0xa1, 0x2b);
        private static readonly Color ObsoletedColor = Color.FromArgb(0xff, 0x66, 0x00);
        private static readonly Color WarningColor   = Color.FromArgb(0xff, 0xb3, 0x14);
        private static readonly Color DangerColor    = Color.FromArgb(0xd9, 0x08, 0x0c);

        private static bool FormatArgs(string[] args, out BugLevel level, out string dir, out string log)
        {
            if (args is null ||
                args.Length < 3)
            {
                level = BugLevel.Bug;
                dir   = string.Empty;
                log   = string.Empty;
                return false;
            }

            if (!int.TryParse(args[1], out var n))
            {
                level = BugLevel.Bug;
                dir   = string.Empty;
                log   = string.Empty;
                return false;
            }

            level = (BugLevel)n;


            if (string.IsNullOrEmpty(args[0]) ||
                string.IsNullOrEmpty(args[2]))
            {
                level = BugLevel.Bug;
                dir   = string.Empty;
                log   = string.Empty;
                return false;
            }

            dir = args[0].Trim();
            log = args[2].Trim();
            return true;
        }

        static void Main(string[] args)
        {
            var formatter = new Formatter[]
            {
                new Formatter("橘小柚-修复工具", PrimaryColor),
                new Formatter("任意键", Color.Green),
                new Formatter("橘小柚", PrimaryColor)
            };

            //-------------------------------------
            // 打印开头
            //-------------------------------------
            Console.WriteAscii("JuXiaoYou", PrimaryColor);
            Console.WriteLineFormatted(
                "欢迎使用{0},此工具可以帮助您反馈BUG！",
                Color.LightSlateGray,
                formatter);

            WriteEmptyLine();

            //-------------------------------------
            // 检测任务
            //-------------------------------------
            if (FormatArgs(args, out var bug, out var dir, out var log))
            {
                Console.WriteLineFormatted(
                    "此工具由{2}启动，正在执行自动化操作！\n这个过程比较耗费时间,请耐心等待！\n",
                    Color.LightSlateGray,
                    formatter);
                
                Console.WriteLineFormatted(
                    "正在等待应用程序退出，完成此项操作需要5秒，请耐心等待\n",
                    Color.LightSlateGray,
                    formatter);
                Thread.Sleep(5000);
                
                Report(bug, dir, log);
            }
            else
            {
                Console.WriteLineFormatted(
                    "此工具只支持由{2}启动!!",
                    Color.LightSlateGray,
                    formatter);
            }

            WriteEmptyLine();
            Console.WriteLineFormatted(
                "按下{1}即可退出",
                Color.LightSlateGray,
                formatter);
            Console.ReadLine();
        }

        private static void WriteEmptyLine()
        {
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void Report(BugLevel bug, string dir, string log)
        {
            var parent           = Path.GetDirectoryName(log);
            var feedback         = Path.Combine(parent, "Feedbacks");
            var crashes          = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crashes");
            var user             = Path.Combine(parent, "UserData");
            var settingFile      = Path.Combine(user, "juxiaoyou-main.json");
            var outputLogZipFile = Path.Combine(feedback, "日志.zip");
            var outputDbZipFile  = Path.Combine(feedback, "世界观.zip");
            var outputReadmeFile = Path.Combine(feedback, "[readme]看这里.txt");
            var setting          = JSON.OpenSetting<BasicAppSetting>(settingFile, () => new BasicAppSetting { Language = CultureArea.Chinese });

            if (!Directory.Exists(feedback))
            {
                Directory.CreateDirectory(feedback);
            }

            var formatter = new[]
            {
                GetBugFormatter(bug),
                new Formatter(dir, Color.IndianRed),
                new Formatter(log, Color.IndianRed),
                new Formatter(user, Color.IndianRed),
                new Formatter(outputLogZipFile, Color.Peru),
                new Formatter(outputDbZipFile, Color.Peru),
                new Formatter(settingFile, Color.Peru),
                new Formatter(BasicAppSetting.GetName(setting.Language), Color.Teal),
            };

            Console.WriteLineFormatted("BUG等级：{0}\n\n数据位置：{1}\n日志位置：{2}", Color.LightGray, formatter);

            if (bug == BugLevel.Bug)
            {
                Console.WriteLineFormatted("用户数据目录:{3}\n设置位置：{6}", Color.LightGray, formatter);
                Console.WriteLineFormatted("日志压缩包输出位置:{4}\n数据库压缩包输出位置：{5}\n语言:{7}\n", Color.LightGray, formatter);
                Pack(log, outputLogZipFile);
                Focus(crashes, feedback, outputReadmeFile, setting);
            }
            else if (bug == BugLevel.NotImplemented)
            {
                Console.WriteLineFormatted("用户数据目录:{3}\n设置位置：{6}", Color.LightGray, formatter);
                Console.WriteLineFormatted("日志压缩包输出位置:{4}\n数据库压缩包输出位置：{5}\n语言:{7}\n", Color.LightGray, formatter);
                Pack(log, outputLogZipFile);
                Pack(dir, outputDbZipFile);
                Focus(crashes, feedback, outputReadmeFile, setting);
            }
            else if (bug == BugLevel.No)
            {
                SetCrashCount(0, crashes);
            }
            else
            {
                Console.WriteLineFormatted("用户数据目录:{3}\n设置位置：{6}", Color.LightGray, formatter);
                Console.WriteLineFormatted("日志压缩包输出位置:{4}\n数据库压缩包输出位置：{5}\n语言:{7}\n", Color.LightGray, formatter);
                if (HandleCrashException(
                        user,
                        log,
                        dir,
                        crashes,
                        outputLogZipFile,
                        outputDbZipFile))
                {
                    Focus(crashes, feedback, outputReadmeFile, setting);
                }
            }
        }

        private static void Focus(string crashes, string feedback, string outputReadmeFile, BasicAppSetting basicAppSetting)
        {
            
            File.Copy(BasicAppSetting.GetFileName(crashes, basicAppSetting.Language), outputReadmeFile, true);

            Process.Start(new ProcessStartInfo
            {
                FileName  = "explorer.exe",
                Arguments = feedback
            });

            Process.Start(new ProcessStartInfo
            {
                FileName  = "explorer.exe",
                Arguments = outputReadmeFile
            });
        }
        
        private static int GetCrashCount(string crash)
        {
            try
            {
                var fileName = Path.Combine(crash, "bug.txt");

                if (File.Exists(fileName))
                {
                    var lines     = File.ReadAllLines(fileName);
                    var firstLine = lines.FirstOrDefault() ?? "0";
                    return int.TryParse(firstLine, out var n) ? n : 0;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private static void SetCrashCount(int count, string crash)
        {
            var fileName = Path.Combine(crash, "bug.txt");
            File.WriteAllText(fileName, count.ToString());
        }

        private static bool HandleCrashException(string user, string log, string dir, string crash, string outputLogZipFile, string outputDbZipFile)
        {
            var crashCount = GetCrashCount(crash);
            
            if (crashCount == 0)
            {
                //
                // 
                SetCrashCount(1, crash);
                Reboot();
                Console.WriteLine("这是第一次应用崩溃，我们将尝试重启一次应用，如果还有问题我们会有进一步提示！\n", ObsoletedColor);
                return false;
            }
            
            if (crashCount == 1)
            {
                Console.WriteLine("这是第二次应用崩溃，我们将重置设置，如果还有问题我们会有进一步提示！\n", WarningColor);
                //
                //
                var repo = Path.Combine(user, "repo.json");
                var advanced = Path.Combine(user, "advanced.json");
                var repoSetting = JSON.OpenSetting<RepositorySetting>(repo, () => new RepositorySetting
                {
                    Repositories = new ObservableCollection<RepositoryCache>()
                });
                var advancedSetting = JSON.OpenSetting<AdvancedSettingModel>(repo, () => new AdvancedSettingModel
                {
                    AutoSavePeriod = 5,
                    DebugMode = 2
                });
                
                repoSetting.Repositories.Clear();
                repoSetting.LastRepository     = null;
                advancedSetting.DebugMode      = 2;
                advancedSetting.AutoSavePeriod = 5;
                Console.WriteLine($"{DateTime.Now}\t正在重置世界观的启动设置！\n", Color.SlateBlue);
                Console.WriteLine($"{DateTime.Now}\t正在设置数据模式为：保护模式，保证数据不会崩坏！\n", Color.SlateBlue);
                JSON.WriteSetting(repo, repoSetting);
                JSON.WriteSetting(advanced, advancedSetting);
                Console.WriteLine($"{DateTime.Now}\t写入设置！\n", Color.SlateBlue);
                Console.WriteLine($"{DateTime.Now}\t写入设置完成！\n", Color.SlateBlue);
                Reboot();
                SetCrashCount(2, crash);
                return false;
            }
            
            Console.WriteLine("这是第三次应用崩溃，修复工具无法确定BUG的问题所在，请按照提示将BUG提交给开发者！\n", DangerColor);
            Pack(log, outputLogZipFile);
            Pack(dir, outputDbZipFile);
            SetCrashCount(3, crash);
            return true;
        }

        private static void Reboot()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "MigaStudio.exe"
            });
        }

        private static void Pack(string dir, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (string.IsNullOrEmpty(dir))
            {
                throw new ArgumentNullException(nameof(dir));
            }

            var zip = new FastZip();
            var formatter = new[]
            {
                new Formatter(DateTime.Now, Color.OliveDrab),
                new Formatter(fileName, Color.Peru),
            };
            zip.CreateZip(fileName, dir, true, "");
            Console.WriteLineFormatted("{0}\t压缩完毕！\n数据位置：{1}\n", Color.LightGray, formatter);
        }


        private static Formatter GetBugFormatter(BugLevel level)
        {
            return level switch
            {
                BugLevel.Bug            => new Formatter("普通", ObsoletedColor),
                BugLevel.NotImplemented => new Formatter("严重", WarningColor),
                _                       => new Formatter("危险", DangerColor),
            };
        }
    }
}