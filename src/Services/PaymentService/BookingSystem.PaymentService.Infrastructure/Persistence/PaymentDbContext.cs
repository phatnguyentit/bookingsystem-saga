using Microsoft.EntityFrameworkCore;

namespace BookingSystem.PaymentService.Infrastructure.Persistence;

public class Payment
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Payment>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            e.Property(p => p.Currency).HasMaxLength(3);
            e.Property(p => p.Status).HasMaxLength(20);
            e.ToTable("payments");
        });
    }
}

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
}

public class PaymentRepository(PaymentDbContext db) : IPaymentRepository
{
    public Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => db.Payments.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        await db.Payments.AddAsync(payment, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}
