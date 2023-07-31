namespace Task_2.Models
{
	public class EditionsViewModel
	{
		public Edition[] Editions { get; set; } = null;
		public record Edition(string Name, string Description);
	}
}
