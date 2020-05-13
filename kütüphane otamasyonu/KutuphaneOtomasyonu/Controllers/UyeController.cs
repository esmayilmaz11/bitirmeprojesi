using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KutuphaneOtomasyonu.Models;
using System.Web.Security;

namespace KutuphaneOtomasyonu.Controllers
{
    public class UyeController : Controller
    {
        private KütüphaneEntities1 db = new KütüphaneEntities1();

        
        public ActionResult Index()
        {
            return View(db.Uye.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uye.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "uyeID,uyeSifre,uyeAd,uyeSoyad,uyeTel")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                db.Uye.Add(uye);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(uye);
        }

        // GET: Uye/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uye.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "uyeID,uyeSifre,uyeAd,uyeSoyad,uyeTel")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uye).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uye);
        }

        // GET: Uye/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uye.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // POST: Uye/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uye uye = db.Uye.Find(id);
            db.Uye.Remove(uye);
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











        [AllowAnonymous]
        public ActionResult Login()
        {
            if (String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                FormsAuthentication.SignOut();
                return View();
            }
            return Redirect("/Home/Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Giris(AuthModel model)
        {
            
            if (ModelState.IsValid)
            {
               
                var kullanici = db.Uye.Where(degisken => degisken.uyeAd == model.login.isim && degisken.uyeSifre == model.login.Password);

                
                Uye uye = db.Uye.FirstOrDefault(degisken => degisken.uyeAd == model.login.isim && degisken.uyeSifre == model.login.Password);
                
                
                if (kullanici.Count() > 0)
                {
                    FormsAuthentication.SetAuthCookie(model.login.isim+ "|" + uye.uyeID + "|" + uye.uyeSoyad, true);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    //System.Diagnostics.Debug.WriteLine("Mrhba");
                    ModelState.AddModelError("Hata", "İsim veya şifre hatalı!");
                }
            }
            return View("Login");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Uye");
        }
    }
}
