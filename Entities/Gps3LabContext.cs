using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace test_lab.Entities;

public partial class Gps3LabContext : DbContext
{
    public Gps3LabContext()
    {
    }

    public Gps3LabContext(DbContextOptions<Gps3LabContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminUser> AdminUsers { get; set; }

    public virtual DbSet<AdminUserVehicleGroup> AdminUserVehicleGroups { get; set; }

    public virtual DbSet<BcaLicenseType> BcaLicenseTypes { get; set; }

    public virtual DbSet<CompanyCompany> CompanyCompanies { get; set; }

    public virtual DbSet<HrmEmployee> HrmEmployees { get; set; }

    public virtual DbSet<VehicleGroup> VehicleGroups { get; set; }

    public virtual DbSet<VehicleVehicle> VehicleVehicles { get; set; }

    public virtual DbSet<VehicleVehicleGroup> VehicleVehicleGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=192.168.45.49\\BASQL;Initial Catalog=GPS3_LAB;User ID=gps3_lab;Password=gps3_lab;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasKey(e => e.PkUserId);

            entity.ToTable("Admin.Users");

            entity.HasIndex(e => e.Username, "uc_Admin.Users_UserName").IsUnique();

            entity.Property(e => e.PkUserId)
                .ValueGeneratedNever()
                .HasColumnName("PK_UserID");
            entity.Property(e => e.ActivedDate).HasColumnType("datetime");
            entity.Property(e => e.AllowedAccessIp)
                .IsUnicode(false)
                .HasColumnName("AllowedAccessIP");
            entity.Property(e => e.ChangePasswordAfterDays)
                .HasDefaultValue((short)0)
                .HasComment("<= 0 : không yêu c?u ??i m?t kh?u, N : s? ngày yêu c?u ??i sau");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CreatedIP");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExtendChangePasswordDays).HasDefaultValue((short)5);
            entity.Property(e => e.FkCompanyId).HasColumnName("FK_CompanyID");
            entity.Property(e => e.Fullname).HasMaxLength(250);
            entity.Property(e => e.IsActived).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsLock).HasComment("M?c ??nh là không khóa");
            entity.Property(e => e.KeepWeakPasswordDate).HasColumnType("datetime");
            entity.Property(e => e.LastLoginDate)
                .HasComment("Th?i ?i?m ??ng nh?p g?n ?ây")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastPasswordChanged)
                .HasComment("Th?i ?i?m thay ??i m?t kh?u g?n ?ây")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LoginType)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RequiredChangePasswordDays).HasDefaultValue((short)3);
            entity.Property(e => e.SuperiorSaleId).HasColumnName("SuperiorSaleID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UpdatedIP");
            entity.Property(e => e.UseSecurityCodeSms).HasColumnName("UseSecurityCodeSMS");
            entity.Property(e => e.UserNameLower)
                .HasMaxLength(50)
                .HasComment("Tên ??ng nh?p d?ng ch? th??ng h?t.");
            entity.Property(e => e.UserType).HasComment("0: Normal, 1 : Administrator");
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.UsernameBap)
                .HasMaxLength(300)
                .HasColumnName("UsernameBAP");

            entity.HasOne(d => d.FkCompany).WithMany(p => p.AdminUsers)
                .HasForeignKey(d => d.FkCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Company");
        });

        modelBuilder.Entity<AdminUserVehicleGroup>(entity =>
        {
            entity.HasKey(e => new { e.FkUserId, e.FkVehicleGroupId }).HasName("PK_Admin.User_VehicleGroup");

            entity.ToTable("Admin.UserVehicleGroup");

            entity.Property(e => e.FkUserId).HasColumnName("FK_UserID");
            entity.Property(e => e.FkVehicleGroupId).HasColumnName("FK_VehicleGroupID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ParentVehicleGroupId).HasColumnName("ParentVehicleGroupID");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<BcaLicenseType>(entity =>
        {
            entity.HasKey(e => e.PkLicenseTypeId);

            entity.ToTable("BCA.LicenseTypes");

            entity.Property(e => e.PkLicenseTypeId)
                .ValueGeneratedNever()
                .HasColumnName("PK_LicenseTypeID");
            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<CompanyCompany>(entity =>
        {
            entity.HasKey(e => e.PkCompanyId).HasName("Pk_Companies");

            entity.ToTable("Company.Companies");

            entity.Property(e => e.PkCompanyId)
                .ValueGeneratedNever()
                .HasColumnName("PK_CompanyID");
            entity.Property(e => e.AccountantCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.CompanyName).HasMaxLength(250);
            entity.Property(e => e.CompanyType).HasComment("1 : BinhAnh\r\n2: Partner\r\n3: EndUser");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.DateKcschecked)
                .HasDefaultValueSql("((0))")
                .HasColumnType("datetime")
                .HasColumnName("DateKCSChecked");
            entity.Property(e => e.DateOfEstablishment).HasColumnType("smalldatetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FkProvindeId).HasColumnName("FK_ProvindeID");
            entity.Property(e => e.Flags).HasDefaultValue((short)0);
            entity.Property(e => e.HasSimService).HasComment("Sim tr? ti?n");
            entity.Property(e => e.IsBgtforward).HasColumnName("IsBGTForward");
            entity.Property(e => e.IsBlockXncode).HasColumnName("IsBlockXNCode");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);
            entity.Property(e => e.Kcschecked).HasColumnName("KCSChecked");
            entity.Property(e => e.ListOfXnforPartner)
                .IsUnicode(false)
                .HasColumnName("ListOfXNForPartner");
            entity.Property(e => e.LogoImagePath).IsUnicode(false);
            entity.Property(e => e.ParentCompanyId).HasColumnName("ParentCompanyID");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrivateCompanyName).HasMaxLength(250);
            entity.Property(e => e.ReasonOfDeleted).HasMaxLength(250);
            entity.Property(e => e.ReasonOfLocked).HasMaxLength(250);
            entity.Property(e => e.StartLogfileTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TaxCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDateIsBlockXncode)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedDateIsBlockXNCode");
            entity.Property(e => e.UserKcschecked).HasColumnName("UserKCSChecked");
            entity.Property(e => e.Website)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Xncode).HasColumnName("XNCode");
        });

        modelBuilder.Entity<HrmEmployee>(entity =>
        {
            entity.HasKey(e => e.PkEmployeeId);

            entity.ToTable("HRM.Employees");

            entity.Property(e => e.PkEmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("PK_EmployeeID");
            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.DriverAvatar).IsUnicode(false);
            entity.Property(e => e.DriverImage).IsUnicode(false);
            entity.Property(e => e.DriverLicense)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeType).HasDefaultValue((byte)1);
            entity.Property(e => e.ExpireLicenseDate).HasColumnType("datetime");
            entity.Property(e => e.FkCompanyId).HasColumnName("FK_CompanyID");
            entity.Property(e => e.FkDepartmentId)
                .HasDefaultValue(-1)
                .HasColumnName("FK_DepartmentID");
            entity.Property(e => e.FkUserId).HasColumnName("FK_UserID");
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.IssueLicenseDate).HasColumnType("datetime");
            entity.Property(e => e.IssueLicensePlace).HasMaxLength(150);
            entity.Property(e => e.LockDate).HasColumnType("datetime");
            entity.Property(e => e.Mobile)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber1)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber2)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VehicleGroup>(entity =>
        {
            entity.HasKey(e => e.PkVehicleGroupId);

            entity.ToTable("Vehicle.Groups");

            entity.Property(e => e.PkVehicleGroupId)
                .ValueGeneratedNever()
                .HasColumnName("PK_VehicleGroupID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FkBgtprovinceId).HasColumnName("FK_BGTProvinceID");
            entity.Property(e => e.FkCompanyId).HasColumnName("FK_CompanyID");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.ParentVehicleGroupId).HasColumnName("ParentVehicleGroupID");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<VehicleVehicle>(entity =>
        {
            entity.HasKey(e => e.PkVehicleId).HasName("PK_Vehicle.Vehicles_1");

            entity.ToTable("Vehicle.Vehicles");

            entity.HasIndex(e => new { e.VehiclePlate, e.Xncode }, "UQ__Vehicle.__F32E91F8E9FA76DE").IsUnique();

            entity.Property(e => e.PkVehicleId)
                .ValueGeneratedNever()
                .HasColumnName("PK_VehicleID");
            entity.Property(e => e.FkCompanyId).HasColumnName("FK_CompanyID");
            entity.Property(e => e.Imei)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("IMEI");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);
            entity.Property(e => e.PrivateCode).HasMaxLength(50);
            entity.Property(e => e.VehiclePlate)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Xncode).HasColumnName("XNCode");
        });

        modelBuilder.Entity<VehicleVehicleGroup>(entity =>
        {
            entity.HasKey(e => new { e.FkVehicleGroupId, e.FkVehicleId });

            entity.ToTable("Vehicle.VehicleGroups");

            entity.Property(e => e.FkVehicleGroupId).HasColumnName("FK_VehicleGroupID");
            entity.Property(e => e.FkVehicleId).HasColumnName("FK_VehicleID");
            entity.Property(e => e.FkCompanyId).HasColumnName("FK_CompanyID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
