
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup;

namespace BLEPMIS.Controllers.MasterSetup
{
    [AllowAnonymous]
    public class ProviencesController : Controller
    {
        private readonly IProvience _context;

        public ProviencesController(IProvience context)
        {
            _context = context;
        }

        // GET: Proviences
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetAll());
        }

        // GET: Proviences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provience = await _context.GetById(id);
            if (provience == null)
            {
                return NotFound();
            }

            return View(provience);
        }

        // GET: Proviences/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proviences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ProvienceId,Name,Code")] Provience provience)
        {
            if (ModelState.IsValid)
            {
                _context.Insert(provience);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(provience);
        }

        // GET: Proviences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provience = await _context.GetById(id);
            if (provience == null)
            {
                return NotFound();
            }
            return View(provience);
        }

        // POST: Proviences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProvienceId,Name,Code")] Provience provience)
        {
            if (id != provience.ProvienceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(provience);                    
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvienceExists(provience.ProvienceId))
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
            return View(provience);
        }

        // GET: Proviences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provience = await _context.GetById(id);
            if (provience == null)
            {
                return NotFound();
            }

            return View(provience);
        }

        // POST: Proviences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var provience = await _context.GetById(id);
            _context.Remove(provience);
            _context.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvienceExists(int id)
        {
            return _context.Exist(id);
        }
    }
}
