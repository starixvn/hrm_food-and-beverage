namespace NexFB.Models.Authority
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Change Password Model.
    /// </summary>
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
