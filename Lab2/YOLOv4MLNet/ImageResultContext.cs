using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Component
{
    public class ImageResultContext : DbContext
    {
        public DbSet<ImageResultDB> Images { get; set; }
        public DbSet<ObjectsData> Results { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder o) =>
            o.UseLazyLoadingProxies().UseSqlite("DataSource=C:\\Users\\mrded\\406_dedinov\\Lab2\\YOLOv4MLNet\\library.db");
    }
}
