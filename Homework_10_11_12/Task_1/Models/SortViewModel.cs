namespace Task_1.Models
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState RatingSort { get; set; }
        public SortState CompanySort { get; set; }   
        public SortState Current { get; set; }
        public bool Up { get; set; }

        public SortViewModel(SortState sortOrder)
        {
            // значения по умолчанию
            NameSort = SortState.NameAsc;
            RatingSort = SortState.RatingAsc;
            Up = true;

            if (sortOrder == SortState.NameDesc || sortOrder == SortState.RatingDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Current = NameSort = SortState.NameAsc;
                    break;
                case SortState.RatingAsc:
                    Current = RatingSort = SortState.RatingDesc;
                    break;
                case SortState.RatingDesc:
                    Current = RatingSort = SortState.RatingAsc;
                    break;
                default:
                    Current = NameSort = SortState.NameDesc;
                    break;
            }
        }
    }
}
