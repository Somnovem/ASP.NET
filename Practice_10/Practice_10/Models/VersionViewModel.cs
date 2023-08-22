namespace Practice_10.Models
{
    public class VersionViewModel
    {
        public string Version { get; set; }
        public List<Change> Changes { get; set; }
    }

    public class Change
    {
        public string Achievement { get; set; }
        public List<string> SubAchievements { get; set; }
    }
}
