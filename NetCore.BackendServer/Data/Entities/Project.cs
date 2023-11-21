using System.Security.AccessControl;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetCore.BackendServer.Data.Entities;

public class Project
{
    [Key]
    [Display(Name = "Mã DA")]
    [MaxLength(10)]
    public string? Id { get; set; }

    [MaxLength(500)]
    [Required]
    [Display(Name = "Tên dự án")]
    public string? Name { get; set; }

    [Required]
    [Display(Name = "Ngày bắt đầu")]
    public DateTime? StartDateTime { get; set; }

    [Required]
    [Display(Name = "Ngày kết thúc")]
    public DateTime? EndDateTime { get; set; }

    [MaxLength(1000)]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Thành viên tham gia")]
    public string? ThanhVienTG { get; set; }

    [Display(Name = "Trưởng nhóm")]
    public string? TruongNhom { get; set; }

    [Display(Name = "Mã KH")]
    public string? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [Display(Name = "Mã NV")]
    public string? EmployeeId { get; set; }
    public User? Employee { get; set; }
    [Display(Name = "Ngày khởi tạo")]
    public DateTime? DateCreated { get; set; }
}


public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasOne(x => x.Employee)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.EmployeeId);

        builder.HasOne(x => x.Customer)
        .WithMany(x => x.Projects)
        .HasForeignKey(x => x.CustomerId);
    }
}