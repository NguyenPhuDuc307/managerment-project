using NetCore.BackendServer.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace NetCore.BackendServer.Data;

public class DbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public DbInitializer(ApplicationDbContext context,
      UserManager<User> userManager,
      RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task Seed()
    {
        #region Người dùng
        if (!_userManager.Users.Any())
        {
            await _userManager.CreateAsync(new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "example1@gmail.com",
                FullName = "Nhân viên 1",
                Email = "example1@gmail.com",
                LockoutEnabled = false,
                PhoneNumber = "0964732231",
                Dob = new DateTime(2001, 7, 30)
            }, "Admin@123");

            await _userManager.CreateAsync(new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "example2@gmail.com",
                FullName = "Nhân viên 2",
                Email = "example2@gmail.com",
                LockoutEnabled = false,
                PhoneNumber = "0964732231",
                Dob = new DateTime(2001, 7, 30)
            }, "Admin@123");

            await _userManager.CreateAsync(new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "example3@gmail.com",
                FullName = "Nhân viên 3",
                Email = "example3@gmail.com",
                LockoutEnabled = false,
                PhoneNumber = "0964732231",
                Dob = new DateTime(2001, 7, 30)
            }, "Admin@123");

            await _userManager.CreateAsync(new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "example4@gmail.com",
                FullName = "Nhân viên 4",
                Email = "example4@gmail.com",
                LockoutEnabled = false,
                PhoneNumber = "0964732231",
                Dob = new DateTime(2001, 7, 30)
            }, "Admin@123");

            await _userManager.CreateAsync(new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "example5@gmail.com",
                FullName = "Nhân viên 5",
                Email = "example5@gmail.com",
                LockoutEnabled = false,
                PhoneNumber = "0964732231",
                Dob = new DateTime(2001, 7, 30)
            }, "Admin@123");
        }
        #endregion Người dùng

        #region Khách hàng
        if (!_context.Customers.Any())
        {
            _context.Customers.AddRange(new List<Customer>
            {
                new Customer {
                    Id = "KH00001",
                    Name = "Công ty Cổ phần ABC",
                    Dob = new DateTime(2001, 7, 30),
                    GioiTinh = true,
                    PhoneNumber = "0964732231",
                    Address = "Cầu Giấy, Hà Nội",
                    Email = "cty.abc@gmail.com",
                    Status = Enums.CustomerStatus.CHAM_SOC,
                    DateCreated = DateTime.Now
                },
                new Customer {
                    Id = "KH00002",
                    Name = "Công ty TNHH EFG",
                    Dob = new DateTime(2001, 7, 30),
                    GioiTinh = true,
                    PhoneNumber = "0964732231",
                    Address = "Hà Đông, Hà Nội",
                    Email = "cty.efg@gmail.com",
                    Status = Enums.CustomerStatus.CHAM_SOC,
                    DateCreated = DateTime.Now
                },
                new Customer {
                    Id = "KH00003",
                    Name = "Công ty Cổ phần XYZ",
                    Dob = new DateTime(2001, 7, 30),
                    GioiTinh = true,
                    PhoneNumber = "0964732231",
                    Address = "Thanh Xuân, Hà Nội",
                    Email = "cty.xyz@gmail.com",
                    Status = Enums.CustomerStatus.CHAM_SOC,
                    DateCreated = DateTime.Now
                },
                new Customer {
                    Id = "KH00004",
                    Name = "Công ty TNHH MNN",
                    Dob = new DateTime(2001, 7, 30),
                    GioiTinh = true,
                    PhoneNumber = "0964732231",
                    Address = "Cầu Giấy, Hà Nội",
                    Email = "cty.mnn@gmail.com",
                    Status = Enums.CustomerStatus.CHAM_SOC,
                    DateCreated = DateTime.Now
                },
                new Customer {
                    Id = "KH00005",
                    Name = "Công ty Cổ phần AAA",
                    Dob = new DateTime(2001, 7, 30),
                    GioiTinh = true,
                    PhoneNumber = "0964732231",
                    Address = "Từ Sơn, Bắc Ninh",
                    Email = "cty.aaa@gmail.com",
                    Status = Enums.CustomerStatus.CHAM_SOC,
                    DateCreated = DateTime.Now
                }
            });
            await _context.SaveChangesAsync();
        }
        #endregion Khách hàng
    }
}