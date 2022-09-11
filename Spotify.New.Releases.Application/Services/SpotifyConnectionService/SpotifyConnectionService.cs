using Spotify.New.Releases.Domain.Models.Spotify;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify.New.Releases.Application.Services.SpotifyConnectionService
{
    public class SpotifyConnectionService : ISpotifyConnectionService
    {
        private readonly HttpClient HttpClient;

        public SpotifyConnectionService()
        {
            this.HttpClient = new HttpClient();
        }

        public void HelloWorld()
        {
            Console.WriteLine("coucou");
        }

        public async Task Connection()
        {
            List<Item> allReleases = new List<Item>();
            foreach(string country in JustSomeCountries)
            {
                //la limite est aux 50 dernières albums
                Uri path = new Uri($"https://api.spotify.com/v1/browse/new-releases?country={country}&limit=50");
                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path);

                string token = "BQDEBn9-JXk8wwHp50ChgTxhl7D7rLs8sChD4LvSbdVA4RMVTTBF8oRmHPyDWVn3o7Tg40bn9XF0OSPfgqYyr43fRJ-ZpBF42SYT3x26WcEgaoiZBvyDlm5a--ilpBTJNiVto14un6kj-h5UfvU75g6QTTJzdp2G7YWfil3l1BC7iw";


                this.HttpClient.DefaultRequestHeaders.Accept.Clear();
                this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //pour une recherche d'album, il y a ça :
                //https://api.spotify.com/v1/search?q=album%3ARendezvous%20artist%3AJenevieve&type=album&limit=50
                //album:Rendezvous artist:Jenevieve
                //type:album (le type de media qu'il faut sortir, on peut remplacer par artist par exemple

                HttpResponseMessage response = await this.HttpClient.GetAsync(path);



                string responseContent = await response.Content.ReadAsStringAsync();

                SpotifyReleases? deserializedJson = JsonSerializer.Deserialize<SpotifyReleases>(responseContent);
                allReleases.AddRange(deserializedJson?.albums?.items);
            }
            //all the albums have to be different !
            List<Item> filtered = allReleases.DistinctBy(release => release.href).ToList();
        }

        public List<string> JustSomeCountries = new List<string>()
        {
            "BE",
            "FR",
            "US",
            "JP",
            "HK"
        };

        public List<string> countries = new List<string>() {           
          "AD",
          "AE",
          "AG",
          "AL",
          "AM",
          "AO",
          "AR",
          "AT",
          "AU",
          "AZ",
          "BA",
          "BB",
          "BD",
          "BE",
          "BF",
          "BG",
          "BH",
          "BI",
          "BJ",
          "BN",
          "BO",
          "BR",
          "BS",
          "BT",
          "BW",
          "BZ",
          "CA",
          "CD",
          "CG",
          "CH",
          "CI",
          "CL",
          "CM",
          "CO",
          "CR",
          "CV",
          "CW",
          "CY",
          "CZ",
          "DE",
          "DJ",
          "DK",
          "DM",
          "DO",
          "DZ",
          "EC",
          "EE",
          "EG",
          "ES",
          "FI",
          "FJ",
          "FM",
          "FR",
          "GA",
          "GB",
          "GD",
          "GE",
          "GH",
          "GM",
          "GN",
          "GQ",
          "GR",
          "GT",
          "GW",
          "GY",
          "HK",
          "HN",
          "HR",
          "HT",
          "HU",
          "ID",
          "IE",
          "IL",
          "IN",
          "IQ",
          "IS",
          "IT",
          "JM",
          "JO",
          "JP",
          "KE",
          "KG",
          "KH",
          "KI",
          "KM",
          "KN",
          "KR",
          "KW",
          "KZ",
          "LA",
          "LB",
          "LC",
          "LI",
          "LK",
          "LR",
          "LS",
          "LT",
          "LU",
          "LV",
          "LY",
          "MA",
          "MC",
          "MD",
          "ME",
          "MG",
          "MH",
          "MK",
          "ML",
          "MN",
          "MO",
          "MR",
          "MT",
          "MU",
          "MV",
          "MW",
          "MX",
          "MY",
          "MZ",
          "NA",
          "NE",
          "NG",
          "NI",
          "NL",
          "NO",
          "NP",
          "NR",
          "NZ",
          "OM",
          "PA",
          "PE",
          "PG",
          "PH",
          "PK",
          "PL",
          "PS",
          "PT",
          "PW",
          "PY",
          "QA",
          "RO",
          "RS",
          "RW",
          "SA",
          "SB",
          "SC",
          "SE",
          "SG",
          "SI",
          "SK",
          "SL",
          "SM",
          "SN",
          "SR",
          "ST",
          "SV",
          "SZ",
          "TD",
          "TG",
          "TH",
          "TJ",
          "TL",
          "TN",
          "TO",
          "TR",
          "TT",
          "TV",
          "TW",
          "TZ",
          "UA",
          "UG",
          "US",
          "UY",
          "UZ",
          "VC",
          "VE",
          "VN",
          "VU",
          "WS",
          "XK",
          "ZA",
          "ZM",
          "ZW"};
    }
}
