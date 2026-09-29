using Booking.Application.Interfaces;
using Booking.Domain.Entities;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly ApplicationDbContext _dbContext;

    public HotelRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Hotel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Hotels
            .FirstOrDefaultAsync(h => h.Id == id ,cancellationToken);
    }

	public async Task<Hotel?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		return await _dbContext.Hotels
				.FirstOrDefaultAsync(h => h.Id == id && h.IsActive, cancellationToken);
	}
	public async Task<List<Hotel>> GetActiveAsync(
	string? city, string? country, int? starRating,
	int pageNumber, int pageSize, CancellationToken cancellationToken)
	{
		pageNumber = Math.Max(pageNumber, 1);
		pageSize = Math.Clamp(pageSize, 1, 100);

		return await BuildActiveQuery(city, country, starRating)
			 .OrderByDescending(h => h.CreatedAt)
			 .ThenBy(h => h.Id)
			 .Skip((pageNumber - 1) * pageSize)
			 .Take(pageSize)
			 .ToListAsync(cancellationToken);
	}

	public async Task<int> CountActiveAsync(string? city, string? country, int? starRating, CancellationToken cancellationToken)
    {
		return await BuildActiveQuery(city, country, starRating)
			.CountAsync(cancellationToken);
	}

    public async Task AddAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        await _dbContext.Hotels.AddAsync(hotel,cancellationToken);
    }

	public Task SaveChangesAsync(CancellationToken cancellationToken)
	 => _dbContext.SaveChangesAsync(cancellationToken);

	private IQueryable<Hotel> BuildActiveQuery(string? city, string? country, int? starRating)
	{
		var query = _dbContext.Hotels
				.AsNoTracking()
				.Where(h => h.IsActive);

		if (city != null)
			query = query.Where(h => h.City.Contains(city));

		if (country != null)
			query = query.Where(h => h.Country == country);

		if (starRating != null)
			query = query.Where(h => h.StarRating == starRating);

		return query;
	}
}