var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//builder.Services.AddOutputCache();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(10)));
    options.AddPolicy("Query1", builder => builder.SetVaryByQuery("country").Expire(TimeSpan.FromSeconds(500)).Tag("tag-country"));
    options.AddPolicy("Header", builder => builder.SetVaryByHeader("X-MyHeader").Expire(TimeSpan.FromSeconds(1000)));
    options.AddPolicy("NoCache", builder => builder.NoCache());
    options.AddPolicy("NoLock", builder => builder.SetLocking(false));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseOutputCache();
app.MapControllers();

app.Run();

