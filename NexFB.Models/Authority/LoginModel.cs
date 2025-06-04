namespace NexFB.Models.Authority
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Login Model.
    /// </summary>
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
