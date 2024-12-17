using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Library.Infrastructure.Repositories;
public class GenericRepository<T> where T : class //TODO logs
{


	/// <summary>
	/// Retrieves an entity by a specific field value from the database.
	/// </summary>
	/// <typeparam name="TKey">The type of the field used for filtering.</typeparam>
	/// <param name="_dbContext">The database context used for the query.</param>
	/// <param name="fieldSelector">An expression to select the field used for filtering.</param>
	/// <param name="value">The value of the field to search for.</param>
	/// <returns>The entity that matches the specified field value.</returns>
	/// <exception cref="KeyNotFoundException">Thrown if no entity with the specified field value is found.</exception>
	/// <exception cref="ArgumentException">Thrown if the expression does not refer to a valid property.</exception>
	/// <exception cref="Exception">Thrown for any other errors that occur during execution.</exception>
	internal async Task<T> GetByFieldAsync<TKey>(DbContext _dbContext, Expression<Func<T, TKey>> fieldSelector, TKey value)
	{
		try
		{
			var entity = await _dbContext.Set<T>()
				.FirstOrDefaultAsync(e => EF.Property<TKey>(e, GetPropertyName(fieldSelector))!.Equals(value));

			return entity ?? throw new KeyNotFoundException($"Entity with field value '{value}' not found.");
		}
		catch (Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Extracts the property name from an expression that selects a property of the entity.
	/// </summary>
	/// <typeparam name="TKey">The type of the selected property.</typeparam>
	/// <param name="expression">The expression used to select the property.</param>
	/// <returns>The name of the property selected by the expression.</returns>
	/// <exception cref="ArgumentException">Thrown if the expression does not refer to a valid property.</exception>
	private string GetPropertyName<TKey>(Expression<Func<T, TKey>> expression)
	{
		if (expression.Body is MemberExpression memberExpression)
		{
			return memberExpression.Member.Name;
		}
		throw new ArgumentException("The expression does not refer to a valid property.", nameof(expression));
	}

	/// <summary>
	/// Retrieves an entity by its primary key value from the database.
	/// </summary>
	/// <param name="_dbContext">The database context used for the query.</param>
	/// <param name="id">The primary key value of the entity to retrieve.</param>
	/// <returns>The entity that matches the specified primary key value.</returns>
	/// <exception cref="KeyNotFoundException">Thrown if no entity with the specified ID is found.</exception>
	/// <exception cref="Exception">Thrown for any other errors that occur during execution.</exception>
	internal async Task<T> GetByIdAsync(DbContext _dbContext, int id)
	{
		try
		{
			var entity = await _dbContext.Set<T>().FindAsync(id);
			return entity ?? throw new KeyNotFoundException($"Entity with {id} not found"); ;
		}
		catch (Exception)
		{
			throw;
		}
	}
	/// <summary>
	/// Retrieves all entities of type <typeparamref name="T"/>.
	/// </summary>
	/// <returns>A task that returns a collection of all entities.</returns>
	#region GetWithProjectionAsync
	/// <summary>
	/// Asynchronously retrieves a projected list of entities of type <typeparamref name="T"/> from the database.
	/// </summary>
	/// <typeparam name="TResult">The type of the projected result.</typeparam>
	/// <param name="selector">
	/// An expression defining the projection, specifying which properties of the entity to include in the result.
	/// </param>
	/// <param name="includes">
	/// Optional expressions defining related entities to include (e.g., navigation properties).
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result is a list of projected entities of type <typeparamref name="TResult"/>.
	/// </returns>
	/// <example>
	/// Example usage:
	/// <code>
	/// var entity = await repository.GetWithProjectionAsync(
	///     m => new { m.Property1, m.Property2, IncludesCount = m.Includes.Count },
	///     m => m.Includes);
	/// </code>
	/// </example>
	/// <remarks>
	/// - This method allows fetching only the required properties of entities to improve performance.
	/// - Includes are optional and can be used to eagerly load related entities.
	/// </remarks>
	/// <exception cref="ArgumentNullException">
	/// Thrown if <paramref name="selector"/> is null.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the query cannot be executed (e.g., invalid entity state or EF configuration).
	/// </exception>
	public async Task<List<TResult>> GetCollectionWithProjectionAsync<TResult>(
		Expression<Func<T, TResult>> selector,
		DbContext _dbContext,
		params Expression<Func<T, object>>[] includes)
	{
		try
		{
			IQueryable<T> query = _dbContext.Set<T>();

			foreach (var include in includes)
			{
				query = query.Include(include);
			}
			return await query.Select(selector).ToListAsync();
		}
		catch (Exception)
		{
			throw;
		}
	}
	/// <summary>
	/// Asynchronously retrieves a list of projections from the database, applying a filter with a specified field and search value.
	/// </summary>
	/// <typeparam name="TResult">The type of the projection result.</typeparam>
	/// <param name="selector">The projection expression that defines what data to return.</param>
	/// <param name="searchValue">The value to search for in the specified field.</param>
	/// <param name="searchField">The field to apply the filter to (using LIKE).</param>
	/// <param name="includes">An optional array of expressions representing the related entities to include in the query.</param>
	/// <returns>A list of projected results of type <typeparamref name="TResult"/> that match the filter.</returns>
	public async Task<List<TResult>> GetCollectionWithProjectionAsync<TResult>(
	Expression<Func<T, TResult>> selector,
	long searchValue, // The value to filter by
	Expression<Func<T, object>> searchField, // Field to apply the filter on
	DbContext _dbContext,
	params Expression<Func<T, object>>[] includes) // Related entities to include
	{
		try
		{
			// Initialize the query from the DbSet
			IQueryable<T> query = _dbContext.Set<T>();

			// Include related entities
			foreach (var include in includes)
			{
				query = query.Include(include);
			}
			query = query.Where(m => EF.Functions.Like(
				EF.Property<string>(m, GetPropertyName(searchField)).ToString(),
				$"%{searchValue}%"));
			// Perform projection and return results
			return await query.Select(selector).ToListAsync();
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task<TResult> GetOneByIDWithProjectionAsync<TResult>(
		Expression<Func<T, TResult>> selector,
		DbContext _dbContext,
		params Expression<Func<T, object>>[] includes)
	{
		try
		{
			IQueryable<T> query = _dbContext.Set<T>();

			foreach (var include in includes)
			{
				query = query.Include(include);
			}
			return await query.Select(selector).FirstAsync();
		}
		catch (Exception)
		{
			throw;
		}
	}
	/// <summary>
	/// Helper method to extract the property name from a lambda expression.
	/// </summary>
	/// <param name="expression">The lambda expression representing the field to extract the property name from.</param>
	/// <returns>The name of the property represented by the lambda expression.</returns>
	private string GetPropertyName(Expression<Func<T, object>> expression)
	{
		if (expression.Body is MemberExpression memberExpression)
		{
			return memberExpression.Member.Name;
		}

		if (expression.Body is UnaryExpression unaryExpression &&
			unaryExpression.Operand is MemberExpression operandMemberExpression)
		{
			return operandMemberExpression.Member.Name;
		}

		throw new ArgumentException("The expression does not refer to a valid property.", nameof(expression));
	}

	#endregion
	/// <summary>
	/// Adds a new entity to the database.
	/// </summary>
	/// <param name="entity">The entity to add.</param>
	/// <returns>A task that returns <c>true</c> if the entity was successfully added, otherwise <c>false</c>.</returns>
	internal async Task<bool> AddAsync(DbContext _dbContext, T entity)
	{
		try
		{
			await _dbContext.Set<T>().AddAsync(entity);
			var result = await _dbContext.SaveChangesAsync() > 0;

			return result;
		}
		catch (Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Updates specific properties of an existing entity that are attached to the context.
	/// </summary>
	/// <param name="entity">The entity with updated values.</param>
	/// <param name="updatedValues">A dictionary containing the property names and their updated values.</param>
	/// <returns>A task that returns <c>true</c> if the entity was successfully updated, otherwise <c>false</c>.</returns>
	internal async Task<bool> UpdateAttachedFieldsAsync(DbContext _dbContext, T entity)
	{
		try
		{
			if (entity == null) return false;

			// Attach entity if it's detached
			if (_dbContext.Entry(entity).State == EntityState.Detached)
			{
				_dbContext.Attach(entity);
			}

			// Get the entry for the entity
			var entry = _dbContext.Entry(entity);

			// Loop through each property of the entity
			foreach (var property in entry.OriginalValues.Properties)
			{
				// Get the original and current values
				var originalValue = entry.OriginalValues[property];
				var currentValue = entry.CurrentValues[property];

				// Compare the values
				if (originalValue == null && currentValue == null)
				{
					continue; // Skip if both values are null
				}

				if (originalValue is byte[] originalBytes && currentValue is byte[] currentBytes)
				{
					// Compare byte arrays
					if (!originalBytes.SequenceEqual(currentBytes))
					{
						entry.Property(property.Name).IsModified = true;
					}
				}
				else if (originalValue is DateTime originalDateTime && currentValue is DateTime currentDateTime)
				{
					// Compare DateTime values
					if (DateTime.Compare(originalDateTime, currentDateTime) != 0)
					{
						entry.Property(property.Name).IsModified = true;
					}
				}
				else if (!object.Equals(originalValue, currentValue))
				{
					// Compare other values (primitives, etc.)
					entry.Property(property.Name).IsModified = true;
				}
			}

			// Save changes and check if any records were updated
			var result = await _dbContext.SaveChangesAsync();
			return result > 0;
		}
		catch (Exception)
		{
			throw;
		}
	}


	/// <summary>
	/// Deletes an entity by its identifier.
	/// </summary>
	/// <param name="id">The identifier of the entity to delete.</param>
	/// <returns>A task that returns <c>true</c> if the entity was successfully deleted, otherwise <c>false</c>.</returns>
	internal async Task<bool> DeleteAsync<TKey>(DbContext _dbContext, Expression<Func<T, TKey>> fieldSelector, TKey value)
	{
		try
		{
			var entity = await GetByFieldAsync(_dbContext, fieldSelector, value);
			if (entity == null) return false;

			_dbContext.Set<T>().Remove(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}
		catch (Exception)
		{
			throw;
		}
	}
}
