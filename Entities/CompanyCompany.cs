using System;
using System.Collections.Generic;

namespace test_lab.Entities;

public partial class CompanyCompany
{
    public int PkCompanyId { get; set; }

    public int? ParentCompanyId { get; set; }

    public string? CompanyName { get; set; }

    /// <summary>
    /// 1 : BinhAnh
    /// 2: Partner
    /// 3: EndUser
    /// </summary>
    public byte CompanyType { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public DateTime? DateOfEstablishment { get; set; }

    public int? FkProvindeId { get; set; }

    public string? LogoImagePath { get; set; }

    public int Xncode { get; set; }

    public string? ListOfXnforPartner { get; set; }

    public bool? IsLocked { get; set; }

    public string? ReasonOfLocked { get; set; }

    public bool? IsDeleted { get; set; }

    public string? ReasonOfDeleted { get; set; }

    /// <summary>
    /// Sim tr? ti?n
    /// </summary>
    public bool HasSimService { get; set; }

    public short? Flags { get; set; }

    public DateTime? StartLogfileTime { get; set; }

    public Guid CreatedByUser { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedByUser { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string TaxCode { get; set; } = null!;

    public string CustomerCode { get; set; } = null!;

    public int SimServiceType { get; set; }

    public string AccountantCode { get; set; } = null!;

    public bool? Kcschecked { get; set; }

    public DateTime? DateKcschecked { get; set; }

    public Guid? UserKcschecked { get; set; }

    public bool? IsTaxi { get; set; }

    public int? CountLoginInMonth { get; set; }

    public bool? IsBlockXncode { get; set; }

    public DateTime? UpdatedDateIsBlockXncode { get; set; }

    public int? MonthOfSaveData { get; set; }

    public string? PrivateCompanyName { get; set; }

    public bool? IsAllowGoto { get; set; }

    public bool? IsBgtforward { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<AdminUser> AdminUsers { get; set; } = new List<AdminUser>();
}
