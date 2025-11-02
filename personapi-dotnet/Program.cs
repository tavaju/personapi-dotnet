using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.DAO.Interfaces;
using personapi_dotnet.Models.DAO.Implementations;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar el contexto de base de datos con EF Core
builder.Services.AddDbContext<PersonaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information)
    );

// Registrar los DAOs con inyección de dependencias
builder.Services.AddScoped<IPersonaDAO, PersonaDAO>();
builder.Services.AddScoped<ITelefonoDAO, TelefonoDAO>();
builder.Services.AddScoped<IEstudioDAO, EstudioDAO>();
builder.Services.AddScoped<IProfesionDAO, ProfesionDAO>();

var app = builder.Build();

await InitializeDatabaseAsync(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// SWAGGER
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Mapear rutas de controladores MVC primero (controladores tradicionales)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapear controladores API (con atributos [Route])
app.MapControllers();

app.Run();

static async Task InitializeDatabaseAsync(WebApplication app)
{
    const int maxIntentos = 10;
    var espera = TimeSpan.FromSeconds(5);

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<PersonaDbContext>();

    for (var intento = 1; intento <= maxIntentos; intento++)
    {
        try
        {
            if (!await context.Database.CanConnectAsync())
            {
                throw new InvalidOperationException("La base de datos aún no acepta conexiones.");
            }

            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("Base de datos inicializada correctamente en el intento {Intento}.", intento);
            return;
        }
        catch (Exception ex)
        {
            if (intento == maxIntentos)
            {
                logger.LogError(ex, "No se pudo inicializar la base de datos después de {MaxIntentos} intentos.", maxIntentos);
                throw;
            }

            logger.LogWarning(ex, "Error al inicializar la base de datos en el intento {Intento}/{MaxIntentos}. Reintentando en {Espera}...", intento, maxIntentos, espera);
            await Task.Delay(espera);
        }
    }
}
