using System.ComponentModel.DataAnnotations;
namespace MvcProject99.Models
{
    public class AddingProducts
    {
        public string PName { get; set; }
        public string Company { get; set; }
        public string Intense { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }

        public int Amount { get; set; }

        public string ImageURL { get; set; }

        //public int NewPrice {  get; set; }

        public List<AddingProducts> products { get; set; }
    }
}
