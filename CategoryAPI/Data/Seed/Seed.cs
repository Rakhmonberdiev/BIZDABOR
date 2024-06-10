using CategoryAPI.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Text;

namespace CategoryAPI.Data.Seed
{
    public class Seed
    {
        public static async Task SeedData(CategoryDb db)
        {
            if (db.Categories.Any())
            {
                return;
            }
            var data = await File.ReadAllTextAsync("Data/Seed/category.json", Encoding.UTF8);
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            var categories = JsonSerializer.Deserialize<List<Category>>(data, option);
            foreach (var category in categories)
            {
                await db.Categories.AddAsync(category);
            }
            await db.SaveChangesAsync();
        }
    }
}
