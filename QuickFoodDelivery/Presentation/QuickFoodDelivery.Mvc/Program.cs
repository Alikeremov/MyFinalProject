using QuickFoodDelivery.Infrastructure.Middleware;
using QuickFoodDelivery.Persistence.ServiceRegistration;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); 
});
var app = builder.Build();

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:Secretkey"];
//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
