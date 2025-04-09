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

    public virtual DbSet<DailySchedule> DailySchedules { get; set; }

    public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<FileUpload> FileUploads { get; set; }


    public virtual DbSet<HolidaysVacation> HolidaysVacations { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<InterestedParent> InterestedParents { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

    public virtual DbSet<PhoneNumbersType> PhoneNumbersTypes { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserSecret> UserSecrets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=Callejo_System_DB;Trusted_Connection=True;TrustServerCertificate=True");

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
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RegistrationDocument).HasColumnName("registration_document");
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

        modelBuilder.Entity<DailySchedule>(entity =>
        {
            entity.ToTable("Daily_Schedule");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DescSpecial).HasColumnName("desc_special");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<EmergencyContact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Emergenc__3213E83F720F3CF7");

            entity.ToTable("Emergency_Contact");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.FkUsers).HasColumnName("fk_users");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Relationship)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("relationship");

            entity.HasOne(d => d.FkUsersNavigation).WithMany(p => p.EmergencyContacts)
                .HasForeignKey(d => d.FkUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Callejo_Inc_Users");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC072C37F3BF");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Category).HasMaxLength(15);
            entity.Property(e => e.Note).HasMaxLength(300);
        });

        modelBuilder.Entity<FileUpload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FileUplo__3214EC07E90E0243");

            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.DocumentType).HasMaxLength(100);
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.UploadDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HolidaysVacation>(entity =>
        {
            entity.ToTable("Holidays_Vacations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Images__3213E83F8C33DC9C");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(1024)
                .HasColumnName("image_url");
            entity.Property(e => e.IsPublished)
                .HasDefaultValue(false)
                .HasColumnName("is_published");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("uploaded_at");
        });

        modelBuilder.Entity<InterestedParent>(entity =>
        {
            entity.ToTable("Interested_Parents");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Datetime).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.ReasonForInquiry)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("reason_for_inquiry");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB5251C5437");

            entity.Property(e => e.InvoiceId).ValueGeneratedNever();
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.GuardianName).HasMaxLength(100);
            entity.Property(e => e.LastPaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TransactionReference).HasMaxLength(100);
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Username);

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.FkCallejoIncUser).HasColumnName("fkCallejoIncUser");
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");

            entity.HasOne(d => d.FkCallejoIncUserNavigation).WithMany(p => p.Logins)
                .HasForeignKey(d => d.FkCallejoIncUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Logins_Callejo_Inc_Users");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.SentOn).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.FkParent).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.FkParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_Callejo_Inc_Users");
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

        modelBuilder.Entity<Registration>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Registration");

            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        modelBuilder.Entity<UserSecret>(entity =>
        {
            entity.HasKey(e => e.FkUser);

            entity.Property(e => e.FkUser)
                .ValueGeneratedNever()
                .HasColumnName("fk_user");
            entity.Property(e => e.Secret)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("secret");

            entity.HasOne(d => d.FkUserNavigation).WithOne(p => p.UserSecret)
                .HasForeignKey<UserSecret>(d => d.FkUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSecrets_CallejoIncUsers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
