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
                string x_okapi_key = Environment.ExpandEnvironmentVariables("%X_OKAPI_KEY%");
                Rest r = new Rest(x_okapi_key);
                var adr = "11 chemin des crieurs";
                string ueadr = System.Net.WebUtility.UrlEncode(adr);
                dynamic adresses = r.GetAsync<JObject>("https://api.laposte.fr/controladresse/v1/adresses?q=" + ueadr).GetAwaiter().GetResult();
                foreach (dynamic item in adresses)
                {
                    string code = item.Value.code;
                    string adresse = item.Value.adresse;
                    dynamic detail = r.GetAsync<JObject>("https://api.laposte.fr/controladresse/v1/adresses/" + code).GetAwaiter().GetResult();
                    Console.WriteLine(detail);
                    // Console.WriteLine(detail.blocAdresse);
                }

                /*
                 * {
  "destinataire": "",
  "pointRemise": "",
  "numeroVoie": "11",
  "libelleVoie": "CHEMIN DES CRIEURS",
  "lieuDit": "",
  "codePostal": "59650",
  "codeCedex": "",
  "commune": "VILLENEUVE D ASCQ",
  "blocAdresse": [
    "11 CHEMIN DES CRIEURS",
    "59650 VILLENEUVE D ASCQ"
  ]
}
                 */
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
