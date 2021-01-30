using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail_Manager
{
    class UserMail
    {
        public int id { get; set; }
        private string name, server, login, password, user;
        private int portSend, portFrom;

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Server
        {
            get { return server; }
            set { server = value; }
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

        public int PortSend
        {
            get { return portSend; }
            set { portSend = value; }
        }

        public int PortFrom
        {
            get { return portFrom; }
            set { portFrom = value; }
        }

        public UserMail() { }

        public UserMail(string user, string name, string server, int portSend, int portFrom, string login, string pass)
        {
            this.user = user;
            this.name = name;
            this.server = server;
            this.portSend = portSend;
            this.portFrom = portFrom;
            this.login = login;
            this.password = pass;
        }
    }
}
