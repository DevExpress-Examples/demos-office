using BlazorDemo.OfficeFileApi;
using BlazorDemo.OfficeFileApi.DemoShell;
using BlazorDemo.Services;
using DevExpress.Blazor.DocumentMetadata;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDevExpressBlazor(options => {
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});
builder.Services.AddSingleton<InMemoryNWindDataService>();
builder.Services.AddSingleton<FileFormatService>();
builder.Services.AddSingleton<SpreadsheetProtectionOptionsService>();
builder.Services.AddSingleton<IOfficeFileApiBlazorDemoSectionSvgService, SvgService>();
builder.Services.AddOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<DemoConfiguration>();
builder.Services.AddScoped<DemoThemeService>();
builder.Services.AddScoped<IDemoStaticResourceService, DemoStaticResourceService>();
builder.Services.AddDocumentMetadata((sp, metadataCollection) => { sp!.GetService<DemoConfiguration>()!.ConfigureMetadata(metadataCollection); });
builder.Services.AddSingleton<IDemoVersion, DemoVersion>(x => {
    var dxVersion = new Version(AssemblyInfo.Version);
    return new DemoVersion(new Version(dxVersion.Major, dxVersion.Minor, dxVersion.Build).ToString());
});

// Add HTTP client for API calls
builder.Services.AddHttpClient();

// Add MVC services with controllers - REQUIRED for API controllers to work
builder.Services.AddControllers();

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

var app = builder.Build();
if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseStatusCodePagesWithReExecute("/");
app.UseAntiforgery();
app.MapControllers(); // Map controller routes
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();
