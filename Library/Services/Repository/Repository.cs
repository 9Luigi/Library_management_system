using Library;
using Microsoft.EntityFrameworkCore;

public class Repository<T> where T: class
{
	private readonly LibraryContextForEFcore _dbContext;

	/// <summary>
	/// Инициализация нового экземпляра репозитория.
	/// </summary>
	/// <param name="dbContext">Контекст базы данных для работы с сущностями.</param>
	internal Repository(LibraryContextForEFcore dbContext)
	{
		_dbContext = dbContext;
	}

	/// <summary>
	/// Получение сущности по идентификатору.
	/// </summary>
	/// <param name="id">Идентификатор сущности.</param>
	/// <returns>Асинхронная задача, которая возвращает сущность, если она найдена, или <c>null</c>, если сущности не существует.</returns>
	public async Task<T> GetByIdAsync(long id)
	{
		return await _dbContext.Set<T>().FindAsync(id);
	}

	/// <summary>
	/// Получение всех сущностей.
	/// </summary>
	/// <returns>Асинхронная задача, которая возвращает коллекцию всех сущностей.</returns>
	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _dbContext.Set<T>().ToListAsync();
	}

	/// <summary>
	/// Добавление новой сущности.
	/// </summary>
	/// <param name="entity">Сущность для добавления.</param>
	/// <returns>Асинхронная задача, которая возвращает <c>true</c>, если сущность успешно добавлена, иначе <c>false</c>.</returns>
	public async Task<bool> AddAsync(T entity)
	{
		await _dbContext.Set<T>().AddAsync(entity);
		return await _dbContext.SaveChangesAsync() > 0;
	}

	/// <summary>
	/// Обновление существующей сущности.
	/// </summary>
	/// <param name="entity">Сущность с обновленными данными.</param>
	/// <returns>Асинхронная задача, которая возвращает <c>true</c>, если сущность успешно обновлена, иначе <c>false</c>.</returns>
	public async Task<bool> UpdateAsync(T entity)
	{
		_dbContext.Set<T>().Update(entity);
		return await _dbContext.SaveChangesAsync() > 0;
	}

	/// <summary>
	/// Удаление сущности по идентификатору.
	/// </summary>
	/// <param name="id">Идентификатор сущности для удаления.</param>
	/// <returns>Асинхронная задача, которая возвращает <c>true</c>, если сущность успешно удалена, иначе <c>false</c>.</returns>
	public async Task<bool> DeleteAsync(long id)
	{
		var entity = await GetByIdAsync(id);
		if (entity == null) return false;

		_dbContext.Set<T>().Remove(entity);
		return await _dbContext.SaveChangesAsync() > 0;
	}
}
