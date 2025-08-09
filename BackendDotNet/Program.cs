using BackendDotNet.Data;
using BackendDotNet.Models;
using BackendDotNet.Repositories;
using BackendDotNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Add Identity services
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000") // React dev server URLs
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IDonorProfileRepository, DonorProfileRepository>();
builder.Services.AddScoped<IHospitalProfileRepository, HospitalProfileRepository>();
builder.Services.AddScoped<IBloodRequestRepository, BloodRequestRepository>();
builder.Services.AddScoped<IBloodInventoryRepository, BloodInventoryRepository>();
builder.Services.AddScoped<IDonationAppointmentRepository, DonationAppointmentRepository>();
builder.Services.AddScoped<IDonationRecordRepository, DonationRecordRepository>();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDonorService, DonorService>();
builder.Services.AddScoped<IHospitalService, HospitalService>();

// Add HttpContextAccessor for services that need current user context
builder.Services.AddHttpContextAccessor();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Blood Bank Management API", 
        Version = "v1",
        Description = "ASP.NET Core Web API for Blood Bank Management System"
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blood Bank Management API V1");
        c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created and seeded
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Create database if it doesn't exist
        context.Database.EnsureCreated();
        
        // Seed default admin user if not exists
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
        
        var adminEmail = "admin@bloodbank.com";
        var existingAdmin = userRepository.GetByEmailAsync(adminEmail).Result;
        
        if (existingAdmin == null)
        {
            var adminRole = roleRepository.GetByRoleNameAsync("ROLE_ADMIN").Result;
            if (adminRole != null)
            {
                var adminUser = new User
                {
                    Email = adminEmail,
                    RoleId = adminRole.RoleId,
                    IsActive = true
                };
                
                adminUser.Password = passwordHasher.HashPassword(adminUser, "admin123");
                userRepository.AddAsync(adminUser).Wait();
                
                Console.WriteLine($"Default admin user created: {adminEmail} / admin123");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database initialization error: {ex.Message}");
    Console.WriteLine("Application will continue without database seeding...");
}

app.Run();
