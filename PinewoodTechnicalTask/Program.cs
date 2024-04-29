using PinewoodTechnicalTask.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped(typeof(IApiClient), typeof(ApiClient));
builder.Services.AddHttpClient<ApiClient>("MyApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7286/"); // Set your base address here
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Ensure that error handling middleware is added before any other middleware
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Add routing for the default endpoint
    endpoints.MapRazorPages();
});

app.Run();

