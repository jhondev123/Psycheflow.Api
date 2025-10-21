using FastReport.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Entities.Configs;
using Psycheflow.Api.Entities.ValueObjects;

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
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentField> DocumentFields { get; set; }
        public DbSet<Entities.Configs.Config> Configs { get; set; }
        public DbSet<ConfigAi> ConfigAis { get; set; }

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
                          .HasColumnType("timestamp")                
                          .HasDefaultValueSql("CURRENT_TIMESTAMP")   
                          .ValueGeneratedOnAdd();

                    entity.Property<DateTime?>("UpdatedAt")
                          .HasColumnType("timestamp")
                          .HasDefaultValueSql("CURRENT_TIMESTAMP")
                          .ValueGeneratedOnUpdate();

                    entity.Property<DateTime?>("DeletedAt");
                }
            }
            #endregion

            #region [ USER ]
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property<DateTime>("CreatedAt")
                      .HasColumnType("timestamp")                 
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")   
                      .ValueGeneratedOnAdd();

                entity.Property<DateTime?>("UpdatedAt")
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
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
            modelBuilder.Entity<PsychologistHours>(entity =>
            {
                entity.HasOne(ph => ph.Psychologist)
                      .WithMany(p => p.PsychologistHours)
                      .HasForeignKey(ph => ph.PsychologistId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ph => ph.DayOfWeek)
                      .HasConversion<int>()
                      .IsRequired();

                entity.Property(ph => ph.StartTime)
                      .HasColumnType("time")
                      .IsRequired();

                entity.Property(ph => ph.EndTime)
                      .HasColumnType("time")
                      .IsRequired();
            });
            #endregion

            #region [ Schedules ]
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasOne(s => s.Psychologist)
                      .WithMany(p => p.Schedules)
                      .HasForeignKey(s => s.PsychologistId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Company)
                      .WithMany(c => c.Schedules)
                      .HasForeignKey(s => s.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(s => s.ScheduleTypes)
                      .HasConversion<int>()
                      .IsRequired();

                entity.Property(s => s.ScheduleStatus)
                      .HasConversion<int>()
                      .IsRequired();

                entity.Property(s => s.Date)
                  .HasColumnType("timestamp");

                entity.Property(s => s.Id).HasColumnOrder(0);
                entity.Property(s => s.CompanyId).HasColumnOrder(1);
                entity.Property(s => s.PsychologistId).HasColumnOrder(2);
                entity.Property(s => s.Date).HasColumnOrder(3);
                entity.Property(s => s.Start).HasColumnOrder(4);
                entity.Property(s => s.End).HasColumnOrder(5);
                entity.Property(s => s.ScheduleTypes).HasColumnOrder(6);
                entity.Property(s => s.ScheduleStatus).HasColumnOrder(7);
                entity.Property(s => s.CreatedAt).HasColumnOrder(98);
                entity.Property(s => s.UpdatedAt).HasColumnOrder(99);
                entity.Property(s => s.DeletedAt).HasColumnOrder(100);
            });
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

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(s => s.SessionStatus)
                  .HasConversion<int>()
                  .IsRequired();

                entity.Property(s => s.Id).HasColumnOrder(0);
                entity.Property(s => s.CompanyId).HasColumnOrder(1);
                entity.Property(s => s.ScheduleId).HasColumnOrder(2);
                entity.Property(s => s.PsychologistId).HasColumnOrder(3);
                entity.Property(s => s.PatientId).HasColumnOrder(4);
                entity.Property(s => s.Feedback).HasColumnOrder(5);
                entity.Property(s => s.Description).HasColumnOrder(6);
                entity.Property(s => s.SessionStatus).HasColumnOrder(7);
                entity.Property(s => s.CreatedAt).HasColumnOrder(98);
                entity.Property(s => s.UpdatedAt).HasColumnOrder(99);
                entity.Property(s => s.DeletedAt).HasColumnOrder(100);

            });

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

            #region [ DOCUMENT ]

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Documents");

                entity.HasKey(d => d.Id);

                entity.Property(d => d.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(d => d.Description)
                      .HasMaxLength(500);

                entity.HasOne(d => d.Company)
                      .WithMany()
                      .HasForeignKey(d => d.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region [ DOCUMENT FIELD ]

            modelBuilder.Entity<DocumentField>(entity =>
            {
                entity.ToTable("DocumentFields");

                entity.HasKey(f => f.Id);

                entity.Property(f => f.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(f => f.Order)
                      .IsRequired();

                entity.Property(f => f.IsRequired)
                      .IsRequired();

                entity.Property(f => f.DefaultValue)
                      .HasMaxLength(500);

                entity.HasOne(f => f.Document)
                      .WithMany(d => d.Fields)
                      .HasForeignKey(f => f.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Ignore(f => f.Value);

            });

            #endregion

            #region [ CONFIG ]

            modelBuilder.Entity<Entities.Configs.Config>(entity =>
            {
                entity.ToTable("Config");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Key)
                      .HasMaxLength(255)
                      .IsRequired();

                // Constraint única (CompanyId, UserId, Key)
                entity.HasIndex(c => new { c.CompanyId, c.UserId, c.Key })
                      .IsUnique()
                      .HasDatabaseName("UQ_Config");

                // Relacionamentos
                entity.HasOne(c => c.Company)
                      .WithMany(co => co.Configs)
                      .HasForeignKey(c => c.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.User)
                      .WithMany(u => u.Configs)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #region [ CONFIG AI ]
            // tabela de ConfigAi

            modelBuilder.Entity<ConfigAi>(entity =>
            {
                entity.ToTable("ConfigAi");

                entity.HasKey(ai => ai.Id);

                entity.HasOne(ai => ai.Config)
                      .WithOne(c => c.ConfigAi)
                      .HasForeignKey<ConfigAi>(ai => ai.ConfigId);
            });

            #endregion

            #endregion
        }
    }

}

