using Task_1.Models;
namespace Task_1.ViewModels
{
    public class DetailsViewModel
    {
        public Product Product { get; }
        public IEnumerable<Review> Reviews { get; }
        public PageViewModel PageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public SortViewModel SortViewModel { get; }
        public DetailsViewModel(Product product,IEnumerable<Review> reviews, PageViewModel pageViewModel,
            FilterViewModel filterViewModel, SortViewModel sortViewModel)
        {
            Product = product; ;
            Reviews = reviews;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
