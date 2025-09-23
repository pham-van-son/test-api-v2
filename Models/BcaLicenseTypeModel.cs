namespace test_lab.Models
{
    public class BcaLicenseTypeModel
    {
        public int PkLicenseTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public bool IsActived { get; set; }

        public bool IsDeteted { get; set; }

        public Guid CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
