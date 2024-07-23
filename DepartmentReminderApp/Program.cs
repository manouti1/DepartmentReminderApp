using DepartmentReminderApp.Data;
using DepartmentReminderApp.Jobs;
using DepartmentReminderApp.Repositories;
using DepartmentReminderApp.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddFluentEmail(builder.Configuration["EmailSettings:FromEmail"])
    .AddSmtpSender(new SmtpClient(builder.Configuration["EmailSettings:SmtpHost"])
    {
        Port = int.Parse(builder.Configuration["EmailSettings:SmtpPort"]),
        Credentials = new NetworkCredential(builder.Configuration["EmailSettings:SmtpUser"], builder.Configuration["EmailSettings:SmtpPass"]),
        EnableSsl = true
    });

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.ScheduleJob<ReminderJob>(trigger => trigger
        .WithIdentity("ReminderJob")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(5) // Adjust the interval as needed
            .RepeatForever()));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddScoped<ReminderService>();


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

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Departments}/{action=Index}/{id?}");

app.Run();
