using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace AnyaProject
{
    public partial class Redactirovanie : Window
    {
        private Product _tovar;

        public Redactirovanie()
        {
            InitializeComponent();
        }

        public Redactirovanie(Product tovar)
        {
            _tovar = tovar;
            InitializeComponent();
            DataContext = tovar;
        }

        private void ApplyRedactirovanie(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TovarName.Text)) //если наша строка непустая
            {
                _tovar.TovarName = TovarName.Text;
            }
            if (!string.IsNullOrWhiteSpace(TovarProizvoditel.Text))
            {
                _tovar.Manufacturer = TovarProizvoditel.Text;
            }

            if (!string.IsNullOrWhiteSpace(TovarOpisanie.Text))
            {
                _tovar.Description = TovarOpisanie.Text;
            }
            if (!string.IsNullOrWhiteSpace(TovarOstatok.Text))
            {
                _tovar.Stock = Convert.ToInt32(TovarOstatok.Text);
            }
            if (!string.IsNullOrWhiteSpace(TovarPrice.Text))
            {
                _tovar.Price = Convert.ToDouble(TovarPrice.Text);
            }

            TadaText.Text = "ТА - ДА!";
        }
    }
}
