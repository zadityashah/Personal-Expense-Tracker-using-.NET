namespace PersonalExpenseTracker.DataModel.Model
{
    public class Tag
    {
        public Guid TagID { get; set; }
        public string TagName { get; set; }

        public Tag(string tagName)
        {
            TagID = Guid.NewGuid();
            TagName = tagName;
        }
    }
}