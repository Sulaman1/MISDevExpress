
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBContext.Data;
using DAL.Models.Domain.MasterSetup;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class TrainingBiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingBiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainingBies
        public async Task<IActionResult> Index()
        {
              return View(await _context.TrainingBy.ToListAsync());
        }

        // GET: TrainingBies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TrainingBy == null)
            {
                return NotFound();
            }

            var trainingBy = await _context.TrainingBy
                .FirstOrDefaultAsync(m => m.TrainingById == id);
            if (trainingBy == null)
            {
                return NotFound();
            }

            return View(trainingBy);
        }

        // GET: TrainingBies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingBies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingById,Name")] TrainingBy trainingBy)
        {
            if (ModelState.IsValid)
            {
                var result = _context.TrainingBy.Count(a => a.Name.ToLower() == trainingBy.Name.ToLower());
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(trainingBy.Name), "Name already exist!");
                    return View(trainingBy);
                }
                _context.Add(trainingBy);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingBy);
        }

        // GET: TrainingBies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TrainingBy == null)
            {
                return NotFound();
            }

            var trainingBy = await _context.TrainingBy.FindAsync(id);
            if (trainingBy == null)
            {
                return NotFound();
            }
            return View(trainingBy);
        }

        // POST: TrainingBies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingById,Name")] TrainingBy trainingBy)
        {
            if (id != trainingBy.TrainingById)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _context.TrainingBy.Count(a => a.Name.ToLower() == trainingBy.Name.ToLower());
                    if (result > 0)
                    {
                        ModelState.AddModelError(nameof(trainingBy.Name), "Name already exist!");
                        return View(trainingBy);
                    }
                    _context.Update(trainingBy);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingByExists(trainingBy.TrainingById))
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
            return View(trainingBy);
        }

        // GET: TrainingBies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TrainingBy == null)
            {
                return NotFound();
            }

            var trainingBy = await _context.TrainingBy
                .FirstOrDefaultAsync(m => m.TrainingById == id);
            if (trainingBy == null)
            {
                return NotFound();
            }

            return View(trainingBy);
        }

        // POST: TrainingBies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TrainingBy == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TrainingBy'  is null.");
            }
            var trainingBy = await _context.TrainingBy.FindAsync(id);
            if (trainingBy != null)
            {
                _context.TrainingBy.Remove(trainingBy);
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingByExists(int id)
        {
          return _context.TrainingBy.Any(e => e.TrainingById == id);
        }
    }
}
