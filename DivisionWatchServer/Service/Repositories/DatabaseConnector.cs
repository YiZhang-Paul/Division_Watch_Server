using Core.Configurations;
using Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class DatabaseConnector<T> where T : DatabaseRecord
    {
        protected IMongoCollection<T> Collection { get; set; }

        public DatabaseConnector(IOptions<DatabaseConfiguration> configuration, string collection)
        {
            Collection = Connect(configuration.Value, collection);
        }

        public async Task<IEnumerable<T>> Get(int limit = 0)
        {
            return await Collection.Find(new BsonDocument()).Limit(limit).ToListAsync().ConfigureAwait(false);
        }

        public async Task<T> Get(string id)
        {
            var filter = Builders<T>.Filter.Eq(_ => _.Id, id);

            return await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task Add(T document)
        {
            await Collection.InsertOneAsync(document).ConfigureAwait(false);
        }

        public async Task Replace(T document)
        {
            var filter = Builders<T>.Filter.Eq(_ => _.Id, document.Id);

            await Collection.ReplaceOneAsync(filter, document).ConfigureAwait(false);
        }

        public async Task ReplaceMany(IEnumerable<T> documents)
        {
            var filter = Builders<T>.Filter;
            var requests = documents.Select(_ => new ReplaceOneModel<T>(filter.Eq(entry => entry.Id, _.Id), _)).ToList();

            if (requests.Any())
            {
                await Collection.BulkWriteAsync(requests).ConfigureAwait(false);
            }
        }

        public async Task Delete(string id)
        {
            var filter = Builders<T>.Filter.Eq(_ => _.Id, id);

            await Collection.DeleteOneAsync(filter).ConfigureAwait(false);
        }

        private IMongoCollection<T> Connect(DatabaseConfiguration configuration, string collection)
        {
            var database = new MongoClient(configuration.Url).GetDatabase(configuration.Name);

            return database.GetCollection<T>(collection);
        }
    }
}
