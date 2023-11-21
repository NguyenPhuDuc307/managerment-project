using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCore.BackendServer.Data;
using NetCore.BackendServer.Data.Entities;
using NetCore.BackendServer.Helpers;
using NetCore.BackendServer.Models;

namespace NetCore.BackendServer.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [Route("danh-sach-du-an")]
        public async Task<IActionResult> Index(string? sortOrder, string currentFilter, string? searchString, int? pageNumber)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }

            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StartDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "sd_desc" : "";
            ViewData["EndDateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ed_desc" : "";
            ViewData["TVSortParm"] = String.IsNullOrEmpty(sortOrder) ? "tv_desc" : "";
            ViewData["TNSortParm"] = String.IsNullOrEmpty(sortOrder) ? "tn_desc" : "";
            ViewData["MaKHSortParm"] = String.IsNullOrEmpty(sortOrder) ? "makh_desc" : "";
            ViewData["MaNVSortParm"] = String.IsNullOrEmpty(sortOrder) ? "manv_desc" : "";
            ViewData["DateCreatedSortParm"] = String.IsNullOrEmpty(sortOrder) ? "datecreated_desc" : "";
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

            var projects = from m in _context.Projects
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name!.Contains(searchString)
                || s.Id!.ToString().Contains(searchString)
                || s.CustomerId!.Contains(searchString)
                || s.ThanhVienTG!.Contains(searchString)
                || s.TruongNhom!.Contains(searchString)
                || s.EmployeeId!.Contains(searchString)
                );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "sd":
                    projects = projects.OrderBy(s => s.StartDateTime);
                    break;
                case "sd_desc":
                    projects = projects.OrderByDescending(s => s.StartDateTime);
                    break;
                case "id":
                    projects = projects.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    projects = projects.OrderByDescending(s => s.Id);
                    break;
                case "ed":
                    projects = projects.OrderBy(s => s.EndDateTime);
                    break;
                case "ed_desc":
                    projects = projects.OrderByDescending(s => s.EndDateTime);
                    break;
                case "tv":
                    projects = projects.OrderBy(s => s.ThanhVienTG);
                    break;
                case "tv_desc":
                    projects = projects.OrderByDescending(s => s.ThanhVienTG);
                    break;
                case "tn":
                    projects = projects.OrderBy(s => s.TruongNhom);
                    break;
                case "tn_desc":
                    projects = projects.OrderByDescending(s => s.TruongNhom);
                    break;
                case "makh":
                    projects = projects.OrderBy(s => s.CustomerId);
                    break;
                case "makh_desc":
                    projects = projects.OrderByDescending(s => s.CustomerId);
                    break;
                case "manv":
                    projects = projects.OrderBy(s => s.EmployeeId);
                    break;
                case "manv_desc":
                    projects = projects.OrderByDescending(s => s.EmployeeId);
                    break;
                case "datecreated":
                    projects = projects.OrderBy(s => s.DateCreated);
                    break;
                case "datecreated_desc":
                    projects = projects.OrderByDescending(s => s.DateCreated);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Project>.CreateAsync(projects.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Route("thong-tin-du-an")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [Route("them-du-an")]
        public async Task<IActionResult> Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["Employee"] = new SelectList(_context.Users, "FullName", "FullName");
            Project project = new Project();
            project.StartDateTime = DateTime.Now;
            project.EndDateTime = DateTime.Now;
            var user = await _userManager.GetUserAsync(User);
            project.EmployeeId = user!.Id;
            return View(project);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("them-du-an")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDateTime,EndDateTime,Description,ThanhVienTG,TruongNhom,CustomerId,EmployeeId,DateCreated")] Project project)
        {
            if (ModelState.IsValid)
            {
                string id = "DA" + TextHelper.GetRanDomCodeInt(5);
                bool idExists = await CheckIdExistsInDatabase(id);

                while (idExists)
                {
                    id = "DA" + TextHelper.GetRanDomCodeInt(5);
                    idExists = await CheckIdExistsInDatabase(id);
                }

                project.Id = id;
                project.DateCreated = DateTime.Now;
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", project.CustomerId);
            return View(project);
        }

        private async Task<bool> CheckIdExistsInDatabase(string id)
        {
            // Check if the ID exists in the Customer table in the database
            bool idExists = await _context.Projects.AnyAsync(c => c.Id == id);
            return idExists;
        }

        [Route("chinh-sua-du-an")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", project.CustomerId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("chinh-sua-du-an")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,StartDateTime,EndDateTime,Description,ThanhVienTG,TruongNhom,CustomerId,EmployeeId,DateCreated")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address", project.CustomerId);
            return View(project);
        }

        [Route("xoa-du-an")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost("xoa-du-an"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Project'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(string id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
