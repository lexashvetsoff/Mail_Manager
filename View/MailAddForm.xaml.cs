using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mail_Manager
{
    /// <summary>
    /// Логика взаимодействия для MailAddForm.xaml
    /// </summary>
    public partial class MailAddForm : Window
    {
        public string currentUser;
        AppMailContext db;

        public MailAddForm(string currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;

            db = new AppMailContext();
        }

        private void Button_Add_Mail_Click(object sender, RoutedEventArgs e)
        {
            string name = textBoxAddName.Text.Trim();
            string server = textBoxAddServer.Text.Trim();
            int portSend = int.Parse(textBoxAddPortSend.Text.Trim());
            int portFrom = int.Parse(textBoxAddPortFrom.Text.Trim());
            string login = textBoxAddLogin.Text.Trim();
            string password = passAddBox.Password.Trim();
            string user = currentUser;

            // Проверка введенных данных
            if (name.Length < 1)
            {
                textBoxAddName.ToolTip = "Имя не может быть пустым!";
                textBoxAddName.Background = Brushes.Red;
            }
            else if (server.Length < 4  &&  !server.Contains("."))
            {
                textBoxAddName.ToolTip = "";
                textBoxAddName.Background = Brushes.LimeGreen;
                textBoxAddServer.ToolTip = "Сервер указан не корректно!";
                textBoxAddServer.Background = Brushes.Red;
            }
            else if (login.Length < 5)
            {
                textBoxAddServer.ToolTip = "";
                textBoxAddServer.Background = Brushes.LimeGreen;
                textBoxAddLogin.ToolTip = "Логин указан не корректно!";
                textBoxAddLogin.Background = Brushes.Red;
            }
            else if (password.Length < 5)
            {
                textBoxAddLogin.ToolTip = "";
                textBoxAddLogin.Background = Brushes.LimeGreen;
                passAddBox.ToolTip = "Проверте пароль!";
                passAddBox.Background = Brushes.Red;
            }
            else
            {
                UserMail usersMail = new UserMail(user, name, server, portSend, portFrom, login, password);
                db.UserMails.Add(usersMail);
                db.SaveChanges();
                Close();
            }
        }
    }
}
