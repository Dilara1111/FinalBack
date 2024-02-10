using System.ComponentModel.DataAnnotations;

namespace Final_Back.ViewModels.Account
{
    public class AccountLoginVM
    {
        [Required]
        public  string Username{ get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
