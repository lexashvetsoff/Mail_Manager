using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail_Manager
{
    class User
    {
        public int id { get; set; }
        private string name, login, password;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public User() { }

        public User(string name, string login, string password)
        {
            this.name = name;
            this.login = login;
            this.password = password;
        }
    }
}
