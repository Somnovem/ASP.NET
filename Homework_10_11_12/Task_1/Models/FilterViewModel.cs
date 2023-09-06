using Microsoft.AspNetCore.Mvc.Rendering;

namespace Task_1.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(List<int> ratings, int rating, string name)
        {
            ratings.Insert(0, 0);
            Ratings = new SelectList(ratings, "Id", "Name", rating);
            SelectedRating = rating;
            SelectedName = name;
        }
        public SelectList Ratings { get; }
        public int SelectedRating { get; } 
        public string SelectedName { get; } 
    }
}
