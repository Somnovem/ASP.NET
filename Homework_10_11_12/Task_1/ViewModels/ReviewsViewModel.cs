using Task_1.Models;

namespace Task_1.ViewModels
{
    public class ReviewsViewModel
    {
        public bool IsManager { get; set; }
        public string Username { get; set; }
        public List<UserMessage> UserMessages { get; set; }
    }
}
