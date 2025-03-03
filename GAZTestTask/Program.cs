using GAZTestTask.Repositories;
using System.Xml.Linq;
using System.Xml.XPath;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    foreach (var fi in dir.EnumerateFiles("*.xml"))
    {
        var doc = XDocument.Load(fi.FullName);
        options.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), true);
    }
});

builder.Services.AddScoped<EmployeeRepository>()
                .AddScoped<DepartmentRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allows",
        builder => builder.AllowAnyOrigin()
                          .WithHeaders("Content-Type"));
});

var app = builder.Build();

app.UseCors("Allows");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
