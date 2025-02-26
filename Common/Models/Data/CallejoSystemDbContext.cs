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
    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<HolidaysVacations> HolidaysVacations { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\;Database=Callejo_System_DB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.IsPublished)
                .HasColumnName("is_published");
            entity.Property(e => e.UploadedAt)
                .HasColumnName("uploaded_at")
                .HasDefaultValueSql("GETUTCDATE()");
        });


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
            entity.Property(e => e.RegistrationDocument)
                .HasColumnType("varbinary(max)")
                .HasColumnName("registration_document");

            entity.HasOne(d => d.FkRoleNavigation).WithMany(p => p.CallejoIncUsers)
                .HasForeignKey(d => d.FkRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Callejo_Inc_Users_Role");


            modelBuilder.Entity<Child>(entity =>
            {
                entity.ToTable("Children");

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

            modelBuilder.Entity<HolidaysVacations>(entity =>
            {
                entity.ToTable("Holidays_Vacations");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasColumnName("title");

                entity.Property(e => e.Description)
                      .HasColumnName("description");

                entity.Property(e => e.StartDate)
                      .IsRequired()
                      .HasColumnName("start_date");

                entity.Property(e => e.EndDate)
                      .IsRequired()
                      .HasColumnName("end_date");

                entity.Property(e => e.Type)
                      .IsRequired()
                      .HasColumnName("type");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("GETUTCDATE()");
            });


            OnModelCreatingPartial(modelBuilder);
        }); 
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}