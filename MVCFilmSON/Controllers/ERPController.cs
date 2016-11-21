using MVCFilmSON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MVCFilmSON.Controllers
{
    public class ERPController : Controller
    {
        // GET: ERP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialSlider()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            return View(ctx.Filmler.Take(3).ToList());
        }

        public ActionResult PartialOzetler()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            return View(ctx.Filmler.OrderByDescending(x=>x.FilmID).Take(4).ToList());
        }

        public ActionResult Arama(string aranan)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            //.StartsWith("A") A ile başlayan
            //.EndsWith("B") B ile biten
            var liste = ctx.Filmler
                .Where(x => x.FilmAdi.Contains(aranan))
                .ToList();
            ViewBag.aranankelime = aranan;
            return View(liste);
        }

        public ActionResult Yerli(string sirala, int? y, int? gelensayfa)
        {
            int simdikisayfa = gelensayfa ?? 1;
            int sayfaBasiGosterim = 1;

            ViewBag.secilen = y;
            ViewBag.s = sirala;

            ApplicationDbContext ctx = new ApplicationDbContext();
            var liste = ctx.Filmler.Where(x => !x.Yabanci).ToList();

            if (sirala != null)
                FilmleriSirala(sirala, liste);

            if (y != null)//yıl seçildiyse ona göre filtrele
                liste = liste.Where(x => x.Yil == y).ToList();


            return View(liste.ToPagedList(simdikisayfa, sayfaBasiGosterim));
        }

        public ActionResult Yabanci(string sirala, int? y, int? gelensayfa)
        {
            int simdikisayfa = gelensayfa ?? 1;
            int sayfaBasiGosterim = 1;

            ViewBag.secilen = y;
            ViewBag.s = sirala;

            ApplicationDbContext ctx = new ApplicationDbContext();
            var liste = ctx.Filmler.Where(x => x.Yabanci).ToList();

            if (sirala != null)
                liste= FilmleriSirala(sirala, liste);

            if (y != null)//yıl seçildiyse ona göre filtrele
                liste = liste.Where(x => x.Yil == y).ToList();

            
            return View(liste.ToPagedList(simdikisayfa, sayfaBasiGosterim));
        }

        public ActionResult Yil(short y, string sirala, int? gelensayfa)
        {
            int simdikisayfa = gelensayfa ?? 1;
            int sayfaBasiGosterim = 1;

            ViewBag.secilen = y;
            ApplicationDbContext ctx = new ApplicationDbContext();
            var liste = ctx.Filmler.Where(x => x.Yil == y).ToList();

            if (sirala != null)
                liste = FilmleriSirala(sirala, liste);

            return View(liste.ToPagedList(simdikisayfa, sayfaBasiGosterim));
        }
        List<Film> FilmleriSirala(string sirala, List<Film> liste) {
            switch (sirala){
                case "Alfabetik A-Z":
                    liste = liste.OrderBy(x => x.FilmAdi).ToList();
                    break;
                case "Alfabetik Z-A":
                    liste = liste.OrderByDescending(x => x.FilmAdi).ToList();
                    break;
                case "Yeniden Eskiye":
                    liste = liste.OrderByDescending(x => x.FilmID).ToList();
                    break;
                case "Eskiden Yeniye":
                    liste = liste.OrderBy(x => x.FilmID).ToList();
                    break;
                default:
                    break;
            }
            return liste;
        }
    }
}