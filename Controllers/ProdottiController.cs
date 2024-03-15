using CiroKebab.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CiroKebab.Controllers
{
    [Authorize]
    public class ProdottiController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Prodotti
        public ActionResult Index()
        {
            return View(db.Prodotti.ToList());
        }

        // GET: Prodotti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // GET: Prodotti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prodotti/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProdotto,Nome,Foto,Foto2,Foto3,Prezzo,Consegna,Ingredienti")] Prodotti prodotti)
        {
            if (ModelState.IsValid)
            {
                db.Prodotti.Add(prodotti);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(prodotti);
        }

        // GET: Prodotti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // POST: Prodotti/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProdotto,Nome,Foto,Foto2,Foto3,Prezzo,Consegna,Ingredienti")] Prodotti prodotti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prodotti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prodotti);
        }

        // GET: Prodotti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // POST: Prodotti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prodotti prodotti = db.Prodotti.Find(id);
            db.Prodotti.Remove(prodotti);
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

        // Questo metodo gestisce l'aggiunta di un prodotto al carrello.
        public ActionResult AddToCart(int id, int Quantita)
        {
            // Trova il prodotto nel database utilizzando l'ID fornito.
            var prodotto = db.Prodotti.Find(id);
            if (prodotto != null)
            {
                // Ottiene il carrello dalla sessione o ne crea uno nuovo se non esiste.
                var cart = Session["cart"] as List<Prodotti> ?? new List<Prodotti>();

                // Imposta la quantità del prodotto.
                prodotto.Quantita = Quantita;

                // Controlla se il prodotto è già nel carrello.
                if (cart.Any(p => p.idProdotto == id))
                {
                    // Se il prodotto è già nel carrello, aumenta la quantità.
                    var productInCart = cart.First(p => p.idProdotto == id);
                    productInCart.Quantita += Quantita;
                }
                else
                {
                    // Se il prodotto non è nel carrello, lo aggiunge.
                    cart.Add(prodotto);
                }

                // Salva il carrello aggiornato nella sessione.
                Session["cart"] = cart;

                // Imposta un messaggio temporaneo per informare l'utente che il prodotto è stato aggiunto al carrello.
                TempData["AddCart"] = "Prodotto aggiunto correttamente";
            }

            // Reindirizza l'utente all'indice.
            return RedirectToAction("Index");
        }
    }
}
