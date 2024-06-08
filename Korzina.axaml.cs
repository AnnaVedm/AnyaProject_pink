using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace AnyaProject;

public partial class Korzina : Window
{
    private User _user;
    public Korzina()
    {
        InitializeComponent();
    }
    public Korzina(User user)
    {
        InitializeComponent();

        
        _user = user;

        DataContext = this;

        foreach (var p in _user.Products)
        {
            Tovarslistbox_Korzina.Items.Add(p);
        }
    }
}
