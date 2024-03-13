using CiroKebab.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace CiroKebab.Controllers
{


    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        // Metodo GET per il login
        public ActionResult LogIn()
        {
            return View();
        }

        // Metodo POST per il login
        [HttpPost]
        public ActionResult LogIn(string email, string psw)
        {
            // Utilizzo del contesto del database
            using (var context = new ModelDbContext())
            {
                // Ricerca dell'utente nel database
                var user = context.Utenti.FirstOrDefault(u => u.Email == email && u.Psw == psw);
                if (user != null)
                {
                    // Autenticazione dell'utente
                    FormsAuthentication.SetAuthCookie(email, false);
                    // Reindirizzamento alla pagina principale
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Ritorno alla vista di login in caso di fallimento
                    return View();
                }
            }
        }

        public ActionResult LogOut()
        {
            // Logout dell'utente
            FormsAuthentication.SignOut();
            // Reindirizzamento alla pagina principale
            return RedirectToAction("Index", "Home");
        }

        // Metodo GET per la registrazione
        public ActionResult Registrati()
        {
            return View();
        }

        // Metodo POST per la registrazione
        [HttpPost]
        public ActionResult Registrati(Utenti utente)
        {
            // Utilizzo del contesto del database
            using (var context = new ModelDbContext())
            {
                // Aggiunta del nuovo utente al database
                context.Utenti.Add(utente);
                context.SaveChanges();
            }
            // Reindirizzamento alla pagina principale dopo la registrazione
            return RedirectToAction("Index");
        }
    }
}
