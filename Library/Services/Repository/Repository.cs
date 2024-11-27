using Library;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;

public class Repository<T> where T : class
{
	internal readonly LibraryContextForEFcore _dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="Repository{T}"/> class.
	/// </summary>
	/// <param name="dbContext">The database context used for interacting with the entities.</param>
	internal Repository(LibraryContextForEFcore dbContext)
	{
		_dbContext = dbContext;
	}

	/// <summary>
	/// Retrieves an entity by its identifier.
	/// </summary>
	/// <param name="id">The identifier of the entity.</param>
	/// <returns>A task that returns the entity if found, or <c>null</c> if not found.</returns>
	internal async Task<T> GetByIdAsync(long id)
	{
		try
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}
		catch (Exception ex)
		{
			// Log or handle the exception as needed
			MessageBox.Show($"An error occurred while fetching the entity: {ex.Message}");
			return null;
		}
	}

	/// <summary>
	/// Retrieves all entities of type <typeparamref name="T"/>.
	/// </summary>
	/// <returns>A task that returns a collection of all entities.</returns>
	internal async Task<IEnumerable<T>> GetAllAsync()
	{
		try
		{
			return await _dbContext.Set<T>().ToListAsync();
		}
		catch (Exception ex)
		{
			// Log or handle the exception as needed
			MessageBox.Show($"An error occurred while fetching all entities: {ex.Message}. Inner exception:  {ex.InnerException?.Message ?? "None"}");
			return Enumerable.Empty<T>();
		}
	}

	/// <summary>
	/// Adds a new entity to the database.
	/// </summary>
	/// <param name="entity">The entity to add.</param>
	/// <returns>A task that returns <c>true</c> if the entity was successfully added, otherwise <c>false</c>.</returns>
	internal async Task<bool> AddAsync(T entity)
	{
		using (var transaction = await _dbContext.Database.BeginTransactionAsync())
		{
			try
			{
				// Temporarily allow inserting explicit values for the identity column
				await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Members ON");
				await _dbContext.Set<T>().AddAsync(entity);

				var result = await _dbContext.SaveChangesAsync() > 0;

				// Disable the identity insert after the operation
				await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Members OFF");
				await transaction.CommitAsync();

				return result;
			}
			catch (DbUpdateException ex)
			{
				// Log or handle the exception as needed
				MessageBox.Show($"Database update error while adding the entity: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "None"}");
				return false;
			}
			catch (Exception ex)
			{
				// Rollback the transaction if an error occurs
				await transaction.RollbackAsync();

				// Log or handle the exception as needed
				MessageBox.Show($"An error occurred while adding the entity: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "None"}");
				return false;
			}
		}
	}

	/// <summary>
	/// Updates an existing entity in the database.
	/// </summary>
	/// <param name="entity">The entity with updated data.</param>
	/// <returns>A task that returns <c>true</c> if the entity was successfully updated, otherwise <c>false</c>.</returns>
	internal async Task<bool> UpdateAsync(T entity)
	{
		try
		{
			_dbContext.Set<T>().Update(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}
		catch (DbUpdateException ex)
		{
			// Log or handle the exception as needed
			MessageBox.Show($"Database update error while updating the entity: {ex.Message}. Inner exception:  {ex.InnerException?.Message ?? "None"}");
			return false;
		}
		catch (Exception ex)
		{
			// Log or handle the exception as needed
			MessageBox.Show($"An error occurred while updating the entity: {ex.Message}. Inner exception:  {ex.InnerException?.Message ?? "None"}");
			return false;
		}
	}

	/// <summary>
	/// Updates specific properties of an existing entity that are attached to the context.
	/// </summary>
	/// <param name="entity">The entity with updated values.</param>
	/// <param name="updatedValues">A dictionary containing the property names and their updated values.</param>
	/// <returns>A task that returns <c>true</c> if the entity was successfully updated, otherwise <c>false</c>.</returns>
	internal async Task<bool> UpdateAttachedAsync(T entity)
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
		catch (DbUpdateException ex)
		{
			// Log or handle the exception as needed
			MessageBox.Show($"Database update error while updating attached entity: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "None"}");
			return false;
		}
		catch (Exception ex)
		{
			// Log or handle the exception as needed
			MessageBox.Show($"An error occurred while updating attached entity: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "None"}");
			return false;
		}
	}


	/// <summary>
	/// Deletes an entity by its identifier.
	/// </summary>
	/// <param name="id">The identifier of the entity to delete.</param>
	/// <returns>A task that returns <c>true</c> if the entity was successfully deleted, otherwise <c>false</c>.</returns>
	internal async Task<bool> DeleteAsync(long id)
	{
		try
		{
			var entity = await GetByIdAsync(id);
			if (entity == null) return false;

			_dbContext.Set<T>().Remove(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}
		catch (DbUpdateException ex)
		{
			// Log or handle the exception as needed
			MessageBox.Show($"Database update error while deleting the entity: {ex.Message}. Inner exception:  {ex.InnerException?.Message ?? "None"}");
			return false;
		}
		catch (Exception ex)
		{
			// Log or handle the exception as needed
			MessageBox.Show($"An error occurred while deleting the entity: {ex.Message}. Inner exception:  {ex.InnerException?.Message ?? "None"}");
			return false;
		}
	}
}
