using Spotify.New.Releases.Domain.Models.Spotify;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.New.Releases.Infrastructure.Repositories
{
    public class AlbumsRepository : IGenericRepository<Item>
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public AlbumsRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task AddAsync(Item entity)
        {
            IDatabase db = this._connectionMultiplexer.GetDatabase();
            bool result = await db.StringSetAsync(entity.id, "coucou");
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
            IDatabase db = this._connectionMultiplexer.GetDatabase();
            var result = await db.StringGetAsync(id);
            return null;
        }

        public async Task UpdateAsync(Item entity)
        {
            throw new NotImplementedException();
        }
    }
}
