using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

//Person types and validations
// These Fileds will be created in the database
namespace User.Models
{
	public class User
	{
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
    }
}