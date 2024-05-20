using Sentinel.Frontend.Components;
using Sentinel.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazorBootstrap();
builder.Services.AddSingleton<INewsService>(new NewsService());
builder.Services.AddSingleton<IExtensionService>(new InMemoryExtensionService());
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapPost("/extension", (IExtensionService extensionService, ExtensionInput input) => extensionService.Save(input.Text));

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.Run();

record ExtensionInput(string Text);