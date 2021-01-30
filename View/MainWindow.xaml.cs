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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mail_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db;

        public MainWindow()
        {
            InitializeComponent();

            db = new ApplicationContext();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string name = textBoxName.Text.Trim();
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();
            string pass_2 = passBox_2.Password.Trim();

            if (name.Length < 1)
            {
                textBoxName.ToolTip = "Имя не может быть пустым!";
                textBoxName.Background = Brushes.Red;
            }
            else if (login.Length < 1)
            {
                textBoxName.ToolTip = "";
                textBoxName.Background = Brushes.LimeGreen;
                textBoxLogin.ToolTip = "Логин не может быть пустым!";
                textBoxLogin.Background = Brushes.Red;
            }
            else if (pass.Length < 5)
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.LimeGreen;
                passBox.ToolTip = "Пароль слишком короткий!";
                passBox.Background = Brushes.Red;
            }
            else if (pass != pass_2)
            {
                passBox.ToolTip = "";
                passBox.Background = Brushes.LimeGreen;
                passBox_2.ToolTip = "Пароли не совпадают!";
                passBox_2.Background = Brushes.Red;
            }
            else
            { 
                passBox_2.ToolTip = "";
                passBox_2.Background = Brushes.LimeGreen;

                // Добавляем и сохраняем пользователя в базу данных 
                // Предварительно проверив наличие введенного логина в базе
                User logUser = null;
                using (ApplicationContext context = new ApplicationContext())
                {
                    logUser = context.Users.Where(l => l.Login == login).FirstOrDefault();
                }

                if (logUser == null)
                {
                    User user = new User(name, login, pass);
                    db.Users.Add(user);
                    db.SaveChanges();

                    textBoxLogin.ToolTip = "";
                    textBoxLogin.Background = Brushes.LimeGreen;
                    MessageBox.Show("This is really great!");

                    AuthWindow authWindow = new AuthWindow();
                    authWindow.Show();
                    Close();
                }
                else
                {
                    textBoxLogin.ToolTip = "Этот логин уже занят!";
                    textBoxLogin.Background = Brushes.Red;
                }
            }
        }

        /// <summary>
        /// Переход на страницу авторизации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            Close();
        }
    }
}
