using User.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;
using System.IO;
using System.Web;
using System.Configuration;

namespace User.Models
{
	public class UserContext : DbContext
	{
        public UserContext()
        {
            //using inline paths instead of web.config because default values weren't working
            string projectPath = HttpContext.Current.Server.MapPath("~");
            string dbPath = String.Format("{0}{1}", projectPath, "App_Data\\Database.mdf");
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