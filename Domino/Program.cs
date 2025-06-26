using Domino.Orchestrators;
using Domino.Services;
using Domino.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ISpeechToTextService, SpeechToTextService>();
builder.Services.AddScoped<ITextToSpeechService, TextToSpeechService>();
//builder.Services.AddHttpClient<IGPTService, GPTService>();

//builder.Services.AddHttpClient<IOllamaService, OllamaService>(client =>
//{
//    client.BaseAddress = new Uri("http://localhost:11434");
//});
builder.Services.AddScoped<ConversationOrchestrator>();

builder.Services.AddHttpClient<IOllamaService, TinyLlamaService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
