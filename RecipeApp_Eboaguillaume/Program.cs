using RecipeApp_Eboaguillaume.Components;
using RecipeApp_Eboaguillaume.Services;
using Radzen;
using SQLAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//ce que j'ajoute 

builder.Services.AddRadzenComponents();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<MySqlRequestService>();
builder.Services.AddSingleton<IRecipeService, MySqlRecipeService>();
builder.Services.AddScoped<IRequestService, MySqlRequestService>();
builder.Services.AddSingleton<IMySQLDataAccess, MySQLDataAccess>();

// Client HTTP pour l'API OpenAI
builder.Services.AddHttpClient("OpenAI", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/");
});

// Service pour interagir avec ChatGPT
builder.Services.AddScoped<IChatGptService, ChatGptService>();


// fin de ce que j'ajoute
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

internal class intIdGenerator
{
}