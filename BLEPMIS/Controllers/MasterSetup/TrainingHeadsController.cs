using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup.Training;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class TrainingHeadsController : Controller
    {
        private readonly ITrainingHead _context;

        public TrainingHeadsController(ITrainingHead context)
        {
            _context = context;
        }

        // GET: TrainingHeads
        public async Task<IActionResult> Index()
        {
              return View(await _context.GetAll());
        }

        // GET: TrainingHeads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingHead = await _context.GetById(id);
            if (trainingHead == null)
            {
                return NotFound();
            }

            return View(trainingHead);
        }

        // GET: TrainingHeads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingHeads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingHead trainingHead)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Count(trainingHead.TrainingHeadName);
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(trainingHead.TrainingHeadName), "UC already exist!");
                    return View(trainingHead);
                }
                _context.Insert(trainingHead);
                
                return RedirectToAction(nameof(Index));
            }
            return View(trainingHead);
        }

        // GET: TrainingHeads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingHead = await _context.GetById(id);
            if (trainingHead == null)
            {
                return NotFound();
            }
            return View(trainingHead);
        }

        // POST: TrainingHeads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  TrainingHead trainingHead)
        {
            if (id != trainingHead.TrainingHeadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _context.Count(trainingHead.TrainingHeadName);
                    if (result > 1)
                    {
                        ModelState.AddModelError(nameof(trainingHead.TrainingHeadName), "Training Head already exist!");
                        return View(trainingHead);
                    }
                    _context.Update(trainingHead);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingHeadExists(trainingHead.TrainingHeadId))
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
            return View(trainingHead);
        }

        // GET: TrainingHeads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingHead = await _context.GetById(id);
            if (trainingHead == null)
            {
                return NotFound();
            }

            return View(trainingHead);
        }

        // POST: TrainingHeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {           
            var trainingHead = await _context.GetById(id);
            if (trainingHead != null)
            {
                _context.Remove(trainingHead);
            }                        
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingHeadExists(int id)
        {
          return _context.Exist(id);
        }
    }
}
