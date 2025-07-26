using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Entities.ValueObjects;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Psycheflow.Api.Contexts
{
    public class AppDbContext : IdentityDbContext<User>
    {
        #region [ DBSETS ]
        public DbSet<Company> Companies { get; set; }
        public DbSet<Psychologist> Psychologists { get; set; }
        public DbSet<PsychologistHours> PsychologistsHours { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        #endregion
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [ BASE ENTITY ]

            modelBuilder.Ignore<BaseEntity>();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var entity = modelBuilder.Entity(entityType.ClrType);

                    entity.Property<DateTime>("CreatedAt")
                          .HasDefaultValueSql("GETUTCDATE()")
                          .ValueGeneratedOnAdd();

                    entity.Property<DateTime?>("UpdatedAt")
                          .HasDefaultValueSql("GETUTCDATE()")
                          .ValueGeneratedOnUpdate();
                    entity.Property<DateTime?>("DeletedAt");
                }
            }
            #endregion

            #region [ USER ]
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property<DateTime>("CreatedAt")
                      .HasDefaultValueSql("GETUTCDATE()")
                      .ValueGeneratedOnAdd();

                entity.Property<DateTime?>("UpdatedAt")
                      .HasDefaultValueSql("GETUTCDATE()")
                      .ValueGeneratedOnUpdate();
                entity.Property<DateTime?>("DeletedAt");
            });
            #endregion

            #region [ Company ]
            modelBuilder.Entity<Company>(entity =>
            {

            });
            #endregion

            #region [ PSYCHOLOGIST ]


            modelBuilder.Entity<Psychologist>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.LicenseNumber)
                .HasConversion(
                    v => v.Value,
                    v => new LicenseNumber(v)
                )
                .HasMaxLength(20)
                .IsRequired();

            entity.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        });

            #endregion

            #region [ PSYCHOLOGIST HOURS ]

            modelBuilder.Entity<PsychologistHours>()
            .HasOne(ph => ph.Psychologist)
            .WithMany(p => p.PsychologistHours)
            .HasForeignKey(ph => ph.PsychologistId)
            .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region [ Schedules ]
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Psychologist)
                .WithMany(p => p.Schedules)
                .HasForeignKey(s => s.PsychologistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Patient)
                .WithMany(p => p.Schedules)
                .HasForeignKey(s => s.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Company)
                .WithMany(c => c.Schedules)
                .HasForeignKey(s => s.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region [ Sessions ]
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Schedule)
                .WithMany()
                .HasForeignKey(s => s.ScheduleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Psychologist)
                .WithMany(p => p.Sessions)
                .HasForeignKey(s => s.PsychologistId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Patient)
                .WithMany(p => p.Sessions)
                .HasForeignKey(s => s.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Company)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region [ PATIENT ]
            modelBuilder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            #endregion
            #region [ Payments ]
            #endregion
        }

    }
}
