using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.BackendServer.Data;
using NetCore.BackendServer.Data.Entities;
using NetCore.BackendServer.Models;

namespace NetCore.BackendServer.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("danh-sach-nhan-vien")]
        public async Task<IActionResult> Index(string? sortOrder, string currentFilter, string? searchString, int? pageNumber)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }

            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["FullNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fullname_desc" : "";
            ViewData["DobSortParm"] = String.IsNullOrEmpty(sortOrder) ? "dob_desc" : "";
            ViewData["UserNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewData["EmailSortParm"] = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "";
            ViewData["PhoneNumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "phonenumber_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var users = from m in _context.Users
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.FullName!.Contains(searchString)
                || s.Id.Contains(searchString)
                || s.UserName!.Contains(searchString)
                || s.Email!.Contains(searchString)
                || s.PhoneNumber!.Contains(searchString)
                );
            }

            switch (sortOrder)
            {
                case "fullname_desc":
                    users = users.OrderByDescending(s => s.FullName);
                    break;
                case "email":
                    users = users.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                case "id":
                    users = users.OrderBy(s => s.Email);
                    break;
                case "id_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                case "username":
                    users = users.OrderBy(s => s.UserName);
                    break;
                case "username_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                case "phonenumber":
                    users = users.OrderBy(s => s.PhoneNumber);
                    break;
                case "phonenumber_desc":
                    users = users.OrderByDescending(s => s.PhoneNumber);
                    break;
                case "dob":
                    users = users.OrderBy(s => s.Dob);
                    break;
                case "dob_desc":
                    users = users.OrderByDescending(s => s.Dob);
                    break;
                default:
                    users = users.OrderBy(s => s.FullName);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Route("thong-tin-nhan-vien")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
