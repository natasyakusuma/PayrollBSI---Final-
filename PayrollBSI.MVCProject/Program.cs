using PayrollBSI.MVCProject.Services;
using PayrollBSI.MVCProject.Services.Interface;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//register session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});



// Register HttpClient
builder.Services.AddHttpClient();

//Register DI
builder.Services.AddScoped<IPosition, PositionServices>();
builder.Services.AddScoped<IEmployee, EmployeeService>();
builder.Services.AddScoped<IAttendance, AttendanceService>();
builder.Services.AddScoped<IPayrollDetails, PayrollDetailService>();



var app = builder.Build();

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

app.UseAuthentication();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
