using KurumsalWeb2.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KurumsalWeb2.Models.DataContext
{
    public class KurumsalDBContext :DbContext
    {
        public KurumsalDBContext() :base("KurumsalWebDB2")
        {
                
        }

        public DbSet<Admin> Admin{ get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Hakkimizda> Hakkimizda { get; set; }
        public DbSet<Hizmet> Hizmet { get; set; }
        public DbSet<Iletisim> Iletisim { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<Kimlik> Kimlik { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Yorum> Yorum { get; set; }
    }
}