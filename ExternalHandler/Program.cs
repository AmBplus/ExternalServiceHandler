using ExternalHandler.MinimalApi;
using ExternalHandler.Settings;
using ExternalHandler.UseCase.FinotechVerifyBankCard;
using ExternalHandler.UseCase.ZibalPayment;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<VerifyAccountSettings>(
 builder.Configuration.GetSection(VerifyAccountSettings.Configuration));

builder.Services.Configure<ZibalPaymentSettings>(
builder.Configuration.GetSection(ZibalPaymentSettings.Configuration));
builder.Services.Configure<AppSettings>(
builder.Configuration.GetSection(AppSettings.Configuration));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IVerifyCardToken, VerifyCardToken>();
builder.Services.AddTransient<IVerifyUserCard, VerifyUserCardHandler>();
builder.Services.AddTransient<IZibalVerifyPaymentHandler, ZibalVerifyPaymentHandler>();
builder.Services.AddTransient<IZiBalPaymentHandler, ZiBalPaymentHandler>();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(88); // to listen for incoming http connection on port 5001  
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();
app.MapVerifyUserCard();
app.MapGetPaymentLink();
app.MapVerifyPayment();





app.Run();

