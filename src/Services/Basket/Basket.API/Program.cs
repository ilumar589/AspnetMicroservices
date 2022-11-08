using Basket.API.GrpcServices;
using Basket.API.Repositories;
using static Discount.Grpc.Protos.DiscountProtoService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpcClient<DiscountProtoServiceClient>(grpcClient =>
{
    string connectionUrl = builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl");
    grpcClient.Address = new Uri(connectionUrl);
});
builder.Services.AddSingleton<IBasketRepository, BasketRepository>();
builder.Services.AddSingleton<DiscountGrpcService>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    string connectionUrl = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
    options.Configuration = connectionUrl;
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

app.MapControllers();

app.Run();
