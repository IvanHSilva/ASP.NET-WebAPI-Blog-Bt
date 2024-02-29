using Blog.Services;
using BlogEFCore.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(
    options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddDbContext<DataContext>();
builder.Services.AddTransient<TokenService>();

// Transiente - cria uma nova instância a cada referência ao objeto
// Scoped - cria uma nova instância a cada requisição, reaproveitando o existente
// Singleton - cria uma única instância durante toda a execução de aplicação

//Guid guid = Guid.NewGuid();
//Console.WriteLine(guid.ToString());

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

