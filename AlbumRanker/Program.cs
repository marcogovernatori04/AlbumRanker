using AlbumRanker.Data;
using AlbumRanker.Repositories;
using AlbumRanker.Repositories.Interfaces;
using AlbumRanker.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IArtistaRepository,  ArtistaRepository>();
builder.Services.AddScoped<ICancionRepository, CancionRepository>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<ArtistaService>();
builder.Services.AddScoped<CancionService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowVue");
app.UseAuthorization();

app.MapControllers();

app.Run();
