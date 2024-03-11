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

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string email, string psw)
        {
            using (var context = new ModelDbContext())
            {
                var user = context.Utenti.FirstOrDefault(u => u.Email == email && u.Psw == psw);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult Registrati()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrati(Utenti utente)
        {
            using (var context = new ModelDbContext())
            {
                context.Utenti.Add(utente);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
