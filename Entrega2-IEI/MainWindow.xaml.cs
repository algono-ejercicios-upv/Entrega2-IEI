using Entrega2_IEI.Library;
using Entrega2_IEI.Library.Scrapers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Entrega2_IEI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = new CultureInfo("es-ES");
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            BusquedaListBox.Items.Clear();

            List<IPhoneScraper> scrapers = new List<IPhoneScraper>();
            foreach (CheckBox box in ScraperBoxes.Children)
            {
                if (box.IsChecked ?? false)
                {
                    if (box.Tag is Type type)
                    {
                        if (typeof(IPhoneScraper).IsAssignableFrom(type))
                        {
                            IPhoneScraper scraper = (IPhoneScraper)Activator.CreateInstance(box.Tag as Type);
                            scrapers.Add(scraper);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(
                                $"El objeto de la {nameof(CheckBox)} con contenido '{box.Content}' tiene un valor de un {nameof(Type)} " +
                                $"que no implementa la interfaz {nameof(IPhoneScraper)}.");
                        }
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"El objeto de la {nameof(CheckBox)} con contenido '{box.Content}' " +
                            $"tiene un valor que no es de tipo {nameof(Type)}.");
                    }
                }
            }

            if (scrapers.Count > 0)
            {
                Phone.SearchPhoneInMultipleScrapers(MarcaBox.Text, ModeloBox.Text, (scraper, phones) =>
                {
                    BusquedaListBox.Items.Add($"--------- {scraper.GetType().Name} ----------");

                    foreach (Phone phone in phones)
                    {
                        BusquedaListBox.Items.Add(phone);
                    }
                }, scrapers);

                if (BusquedaListBox.Items.Count > 0)
                {
                    BusquedaListBox.Items.Add("---------------------------------------------");
                }
            }
            
        }
    }
}
