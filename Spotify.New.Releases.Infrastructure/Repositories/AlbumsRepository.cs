using Spotify.New.Releases.Domain.Models.Spotify;
using StackExchange.Redis;
using System.Text.Json;

namespace Spotify.New.Releases.Infrastructure.Repositories
{
    public class AlbumsRepository : IGenericRepository<Item>
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
            string value = JsonSerializer.Serialize(entity);
            bool result = await this._database.StringSetAsync(entity.id, value);
        }

        public async Task DeleteAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Item>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Item> GetByIdAsync(string id)
        {
            RedisValue result = await this._database.StringGetAsync(id);
            if (result.HasValue)
            {
                return JsonSerializer.Deserialize<Item>(result);
            }
            return null;
        }

        public async Task UpdateAsync(Item entity)
        {
            throw new NotImplementedException();
        }
    }
}
