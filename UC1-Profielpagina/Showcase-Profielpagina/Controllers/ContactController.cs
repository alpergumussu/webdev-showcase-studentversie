using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Showcase_Profielpagina.Services;

namespace Showcase_Contactpagina.Controllers
{
    public class ContactController : Controller
    {
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration; // 🔑 Add IConfiguration to get the reCAPTCHA secret key

        public ContactController(EmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        // GET: ContactController
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> SubmitContactForm([FromBody] ContactForm form)
        {
            if (form == null || !ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid form data." });
            }

       

            // ✅ Step 2: Build the email body
            var body = $"<h3>New Contact Form Submission</h3>" +
                       $"<p><strong>First Name:</strong> {form.FirstName}</p>" +
                       $"<p><strong>Last Name:</strong> {form.LastName}</p>" +
                       $"<p><strong>Email:</strong> {form.Email}</p>" +
                       $"<p><strong>Phone:</strong> {form.Phone}</p>" +
                       $"<p><strong>Subject:</strong> {form.Subject}</p>" +
                       $"<p><strong>Message:</strong><br>{form.Message}</p>";

            // ✅ Step 3: Send email
            try
            {
                await _emailService.SendEmailAsync(form.Email, "Contact Form Submission", body);
                return Ok(new { message = "Your message has been sent successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error sending email.", details = ex.Message });
            }
        }

        // 🔹 Function to verify reCAPTCHA with Google
        private async Task<bool> VerifyRecaptcha(string recaptchaResponse)
        {
            if (string.IsNullOrWhiteSpace(recaptchaResponse))
                return false;

            using (var client = new HttpClient())
            {
                var secretKey = _configuration["GoogleReCaptcha:SecretKey"]; // ✅ Get reCAPTCHA secret key from appsettings.json
                var response = await client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}", null);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RecaptchaResponse>(jsonResponse);

                return result?.Success ?? false;
            }
        }
    }
}
