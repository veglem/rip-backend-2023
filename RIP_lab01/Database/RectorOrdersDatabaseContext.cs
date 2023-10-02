using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RIP_lab01;

public partial class RectorOrdersDatabaseContext : DbContext
{
    public RectorOrdersDatabaseContext()
    {
    }

    public RectorOrdersDatabaseContext(DbContextOptions<RectorOrdersDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Moderator> Moderators { get; set; }

    public virtual DbSet<RectorOrder> RectorOrders { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<UniversityEmployee> UniversityEmployees { get; set; }

    public virtual DbSet<UnivesityUnit> UnivesityUnits { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=RectorOrdersDatabase;Username=veglem;Password=1812;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Client_pk");

            entity.ToTable("Client");

            entity.HasOne(d => d.User).WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Client_User_Id_fk");
        });

        modelBuilder.Entity<Moderator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Moderator_pk");

            entity.ToTable("Moderator");

            entity.HasOne(d => d.User).WithMany(p => p.Moderators)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Moderator_User_Id_fk");
        });

        modelBuilder.Entity<RectorOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RectorOrder_pk");

            entity.ToTable("RectorOrder");

            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EndDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.FormationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasMaxLength(40);

            entity.HasOne(d => d.Moderator).WithMany(p => p.RectorOrders)
                .HasForeignKey(d => d.ModeratorId)
                .HasConstraintName("RectorOrder_Moderator_Id_fk");

            entity.HasOne(d => d.Status).WithMany(p => p.RectorOrders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("RectorOrder_Status_Id_fk");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Request_pk");

            entity.ToTable("Request");

            entity.HasOne(d => d.Order).WithMany(p => p.Requests)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("Request_RectorOrder_Id_fk");

            entity.HasOne(d => d.Unit).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("Request_UnivesityUnit_Id_fk");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Status_pk");

            entity.ToTable("Status");

            entity.Property(e => e.Name).HasMaxLength(30);
        });

        modelBuilder.Entity<UniversityEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UniversityEmployee_pk");

            entity.ToTable("UniversityEmployee");

            entity.Property(e => e.Cabinet).HasMaxLength(50);
            entity.Property(e => e.Division).HasMaxLength(1000);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(70);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Number).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(100);

            entity.HasOne(d => d.Unit).WithMany(p => p.UniversityEmployees)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("UniversityEmployee_UnivesityUnit_Id_fk");
        });

        modelBuilder.Entity<UnivesityUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UnivesityUnit_pk");

            entity.ToTable("UnivesityUnit");

            entity.Property(e => e.ImgUrl).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pk");

            entity.ToTable("User");

            entity.Property(e => e.Passord).HasMaxLength(30);
            entity.Property(e => e.Username).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
