using Showcase_Profielpagina.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register MVC Controllers with Views
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();  // Registers IHttpClientFactory

// ✅ Correctly register MailSettings using IOptions
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// ✅ Register EmailService using IOptions<MailSettings>
builder.Services.AddTransient<EmailService>();

var app = builder.Build();

// ✅ Configure the HTTP request pipeline properly
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
