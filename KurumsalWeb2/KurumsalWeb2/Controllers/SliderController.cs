using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KurumsalWeb2.Models.DataContext;
using KurumsalWeb2.Models.Model;

namespace KurumsalWeb2.Controllers
{
    public class SliderController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        // GET: Slider
        public ActionResult Index()
        {
            return View(db.Sliders.ToList());
        }

        // GET: Slider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Slider/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Slider/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SliderId,Baslik,Aciklama,ResimURL")] Slider slider,HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {

                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string sliderimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Slider/" + sliderimgname);

                    slider.ResimURL = "/Uploads/Slider/" + sliderimgname;
                }
                db.Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Slider/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Slider/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SliderId,Baslik,Aciklama,ResimURL")] Slider slider,HttpPostedFileBase ResimURL,int id)
        {
            if (ModelState.IsValid)
            {
                var s = db.Sliders.Where(x=>x.SliderId == id).SingleOrDefault();
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(s.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(s.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string sliderimgname = ResimURL.FileName + imginfo.Extension;
                    img.Resize(300, 300);
                    img.Save("~/Uploads/Slider/" + sliderimgname);

                    s.ResimURL = "/Uploads/Slider/" + sliderimgname;
                }
                s.Baslik = slider.Baslik;
                s.Aciklama = slider.Aciklama;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Slider/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Slider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Sliders.Find(id);
            if (System.IO.File.Exists(Server.MapPath(slider.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(slider.ResimURL));
            }
            db.Sliders.Remove(slider);
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
