using MinimalApiApp.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.Run();


var person = new Person
{
    Birthday = DateTime.Now,
};
ShowName(null);


Console.WriteLine(person.Age);

void ShowName(Person person)
{
    var name = person?.Name ?? "John Doe";

    string currentName = null;
    string currentTitle = null;
    if (person != null) { 
        currentName = person.Name; 
        currentTitle = person.Title;
    }

    if (currentName != null)
    {
        name = $"{currentTitle} {currentName}";
        //name = currentTitle + " " + currentName;
    }
    else
    {
        name = "John Doe";
    }
    Console.WriteLine(name);
}