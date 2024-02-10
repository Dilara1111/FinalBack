using System.ComponentModel.DataAnnotations;

namespace Final_Back.ViewModels.Account
{
    public class AccountRegisterVM
    {
        [Required]
        public string Fullname { get; set; }
        [Required,MinLength(3)]
        public string Username { get;set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,DataType(DataType.Password), MinLength(3)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
