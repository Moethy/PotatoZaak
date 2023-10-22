namespace PotatoZaak.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
