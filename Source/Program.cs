
using Azure.Core;
using Azure.Identity;
using FlowGate;
using FlowGate.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<SynapseTriggerServiceOptions>(e =>
{
    var clientId = builder.Configuration.GetValue<string>("ClientId");
    var clientSecret = builder.Configuration.GetValue<string>("ClientSecret");
    var tenantId = builder.Configuration.GetValue<string>("TenantId");
    var authorityUri = new Uri($"https://login.microsoftonline.com/{tenantId}");
 
    return new SynapseTriggerServiceOptions()
    {
        WorkspaceName = builder.Configuration.GetValue<string>("WorkspaceName"),
        SubscriptionId = builder.Configuration.GetValue<string>("SubscriptionId"),
        Credential = new ClientSecretCredential(tenantId, clientId, clientSecret),
    };
});
builder.Services.AddScoped<SynapseTriggerService>();

builder.Services.AddSingleton<FlowGatePipelineRunOptions>(e =>
{
    return new FlowGatePipelineRunOptions()
    {
        PipelineName = builder.Configuration.GetValue<string>("PipelineName")
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
