using Library.Server.Data;
using Library.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// One LibraryContext and one MemberService per HTTP request
var connectionString = builder.Configuration.GetConnectionString("Library");
builder.Services.AddDbContext<LibraryContext>(o => o.UseNpgsql(connectionString));
builder.Services.AddScoped<MemberService>();

// TODO (later): AddAuthentication().AddJwtBearer(...) before UseAuthorization

var app = builder.Build();

// Apply migrations that are not in the database yet (the database itself runs in Docker: docker compose up -d)
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
	context.Database.Migrate();

	// 100 fictional members, only if the table is empty
	if (app.Environment.IsDevelopment()) DevDataSeeder.Seed(context);
}

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
