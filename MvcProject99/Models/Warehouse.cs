using MvcProject99.Models;
namespace MvcProject99.Models
{
    public class Warehouse
    {
        public AddingProducts product {  get; set; }
        public List<AddingProducts> products { get; set; }

        public string CatgN {  get; set; }

        public string FGname { get; set; }

        public List<AddingProducts> searchList { get; set; }

        public List<AddingProducts> realatedList { get; set; }


    }
}
