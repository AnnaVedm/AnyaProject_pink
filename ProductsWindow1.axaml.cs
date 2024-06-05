using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AnyaProject
{
    public partial class ProductsWindow1 : Window
    {
        //private Border collapsiblePanel;
        private User queenUser;

        public List<Product> tovars = new List<Product>() { }; //список товаров
        public List<string> ItemsList { get; set; } = new List<string> {};

        public ProductsWindow1()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ProductsWindow1(User QueenUser, List<Product> Tovars)
        {
            queenUser = QueenUser;
            tovars = Tovars;

            //tovars.Add(
            //   new Product()
            //   {
            //       TovarName = "Товар",
            //       Manufacturer = "РОЛПОЛ",
            //       Description = "Я самая пиздатая блять поняли блять. Слободская это центр мира блять понятно блять все дороги ведут к слободской. ДЯНА ТЫ АХУЕЛА БЛЯТЬ ПОШЛА ВОН БЛЯТЬ",
            //       Price = 482492,
            //       Stock = 10
            //   });

            InitializeComponent();


            DataContext = this;

            //foreach (var item in ItemsList)
            //{
            //    ProizvoditeliList.Items.Add(item);
            //}

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
                if (tovar.Stock == 0)
                {
                    tovar.change_color = new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    tovar.change_color = new SolidColorBrush(Colors.White);
                }
                Tovarslistbox.Items.Add(tovar);
            }
            this.Opened += UpdateWindow;
        }

        //чтобы не передавать список в окно добавления товара, создаем Public метод, который будем запускать в окне Add
        //public void UpdateItemsList(string proizvoditel)
        //{
        //    ItemsList.Add(proizvoditel);
        //}

        //private void SortProizvoditel(object sender, SelectionChangedEventArgs e)
        //{
        //    if (ProizvoditeliList == null || ProizvoditeliList.SelectedItem == null)
        //    {
        //        return; // Выход из метода, если ничего не выбрано или ProizvoditeliList равен null
        //    }

        //    else
        //    {
        //        string selectedProizvoditel = (string)ProizvoditeliList.SelectedItem;

        //        if (selectedProizvoditel == "Все производители")
        //        {
        //            return;
        //        }

        //        //нужно сделать так, чтобы те товары, которые нам не подходят, не были видимы
        //        //значит нужно для stackpanel через binding установить видимость

        //        else
        //        {
        //            foreach (var tovar in tovars)
        //            {
        //                if (!(tovar.Manufacturer == selectedProizvoditel))
        //                {
        //                    //если производитель не соответствует выбранному, то видимость товара устанавливаетсяя на false
        //                    tovar.tovarsVisibility = false;
        //                }
        //            }
        //        }
        //    }
        //}

        private void AddTovar_Button(object sender, RoutedEventArgs e)
        {
            Add addtovar = new Add(queenUser, tovars /*this*/);
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

            Redactirovanie tovar = new Redactirovanie(editTovar, this, tovars);
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

