using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyAspNetCore.Web.Filters;
using MyAspNetCore.Web.Helpers;
using MyAspNetCore.Web.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => { 
	options.UseSqlServer(builder.Configuration.GetConnectionString("Conn"));
});
builder.Services.AddSingleton<IHelper,Helper>(); // Herhangi bir class const methodunda IHelper isimli interface görürsen git bundan Helper isimli siniftan nesne üret.

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<NotFoundFilter>(); // bunu filter için yapmamızın sebebi bir parametreye göre  filter çalışma düzenini belirler.

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory())); // file ile çalışmamızı sağlar ıo yu soyutlamaya tabi tutar file provider somut bir class isteyecek içeriye new PhysicalFileProvider() yazarız o da içeriye bir path isteyecek nereler için işlem yapmamızı istediği için bizde projenin adını vererek her yerde kullanımını sağlarız. "MyAspNetCore.Web" karşılık gelecek olan Directory.GetCurrentDirectory() yazarız.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

	app.UseHsts(); //Tarayıcıyı bilgilendirir. http üstünden gelen isteği https e çevirir.
}

app.UseHttpsRedirection(); // urlde http ile istek başlarsa httpse çeviriyor.

app.UseStaticFiles(); // wwwroot içindeki dosyalara dışarıdan erişimi sağlar.

app.UseRouting(); // routlama middleware i coventional yada att bazlı routinge göre yapar. 

app.UseAuthorization(); // kimlik yetkilendirme ilgili sayfaya kullanıcı yetkilimi değilmi onu kont eder.

app.UseAuthentication(); // kimlik doğrulama  request geldiğinde cookiedeki tokenı kontrol eder.

app.MapControllers();

/*app.MapControllerRoute(
	name: "productpages",
	pattern: "{controller=Home}/{action=Pages}/{page}/{pagesize}");*/

/*app.MapControllerRoute(
	name: "productgetbyid",
	pattern: "{controller=Home}/{action=Index}/{productid}");*/ // bu metot aşağıdaki metodu ezer.

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
