using Discord;
using Spotify.New.Releases.Domain.Models.Spotify;
using Spotify.New.Releases.Infrastructure.Repositories;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Spotify.New.Releases.Application.Services.SpotifyConnectionService
{
    public class SpotifyConnectionService : ISpotifyConnectionService
    {
        private readonly HttpClient HttpClient;
        private readonly IGenericRepository<Item> _albumsRepository;

        public SpotifyConnectionService(IGenericRepository<Item> albumsRepository)
        {
            this.HttpClient = new HttpClient();
            this._albumsRepository = albumsRepository;
        }

        public async Task<EmbedBuilder> GetLatestRelease()
        {
            List<Item> latestReleases = await this.GetAllReleases(1);
            Item latestRelease = latestReleases.FirstOrDefault();
            if (latestRelease != null)
            {
                EmbedBuilder embedded = this.CreateEmbeddedRelease(latestRelease);

                return embedded;
            }
            return null;
        }

        public async Task<List<EmbedBuilder>> GetLatestReleases(uint releasesNumber)
        {
            List<Item> allReleases = await this.GetAllReleases(releasesNumber);
            List<EmbedBuilder> latestDiscordReleases = new List<EmbedBuilder>((int)releasesNumber);
            for (int i = 0; i <= releasesNumber; i++)
            {
                EmbedBuilder embeddedRelease = this.CreateEmbeddedRelease(allReleases.ElementAt(i));
                latestDiscordReleases.Add(embeddedRelease);
            }
            return latestDiscordReleases;
        }

        public EmbedBuilder CreateEmbeddedRelease(Item release)
        {
            return new EmbedBuilder()
                   .WithAuthor(release.artists.First().name)
                   .WithUrl(release.external_urls.spotify)
                   .WithColor(Discord.Color.DarkGreen)
                   .WithDescription($"type: {release.album_type}")
                   .WithTitle(release.name)
                   .WithThumbnailUrl(release.images.First().url)
                   .WithFooter(release.release_date);
        }

        private async Task<SpotifyToken> GetSpotifyToken()
        {
            var clientId = "";
            var clientSecret = "";
            var accessUrl = "https://accounts.spotify.com/api/token";
            string credentials = String.Format("{0}:{1}", clientId, clientSecret);

            //faire une policy pour gérer les client selon le get token ou selon le reste de spotify
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

                FormUrlEncodedContent requestBody = this.GetRequestBody();
                //Request Token
                var request = await client.PostAsync(accessUrl, requestBody);
                var response = await request.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SpotifyToken>(response);
            }
        }

        private async Task<List<Item>> GetLastReleasesByCountry(string country, SpotifyToken token, uint limit)
        {
            //la limite est aux 50 dernières albums
            if (limit > 50) limit = 50;
            Uri path = new Uri($"https://api.spotify.com/v1/browse/new-releases?country={country}&limit={(int)limit}");


            this.HttpClient.DefaultRequestHeaders.Accept.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);

            //pour une recherche d'album, il y a ça :
            //https://api.spotify.com/v1/search?q=album%3ARendezvous%20artist%3AJenevieve&type=album&limit=50
            //album:Rendezvous artist:Jenevieve
            //type:album (le type de media qu'il faut sortir, on peut remplacer par artist par exemple

            HttpResponseMessage response = await this.HttpClient.GetAsync(path);
            string responseContent = await response.Content.ReadAsStringAsync();

            SpotifyReleases? deserializedJson = JsonSerializer.Deserialize<SpotifyReleases>(responseContent);
            return deserializedJson?.albums?.items;
        }

        public async Task<List<Item>> GetAllReleases(uint limit)
        {
            SpotifyToken token = await this.GetSpotifyToken();
            List<Item> allReleases = new List<Item>();
            try
            {
                foreach (string country in countries)
                {
                    List<Item> receivedReleases = await this.GetLastReleasesByCountry(country, token, limit);
                    allReleases.AddRange(receivedReleases);
                }
            }
            catch (Exception exception)
            {
                //faire une exception perso
                throw;
            }
            return allReleases.DistinctBy(release => release.id).OrderBy(release => release.release_date).Reverse().ToList();
        }

        public async Task Add(Item release)
        {
            await this._albumsRepository.AddAsync(release);
            Console.WriteLine($"added release ${release.id}");
        }

        private FormUrlEncodedContent GetRequestBody()
        {
            List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
            requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            return new FormUrlEncodedContent(requestData);
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
