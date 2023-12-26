using Rpd.Interv.Core.Abstraction;
using Rpd.Interv.Core.Middlewares;
using Rpd.Interv.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient("TimeEntryHttpClient", option =>
{
    option.BaseAddress = new Uri(builder.Configuration["EmployeTimeEntryUrl"] ?? "");
});

builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleWare>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // To serve static files like JavaScript libraries

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}");

app.Run();
