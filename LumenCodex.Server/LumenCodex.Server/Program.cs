using LumenCodex.Server.Middlewares;
using LumenCodex.Services;
using LumenCodex.ServicesContracts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<GlobalExceptionMiddleware>();

builder.Services.AddControllers();
builder.Services.AddScoped<IFileScanner, FileScanner>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ISubtitleService, SubtitleService>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                          ?? "Data Source=lumen.db";
builder.Services.AddDbContext<LumenContext>(options =>
{
    options.UseSqlite(connectionString);
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();