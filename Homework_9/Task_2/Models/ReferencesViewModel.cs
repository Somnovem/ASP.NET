namespace Task_2.Models
{
    public class ReferencesViewModel
    {
        public DNDShow[] DNDShows { get; set; }
    }
    public record DNDShow(string Name, string Description, string WikiLink);
}
