using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Domain + nginx/IIS / hosting paneli reverse-proxy arkası
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

// Hosting paneli / Docker PORT veya ASPNETCORE_URLS ile dinler (varsayılan 8080 Dockerfile'da)
app.Run();
