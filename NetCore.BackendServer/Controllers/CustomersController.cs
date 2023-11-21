using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.BackendServer.Data;
using NetCore.BackendServer.Data.Entities;
using NetCore.BackendServer.Helpers;
using NetCore.BackendServer.Models;

namespace NetCore.BackendServer.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("danh-sach-khach-hang")]
        public async Task<IActionResult> Index(string? sortOrder, string currentFilter, string? searchString, int? pageNumber)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }

            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DobSortParm"] = String.IsNullOrEmpty(sortOrder) ? "dob_desc" : "";
            ViewData["GioiTinhSortParm"] = String.IsNullOrEmpty(sortOrder) ? "gioitinh_desc" : "";
            ViewData["AddressSortParm"] = String.IsNullOrEmpty(sortOrder) ? "address_desc" : "";
            ViewData["StatusSortParm"] = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "";
            ViewData["DateCreatedSortParm"] = String.IsNullOrEmpty(sortOrder) ? "datecreated_desc" : "";
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

            var customers = from m in _context.Customers
                            select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.Name!.Contains(searchString)
                || s.Id!.Contains(searchString)
                || s.Address!.Contains(searchString)
                || s.Email!.Contains(searchString)
                || s.PhoneNumber!.Contains(searchString)
                );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(s => s.Name);
                    break;
                case "email":
                    customers = customers.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    customers = customers.OrderByDescending(s => s.Email);
                    break;
                case "id":
                    customers = customers.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    customers = customers.OrderByDescending(s => s.Id);
                    break;
                case "gioitinh":
                    customers = customers.OrderBy(s => s.GioiTinh);
                    break;
                case "gioitinh_desc":
                    customers = customers.OrderByDescending(s => s.GioiTinh);
                    break;
                case "phonenumber":
                    customers = customers.OrderBy(s => s.PhoneNumber);
                    break;
                case "phonenumber_desc":
                    customers = customers.OrderByDescending(s => s.PhoneNumber);
                    break;
                case "dob":
                    customers = customers.OrderBy(s => s.Dob);
                    break;
                case "dob_desc":
                    customers = customers.OrderByDescending(s => s.Dob);
                    break;
                case "address":
                    customers = customers.OrderBy(s => s.Address);
                    break;
                case "address_desc":
                    customers = customers.OrderByDescending(s => s.Address);
                    break;
                case "status":
                    customers = customers.OrderBy(s => s.Status);
                    break;
                case "status_desc":
                    customers = customers.OrderByDescending(s => s.Status);
                    break;
                case "datecreated":
                    customers = customers.OrderBy(s => s.DateCreated);
                    break;
                case "datecreated_desc":
                    customers = customers.OrderByDescending(s => s.DateCreated);
                    break;
                default:
                    customers = customers.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Customer>.CreateAsync(customers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Route("thong-tin-khach-hang")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [Route("them-khach-hang")]
        public IActionResult Create()
        {
            Customer customer = new Customer();
            customer.Dob = new DateTime(2000, 01, 01);
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("them-khach-hang")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Dob,GioiTinh,PhoneNumber,Address,Email,Note,Status,DateCreated")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                string id = "KH" + TextHelper.GetRanDomCodeInt(5);
                bool idExists = await CheckIdExistsInDatabase(id);

                while (idExists)
                {
                    id = "KH" + TextHelper.GetRanDomCodeInt(5);
                    idExists = await CheckIdExistsInDatabase(id);
                }

                customer.Id = id;
                customer.Status = Data.Enums.CustomerStatus.CHAM_SOC;
                customer.DateCreated = DateTime.Now;
                _context.Add(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        private async Task<bool> CheckIdExistsInDatabase(string id)
        {
            // Check if the ID exists in the Customer table in the database
            bool idExists = await _context.Customers.AnyAsync(c => c.Id == id);
            return idExists;
        }

        [Route("chinh-sua-khach-hang")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("chinh-sua-khach-hang")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Dob,GioiTinh,PhoneNumber,Address,Email,Note,Status,DateCreated")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.DateCreated = DateTime.Now;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        [Route("xoa-khach-hang")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost("xoa-khach-hang"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
