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

using ImapX;
using ImapX.Collections;
using Mail_Manager.Model;

namespace Mail_Manager
{
    /// <summary>
    /// Логика взаимодействия для UserPageWindow.xaml
    /// </summary>
    public partial class UserPageWindow : Window
    {
        public string loginUser;
        private string curPressMail;
        private string curFolder;

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

            var con = (from x in db.UserMails where x.User == loginUser &&
                       x.Name == curPressMail select x).First();
            string imap = "imap." + con.Server;

            MessageBox.Show("Почтовый ящик: " + con.Name);

            var client = new ImapX.ImapClient(imap, con.PortFrom, true);
            if (client.Connect())
            {
                if (client.Login(con.Login, con.Password))
                {
                    var folders = new List<EmailFolde>();
                    foreach(var folder in client.Folders)
                    {
                        folders.Add(new EmailFolde { Title = folder.Name });
                    }
                    foldersList.ItemsSource = folders;
                    MessageBox.Show("Подключение успешно!");
                }
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к серверу!");
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

        /// <summary>
        /// Отображения писем выбранной папки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Folder_Click(object sender, RoutedEventArgs e)
        {
            var senderBtn = sender as Button;
            curFolder = senderBtn.Content.ToString();
            MessageBox.Show(curFolder);

            var con = (from x in db.UserMails where x.User == loginUser &&
                       x.Name == curPressMail select x).First();
            string imap = "imap." + con.Server;

            MessageBox.Show("Почтовый ящик: " + con.Name);

            var client = new ImapX.ImapClient(imap, con.PortFrom, true);
            if (client.Connect())
            {
                if (client.Login(con.Login, con.Password))
                {
                    messagesList.ItemsSource = GetMessagesForFolder(client, curFolder);
                }
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к серверу!");
            }
        }


        /// <summary>
        /// Получение списка писем
        /// </summary>
        /// <param name="cln"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MessageCollection GetMessagesForFolder(ImapX.ImapClient cln, string name)
        {
            cln.Folders[name].Messages.Download("ALL", ImapX.Enums.MessageFetchMode.Basic, 10); // начало закачки
            return cln.Folders[name].Messages;
        }
    }
}