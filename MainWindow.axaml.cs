using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using NAudio;
using NAudio.Wave;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AnyaProject
{
    public partial class MainWindow : Window
    {
        private List<Product> Tovars1 = new List<Product>();

        User user = new User();

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(List<Product> tovars)
        {
            Tovars1 = tovars;
            InitializeComponent();
            DataContext = this;
        }

        private async void VoitiVAkkaynt(object sender, RoutedEventArgs e)
        {
            ProductsWindow1 w = new ProductsWindow1();

            if (!string.IsNullOrWhiteSpace(Name.Text)) //Если строка непустая, то проверяем наш список на наличие этих полей
            {
                User proverka = w.Users.FirstOrDefault(u => u.UserName == Name.Text && u.UserPassword == Password.Text);

                if (proverka != null)
                {
                    // Вход выполнен успешно
                    Oshibka.Text = null;

                    ProductsWindow1 wp = new ProductsWindow1(proverka, Tovars1);
                    wp.Show();

                    this.Close();
                    
                }
                else
                {
                    // Ошибка входа
                    Oshibka.Text = "Неверно введены данные";
                    Password.Clear();
                }
            }
        }

        private void Guest_Button(object sender, RoutedEventArgs e)
        {
            //для получения доступа к списку пользователей
            ProductsWindow1 q = new ProductsWindow1();

            //для входа в режим гостя выбираем пользователя под нулевым индексом - со статусом гость и без пароля
            ProductsWindow1 open_okno = new ProductsWindow1(q.Users[0], Tovars1);
            open_okno.Show();
            this.Close();
        }
    }
}