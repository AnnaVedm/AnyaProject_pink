using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using NAudio;
using System;
using System.Collections.Generic;

namespace AnyaProject
{
    public partial class Add : Window
    {
        public List<Product> tovars2 = new List<Product>();
        private User _user;

        public Add()
        {
            InitializeComponent();
        }

        public Add(User user, List<Product> Tovars)
        {
            InitializeComponent();
            DataContext = this;

            _user = user;
            tovars2 = Tovars;
        }

        private void DobavitTovar_Button(object sender, RoutedEventArgs e)
        {
            tovars2.Add(
                new Product()
                {
                    TovarName = TovarName.Text,
                    Manufacturer = TovarProizvoditel.Text,
                    Description = TovarOpisanie.Text,
                    Price = Convert.ToDouble(TovarPrice.Text),
                    Stock = Convert.ToInt32(TovarOstatok.Text)
                    
                });

            ProductsWindow1 productsWindow1 = new ProductsWindow1(_user, tovars2);
            productsWindow1.Show();

            this.Close();
        }
    }
}
