using MongoDB.Driver;
using Spotify.New.Releases.Domain.Models.Spotify;

namespace Spotify.New.Releases.Infrastructure.Repositories
{
    public class AlbumsMongoRepository : IAlbumsRepository
    {
        private IMongoCollection<Item> _rawCollection { get; set; }

        public AlbumsMongoRepository(IMongoClient client)
        {
            var database = client.GetDatabase("releases");
            this._rawCollection = database.GetCollection<Item>("raw");
        }


        public async Task AddAsync(Item entity)
        {
            await this._rawCollection.InsertOneAsync(entity);
        }

        public Task DeleteAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        public async Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Item> GetByIdAsync(string id)
        {
            var filter = Builders<Item>.Filter.Eq("id", id);
            return await _rawCollection.Find(filter).FirstOrDefaultAsync();
        }

        public Task<Item> GetLatestRelease()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Item entity)
        {
            throw new NotImplementedException();

        }
    }
}
