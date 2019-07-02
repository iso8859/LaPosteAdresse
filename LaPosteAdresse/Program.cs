using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace ovh.api
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Rest r = new Rest("/GxhiviTj1kZaVtsdAC7zQlXLpo5/W2dQtAI92CvqjFFVrOGJLmx73vqzLlDn2Vp");
                var adr = "11 chemin des crieurs";
                string ueadr = System.Net.WebUtility.UrlEncode(adr);
                dynamic adresses = r.GetAsync<JObject>("https://api.laposte.fr/controladresse/v1/adresses?q=" + ueadr).GetAwaiter().GetResult();
                foreach (dynamic item in adresses)
                {
                    string code = item.Value.code;
                    string adresse = item.Value.adresse;
                    dynamic detail = r.GetAsync<JObject>("https://api.laposte.fr/controladresse/v1/adresses/" + code).GetAwaiter().GetResult();
                    Console.WriteLine(detail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
