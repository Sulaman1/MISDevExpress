using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBContext.Data;
using DAL.Models.Domain.MasterSetup;
using BAL.IRepository.MasterSetup.Training;

namespace BLEPMIS.Controllers.MasterSetup
{
    public class TrainingTypesController : Controller
    {
        private readonly ITrainingType _context;

        public TrainingTypesController(ITrainingType context)
        {
            _context = context;
        }

        // GET: TrainingTypes
        public async Task<IActionResult> Index()
        {            
            return View(await _context.GetAll());
        }

        // GET: TrainingTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingType = await _context.GetById(id);
            if (trainingType == null)
            {
                return NotFound();
            }

            return View(trainingType);
        }

        // GET: TrainingTypes/Create
        public IActionResult Create()
        {
            ViewData["TrainingHeadId"] = new SelectList(_context.GetAllTrainingHead(), "TrainingHeadId", "TrainingHeadName");
            return View();
        }

        // POST: TrainingTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingTypeId,TrainingTypeName,TrainingHeadId")] TrainingType trainingType)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Count(trainingType.TrainingTypeName);
                if (result > 0)
                {
                    ModelState.AddModelError(nameof(trainingType.TrainingTypeName), "Name already exist!");
                    return View(trainingType);
                }
                _context.Insert(trainingType);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingHeadId"] = new SelectList(_context.GetAllTrainingHead(), "TrainingHeadId", "TrainingHeadName", trainingType.TrainingHeadId);
            return View(trainingType);
        }

        // GET: TrainingTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingType = await _context.GetById(id);
            if (trainingType == null)
            {
                return NotFound();
            }
            ViewData["TrainingHeadId"] = new SelectList(_context.GetAllTrainingHead(), "TrainingHeadId", "TrainingHeadName", trainingType.TrainingHeadId);
            return View(trainingType);
        }

        // POST: TrainingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingTypeId,TrainingTypeName,TrainingHeadId")] TrainingType trainingType)
        {
            if (id != trainingType.TrainingTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _context.Count(trainingType.TrainingTypeName);
                    if (result > 0)
                    {
                        ModelState.AddModelError(nameof(trainingType.TrainingTypeName), "Name already exist!");
                        return View(trainingType);
                    }
                    _context.Update(trainingType);
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingTypeExists(trainingType.TrainingTypeId))
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
            ViewData["TrainingHeadId"] = new SelectList(_context.GetAllTrainingHead(), "TrainingHeadId", "TrainingHeadName", trainingType.TrainingHeadId);
            return View(trainingType);
        }

        // GET: TrainingTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingType = await _context.GetById(id);
            if (trainingType == null)
            {
                return NotFound();
            }

            return View(trainingType);
        }

        // POST: TrainingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var trainingType = await _context.GetById(id);
            if (trainingType != null)
            {
                _context.Remove(trainingType);
            }
            
            _context.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingTypeExists(int id)
        {
          return _context.Exist(id);
        }
    }
}
