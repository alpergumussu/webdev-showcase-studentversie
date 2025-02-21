using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Showcase_Profielpagina.Services;

namespace Showcase_Contactpagina.Controllers
{
    public class ContactController : Controller
    {
        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitContactForm([FromBody] ContactForm form)
        {
            if (form == null || !ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid form data." });
            }

            var body = $"<h3>New Contact Form Submission</h3>" +
                       $"<p><strong>First Name:</strong> {form.FirstName}</p>" +
                       $"<p><strong>Last Name:</strong> {form.LastName}</p>" +
                       $"<p><strong>Email:</strong> {form.Email}</p>" +
                       $"<p><strong>Phone:</strong> {form.Phone}</p>" +
                       $"<p><strong>Subject:</strong> {form.Subject}</p>" +
                       $"<p><strong>Message:</strong><br>{form.Message}</p>";

            try
            {
                await _emailService.SendEmailAsync(form.Email, "Contact Form Submission", body);
                return Ok(new { message = "Form submitted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error sending email.", details = ex.Message });
            }
        }
    }
}

