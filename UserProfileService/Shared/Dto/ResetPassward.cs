using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserProfileService.Shared.Dto
{
    public class ResetPassward
    {
        public Guid Userid; 
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
