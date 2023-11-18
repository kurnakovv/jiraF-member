using jiraF.Member.API.GlobalVariables;
using jiraF.Member.API.Infrastructure.Data.Contexts;
using jiraF.Member.API.Infrastructure.RabbitMQ;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DefaultMemberVariables.Id = builder.Configuration.GetValue<string>("DefaultMemberId");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(TestVariables.IsWorkNow
        ? Guid.NewGuid().ToString()
        : "TestData");
    //options.UseNpgsql(builder.Configuration["ConnectionString"]);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

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

app.UseHealthChecks("/ping");

app.Run();

