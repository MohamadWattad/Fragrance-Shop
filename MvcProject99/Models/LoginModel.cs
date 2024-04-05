using System.ComponentModel.DataAnnotations;
namespace MvcProject99.Models
{
    public class LoginModel
    {
        [Required]
        [RegularExpression("^[\\w-]+(?:\\.[\\w-]+)*@(?:[\\w-]+\\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Must contain @")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please enter your password")]
        //[RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Invailed")]
        public string Password { get; set; }
    }
}
