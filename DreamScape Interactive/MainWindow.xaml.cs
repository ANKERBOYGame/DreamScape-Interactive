using DreamScape_Interactive.Pages;
using Microsoft.UI.Xaml;

namespace DreamScape_Interactive
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RootFrame.Navigate(typeof(CatalogPage));
        }
    }
}
