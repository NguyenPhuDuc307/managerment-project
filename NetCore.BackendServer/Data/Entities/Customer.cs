using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.BackendServer.Data.Enums;

namespace NetCore.BackendServer.Data.Entities;

public class Customer
{
    [Key]
    [MaxLength(10)]
    [Display(Name = "Mã KH")]
    public string? Id { get; set; }

    [MaxLength(500)]
    [Required]
    [Display(Name = "Tên KH")]
    public string? Name { get; set; }

    [DataType(DataType.Date)]
    [Required]
    [Display(Name = "Ngày sinh")]
    public DateTime? Dob { get; set; }

    [Display(Name = "Giới tính")]
    public bool GioiTinh { get; set; } = true;

    [MaxLength(14)]
    [Required]
    [Display(Name = "Số điện thoại")]
    public string? PhoneNumber { get; set; }

    [MaxLength(1000)]
    [Required]
    [Display(Name = "Địa chỉ")]
    public string? Address { get; set; }

    [MaxLength(100)]
    [Required]
    public string? Email { get; set; }

    [MaxLength(1000)]
    [Display(Name = "Ghi chú")]
    public string? Note { get; set; }
    [Display(Name = "Trạng thái")]
    public CustomerStatus? Status { get; set; }
    [Display(Name = "Ngày tạo")]
    public DateTime? DateCreated { get; set; }
    public ICollection<Project>? Projects { get; set; }
}