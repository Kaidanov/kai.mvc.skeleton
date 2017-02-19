using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Kai.MVC.Skeleton.Filters;

namespace Kai.MVC.Skeleton.Controllers
{
    [LogActionFilter]
    public class LinesController : Controller
    {
        private Entities db = new Entities();

        // GET: Lines
        public async Task<ActionResult> Index()
        {
            List<Line> list = await db.Lines.ToListAsync();
            if (list.Where(x => x.Status == 1).ToList().Count == 0)
            {
                List<Line> nextInLineList = list.Where(x => x.Status == 0).ToList();
                Line nextInLine = nextInLineList.OrderBy(x => x.DateTime).FirstOrDefault();
                nextInLine.Status = 1;
                await db.SaveChangesAsync();
            }
           
            return View(list);
        }
    
        public async Task<ActionResult> GetNext(int? id)
        {
           
           
            List<Line> list = await db.Lines.ToListAsync();
            Line currentInLine = list.Where(x => x.Status == 1).FirstOrDefault();
            currentInLine.Status = 2;
            List<Line> nextInLineList = list.Where(x => x.Status == 0).ToList();
            Line nextInLine = nextInLineList.OrderBy(x => x.DateTime).FirstOrDefault();
            if(nextInLine!=null)
                nextInLine.Status = 1;

            await db.SaveChangesAsync();

            return View("Index",await db.Lines.ToListAsync());
        }

        // GET: Lines/Details/5
        public async Task<ActionResult>Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Line line = await db.Lines.FindAsync(id);
            if (line == null)
            {
                return HttpNotFound();
            }
            return View(line);
        }

        // GET: Lines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Line line)
        {
            if (ModelState.IsValid)
            {
                db.Lines.Add(line);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(line);
        }

        // GET: Lines/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Line line = await db.Lines.FindAsync(id);
            if (line == null)
            {
                return HttpNotFound();
            }
            return View(line);
        }

        // POST: Lines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,DateTime")] Line line)
        {
            if (ModelState.IsValid)
            {
                db.Entry(line).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(line);
        }

        // GET: Lines/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Line line = await db.Lines.FindAsync(id);
            if (line == null)
            {
                return HttpNotFound();
            }
            return View(line);
        }

        // POST: Lines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Line line = await db.Lines.FindAsync(id);
            db.Lines.Remove(line);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
