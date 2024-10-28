using sensores_web.Services;

var builder = WebApplication.CreateBuilder(args);

// ADD SERVICES HERE (BEFORE builder.Build()):
builder.Services.AddRazorPages();
builder.Services.AddScoped<SensorApiService>(); // <-- Move it here!

builder.Services.AddHttpClient<SensorApiService>(client =>
{
    client.BaseAddress = new Uri("http://192.168.100.34:5202/api/"); // Your API base URL
});

var app = builder.Build();

// Syncfusion Licensing (can stay here):
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("ngo9bigboggjhtqxar8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH9feHZVRmVcVkBzV0o=");

// Configure the HTTP request pipeline (rest of the code):
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();