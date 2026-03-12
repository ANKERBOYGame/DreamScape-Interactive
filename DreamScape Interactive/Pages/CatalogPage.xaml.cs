using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DreamScape_Interactive.Models;
using DreamScape_Interactive.Services;
using Microsoft.UI.Xaml.Controls;

namespace DreamScape_Interactive.Pages
{
    public sealed partial class CatalogPage : Page
    {
        public ObservableCollection<Item> Items { get; } = new();

        private readonly DatabaseService _databaseService;

        public CatalogPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _ = LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            await _databaseService.InitializeDatabaseAsync();

            Items.Clear();
            foreach (var item in await _databaseService.GetAllItemsAsync())
            {
                Items.Add(item);
            }
        }
    }
}
