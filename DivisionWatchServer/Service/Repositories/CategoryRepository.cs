using Core.Configurations;
using Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class CategoryRepository : DatabaseConnector<Category>
    {
        public CategoryRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "Category") { }

        public async Task<Category> GetCategoryByName(string name)
        {
            var regex = new BsonRegularExpression($"/^{(name ?? string.Empty).Trim()}$/i");
            var filter = Builders<Category>.Filter.Regex(_ => _.Name, regex);

            return await Collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
