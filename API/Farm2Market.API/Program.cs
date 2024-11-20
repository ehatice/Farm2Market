using Farm2Market.Domain;
using Farm2Market.Domain.Entities;
using Farm2Market.Infrastructure.Data;
using Farm2Market.Infrastructure.Repository;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Manager;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// CORS politikasý tanýmlama
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder=>
        {
            builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Example API",
		Version = "v1",
		Description = "An example of an ASP.NET Core Web API",
		Contact = new OpenApiContact
		{
			Name = "Example Contact",
			Email = "example@example.com",
			Url = new Uri("https://example.com/contact"),
		},
	});
}); ;
builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(9, 0, 0))));
builder.
    Services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
	options.TokenLifespan = TimeSpan.FromHours(24); 
});
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

builder.Services.AddAuthorization();


var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
	});

}

app.UseCors("AllowAll");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
