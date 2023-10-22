using PotatoZaak.Models;

namespace PotatoZaak.ViewModels
{
    public class OrderViewModel
    {
        public DateTime DateTime { get; set; }
        public int OrderNr { get; set; }
        public List<Product>? AvailableProducts { get; set; }
        public List<int>? SelectedProductIds { get; set; }
        public List<int>? Quantities { get; set; }
    }




}
