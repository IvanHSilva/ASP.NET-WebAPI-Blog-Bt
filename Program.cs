using Blog.Services;
using BlogEFCore.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(
    options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddDbContext<DataContext>();
builder.Services.AddTransient<TokenService>();

// Transiente - cria uma nova inst�ncia a cada refer�ncia ao objeto
// Scoped - cria uma nova inst�ncia a cada requisi��o, reaproveitando o existente
// Singleton - cria uma �nica inst�ncia durante toda a execu��o de aplica��o

//Guid guid = Guid.NewGuid();
//Console.WriteLine(guid.ToString());

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

