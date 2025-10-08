using System.Diagnostics;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHealthChecks("/health");

app.MapGet("/environment", () =>
{
    EnvironmentInfo info = new EnvironmentInfo();

    string json = JsonSerializer.Serialize(info);
    EnvironmentInfo.LogInfo("/environment",json); 

    return info;
});

app.MapGet("/variables", () =>
{
    string variables = EnvironmentInfo.GetEnvironmentVariables();

    EnvironmentInfo.LogInfo("/variables",variables); 

    return variables;
});


app.MapGet("/request", (HttpRequest request) =>
{

    RequestInfo info = new RequestInfo(request.HttpContext);

    string json = JsonSerializer.Serialize(info);
    EnvironmentInfo.LogInfo("/request", json);

    return info;
});

app.MapPost("/postjson", (HttpRequest request, HttpResponse response) =>
{
    try
    {
        //response.CompleteAsync();
        if (request.BodyReader.TryRead(out ReadResult result))
        {
            var resultString = Encoding.UTF8.GetString(result.Buffer);
            return "{\"Body\":\"" + resultString + "\"}";
        }
        else
        {
            return "{\"Body\":\"(Couldn't read Body)\"}";
        }

    }
    catch (System.Exception exc)
    {
        string msg = "/postjson exception: " + exc.Message;
        Console.WriteLine(msg);
        return msg;
    }

});


//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();


