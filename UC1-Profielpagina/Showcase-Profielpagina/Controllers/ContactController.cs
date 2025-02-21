using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Showcase_Profielpagina.Services;
using Showcase_Profielpagina.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace Showcase_Contactpagina.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RecaptchaSettings _recaptchaSettings;
        private const string RecaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";
        private readonly EmailService _emailService;

        public ContactController(IHttpClientFactory httpClientFactory, IOptions<MailSettings> mailSettings, 
            IOptions<RecaptchaSettings> recaptchaSettings, EmailService emailService)
        {
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;
            _recaptchaSettings = recaptchaSettings.Value;
        }

        // GET: ContactController
        public ActionResult Index()
        {
            ViewBag.RecaptchaSiteKey = _recaptchaSettings.SiteKey;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitContactForm(ContactForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid form data." });

            // Verify reCAPTCHA
            var recaptchaResponse = form.gRecaptchaResponse;
            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                return BadRequest(new { message = "reCAPTCHA verification failed." });
            }

            var httpClient = _httpClientFactory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", _recaptchaSettings.SecretKey),
                new KeyValuePair<string, string>("response", recaptchaResponse),
            });

            var response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var recaptchaVerificationResult = JsonSerializer.Deserialize<RecaptchaResponse>(jsonResponse);

            if (!recaptchaVerificationResult.Success)
            {
                return BadRequest(new { message = "reCAPTCHA verification failed." });
            }

            // Build email body
            var body = $"<h3>New Contact Form Submission</h3>" +
                       $"<p><strong>First Name:</strong> {form.FirstName}</p>" +
                       $"<p><strong>Last Name:</strong> {form.LastName}</p>" +
                       $"<p><strong>Email:</strong> {form.Email}</p>" +
                       $"<p><strong>Phone:</strong> {form.Phone}</p>" +
                       $"<p><strong>Subject:</strong> {form.Subject}</p>" +
                       $"<p><strong>Message:</strong><br>{form.Message}</p>";

            // Send email
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
    }
}