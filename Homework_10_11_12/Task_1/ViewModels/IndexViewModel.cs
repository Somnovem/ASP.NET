using Task_1.Models;

namespace Task_1.ViewModels;

public class IndexViewModel
{
    public List<Product> TopViewedProducts { get; set; } = new();
}