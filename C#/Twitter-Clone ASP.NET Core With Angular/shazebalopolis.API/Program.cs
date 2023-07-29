using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using shazebs.api;
using shazebs.api.Models;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Application Insights
builder.Services.AddApplicationInsightsTelemetry(config =>
{
    config.ConnectionString = builder.Configuration.GetSection("ApplicationInsights")["ConnectionString"];
});


// Add Odata
builder.Services.AddControllers().AddOData(options =>
    options.EnableQueryFeatures().AddRouteComponents("api", GetEdmModel()));

// Add Odata EDM
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Tweet>("Tweets");
    //builder.EntitySet<User>("Users");
    return builder.GetEdmModel();
}


// Add DbContext
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


// Inject Services (DI)


// Enable CORS
builder.Services.AddCors(setup =>
{
    setup.AddPolicy("default", (options) =>
    {
        options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});


// Add Identity.Web Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/" + builder.Configuration["AzureAd:TenantId"] + "/v2.0";
        options.Audience = builder.Configuration["AzureAd:ClientId"];
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = true;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                context.Request.Headers.TryGetValue("Authorization", out var BearerToken);
                if (BearerToken.Count == 0)
                    BearerToken = "no Bearer token sent";
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Debug.WriteLine("token: " + context.SecurityToken.ToString());
                return Task.CompletedTask;
            },
        };
    });


builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


// Ensure DB is created.
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Database.EnsureCreated();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}


app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("default");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();