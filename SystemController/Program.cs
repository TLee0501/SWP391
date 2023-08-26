using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.ClassService;
using Service.CourseService;
using Service.OnGoingReportService;
using Service.ProjectService;
using Service.ProjectTeamService;
using Service.TaskService;
using Service.UserService;
using Service.RoleService;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Service.SemesterService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Swp391onGoingReportContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Swp391onGoingReportContext"));
});

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IProjectTeamServise, ProjectTeamService>();
builder.Services.AddScoped<IOnGoingReportService, OnGoingReportService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//JWT
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Appsettings:Token").Value!))
    };
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//swagger
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//fix cors error
app.UseCors(builder => builder.WithOrigins("*")
                               .AllowAnyMethod()
                               .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
