using BackendDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotNet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<DonorProfile> DonorProfiles { get; set; }
        public DbSet<HospitalProfile> HospitalProfiles { get; set; }
        public DbSet<BloodInventory> BloodInventories { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }
        public DbSet<DonationAppointment> DonationAppointments { get; set; }
        public DbSet<DonationRecord> DonationRecords { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                
                // Configure relationship with Role
                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.HasIndex(e => e.RoleName).IsUnique();
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(20);
            });

            // Configure DonorProfile entity
            modelBuilder.Entity<DonorProfile>(entity =>
            {
                entity.HasKey(e => e.ProfileId);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.BloodGroup).IsRequired().HasMaxLength(5);
                entity.Property(e => e.ContactNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.IsEligible).HasDefaultValue(true);
                
                // Configure relationship with User
                entity.HasOne(e => e.User)
                    .WithOne(u => u.DonorProfile)
                    .HasForeignKey<DonorProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure HospitalProfile entity
            modelBuilder.Entity<HospitalProfile>(entity =>
            {
                entity.HasKey(e => e.ProfileId);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.HasIndex(e => e.LicenseNumber).IsUnique();
                entity.Property(e => e.HospitalName).IsRequired();
                entity.Property(e => e.Address).IsRequired();
                entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContactPerson).IsRequired();
                entity.Property(e => e.IsVerified).HasDefaultValue(false);
                
                // Configure relationship with User
                entity.HasOne(e => e.User)
                    .WithOne(u => u.HospitalProfile)
                    .HasForeignKey<HospitalProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure BloodInventory entity
            modelBuilder.Entity<BloodInventory>(entity =>
            {
                entity.HasKey(e => e.InventoryId);
                entity.HasIndex(e => e.BloodGroup).IsUnique();
                entity.Property(e => e.BloodGroup).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Units).IsRequired();
                entity.Property(e => e.StatusId).HasDefaultValue(1);
            });

            // Configure BloodRequest entity
            modelBuilder.Entity<BloodRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);
                entity.Property(e => e.BloodGroup).IsRequired().HasMaxLength(5);
                entity.Property(e => e.UnitsRequested).IsRequired();
                entity.Property(e => e.Urgency).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.IsFulfilled).HasDefaultValue(false);
                
                // Configure relationship with User (Hospital)
                entity.HasOne(e => e.Hospital)
                    .WithMany(u => u.BloodRequests)
                    .HasForeignKey(e => e.HospitalUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure DonationAppointment entity
            modelBuilder.Entity<DonationAppointment>(entity =>
            {
                entity.HasKey(e => e.AppointmentId);
                entity.Property(e => e.AppointmentDate).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                
                // Configure relationship with User (Donor)
                entity.HasOne(e => e.Donor)
                    .WithMany(u => u.DonationAppointments)
                    .HasForeignKey(e => e.DonorUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure DonationRecord entity
            modelBuilder.Entity<DonationRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId);
                entity.Property(e => e.DonationDate).IsRequired();
                entity.Property(e => e.UnitsDonated).IsRequired().HasDefaultValue(1);
                
                // Configure relationship with User (Donor)
                entity.HasOne(e => e.Donor)
                    .WithMany(u => u.DonationRecords)
                    .HasForeignKey(e => e.DonorUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ContactMessage entity
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.IsRead).HasDefaultValue(false);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "ROLE_ADMIN" },
                new Role { RoleId = 2, RoleName = "ROLE_DONOR" },
                new Role { RoleId = 3, RoleName = "ROLE_HOSPITAL" }
            );

            // Seed Blood Groups in Inventory
            var bloodGroups = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            var bloodInventories = bloodGroups.Select((bg, index) => new BloodInventory
            {
                InventoryId = index + 1,
                BloodGroup = bg,
                Units = 0,
                StatusId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToArray();

            modelBuilder.Entity<BloodInventory>().HasData(bloodInventories);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Property("CreatedAt") != null)
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entry.Property("UpdatedAt") != null)
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
