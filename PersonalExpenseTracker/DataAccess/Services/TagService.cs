using PersonalExpenseTracker.DataModel.Model;
using PersonalExpenseTracker.DataAccess.Services.Interface;
using System.Text.Json;


namespace PersonalExpenseTracker.DataAccess.Services
{
    public class TagService : ITagService
    {
        private readonly string filePath;

        public TagService()
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tags.json");
        }

        // Adds a tag to the list and saves to a JSON file
        public async Task AddTag(Tag tag)
        {
            var tags = await LoadTags();
            tags.Add(tag);
            await SaveTags(tags);
        }

        // Returns all tags from the JSON file
        public async Task<List<Tag>> GetAllTags()
        {
            var response = await LoadTags();

            var listTag = response.ToList();

            return listTag;
        }

        // Loads tags from the JSON file
        private async Task<List<Tag>> LoadTags()
        {
            if (!File.Exists(filePath))
            {
                return new List<Tag>();
            }

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Tag>>(json) ?? new List<Tag>();
        }

        // Saves tags to the JSON file
        private async Task SaveTags(List<Tag> tags)
        {
            var json = JsonSerializer.Serialize(tags, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}