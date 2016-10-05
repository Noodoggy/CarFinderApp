using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Cars> Car { get; set; }

        public async Task<List<string>> GetYears()
        {
            return await Database.SqlQuery<string>("GetAllYears").ToListAsync();
        }

        public async Task<List<string>> GetMakesByYear(string year)
        {
            return await Database.SqlQuery<string>("GetMakesByYear @year",
                new SqlParameter("year", year)).ToListAsync();
        }

        public async Task<List<string>> GetMakesByYear2(string year)
        {
            return await Database.SqlQuery<string>("GetMakesByYear @year",
                new SqlParameter("year", year)).ToListAsync();
        }

        public async Task<List<string>> GetModelsByYearAndMake(string year, string make)
        {
            return await Database.SqlQuery<string>("GetModelsByYearAndMake @year, @make",
                new SqlParameter("year", year),
                new SqlParameter("make", make)).ToListAsync();
        }

        public async Task<List<string>> GetTrimsByYearMakeAndModel(string year, string make, string model)
        {
            return await Database.SqlQuery<string>("GetTrimsByYearMakeAndModel @year, @make, @model",
                new SqlParameter("year", year),
                new SqlParameter("make", make),
                new SqlParameter("model", model)).ToListAsync();
        }


        public async Task<List<Cars>> GetCarsByYear(string year)
        {
            return await Database.SqlQuery<Cars>("GetCarsByYear @year",
                new SqlParameter("year", year)).ToListAsync();
        }

        public async Task<List<Cars>> GetCarsByYearAndMake(string year, string make)
        {
            return await Database.SqlQuery<Cars>("GetCarsByYearAndMake @year, @make",
                new SqlParameter("year", year),
                new SqlParameter("make", make)).ToListAsync();
        }

        public async Task<List<Cars>> GetCarsByYearMakeAndModel(string year, string make, string model)
        {
            return await Database.SqlQuery<Cars>("GetCarsByYearMakeAndModel @year, @make, @model",
                new SqlParameter("year", year),
                new SqlParameter("make", make),
                new SqlParameter("model", model)).ToListAsync();
        }

        public async Task<Cars> GetCar(string year, string make, string model, string trim)
        {
            return await Database.SqlQuery<Cars>("GetCar @year, @make, @model, @trim",
                new SqlParameter("year", year),
                new SqlParameter("make", make),
                new SqlParameter("model", model),
                new SqlParameter("trim", trim)).SingleAsync();
        }

        public async Task<Cars> GetNullCar(string year, string make, string model, string trim)
        {

            return await Database.SqlQuery<Cars>("GetNullCars @year, @make, @model, @trim",
                new SqlParameter("year", year),
                new SqlParameter("make", make),
                new SqlParameter("model", model),
                new SqlParameter("trim", trim)).SingleAsync();
        }

        public async Task<Cars> GetNullableCar(string year, string make, string model, string trim)
        {

            return await Database.SqlQuery<Cars>("GetNullCars @year, @make, @model, @trim",
                new SqlParameter("year", year),
                new SqlParameter("make", make),
                new SqlParameter("model", model),
                new SqlParameter("trim", trim)).SingleAsync();
        }
    }
}