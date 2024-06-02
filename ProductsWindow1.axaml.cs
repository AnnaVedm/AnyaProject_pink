using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Threading.Tasks;

namespace AnyaProject
{
    public partial class ProductsWindow1 : Window
    {
        //private Border collapsiblePanel;
        private User queenUser;

        public List<Product> tovars = new List<Product>() { }; //список товаров

        public ProductsWindow1()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ProductsWindow1(User QueenUser, List<Product> Tovars)
        {
            queenUser = QueenUser;
            tovars = Tovars;

            InitializeComponent();

            DataContext = this;

            Button dobavitTovar = this.FindControl<Button>("DobavitTovar");
            Button deleteVibrannoe = this.FindControl<Button>("DeleteVibrannoe");

            if (queenUser.UserStatus == "Queen")
            {
                dobavitTovar.IsVisible = true;
                //deleteVibrannoe.IsVisible = true;
            }
            else
            {
                dobavitTovar.IsVisible = false;
                //deleteVibrannoe.IsVisible = false;
            }

            foreach (var tovar in tovars)
            {
                Tovarslistbox.Items.Add(tovar);
            }

            this.Opened += UpdateWindow;
        }

        private void AddTovar_Button(object sender, RoutedEventArgs e)
        {
            Add addtovar = new Add(queenUser, tovars);
            addtovar.Show();
            this.Close();
        }

        private void PriceVozrastanie(object sender, RoutedEventArgs e)
        {
            var tovars2 = tovars.OrderBy(item => item.Price).ToList();

            Tovarslistbox.Items.Clear();

            foreach (var tovar in tovars2)
            {
                Tovarslistbox.Items.Add(tovar);
            }
        }

        private void ProizvoditelAlfavit(object sender, RoutedEventArgs e)
        {
            var tovars2 = tovars.OrderBy(item => item.Manufacturer).ToList();

            Tovarslistbox.Items.Clear();

            foreach (var tovar in tovars2)
            {
                Tovarslistbox.Items.Add(tovar);
            }
        }

        private void ProizvoditelObratno(object  sender, RoutedEventArgs e)
        {
            var tovars2 = tovars.OrderByDescending(item => item.Manufacturer).ToList();

            Tovarslistbox.Items.Clear();

            foreach (var tovar in tovars2)
            {
                Tovarslistbox.Items.Add(tovar);
            }
        }

        private void PriceUbivanie(object sender, RoutedEventArgs e)
        {
            var tovars2 = tovars.OrderByDescending(item => item.Price).ToList();

            Tovarslistbox.Items.Clear();

            foreach (var tovar in tovars2)
            {
                Tovarslistbox.Items.Add(tovar);
            }
        }

        private void UpdateWindow(object? sender, EventArgs e)
        {
            // Установка значения свойства IsAdmin для каждого элемента в ProductsList
            foreach (var product in tovars)
            {
                product.Otobrazhenie = queenUser.UserStatus == "Queen";
            }
        }

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>()
        {
            new User()
            {
                UserName = "Отсутствует",
                UserPassword = "",
                UserStatus = "Гость"
            },

            new User()
            {
                UserName = "AnnaVedm",
                UserPassword = "1234",
                UserStatus = "Queen"
            },

            new User()
            {
                UserName = "Anna",
                UserPassword = "1234",
                UserStatus = "Thief"
            }
        };

        private void DeleteTovar_Button(object sender, RoutedEventArgs e)
        {
            //получаем кнопку на которую нажали
            Button deletetovar = (Button)sender;

            //получаем все данные принадлежащие этой кнопке
            Product tovar2 = (Product)deletetovar.DataContext;

            //удаляем
            tovars.Remove(tovar2);
            Tovarslistbox.Items.Remove(tovar2);
        }


        public void RedactButton(object sender, RoutedEventArgs e)
        {
            Button edittovar = (Button)sender;

            Product editTovar = (Product)edittovar.DataContext;

            Redactirovanie tovar = new Redactirovanie(editTovar);
            tovar.Show();
        }

        private async void Exit_ButtonClick(object sender, RoutedEventArgs e) //кнопка перехода обратно на окно авторизации
        {
            MainWindow window = new MainWindow(tovars);
            
            window.Show();
            this.Close();
        }
    }
}

