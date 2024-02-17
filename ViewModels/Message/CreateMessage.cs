using System.ComponentModel.DataAnnotations;

namespace Final_Back.ViewModels
{
    public class CreateMessage
    {
        //public List<Message> Messages { get; set; }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string MessageInfo { get; set; }
    }
}
