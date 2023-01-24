using Spotify.New.Releases.Domain.Exceptions;
using Spotify.New.Releases.Domain.Models.Spotify;
using StackExchange.Redis;
using System.Text.Json;

namespace Spotify.New.Releases.Infrastructure.Repositories
{
    public class AlbumsRepository : IAlbumsRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        public AlbumsRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            this._database = this._connectionMultiplexer.GetDatabase();
        }
        public async Task AddAsync(Item entity)
        {
            try
            {
                string value = JsonSerializer.Serialize(entity);
                bool result = await this._database.StringSetAsync(entity.id, value);
                if (result == false)
                {
                    throw new SnrBaseException(nameof(AlbumsRepository));
                }
            }
            catch (Exception exception)
            {
                throw new SnrBaseException(nameof(AlbumsRepository), exception);
            }
        }

        public async Task DeleteAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        public async Task GetAllAsync()
        {
            RedisResult result = await this._database.ExecuteAsync("scan", "0 COUNT 100");
            RedisResult values = result.ToDictionary().FirstOrDefault().Value;
            string values2 = values.ToDictionary().FirstOrDefault().Key;
            Item item = await this.GetByIdAsync(values2);
            Console.WriteLine("coucou");
            //this._database.ListRangeAsync()
        }

        public async Task<Item> GetByIdAsync(string id)
        {
            try
            {
                RedisValue result = await this._database.StringGetAsync(id);
                if (!result.HasValue) return null;
                if (result.HasValue) return this.Convert(result);
                throw new SnrBaseException(nameof(AlbumsRepository));
            }
            catch (Exception exception)
            {
                throw new SnrBaseException(nameof(AlbumsRepository), exception);
            }
        }

        public async Task<Item> GetLatestRelease()
        {
            try
            {
                await this.GetAllAsync();
                RedisValue result = new RedisValue();
                if (!result.HasValue) return null;
                if (result.HasValue) return this.Convert(result);
                throw new SnrBaseException(nameof(AlbumsRepository));
            }
            catch (Exception exception)
            {
                throw new SnrBaseException(nameof(AlbumsRepository), exception);
            }
        }

        public async Task UpdateAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        private Item Convert(string stringifiedJson)
        {
            if (String.IsNullOrWhiteSpace(stringifiedJson)) throw new SnrBaseException(nameof(AlbumsRepository));
            try
            {
                return JsonSerializer.Deserialize<Item>(stringifiedJson);
            }
            catch (Exception exception)
            {
                throw new SnrBaseException(nameof(AlbumsRepository), exception);
            }
        }
    }
}
