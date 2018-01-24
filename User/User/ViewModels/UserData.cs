using System.Collections.Generic;
using User.Models;

namespace User.ViewModels
{
    public class UserData
    {
        public IEnumerable<Models.User> Users { get; set; }
    }
}