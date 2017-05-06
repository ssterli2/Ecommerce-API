
namespace Ecomm.Models
{
    public class CartItemViewModel : BaseEntity
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}