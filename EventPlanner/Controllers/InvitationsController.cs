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
    public class InvitationsController : Controller
    {
        private readonly EventPlannerDbContext _context;

        public InvitationsController(EventPlannerDbContext context)
        {
            _context = context;
        }

        // GET: Invitations
        public async Task<IActionResult> Index()
        {
            var eventPlannerDbContext = _context.Invations.Include(i => i.Event).Include(i => i.User);
            return View(await eventPlannerDbContext.ToListAsync());
        }

        // GET: Invitations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invations
                .Include(i => i.Event)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return NotFound();
            }

            return View(invitation);
        }

        // GET: Invitations/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,UserId,Status")] Invitation invitation)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == invitation.UserId);
            invitation.User = user;
            var our_event = await _context.Events.FirstOrDefaultAsync(e => e.Id == invitation.EventId);
            invitation.Event = our_event;
            if ((user is not null) && (our_event is not null))
            {
                _context.Add(invitation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", invitation.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", invitation.UserId);
            return View(invitation);
        }

        // GET: Invitations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invations.FindAsync(id);
            if (invitation == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", invitation.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", invitation.UserId);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,UserId,Status")] Invitation invitation)
        {
            if (id != invitation.Id)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == invitation.UserId);
            invitation.User = user;
            var our_event = await _context.Events.FirstOrDefaultAsync(e => e.Id == invitation.EventId);
            invitation.Event = our_event;
            if ((user is not null) && (our_event is not null))
            {
                try
                {
                    _context.Update(invitation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvitationExists(invitation.Id))
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
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", invitation.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", invitation.UserId);
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invations
                .Include(i => i.Event)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return NotFound();
            }

            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invitation = await _context.Invations.FindAsync(id);
            if (invitation != null)
            {
                _context.Invations.Remove(invitation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvitationExists(int id)
        {
            return _context.Invations.Any(e => e.Id == id);
        }
    }
}
