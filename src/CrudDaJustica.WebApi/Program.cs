using CrudDaJustica.Data.Lib.Repositories;
using CrudDaJustica.Data.Lib.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PagingService>();

builder.Services.AddScoped<HeroRepository, SqlServerRepository>(serviceProvider =>
{
    var pagingService = serviceProvider.GetRequiredService<PagingService>();

    var username = Environment.GetEnvironmentVariable("MJVSCHOOLDB_USERNAME");
    var password = Environment.GetEnvironmentVariable("MJVSCHOOLDB_PASSWORD");
    var connectionString = builder.Configuration.GetConnectionString("SqlServer");
    connectionString = string.Format(connectionString!, username, password);

    return new(pagingService, connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
