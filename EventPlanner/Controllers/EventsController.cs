using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventPlanner;
using EventPlanner.Models;

namespace EventPlanner.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventPlannerDbContext _context;

        public EventsController(EventPlannerDbContext context)
        {
            _context = context;
        }

        private void PopulateSelectLists(Event? current = null)
        {
            // Categories by name
            ViewData["CategoryId"] = new SelectList(
                _context.Categories.OrderBy(c => c.Name),
                nameof(Category.Id),
                nameof(Category.Name),
                current?.CategoryId);

            // Users: "First Last (email)" for clarity
            var userItems = _context.Users
                                     .Select(u => new
                                     {
                                         u.Id,
                                         FullName = u.FirstName + " " + u.LastName + " (" + u.Email + ")"
                                     })
                                     .OrderBy(u => u.FullName)
                                     .ToList();

            ViewData["OrganizerId"] = new SelectList(
                userItems,
                "Id",
                "FullName",
                current?.OrganizerId);
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = _context.Events
                                 .Include(e => e.Category)
                                 .Include(e => e.Organizer);

            return View(await events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                                       .Include(e => e.Category)
                                       .Include(e => e.Organizer)
                                       .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null) return NotFound();
            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            PopulateSelectLists();
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartTime,EndTime,Location,OrganizerId,CategoryId")] Event @event)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == @event.OrganizerId);
            @event.Organizer = user;
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == @event.CategoryId);
            @event.Category = category;
            ModelState.Clear();
            TryValidateModel(@event);
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateSelectLists(@event);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            PopulateSelectLists(@event);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartTime,EndTime,Location,OrganizerId,CategoryId")] Event @event)
        {
            if (id != @event.Id) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == @event.OrganizerId);
            @event.Organizer = user;
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == @event.CategoryId);
            @event.Category = category;
            ModelState.Clear();
            TryValidateModel(@event);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateSelectLists(@event);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                                       .Include(e => e.Category)
                                       .Include(e => e.Organizer)
                                       .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null) return NotFound();
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id) => _context.Events.Any(e => e.Id == id);
    }
}
