using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Teatime.Model;
using Teatime.Utils;

namespace Teatime
{
    public partial class AddressBookWindow : Window
    {
        private readonly TextBlockLogger logger;

        public AddressBookWindow(List<Participant> addressBook, TextBlockLogger logger)
        {
            this.logger = logger;

            this.InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            ObservableCollection<Participant> observableCollection = new ObservableCollection<Participant>(addressBook);
            observableCollection.CollectionChanged += this.AddressBookDataGrid_CollectionChanged;
            this.AddressBookDataGrid.ItemsSource = observableCollection;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddressBookDataGrid_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            this.logger.LogInfo("OnRowEditEnding, e.EditAction = " + e.EditAction + ", e.Cancel = " + e.Cancel);
        }

        void AddressBookDataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.logger.LogInfo("CollectionChanged, e.Action = " + e.Action);
        }
    }
}
