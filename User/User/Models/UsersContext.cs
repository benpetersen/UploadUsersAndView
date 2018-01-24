using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace User.Models
{
	public partial class UsersContext : DbContext
	{
		public UsersContext()
			: base("name=UsersContext")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			throw new UnintentionalCodeFirstException();
		}

		public virtual DbSet<Users> Users { get; set; }
	}
}