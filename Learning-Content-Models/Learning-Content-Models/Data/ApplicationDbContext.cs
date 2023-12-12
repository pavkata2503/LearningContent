using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Learning_Content_Models.Models;
using Humanizer.Localisation;
using System.Net.Sockets;

namespace Learning_Content_Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<StudyMaterial> StudyMaterials { get; set; }
        public DbSet<Message> Messages { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
