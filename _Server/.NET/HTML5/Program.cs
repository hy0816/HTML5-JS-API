using HTML5.Controllers;
using HTML5.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//���U WebSocket ���A��
builder.Services.AddSingleton<Ws1Controller>();
builder.Services.AddSingleton<Ws2Controller>();

//���U SignalR ���A��
builder.Services.AddSignalR();

//���\���ШD
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://127.0.0.1:5500") // ���w���\���e�ݨӷ�
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // ���\����
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(); //�ϥθ��ШD
app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(60) });
app.MapHub<ChatHub>("/chatSr");    //�]�wSignalR���{���i�J�I
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.Run();
