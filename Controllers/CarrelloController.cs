using CiroKebab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CiroKebab.Controllers
{
    public class CarrelloController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Carrello
        public ActionResult Index()
        {

            var cart = Session["cart"] as List<Prodotti>;
            if (cart == null || !cart.Any()) // Determina se una sequenza contiene elementi
            {
                return RedirectToAction("Index", "Prodotti");
            }
            return View(cart);
        }


        public ActionResult Delete(int? id)
        {
            var cart = Session["cart"] as List<Prodotti>;
            if (cart != null)
            {
                var productToRemove = cart.FirstOrDefault(p => p.idProdotto == id);
                if (productToRemove != null)
                {
                    cart.Remove(productToRemove);
                }
            }

            return RedirectToAction("Index");
        }

        // Push verso il db
        [HttpPost]
        public ActionResult Ordina(string note, string indirizzo)
        {
            // Creazione di un nuovo contesto del database
            ModelDbContext db = new ModelDbContext();

            // Ricerca dell'utente corrente nel database
            var userId = db.Utenti.FirstOrDefault(u => u.Email == User.Identity.Name).idUtente;

            // Recupero del carrello dalla sessione
            var cart = Session["cart"] as List<Prodotti>;
            if (cart != null && cart.Any())
            {
                // Creazione di un nuovo ordine
                Ordini newOrder = new Ordini();
                newOrder.DataOrdine = DateTime.Now;
                newOrder.idEvaso = false;
                newOrder.idUtente_FK = userId;
                newOrder.Indirizzo = indirizzo;
                newOrder.Totale = cart.Sum(p => p.Prezzo);
                newOrder.Note = note;

                // Aggiunta del nuovo ordine al database
                db.Ordini.Add(newOrder);
                db.SaveChanges();

                // Creazione dei dettagli dell'ordine per ogni prodotto nel carrello
                foreach (var product in cart)
                {
                    Dettagli newDetail = new Dettagli();
                    newDetail.idOrdine_FK = newOrder.idOrdine;
                    newDetail.idProdotto_FK = product.idProdotto;
                    newDetail.Quantita = 1;

                    // Aggiunta dei dettagli dell'ordine al database
                    db.Dettagli.Add(newDetail);
                    db.SaveChanges();
                }
                // Pulizia del carrello
                cart.Clear();
            }

            // Impostazione del messaggio di conferma dell'ordine
            TempData["CreateMess"] = "L'ordine è stato inviato correttamente";

            // Reindirizzamento alla pagina dei prodotti
            return RedirectToAction("Index", "Prodotti");
        }
    }
}
