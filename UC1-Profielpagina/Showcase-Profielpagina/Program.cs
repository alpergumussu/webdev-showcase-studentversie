using Microsoft.Extensions.Options;
using Showcase_Profielpagina.Models;
using Showcase_Profielpagina.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();  // Registers IHttpClientFactory

// Correctly register MailSettings using IOptions
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Register EmailService properly
builder.Services.AddTransient<EmailService>();

var app = builder.Build();

// Resolve EmailService using dependency injection
using (var scope = app.Services.CreateScope())
{
    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

    try
    {
        // Send test email
        await emailService.SendEmailAsync("recipient@example.com", "Test Email", "<p>This is a test email from main.</p>");
        Console.WriteLine("Email sent successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error sending email: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
