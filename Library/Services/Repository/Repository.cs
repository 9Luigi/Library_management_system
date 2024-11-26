using Library.Services.Repository;
using Library;
using Microsoft.EntityFrameworkCore;

namespace Library.Services.Repository
{
	/// <summary>
	/// Repository class implementing the <see cref="IRepository{T}"/> interface.
	/// Provides CRUD operations for entities of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the entity to perform CRUD operations on.</typeparam>
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly LibraryContextForEFcore _dbContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="Repository{T}"/> class.
		/// </summary>
		/// <param name="dbContext">The <see cref="LibraryContextForEFcore"/> context to interact with the database.</param>
		internal Repository(LibraryContextForEFcore dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// Retrieves an entity by its identifier.
		/// </summary>
		/// <param name="id">The identifier of the entity.</param>
		/// <returns>An asynchronous task that returns the entity if found, or <c>null</c> if the entity does not exist.</returns>
		public async Task<T> GetByIdAsync(long id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		/// <summary>
		/// Retrieves all entities of type <typeparamref name="T"/>.
		/// </summary>
		/// <returns>An asynchronous task that returns a collection of all entities.</returns>
		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbContext.Set<T>().ToListAsync();
		}

		/// <summary>
		/// Adds a new entity of type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		/// <returns>An asynchronous task that returns <c>true</c> if the entity was successfully added, otherwise <c>false</c>.</returns>
		public async Task<bool> AddAsync(T entity)
		{
			await _dbContext.Set<T>().AddAsync(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}

		/// <summary>
		/// Updates an existing entity of type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="entity">The entity with updated data.</param>
		/// <returns>An asynchronous task that returns <c>true</c> if the entity was successfully updated, otherwise <c>false</c>.</returns>
		public async Task<bool> UpdateAsync(T entity)
		{
			_dbContext.Set<T>().Update(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}

		/// <summary>
		/// Deletes an entity of type <typeparamref name="T"/> by its identifier.
		/// </summary>
		/// <param name="id">The identifier of the entity to delete.</param>
		/// <returns>An asynchronous task that returns <c>true</c> if the entity was successfully deleted, otherwise <c>false</c>.</returns>
		public async Task<bool> DeleteAsync(long id)
		{
			var entity = await GetByIdAsync(id);
			if (entity == null) return false;

			_dbContext.Set<T>().Remove(entity);
			return await _dbContext.SaveChangesAsync() > 0;
		}
	}
}
