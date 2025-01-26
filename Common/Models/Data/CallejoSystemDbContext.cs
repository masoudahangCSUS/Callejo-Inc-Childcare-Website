using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Common.Models.Data;

public partial class CallejoSystemDbContext : DbContext
{
    public CallejoSystemDbContext()
    {
    }

    public CallejoSystemDbContext(DbContextOptions<CallejoSystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CallejoIncUser> CallejoIncUsers { get; set; }

    public virtual DbSet<Child> Children { get; set; }

    public virtual DbSet<InterestedParent> InterestedParents { get; set; }

    public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

    public virtual DbSet<PhoneNumbersType> PhoneNumbersTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\;Database=Callejo_System_DB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CallejoIncUser>(entity =>
        {
            entity.ToTable("Callejo_Inc_Users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.FkRole).HasColumnName("fk_role");
            entity.Property(e => e.LastName)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.State)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.FkRoleNavigation).WithMany(p => p.CallejoIncUsers)
                .HasForeignKey(d => d.FkRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Callejo_Inc_Users_Role");

            entity.HasMany(d => d.FkChildren).WithMany(p => p.FkParents)
                .UsingEntity<Dictionary<string, object>>(
                    "Guardian",
                    r => r.HasOne<Child>().WithMany()
                        .HasForeignKey("FkChildren")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Guardians_Children"),
                    l => l.HasOne<CallejoIncUser>().WithMany()
                        .HasForeignKey("FkParent")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Guardians_Callejo_Inc_Users"),
                    j =>
                    {
                        j.HasKey("FkParent", "FkChildren");
                        j.ToTable("Guardians");
                        j.IndexerProperty<Guid>("FkParent").HasColumnName("fk_parent");
                        j.IndexerProperty<long>("FkChildren").HasColumnName("fk_children");
                    });
        });

        modelBuilder.Entity<Child>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.FirstName)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("middle_name");
        });

        modelBuilder.Entity<InterestedParent>(entity =>
        {
            entity.ToTable("Interested_Parents");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ReasonForInquiry)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("reason_for_inquiry");
        });

        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.ToTable("Phone_Numbers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AreaCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("area_code");
            entity.Property(e => e.FkType).HasColumnName("fk_type");
            entity.Property(e => e.FkUsers).HasColumnName("fk_users");
            entity.Property(e => e.LastFour)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("last_four");
            entity.Property(e => e.Prefix)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("prefix");

            entity.HasOne(d => d.FkTypeNavigation).WithMany(p => p.PhoneNumbers)
                .HasForeignKey(d => d.FkType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phone_Numbers_Phone_Numbers_Type");

            entity.HasOne(d => d.FkUsersNavigation).WithMany(p => p.PhoneNumbers)
                .HasForeignKey(d => d.FkUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Phone_Numbers_Callejo_Inc_Users");
        });

        modelBuilder.Entity<PhoneNumbersType>(entity =>
        {
            entity.ToTable("Phone_Numbers_Type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
