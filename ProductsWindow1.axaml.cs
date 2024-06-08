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
        private User _currentUser;

        //список продуктов, который мы будем потом передавать в другой список
        public static List<Product> ProductsList = new List<Product> { };

        //список товаров, которые в корзине
        //public List<Product> ProductToBasket = new List<Product>() { };

        public ProductsWindow1()
        {
            InitializeComponent();

            //Установка контекста данных окна на текущий объект
            DataContext = this;
        }

        public ProductsWindow1(User currentUser, List<Product> productslist)
        {
            ProductsList = productslist;

            DataContext = this;
            _currentUser = currentUser; // Инициализация _currentUser
            InitializeComponent(); // Инициализация компонентов

            queenStatus.Text = "Статус: " + _currentUser.UserStatus;
            queenName.Text = "Имя: " + _currentUser.UserName;

            ProductsList.Add(
               new Product()
               {
                   TovarName = "Samsung Galaxy A72",
                   Manufacturer = "Samsung",
                   Description = "Безграничный экран AMOLED 6.7', 256ГБ, Черный",
                   Stock = 34,
                   Price = 33990
               });

            ProductsList.Add(
               new Product()
               {
                   TovarName = "Poco F5 Pro",
                   Manufacturer = "Xiaomi",
                   Description = "256/8ГБ, Белый, AMOLED FHD+, 6.67', 2G/3G/4G (LTE)/5G",
                   Stock = 34,
                   Price = 33990
               });

            ProductsList.Add(
               new Product()
               {
                   TovarName = "ITEL A70",
                   Manufacturer = "ATEL",
                   Description = "IPS, 6.6' (1612x720), Unisoc T603, 2G/3G/4G (LTE)",
                   Stock = 34,
                   Price = 33990
               });

            //задаем видимость кнопок Редактировать и Удалить для каждого товара в зависимости от статуса
            foreach (var product in ProductsList)
            {
                product.Otobrazhenie = _currentUser.UserStatus == "Queen";
            }

            //-------------------------------------------
            foreach (var product in ProductsList) //добавляем все товары из списка в Listbox
            {
                if (product.Stock == 0)
                {
                    Color customColor = Color.FromRgb(102, 98, 103);
                    SolidColorBrush brush = new SolidColorBrush(customColor);
                    product.change_color = brush;
                }

                Tovarslistbox.Items.Add(product);
            }

            //-------------------------------------------

            // Добавляем элемент в список
            Button addProductButton = this.FindControl<Button>("DobavitTovar");
            if (_currentUser.UserStatus != "Queen")
            {
                addProductButton.IsVisible = false;
            }

            else if (_currentUser.UserStatus == "гость")
            {
                queenName.Text = "";
            }
            else
            {
                addProductButton.IsVisible = true;
            }
        }

        //удалить товар
        private void DeleteTovar_Button(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, на которую было нажатие
            Button deleteButton = (Button)sender;

            // Получаем контекст данных этой кнопки (т.е. элемент Products, к которому привязана кнопка)
            Product product = (Product)deleteButton.DataContext;

            //если удаляемый товар есть в корзине, То выводим сообщение 
            if (_currentUser.Products.Contains(product))
            {
                Message message = new Message();

                message.oshibka1.Text = "Ошибка!!! Вы не можете удалить товар из ассортимента, т.к он присутствует в корзине у пользователя!";

                message.Show();
            }
            else
            {
                //если нет в корзине, То удаляем из списка товаров
                ProductsList.Remove(product);
                Tovarslistbox.Items.Remove(product);
            }
        }

        //отредактировать товар
        public void RedactButton(object sender, RoutedEventArgs e)
        {
            Button edittovar = (Button)sender;

            Product editTovar = (Product)edittovar.DataContext;

            Redactirovanie tovar = new Redactirovanie(editTovar, this, ProductsList);
            tovar.Show();
        }

        //это запускаем в окне AddProduct, чтобы прямо в этот список добавить товар
        public void UpdateProductsList(Product newProduct)
        {
            ProductsList.Add(newProduct);
            newProduct.Otobrazhenie = _currentUser.UserStatus == "Queen";

            if (newProduct.Stock == 0)
            {
                Color customColor = Color.FromRgb(102, 98, 103);
                SolidColorBrush brush = new SolidColorBrush(customColor);
                newProduct.change_color = brush;
            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush(Colors.Transparent);
                newProduct.change_color = brush;
            }
            Tovarslistbox.Items.Add(newProduct);
        }

        //добавляем товар в наш список
        private void AddTovar_Button(object sender, RoutedEventArgs e)
        {
            // Передаем ссылку на исходный список товаров в окно добавления товара
            Add ap = new Add(this);
            ap.Show();
        }

        //возвращаемся на окно авторизации
        private void Exit_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow auth = new MainWindow(ProductsList);
            auth.Show();

            this.Close();
        }

        private void PriceUbivanie(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderByDescending(item => item.Price).ToList();
            UpdateListBox(SortedList);
        }

        private void PriceVozrastanie(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderBy(item => item.Price).ToList();
            UpdateListBox(SortedList);
        }

        private void ProizvoditelAlfavit(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderBy(item => item.Manufacturer).ToList();
            UpdateListBox(SortedList);
        }

        private void ProizvoditelObratno(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderByDescending(item => item.Manufacturer).ToList();
            UpdateListBox(SortedList);
        }

        private void UpdateListBox(List<Product> ListToAdd) //Обновляем список товаров
        {
            Tovarslistbox.Items.Clear();

            foreach (var item in ListToAdd)
            {
                Tovarslistbox.Items.Add(item);
            }
        }

        private void DobavitKorzinu(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, на которую было нажатие
            Button addtobasket = (Button)sender;

            // Получаем контекст данных этой кнопки (т.е. элемент Products, к которому привязана кнопка)
            Product product = (Product)addtobasket.DataContext;

            //проверка на добавление 
            if (product.Stock == 0)
            {
                Message message = new Message();
                message.oshibka1.Text = "ОШИБКА!!! Вы не можете добавить в корзину товар, которого нет на складе!!";

                message.Show();
            }

            else
            {
                _currentUser.Products.Add(product);

                //Korzina basket = new Korzina(_currentUser);
                //basket.Show();
            }

        }

        //Перейти в корзину
        private void PoitiKorzinu(object sender, RoutedEventArgs e)
        {
            Korzina basket = new Korzina(_currentUser);
            basket.Show();
        }
    }
}

