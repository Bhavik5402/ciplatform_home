using System.ComponentModel.DataAnnotations;

namespace CI_Platform_MVC.Entity.Models.ViewModel
{
    public class RegisterVM
    {
        public User User { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
