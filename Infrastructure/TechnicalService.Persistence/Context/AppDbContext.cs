using Microsoft.EntityFrameworkCore;
using TechnicalService.Domain.Entities;

namespace TechnicalService.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
        public DbSet<SerialNumber> SerialNumbers { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }
        public DbSet<ServiceRecord> ServiceRecords { get; set; }
        public DbSet<ServiceRecordStep> ServiceRecordSteps { get; set; }
        public DbSet<Domain.Entities.TechnicalService> TechnicalServices { get; set; }
        public DbSet<PhoneNumberVerificationCode> PhoneNumberVerificationCodes { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<LegalDocument> LegalDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         
            modelBuilder.Entity<ServiceRecord>()
                .HasOne(r => r.User)
                .WithMany(u => u.ServiceRecords)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceRecord>()
                .HasOne(x => x.Personnel)
                .WithMany(p => p.ServiceRecords)
                .HasForeignKey(x => x.PersonnelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceRecordStep>()
                .HasOne(x => x.Personnel)
                .WithMany(p => p.ServiceRecordSteps)
                .HasForeignKey(x => x.PersonnelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.RefreshToken)
                    .IsUnique();
            });
            
            modelBuilder.Entity<Personnel>(entity =>
            {
                entity.HasIndex(x => x.RefreshToken)
                    .IsUnique();
            });
        }
    }
}
