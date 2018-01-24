using User.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;
using System.Web;
using System.Configuration;

namespace User.Models
{
	public class UserContext : DbContext
	{
        public UserContext()
        {
            string dbPath = HttpContext.Current.Server.MapPath("..\\App_Data\\Database.mdf");
            string serverName = "(LocalDB)\\MSSQLLocalDB";
            string connString = String.Format("Data Source={0};AttachDbFilename={1};Integrated Security=True;Connect Timeout=30", serverName, dbPath);

            this.Database.Connection.ConnectionString = connString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<User> Users { get; set; }
    }
}