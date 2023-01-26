using Microsoft.EntityFrameworkCore;

namespace SantaBackEnd
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Elf> Elves { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Santaclau> Santaclaus { get; set; } = null!;
        public virtual DbSet<Shiftblock> Shiftblocks { get; set; } = null!;
        public virtual DbSet<Shifttype> Shifttypes { get; set; } = null!;
        public virtual DbSet<Unavailableblock> Unavailableblocks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Workshop> Workshops { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost:5432;Username=postgres;Password=postgres;Database=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

            modelBuilder.Entity<Elf>(entity =>
            {
                entity.ToTable("elf");

                entity.HasIndex(e => e.Workshopid, "IX_Relationship3");

                entity.HasIndex(e => e.Roleid, "IX_Relationship9");

                entity.HasIndex(e => e.Userid, "IX_Relationship91");

                entity.Property(e => e.Elfid).HasColumnName("elfid");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Workshopid).HasColumnName("workshopid");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Elves)
                    .HasForeignKey(d => d.Roleid)
                    .HasConstraintName("Relationship5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Elves)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Relationship9");

                entity.HasOne(d => d.Workshop)
                    .WithMany(p => p.Elves)
                    .HasForeignKey(d => d.Workshopid)
                    .HasConstraintName("Relationship2");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Description)
                    .HasMaxLength(30)
                    .HasColumnName("description");

                entity.Property(e => e.Role1)
                    .HasMaxLength(10)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<Santaclau>(entity =>
            {
                entity.HasKey(e => e.Santaclausid)
                    .HasName("PK_SantaClaus");

                entity.ToTable("santaclaus");

                entity.HasIndex(e => e.Userid, "IX_Relationship8");

                entity.Property(e => e.Santaclausid).HasColumnName("santaclausid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Santaclaus)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Relationship8");
            });

            modelBuilder.Entity<Shiftblock>(entity =>
            {
                entity.HasKey(e => e.Shiftid)
                    .HasName("PK_ShiftBlock");

                entity.ToTable("shiftblock");

                entity.HasIndex(e => e.Shifttypeid, "IX_Relationship6");

                entity.HasIndex(e => e.Elfid, "IX_Relationship7");

                entity.Property(e => e.Shiftid).HasColumnName("shiftid");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Elfid).HasColumnName("elfid");

                entity.Property(e => e.Shifttypeid).HasColumnName("shifttypeid");

                entity.HasOne(d => d.Elf)
                    .WithMany(p => p.Shiftblocks)
                    .HasForeignKey(d => d.Elfid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Relationship7");

                entity.HasOne(d => d.Shifttype)
                    .WithMany(p => p.Shiftblocks)
                    .HasForeignKey(d => d.Shifttypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Relationship6");
            });

            modelBuilder.Entity<Shifttype>(entity =>
            {
                entity.ToTable("shifttype");

                entity.Property(e => e.Shifttypeid).HasColumnName("shifttypeid");

                entity.Property(e => e.Endtime).HasColumnName("endtime");

                entity.Property(e => e.Starttime).HasColumnName("starttime");

                entity.Property(e => e.Type)
                    .HasMaxLength(2)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Unavailableblock>(entity =>
            {
                entity.HasKey(e => e.Blockid)
                    .HasName("PK_UnavailableBlock");

                entity.ToTable("unavailableblock");

                entity.HasIndex(e => e.Elfid, "IX_Relationship10");

                entity.Property(e => e.Blockid).HasColumnName("blockid");

                entity.Property(e => e.Elfid).HasColumnName("elfid");

                entity.Property(e => e.Endtime)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("endtime");

                entity.Property(e => e.Note)
                    .HasMaxLength(20)
                    .HasColumnName("note");

                entity.Property(e => e.Starttime)
                    .HasColumnType("timestamp(0) without time zone")
                    .HasColumnName("starttime");

                entity.HasOne(d => d.Elf)
                    .WithMany(p => p.Unavailableblocks)
                    .HasForeignKey(d => d.Elfid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Relationship10");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");

                entity.Property(e => e.Dateofregistration).HasColumnName("dateofregistration");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(20)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(20)
                    .HasColumnName("lastname");

                entity.Property(e => e.username)
                    .HasMaxLength(35)
                    .HasColumnName("username");

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<Workshop>(entity =>
            {
                entity.ToTable("workshop");

                entity.HasIndex(e => e.Santaclausid, "IX_Relationship5");

                entity.Property(e => e.Workshopid).HasColumnName("workshopid");

                entity.Property(e => e.Location)
                    .HasMaxLength(40)
                    .HasColumnName("location");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasColumnName("name");

                entity.Property(e => e.Santaclausid).HasColumnName("santaclausid");

                entity.HasOne(d => d.Santaclaus)
                    .WithMany(p => p.Workshops)
                    .HasForeignKey(d => d.Santaclausid)
                    .HasConstraintName("Relationship1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
