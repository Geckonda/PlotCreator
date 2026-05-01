using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlotCreator.DAL;
using PlotCreator.DAL.Interceptors;
using PlotCreator_API.Middleware;

namespace PlotCreator_API
{
    public class Program
    {
        public const string FrontendCorsPolicy = "frontend";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options
                    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(new AuditInterceptor()));

            builder.Services.AddHttpContextAccessor();
            builder.Services.InitialiseRepositories();
            builder.Services.InitialiseServices();

            // ── JWT auth ────────────────────────────────────────────────
            var jwtSection = builder.Configuration.GetSection("Jwt");
            var jwtKey = jwtSection["Key"]
                ?? throw new InvalidOperationException("Jwt:Key is missing in configuration.");
            var jwtIssuer = jwtSection["Issuer"] ?? "PlotCreator";
            var jwtAudience = jwtSection["Audience"] ?? "PlotCreator.Frontend";
            var cookieName = jwtSection["CookieName"] ?? "pc_auth";

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                    // Read the token from the auth cookie if no Authorization header is present.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            if (string.IsNullOrEmpty(ctx.Token)
                                && ctx.Request.Cookies.TryGetValue(cookieName, out var cookie))
                            {
                                ctx.Token = cookie;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(FrontendCorsPolicy, policy => policy
                    .WithOrigins("http://localhost:5173", "https://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            builder.Services
                .AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    o.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower));
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(FrontendCorsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
