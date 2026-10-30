var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// TODO (step 1): register the DbContext after moving the models here:
//   var connectionString = builder.Configuration.GetConnectionString("Library");
//   builder.Services.AddDbContext<LibraryContext>(o => o.UseSqlite(connectionString));

// TODO (later): AddAuthentication().AddJwtBearer(...) before UseAuthorization

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
