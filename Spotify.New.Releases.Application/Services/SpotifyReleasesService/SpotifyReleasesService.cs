using Microsoft.Extensions.Logging;
using Spotify.New.Releases.Domain.Enums;
using Spotify.New.Releases.Domain.Models.Spotify;
using Spotify.New.Releases.Infrastructure.Repositories;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Spotify.New.Releases.Application.Services.SpotifyReleasesService
{
    public class SpotifyReleasesService : ISpotifyReleasesService
    {
        private readonly HttpClient HttpClient;
        private readonly IGenericRepository<Item> _albumsRepository;
        private readonly ILogger<SpotifyReleasesService> _logger;

        public SpotifyReleasesService(IGenericRepository<Item> albumsRepository, ILogger<SpotifyReleasesService> logger)
        {
            this.HttpClient = new HttpClient();
            this._albumsRepository = albumsRepository;
            this._logger = logger;
        }

        public async Task<Item> GetLatestRelease()
        {
            List<Item> latestReleases = await this.GetLatestReleases(1);
            Item latestRelease = latestReleases.FirstOrDefault();
            return latestRelease ?? null;
        }

        public async Task<List<Item>> GetLatestReleases(uint releasesNumber = 50)
        {
            SpotifyToken token = await this.GetSpotifyToken();
            List<Item> allReleases = new List<Item>();
            try
            {
                foreach (string country in countries)
                {
                    List<Item> receivedReleases = await this.GetLastReleasesByCountry(country, token, releasesNumber);
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

        private async Task<SpotifyToken> GetSpotifyToken()
        {
            string clientId = "";
            string clientSecret = "";
            string accessUrl = "https://accounts.spotify.com/api/token";
            this.ConfigureHttpClientResponseType();
            this.ConfigureHttpClientAuthorization(AuthentificationScheme.Basic, this.GetCredentials(clientId, clientSecret));

            FormUrlEncodedContent requestBody = this.GetRequestBody();
            //Request Token
            var request = await this.HttpClient.PostAsync(accessUrl, requestBody);
            var response = await request.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SpotifyToken>(response);
        }

        private string GetCredentials(string clientId, string clientSecret)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", clientId, clientSecret)));
        }

        private void ConfigureHttpClientResponseType()
        {
            this.HttpClient.DefaultRequestHeaders.Accept.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void ConfigureHttpClientAuthorization( AuthentificationScheme scheme, string parameter )
        {
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme.ToString(), parameter);
        }

        private async Task<List<Item>> GetLastReleasesByCountry(string country, SpotifyToken token, uint limit)
        {
            Uri path = new Uri($"https://api.spotify.com/v1/browse/new-releases?country={country}&limit={(int)limit}");


            this.ConfigureHttpClientResponseType();
            this.ConfigureHttpClientAuthorization(AuthentificationScheme.Bearer, token.access_token);

            //pour une recherche d'album, il y a ça :
            //https://api.spotify.com/v1/search?q=album%3ARendezvous%20artist%3AJenevieve&type=album&limit=50
            //album:Rendezvous artist:Jenevieve
            //type:album (le type de media qu'il faut sortir, on peut remplacer par artist par exemple

            HttpResponseMessage response = await this.HttpClient.GetAsync(path);
            string responseContent = await response.Content.ReadAsStringAsync();

            SpotifyReleases? deserializedJson = JsonSerializer.Deserialize<SpotifyReleases>(responseContent);
            return deserializedJson?.albums?.items;
        }

        public async Task AddRelease(Item release)
        {
            await this._albumsRepository.AddAsync(release);
            _logger.LogInformation("{datetime} - {service} added a new release to the database : {id}",
                DateTimeOffset.Now,
                nameof(SpotifyReleasesService),
                release.id);
        }

        private FormUrlEncodedContent GetRequestBody()
        {
            List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
            requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            return new FormUrlEncodedContent(requestData);
        }

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
