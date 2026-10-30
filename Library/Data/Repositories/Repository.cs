using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Library.Data.Repositories;

public class Repository<T> where T : class //TODO repositoty only for basic CRUD operations
{

	// Retrieves an entity by a specific field value from the database
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

	// Extracts the property name from an expression that selects a property of the entity
	private string GetPropertyName<TKey>(Expression<Func<T, TKey>> expression)
	{
		if (expression.Body is MemberExpression memberExpression)
		{
			return memberExpression.Member.Name;
		}
		throw new ArgumentException("The expression does not refer to a valid property.", nameof(expression));
	}

	// Retrieves an entity by its primary key value from the database
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

	// Adds a new entity to the database
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

	// Updates specific properties of an existing entity that are attached to the context
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

	// Deletes an entity by its identifier
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

	// Asynchronously checks if any record in the database matches the specified condition
	public async Task<bool> AnyAsync(DbContext context, Expression<Func<T, bool>> predicate)
	{
		if (context == null) throw new ArgumentNullException(nameof(context));
		if (predicate == null) throw new ArgumentNullException(nameof(predicate));

		return await context.Set<T>().AnyAsync(predicate);
	}

}