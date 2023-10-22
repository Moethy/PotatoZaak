namespace PotatoZaak.Models
{
    public class OrderProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }
        public List<Order> Orders { get; set; }

        public OrderProductViewModel()
        {

            Products = new List<Product>();
            Orders = new List<Order>();
            OrderProducts = new List<OrderProduct>();
           

            


        }
    }

}
