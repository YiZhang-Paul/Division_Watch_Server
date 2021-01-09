using Core.Configurations;
using Core.Models;
using Microsoft.Extensions.Options;

namespace Service.Repositories
{
    public class CategoryRepository : DatabaseConnector<Category>
    {
        public CategoryRepository(IOptions<DatabaseConfiguration> configuration) : base(configuration, "Category") { }
    }
}
