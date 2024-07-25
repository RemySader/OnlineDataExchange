using SoapCore;
using OnlineDataExchange;
using OnlineDataExchange.Contracts;
using OnlineDataExchange.Services;
using SoapCore.Extensibility;
using OnlineDataExchange.Interface;
using OnlineDataExchange.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IPinService, PinService>();
builder.Services.AddSingleton<IBalanceService, BalanceService>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();
builder.Services.AddSingleton<IFailedLoginTracker, FailedLoginTracker>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
/*builder.Services.AddSingleton<IMessageInspector, CustomMessageInspector>();*/
builder.Services.AddSoapCore();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
app.UseMiddleware<SoapHeaderMiddleware>();

app.UseSoapEndpoint<IPinService>("/SendPin.asmx", new SoapEncoderOptions());
app.UseSoapEndpoint<IBalanceService>("/CheckBalance.asmx", new SoapEncoderOptions());
app.UseSoapEndpoint<IPaymentService>("/MakePayment.asmx", new SoapEncoderOptions());
app.UseAuthorization();
app.MapControllers();
app.Run();