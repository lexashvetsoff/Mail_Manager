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
using System.Data.Entity;
using System.Net;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Imap;
using MailKit.Search;
using System.IO;

namespace Mail_Manager
{
    /// <summary>
    /// Логика взаимодействия для UserPageWindow.xaml
    /// </summary>
    public partial class UserPageWindow : Window
    {
        public string loginUser;
        private string curPressMail;

        private AppMailContext db = new AppMailContext();

        public UserPageWindow(string loginUser)
        {
            InitializeComponent();
            this.loginUser = loginUser;

            AddListMail();
        }

        /// <summary>
        /// Реализация кнопки "Добавить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            MailAddForm mailAddForm = new MailAddForm(loginUser);
            mailAddForm.ShowDialog();
            AddListMail();
        }

        /// <summary>
        /// Извлекает из базы и добавляет на панель список пользовательских ящиков
        /// </summary>
        private void AddListMail()
        {
            List<UserMail> userMails = db.UserMails.ToList();

            List<UserMail> currentUserMail = new List<UserMail>();
            foreach(UserMail um in userMails)
            {
                if (um.User == loginUser)
                    currentUserMail.Add(um);
            }

            if (currentUserMail.Count != 0)
                listOfMails.ItemsSource = currentUserMail;
            else
                MessageBox.Show("У вас пока не добавленно ни одного ящика!");
        }

        /// <summary>
        /// Реализация кнопки "Выход"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("До свидания!");
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            Close();
        }

        /// <summary>
        /// Нажатие на кнопку из списка добавленных ящиков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var senderBtn = sender as Button;
            curPressMail = senderBtn.Content.ToString();

            try
            {
                var con = (from x in db.UserMails where x.User == loginUser &&
                     x.Name == curPressMail select x).First();
                string imap = "imap." + con.Server;

                using (var client = new MailKit.Net.Imap.ImapClient())
                {
                    client.Connect(imap, con.PortFrom, true); // Коннект к серверу
                    client.Authenticate(con.Login, con.Password); // Авторизация
                    client.Inbox.Open(MailKit.FolderAccess.ReadWrite); // Открываем папку Inbox с правами Read Write
                    
                    for(int i = client.Inbox.Count - 1; i < client.Inbox.Count; i++) // Получаем ID последнего сообщения
                    {
                        var message = client.Inbox.GetMessage(i); // Получаем сообщение по ID в данном случае мы получаем id самого нового сообщения
                        RichBox.AppendText(message.Body.ToString()); // Выводит тело сообщение в RichBox
                        client.Inbox.AddFlags(i, MailKit.MessageFlags.Deleted, false); // Удаляем сообщение.
                    }

                    for(int i = 0; i < 10; i++) // Перебираем все ID сообщений
                    {
                        var message = client.Inbox.GetMessage(i); // Получаем сообщение по списку от самого последнего по самое новое
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось авторизоваться на сервере!");
            }
        }

        /// <summary>
        /// Реализация кнопки удаления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var y = (from x in db.UserMails where x.User == loginUser &&
                     x.Name == curPressMail select x).First();

            var entry = db.Entry(y);
            if (entry.State == EntityState.Detached)
                db.UserMails.Attach(y);
            db.UserMails.Remove(y);
            db.SaveChanges();
            AddListMail();
        }
    }
}