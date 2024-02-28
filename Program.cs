using BlogEFCore.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(
    options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddDbContext<DataContext>();

//Guid guid = Guid.NewGuid();
//Console.WriteLine(guid.ToString());

var app = builder.Build();

app.MapControllers();
app.Run();

