using System.ComponentModel.DataAnnotations;

namespace MvcProject99.Models
{
    public class SignupModel
    {
		[Required(ErrorMessage = "Must to be first name")]
		[StringLength(20, MinimumLength = 2, ErrorMessage = "first name must be between 2 and 20 letters")]
		public string FirstName { get; set; }
        [Required(ErrorMessage = "Must to be first name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "first name must be between 2 and 20 letters")]
        public string LastName { get; set; }
        [Required]
		[RegularExpression("^[\\w-]+(?:\\.[\\w-]+)*@(?:[\\w-]+\\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Must contain @")]
		public string Email {  get; set; }
		[Required(ErrorMessage="Try Again")]
		[RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Invailed")]
		public string Password { get; set; }



        
    }
}
