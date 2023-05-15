using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Test_Technique.Models;

namespace Test_Technique.TT_Data
{
    public class AppDbContext :IdentityDbContext <IdentityUser>
    {
        //ctor
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
        }
        public DbSet<Candidat> candidats { get; set; }      
    }
}
