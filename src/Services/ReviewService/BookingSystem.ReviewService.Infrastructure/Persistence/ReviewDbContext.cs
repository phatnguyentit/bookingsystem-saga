using Microsoft.EntityFrameworkCore;

namespace BookingSystem.ReviewService.Infrastructure.Persistence;

public class Review
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid ListingId { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ReviewDbContext(DbContextOptions<ReviewDbContext> options) : DbContext(options)
{
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Review>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Rating);
            e.Property(r => r.Comment).HasMaxLength(2000);
            e.ToTable("reviews");
        });
    }
}

public interface IReviewRepository
{
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetByListingAsync(Guid listingId, CancellationToken cancellationToken = default);
}

public class ReviewRepository(ReviewDbContext db) : IReviewRepository
{
    public async Task AddAsync(Review review, CancellationToken cancellationToken = default)
    {
        await db.Reviews.AddAsync(review, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetByListingAsync(Guid listingId, CancellationToken cancellationToken = default)
        => await db.Reviews
            .Where(r => r.ListingId == listingId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
}
