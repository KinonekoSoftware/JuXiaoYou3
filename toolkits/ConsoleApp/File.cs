using System.Net;
using System.Text;
using HtmlAgilityPack;

var http = new HttpClient(new HttpClientHandler
{
    AutomaticDecompression = DecompressionMethods.All
});

while (true)
{
    var link = Console.ReadLine();

    if (string.IsNullOrEmpty(link))
    {
        continue;
    }

    if (!Uri.TryCreate(link, UriKind.RelativeOrAbsolute,out var uri))
    {
        continue;
    }

    var text     = await http.GetStringAsync(link);
    var document = new HtmlDocument
    {
        OptionReadEncoding = true
    };
    document.LoadHtml(text);

    try
    {

        var titleNode = document.DocumentNode
                                .SelectNodes("//title")
                                .FirstOrDefault();

        var title = titleNode?.InnerText;
        Console.WriteLine(title);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}