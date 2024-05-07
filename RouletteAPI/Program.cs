using RouletteAPI.Helpers;
using RouletteAPI.Interfaces;
using RouletteAPI.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.AllowAnyOrigin()
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});


builder.Services.AddScoped<IBetRepository, BetRepository>();
builder.Services.AddScoped<ISpinResultRepository, SpinResultRepository>();
builder.Services.AddScoped<IPayoutRepository, PayoutRepository>();
builder.Services.AddScoped<IBetHelper, BetHelper>();
builder.Services.AddScoped<ISpinResultHelper, SpinResultHelper>();
builder.Services.AddScoped<IPayoutHelper, PayoutHelper>();
builder.Services.AddSingleton<GlobalExceptionHandler>();

var app = builder.Build();


app.UseMiddleware<GlobalExceptionHandler>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }