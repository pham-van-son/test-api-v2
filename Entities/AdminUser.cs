using System;
using System.Collections.Generic;

namespace TestLab.Entities;

public partial class AdminUser
{
    public Guid PkUserId { get; set; }

    public int FkCompanyId { get; set; }

    public string Username { get; set; } = null!;

    /// <summary>
    /// Tên ??ng nh?p d?ng ch? th??ng h?t.
    /// </summary>
    public string UserNameLower { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    /// <summary>
    /// 0: Normal, 1 : Administrator
    /// </summary>
    public byte UserType { get; set; }

    /// <summary>
    /// M?c ??nh là không khóa
    /// </summary>
    public bool IsLock { get; set; }

    /// <summary>
    /// Th?i ?i?m thay ??i m?t kh?u g?n ?ây
    /// </summary>
    public DateTime? LastPasswordChanged { get; set; }

    /// <summary>
    /// &lt;= 0 : không yêu c?u ??i m?t kh?u, N : s? ngày yêu c?u ??i sau
    /// </summary>
    public short? ChangePasswordAfterDays { get; set; }

    public Guid CreatedByUser { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedByUser { get; set; }

    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Th?i ?i?m ??ng nh?p g?n ?ây
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    public byte? LockLevel { get; set; }

    public bool? IsDeleted { get; set; }

    public string? PhoneNumber { get; set; }

    public string? CreatedIp { get; set; }

    public string? UpdatedIp { get; set; }

    public string? Email { get; set; }

    public string? AllowedAccessIp { get; set; }

    public bool UseSecurityCodeSms { get; set; }

    public string? UsernameBap { get; set; }

    public string? LoginType { get; set; }

    public Guid? SuperiorSaleId { get; set; }

    public short ExtendChangePasswordDays { get; set; }

    public bool IsActived { get; set; }

    public DateTime? ActivedDate { get; set; }

    public short RequiredChangePasswordDays { get; set; }

    public bool? IsWeakPassword { get; set; }

    public DateTime? KeepWeakPasswordDate { get; set; }

    public virtual CompanyCompany FkCompany { get; set; } = null!;
}
