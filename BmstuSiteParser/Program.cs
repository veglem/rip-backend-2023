using System.Text;
using BmstuSiteParser;
using HtmlAgilityPack;
using Newtonsoft.Json;

HttpClient client = new HttpClient();

var response = await client.GetAsync("http://wwv.bmstu.ru/sveden/struct/");

var str = response.Content.ReadAsStream();

HtmlDocument doc = new HtmlDocument();

doc.Load(str);

List<BmstuStruct> bmstuStructs = new List<BmstuStruct>();

foreach (var node in doc.DocumentNode.SelectNodes(
             "//table[@itemprop='structOrgUprav']/tbody/tr"))
{
    var x = node.SelectNodes("./td[@class='subtitle']");
    if (node.SelectNodes("./td[@class='subtitle']") is not null)
    {
        bmstuStructs.Add(new BmstuStruct()
        {
            StructName = node.SelectNodes("./td[@class='subtitle']")[0].InnerText,
            Persons = new List<Person>()
        });
    }

    if (node.SelectNodes("./td[@itemprop='name']") is not null)
    {
        string division =
            node.SelectNodes("./td[@itemprop='name']")[0].InnerText == "&nbsp;"
                ? ""
                : node.SelectNodes("./td[@itemprop='name']")[0].InnerText;
        string fio = 
            node.SelectNodes("./td[@itemprop='fio']")[0].InnerText == "&nbsp;"
                ? ""
                : node.SelectNodes("./td[@itemprop='fio']")[0].InnerText;
        string position = 
            node.SelectNodes("./td[@itemprop='post']")[0].InnerText == "&nbsp;"
                ? ""
                : node.SelectNodes("./td[@itemprop='post']")[0].InnerText;
        string number = 
            node.ChildNodes[7].InnerText == "&nbsp;"
                ? ""
                : node.ChildNodes[7].InnerText;
        string email = 
            node.ChildNodes[9].InnerText == "&nbsp;"
                ? ""
                : node.ChildNodes[9].InnerText;
        string cabint = 
            node.ChildNodes[11].InnerText == "&nbsp;"
                ? ""
                : node.ChildNodes[11].InnerText;
        string location = 
            node.ChildNodes[13].InnerText == "&nbsp;"
                ? ""
                : node.ChildNodes[13].InnerText;

        division = division.Replace("&quot;", "");
        fio = fio.Replace("&quot;", "");
        position = position.Replace("&quot;", "");
        number = number.Replace("&quot;", "");
        email = email.Replace("&quot;", "");
        cabint = cabint.Replace("&quot;", "");
        location = location.Replace("&quot;", "");
        
        bmstuStructs[^1].Persons.Add(new Person()
        {
            Division = division,
            FIO = fio,
            Position = position,
            Number = number,
            Email = email,
            Cabinet = cabint,
            Location = location
        });
    }
    
    
}


await using FileStream fs = new FileStream("db.json", FileMode.OpenOrCreate);
string jsonStr = JsonConvert.SerializeObject(bmstuStructs);
byte[] buffer = Encoding.Default.GetBytes(jsonStr);
await fs.WriteAsync(buffer, 0, buffer.Length);

Console.Write(str);
