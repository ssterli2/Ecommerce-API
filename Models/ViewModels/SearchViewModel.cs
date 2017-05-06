
namespace Ecomm.Models
{
    public class SearchViewModel : BaseEntity
    {
        public string SearchCriteria { get; set; }
        public string Filter { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }

        public SearchViewModel()
        {
            PageNumber = 0;
            ItemsPerPage = 50;
        }
    }
}
