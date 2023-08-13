using Bookify.Web2.Core.mapping;
using Bookify.Web2.Core.Models;
using Bookify.Web2.Seeds;
using Bookify.Web2.setting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;
using Bookify.Web2.Data;
using Bookify.Web2.Helpers;
using Bookify.Web2.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.DataProtection;
using WhatsAppCloudApi.Extensions;
using Hangfire;
using Hangfire.Dashboard;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString!));


/*builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/

builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;

});
builder.Services.AddDataProtection().SetApplicationName(nameof(Bookify));

builder.Services.AddWhatsAppApiClient(builder.Configuration);
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddTransient<IImageService,ImageService>();
builder.Services.AddTransient<IEmailSender,EmailSender>();
builder.Services.AddTransient<IEmailBodyBuilder,EmailBodyBuilder>();
builder.Services.Configure<SecurityStampValidatorOptions>
    (option => option.ValidationInterval = TimeSpan.Zero);

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection(nameof(CloudinarySetting)));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();
builder.Services.AddExpressiveAnnotations();
builder.Services.Configure<AuthorizationOptions>(option => option.AddPolicy("adminOnly", 
    policy=>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole(AppRoles.Admin);
    }));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var scopFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopFactory.CreateScope();

var RoleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


await DefaultRoles.SeedRoles(RoleManger);

await DefaultUsers.SeedAdminUser(userManger);



app.UseHangfireDashboard("/hangfire",new DashboardOptions
{
    DashboardTitle="Bookify Dashboard",
/*    IsReadOnlyFunc=(DashboardContext contex)=>true,
*/    Authorization =new IDashboardAuthorizationFilter[]
    {
        new HangFireAuthorizationFilter("adminOnly")
    }
});;

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
