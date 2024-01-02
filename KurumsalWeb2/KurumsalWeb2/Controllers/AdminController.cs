using KurumsalWeb2.Models.DataContext;
using KurumsalWeb2.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb2.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        KurumsalDBContext db = new KurumsalDBContext();

        [Route("yonetimpaneli")]
        
        public ActionResult Index()
        {
            ViewBag.BlogSay = db.Blog.Count();
            ViewBag.KategoriSay = db.Kategori.Count();
            ViewBag.HizmetSay = db.Hizmet.Count();
            ViewBag.YorumSay = db.Yorum.Count();
            ViewBag.YorumOnay = db.Yorum.Where(x=>x.Onay == false).Count();
            var sorgu = db.Kategori.ToList();
            return View(sorgu);
        }

        [Route("yonetimpaneli/giris")]
        public ActionResult Login()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin,string sifre)
        {
            
            var login = db.Admin.Where(x=>x.Eposta == admin.Eposta).SingleOrDefault();
            
            if (login.Eposta==admin.Eposta && login.Sifre== Crypto.Hash(admin.Sifre, "MD5"))
            {
                Session["adminid"] = login.AdminId;
                Session["eposta"] = login.Eposta;
                Session["yetki"] = login.Yetki;
                return RedirectToAction("Index","Admin");
            }
            ViewBag.Uyari = "Kullanıcı adı yada şifre hatalı";
            return View(admin);
        }

        public ActionResult Logout()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;
            Session.Abandon();
            return RedirectToAction("Login","Admin");
        }

        public ActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifremiUnuttum(string eposta)
        {
            var mail = db.Admin.Where(x => x.Eposta == eposta).SingleOrDefault();
            if (mail != null)
            {
                Random rnd = new Random();
                int yenisifre = rnd.Next();

                Admin admin = new Admin();
                mail.Sifre = Crypto.Hash(Convert.ToString(yenisifre),"MD5");
                db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "kurumsalweb85@gmail.com";
                WebMail.Password = "KurumsalWeb98.98";
                WebMail.SmtpPort = 587;
                WebMail.Send(eposta,"Admin Panel Giriş Şifreniz","Şifreniz :"+yenisifre);
                ViewBag.Uyari = "Başarılı";
            }
            else
            {
                ViewBag.Uyari = "Hata Oluştu.Tekrar Deneyin";
            }
            return View();
        }

        public ActionResult Adminler()
        {
            return View(db.Admin.ToList());
        }

        public ActionResult Create()
        {
            return View() ;
        }

        [HttpPost]
        public ActionResult Create(Admin admin,string sifre,string eposta)
        {
            
            if (ModelState.IsValid)
            {
                admin.Sifre = Crypto.Hash(sifre,"MD5");
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }

        public ActionResult Edit(int id) 
        {
            var a = db.Admin.Where(x=>x.AdminId==id).SingleOrDefault();
            return View(a);
        }

        [HttpPost]
        public ActionResult Edit(int id,Admin admin,string sifre,string eposta,string yetki)
        {
           
            if (ModelState.IsValid)
            {
                var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
                a.Sifre = Crypto.Hash(sifre,"MD5");
                a.Eposta = eposta;
                a.Yetki =  yetki;
                db.SaveChanges();
                return RedirectToAction("Adminler");

            }
            return View(admin);
        }

        public ActionResult Delete(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            if (a!=null)
            {
                db.Admin.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Adminler");

            }
            return View();
        }

    }
}