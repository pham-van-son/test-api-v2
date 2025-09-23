namespace test_lab.Models
{
    public class HrmEmployeeModel
    {
        public int PkEmployeeId { get; set; }

        public string EmployeeCode { get; set; } = null!;

        public int FkCompanyId { get; set; }

        public int FkDepartmentId { get; set; }

        public string Name { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        public byte? Sex { get; set; }

        public string? Address { get; set; }

        public string? Mobile { get; set; }

        public string? PhoneNumber1 { get; set; }

        public string? PhoneNumber2 { get; set; }

        public byte EmployeeType { get; set; }

        public string? IdentityNumber { get; set; }

        public string? DriverLicense { get; set; }

        public DateTime? IssueLicenseDate { get; set; }

        public string? IssueLicensePlace { get; set; }

        public DateTime? ExpireLicenseDate { get; set; }

        public Guid CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int Flags { get; set; }

        public bool? IsSent { get; set; }

        public int? LicenseType { get; set; }

        public string? DriverImage { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? FkUserId { get; set; }

        public string? DriverAvatar { get; set; }

        public DateTime? LockDate { get; set; }
    }

    public class UpdateDriversRequest
    {
        public List<int> EmployeeIds { get; set; } = new();
        public HrmEmployeeModel UpdateData { get; set; } = null!;
    }
}
