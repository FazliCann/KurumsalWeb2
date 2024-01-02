using KurumsalWeb2.Models.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using KurumsalWeb2.Models.Model;

namespace KurumsalWeb2.Controllers
{
    public class HomeController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        [Route("")]
        [Route("Home/Index")]

        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);


            return View();
        }

        public ActionResult SliderPartial()
        { 
            return View(db.Sliders.ToList().OrderByDescending(x=>x.SliderId));
        }


        public ActionResult HizmetPartial()
        { 
            return View(db.Hizmet.ToList().OrderByDescending(x=>x.HizmetId));
        }

     

        public ActionResult Hakkimizda()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hakkimizda.SingleOrDefault());
        }

        public ActionResult Hizmetlerimiz()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        }




        public ActionResult Iletisim()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Iletisim.SingleOrDefault());
        }

        public dynamic GetViewBag()
        {
            return ViewBag;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Iletisim(dynamic viewBag, string ad = null, string email = null, string konu = null, string mesaj = null)
        {
            if (ad != null && email != null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "kurumsalweb85@gmail.com";
                WebMail.Password = "KurumsalWeb98.98";
                WebMail.SmtpPort = 587;
                WebMail.Send("kurumsalweb85@gmail.com", konu, email + "</br>" + mesaj);
                ViewBag.Uyari = "Başarılı";
            }
            else
            {
                ViewBag.Uyari = "Hata Oluştu.Tekrar Deneyin";
            }
            return View();
        }


        
        public ActionResult Blog(int Sayfa=1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.Include("Kategori").OrderByDescending(x => x.BlogId).ToPagedList(Sayfa, 2));
        }

        [Route("Blog/{kategori}/{id:int}")]
        public ActionResult KategoriBlog(int id,int Sayfa = 1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var b = db.Blog.Include("Kategori").OrderByDescending(x=>x.BlogId).Where(x=>x.Kategori.KategoriId==id).ToPagedList(Sayfa,2);
            return View(b);  
        }

        [Route("Blog/{baslik}-{id:int}")]
        public ActionResult BlogDetay(int id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var b = db.Blog.Include("Kategori").Include("Yorums").Where(x=>x.BlogId == id).SingleOrDefault();
            return View(b);
        }

        public JsonResult YorumYap(string adsoyad, string eposta, string icerik, int blogid)
        {
            if (icerik == null)
            {
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            db.Yorum.Add(new Yorum {AdSoyad = adsoyad,Eposta = eposta,Icerik=icerik,BlogId=blogid,Onay=false });
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult BlogKategoriPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return PartialView(db.Kategori.Include("Blogs").ToList().OrderBy(x=>x.KategoriAd));
        }

        public ActionResult BlogKayıtPartial()
        {
            return PartialView(db.Blog.ToList().OrderByDescending(x=>x.BlogId));
        }


        public ActionResult IletisimPartial()
        {
            
            return View(db.Iletisim.SingleOrDefault());
        }

        public ActionResult FooterPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);

            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();

            ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);
            return PartialView();
        }
    }
}