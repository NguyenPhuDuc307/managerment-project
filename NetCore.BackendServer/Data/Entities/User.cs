using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NetCore.BackendServer.Data.Entities;

public class User : IdentityUser
{
    public User()
    {
    }

    public User(string id, string userName, string fullName, string avatar, string email, string phoneNumber, DateTime dob)
    {
        Id = id;
        UserName = userName;
        FullName = fullName;
        Avatar = avatar;
        Email = email;
        PhoneNumber = phoneNumber;
        Dob = dob;
    }

    [MaxLength(50)]
    [Required]
    [Display(Name = "Họ tên")]
    public string? FullName { get; set; }

    public string? Avatar { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    [Required]
    public DateTime Dob { get; set; }
    public ICollection<Project>? Projects { get; set; }
}