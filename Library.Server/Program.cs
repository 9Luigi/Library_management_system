using Library.Server.Data;
using Library.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// One LibraryContext and one MemberService per HTTP request
var connectionString = builder.Configuration.GetConnectionString("Library");
builder.Services.AddDbContext<LibraryContext>(o => o.UseSqlite(connectionString));
builder.Services.AddScoped<MemberService>();

// TODO (later): AddAuthentication().AddJwtBearer(...) before UseAuthorization

var app = builder.Build();

// SQLite creates the file, but not the folder; then apply migrations that are not in the database yet
Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "App_Data"));
using (var scope = app.Services.CreateScope())
{
	scope.ServiceProvider.GetRequiredService<LibraryContext>().Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
