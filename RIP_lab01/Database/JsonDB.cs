using System.Text;
using Newtonsoft.Json;


namespace RIP_lab01.Database;

// public class JsonDB
// {
//     public static async Task<List<UniversityDivisions>?> AsyncRead()
//     {
//         await using FileStream fs = new FileStream("db.json", FileMode.OpenOrCreate);
//         byte[] buffer = new byte[fs.Length];
//         var read = fs.Read(buffer, 0, (int)fs.Length);
//         string resStr = Encoding.Default.GetString(buffer);
//         List<UniversityDivisions>? res = JsonConvert.DeserializeObject<List<UniversityDivisions>>(resStr);
//         return res;
//     }
//
//     public static async void AsyncWrite(List<UniversityDivisions> divs)
//     {
//         await using FileStream fs = new FileStream("db.json", FileMode.OpenOrCreate);
//         string str = JsonConvert.SerializeObject(divs);
//         byte[] buffer = Encoding.Default.GetBytes(str);
//         await fs.WriteAsync(buffer, 0, buffer.Length);
//         
//     }
// }
