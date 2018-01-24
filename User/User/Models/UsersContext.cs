using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace User.Models
{
	public class UsersContext : DbContext
	{
        public UsersContext() : base("UserDatabase")
        {

        }
        public virtual DbSet<Users> Users { get; set; }
		
	}
}