using Testcontainers.PostgreSql;

namespace Library.Tests;

/// <summary>
/// One throwaway PostgreSQL container for all tests of the "Postgres" collection (Docker must be running).
/// Starting a container takes seconds, so it is shared; every test still gets its own database (see TestDatabase).
/// </summary>
public sealed class PostgresFixture : IAsyncLifetime
{
	// The same image as in docker-compose.yml
	private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17").Build();

	public string ConnectionString => _container.GetConnectionString();

	public Task InitializeAsync() => _container.StartAsync();

	public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}

/// <summary>
/// Test classes marked [Collection("Postgres")] share one PostgresFixture.
/// </summary>
[CollectionDefinition("Postgres")]
public class PostgresCollection : ICollectionFixture<PostgresFixture>;
