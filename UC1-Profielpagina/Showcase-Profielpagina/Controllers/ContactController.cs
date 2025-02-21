using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Showcase_Profielpagina.Services;
using Showcase_Profielpagina.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;


namespace Showcase_Contactpagina.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RecaptchaSettings _recaptchaSettings;
        private const string RecaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";

        public ContactController(IHttpClientFactory httpClientFactory, IOptions<RecaptchaSettings> recaptchaSettings)
        {
            _httpClientFactory = httpClientFactory;
            _recaptchaSettings = recaptchaSettings.Value;
        }


        // GET: ContactController
        public ActionResult Index()
        {
            ViewBag.RecaptchaSiteKey = _recaptchaSettings.SiteKey; 
            return View();
        }

        // POST: ContactController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitContactForm([FromBody] ContactForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid form data." });

            var secretKey = _recaptchaSettings.SecretKey;

            using var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync($"{RecaptchaVerifyUrl}?secret={secretKey}&response={form.GRecaptchaResponse}", null);

            var json = await response.Content.ReadAsStringAsync();

            var captchaResult = JsonSerializer.Deserialize<RecaptchaResponse>(json);

            if (captchaResult == null || !captchaResult.Success)
                return BadRequest(new { message = "reCAPTCHA verification failed." });

            // TODO: Implement email sending (Mailtrap, SendGrid, SMTP)

            return Ok(new { message = "Your message has been sent successfully!" });
        }
    }
}
