using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCFilmSON.Models;

namespace MVCFilmSON.Controllers
{
    //class ın üzerine koyduğumuz için bütün sayfalar (metodlar) adminlere özel olmuş oldu
    [Authorize(Roles = "Admin")]
    public class FilmYonetController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //[AllowAnonymous] bir sayfayı giriş yapmamış kişilere açar
        public ActionResult Index()
        {
            var filmler = db.Filmler.Include(f => f.FilminKategorisi);
            return View(filmler.ToList());
        }

        // GET: FilmYonet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Filmler.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // GET: FilmYonet/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi");
            return View();
        }

        // POST: FilmYonet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilmID,FilmAdi,Yonetmen,Konu,KategoriID,ResimURL,Fragman,Yabanci, Yil")] Film film, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL.ContentLength > 0)
                    ResimURL.SaveAs(Server.MapPath("/Content/filmresim/")+ResimURL.FileName);

                film.ResimURL = ResimURL.FileName;

                var yid = film.Fragman.Split('=').Last();
                //https://www.youtube.com/watch?v=GcGPedcPsOs

                string sonuc = string.Format("<iframe width='560' height='315' src='https://www.youtube.com/embed/{0}' frameborder='0' allowfullscreen></iframe>",yid);

                film.Fragman = sonuc;

                //<iframe width="560" height="315" src="https://www.youtube.com/embed/GcGPedcPsOs" frameborder="0" allowfullscreen></iframe>

                db.Filmler.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi", film.KategoriID);
            return View(film);
        }

        // GET: FilmYonet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Filmler.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi", film.KategoriID);
            return View(film);
        }

        // POST: FilmYonet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // html iletebilmek için
        public ActionResult Edit([Bind(Include = "FilmID,FilmAdi,Yonetmen,Konu,KategoriID,ResimURL,Fragman,Yabanci, Yil")] Film film)
        {
            if (ModelState.IsValid)
            {
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi", film.KategoriID);
            return View(film);
        }

        // GET: FilmYonet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Filmler.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: FilmYonet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Filmler.Find(id);
            db.Filmler.Remove(film);
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
    }
}
