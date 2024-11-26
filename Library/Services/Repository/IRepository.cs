
namespace Library.Services.Repository
{
	/// <summary>
	/// Interface for the repository providing standard CRUD operations for entities of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the entity for which CRUD operations are provided.</typeparam>
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Gets an entity by its identifier.
		/// </summary>
		/// <param name="id">The identifier of the entity.</param>
		/// <returns>An asynchronous task that returns the entity if found, or <c>null</c> if not found.</returns>
		Task<T> GetByIdAsync(long id);

		/// <summary>
		/// Gets all entities.
		/// </summary>
		/// <returns>An asynchronous task that returns a collection of all entities.</returns>
		Task<IEnumerable<T>> GetAllAsync();

		/// <summary>
		/// Adds a new entity.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		/// <returns>An asynchronous task that returns <c>true</c> if the entity was successfully added; otherwise, <c>false</c>.</returns>
		Task<bool> AddAsync(T entity);

		/// <summary>
		/// Updates an existing entity.
		/// </summary>
		/// <param name="entity">The entity with updated data.</param>
		/// <returns>An asynchronous task that returns <c>true</c> if the entity was successfully updated; otherwise, <c>false</c>.</returns>
		Task<bool> UpdateAsync(T entity);

		/// <summary>
		/// Deletes an entity by its identifier.
		/// </summary>
		/// <param name="id">The identifier of the entity to delete.</param>
		/// <returns>An asynchronous task that returns <c>true</c> if the entity was successfully deleted; otherwise, <c>false</c>.</returns>
		Task<bool> DeleteAsync(long id);
	}
}
