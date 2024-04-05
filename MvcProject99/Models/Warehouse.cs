using MvcProject99.Controllers;
using MvcProject99.Models;
namespace MvcProject99.Models
{
    public class Warehouse
    {
        public AddingProducts product {  get; set; }
        public List<AddingProducts> products { get; set; }

        public string CatgN {  get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }

        public string FGname { get; set; }

        public List<AddingProducts> searchList { get; set; }

        public List<AddingProducts> realatedList { get; set; }

        public AddingProducts purchasedProduct { get; set; }
        public List<AddingProducts> purchased { get; set; }

        public Messages messages { get; set; }

        public string TopSeller { get; set; }
        public string hasDiscount { get; set; }







    }
}
