using System.ComponentModel.DataAnnotations;

public class ContactForm
{
    [Required, StringLength(60)]
    public string FirstName { get; set; }

    [Required, StringLength(60)]
    public string LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, Phone]
    public string Phone { get; set; }
  
    [Required]
    public string Message { get; set; }

    [Required]
    public string Subject { get; set; }

    public string gRecaptchaResponse { get; set; }
}
