using CiroKebab.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CiroKebab.Controllers
{
    public class OrdiniController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ordini
        public ActionResult Index()
        {
            var ordini = db.Ordini.Include(o => o.Utenti);
            return View(ordini.ToList());
        }

        // GET: Ordini/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // GET: Ordini/Create
        public ActionResult Create()
        {
            ViewBag.idUtente_FK = new SelectList(db.Utenti, "idUtente", "Nome");
            return View();
        }

        // POST: Ordini/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idOrdine,Indirizzo,idUtente_FK,Totale,idEvaso,Note,DataOrdine")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Ordini.Add(ordini);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idUtente_FK = new SelectList(db.Utenti, "idUtente", "Nome", ordini.idUtente_FK);
            return View(ordini);
        }

        // GET: Ordini/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUtente_FK = new SelectList(db.Utenti, "idUtente", "Nome", ordini.idUtente_FK);
            return View(ordini);
        }

        // POST: Ordini/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idOrdine,Indirizzo,idUtente_FK,Totale,idEvaso,Note,DataOrdine")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordini).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idUtente_FK = new SelectList(db.Utenti, "idUtente", "Nome", ordini.idUtente_FK);
            return View(ordini);
        }

        // GET: Ordini/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // POST: Ordini/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordini ordini = db.Ordini.Find(id);
            db.Ordini.Remove(ordini);
            db.SaveChanges();
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

        public ActionResult isEvaso(int id)
        {
            Ordini ordine = db.Ordini.Find(id);
            if (ordine.idEvaso == false)
            {
                ordine.idEvaso = true;
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ordine.idEvaso = false;
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }



        public async Task<ActionResult> GetNumeroOrdini()
        {
            // Conta il numero di ordini nel database
            int totale = await db.Ordini.Where(o => o.idEvaso == true).CountAsync();

            // Restituisce il conteggio come risposta JSON
            return Json(totale, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> IncassatoPerGiorno(DateTime data)
        {
            decimal incasso = await db.Ordini
                .Where(o => o.DataOrdine.Year == data.Year && o.DataOrdine.Month == data.Month && o.DataOrdine.Day == data.Day)
                .SumAsync(o => o.Totale);
            return Json(incasso, JsonRequestBehavior.AllowGet);
        }

    }
}
