using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Mail_Manager
{
    class AppMailContext : DbContext
    {
        public DbSet<UserMail> UserMails { get; set; }

        public AppMailContext() : base("DefaultConnection") { }
    }
}
