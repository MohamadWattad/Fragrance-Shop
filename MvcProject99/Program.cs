//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddSingleton<IHttpContextAccessor, IHttpContextAccessor>();
//builder.Services.TryAddSingleton<IHttpContextAccessor, IHttpContextAccessor>();

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddAuthentication(
//	CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600); // Session timeout of 1 hour
	//options.Cookie.HttpOnly = true;
	//options.Cookie.IsEssential = true;
});


var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
