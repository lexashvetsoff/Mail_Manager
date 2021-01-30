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

// Страница авторизации

namespace Mail_Manager
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();

            if (login.Length < 1)
            {
                textBoxLogin.ToolTip = "Логин не может быть пустым!";
                textBoxLogin.Background = Brushes.Red;
            }
            else if (pass.Length < 5)
            {
                passBox.ToolTip = "Пароль слишком короткий!";
                passBox.Background = Brushes.Red;
            }
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;

                //Проверка на наличие введенных данных в базе данных
                User authUser = null;
                using (ApplicationContext context = new ApplicationContext())
                {
                    authUser = context.Users.Where(b => b.Login == login && 
                        b.Password == pass).FirstOrDefault();
                }

                if (authUser != null)
                {
                    UserPageWindow userPageWindow = new UserPageWindow(login);
                    userPageWindow.Show();
                    Close();
                }
                else
                    MessageBox.Show("Не корректный ввод!");
            }
        }

        private void Button_Window_Reg_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
