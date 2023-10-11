// See https://aka.ms/new-console-template for more information

using ConsoleApp;

while (true)
{
    var str = Console.ReadLine();
    if (File.Exists(str))
    {
        var fi = new FileInfo(str);
        Console.WriteLine(ByteUtilities.GetFileSizeString(fi.Length));
    }
}