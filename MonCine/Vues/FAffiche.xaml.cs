using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MonCine.Data;

namespace MonCine.Vues
{
    /// <summary>
    /// Interaction logic for FAffiche.xaml
    /// </summary>
    public partial class FAffiche : Page
    {
        private DALProjection dalProjection { get; set; }
        private DALFilm dalFilm { get; set; }

        public FAffiche(DALProjection pDalProjection, DALFilm pDalFilm)
        {
            InitializeComponent();

            dalProjection = pDalProjection;
            dalFilm = pDalFilm;
            InitialConfiguration();
        }


        private void InitialConfiguration()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            List<Projection> Projections = dalProjection.GetProjectionsByDate(date);

            lstFilms.ItemsSource = dalFilm.ReadItems();
            lstProjectionsAffiche.ItemsSource = Projections;

            btnRetirerProjection.IsEnabled = false;

        }


        private void BtnReturn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void btnRetirerProjection_Click(object sender, RoutedEventArgs e)
        {
            Projection projection = lstProjectionsAffiche.SelectedItem as Projection;

            if (projection is null)
            {
                MessageBox.Show("Veuillez sélectionner une projection programmée pour la retirer", "Suppression de projection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool result = dalProjection.DeleteItem(projection);
                if (result)
                {
                    lstProjectionsAffiche.SelectedIndex = -1;
                    InitialConfiguration();
                    MessageBox.Show("La projection a été supprimée avec succès!" ,"Suppression de projection", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void lstProjectionsAffiche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            btnRetirerProjection.IsEnabled = lstProjectionsAffiche.SelectedItem is Projection;
        }
    }
}
