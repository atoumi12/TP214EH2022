using System;
using System.Collections.Generic;
using System.Linq;
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
        private DALSalle dalSalle { get; set; }
        private List<Salle> Salles { get; set; }
        private DateTime DateRecherche { get; set; }

        public FAffiche(DALProjection pDalProjection, DALSalle pDalSalle)
        {
            InitializeComponent();

            dalProjection = pDalProjection;
            dalSalle = pDalSalle;

            // Date d'ajourd'hui au premier affichage du formulaire
            DateRecherche= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            InitialConfiguration(DateRecherche);
        }


        private void InitialConfiguration(DateTime pDate)
        {
            GroupBoxInformationsProjection.Visibility = Visibility.Hidden;

            DatePickerRecherche.SelectedDate = pDate;
            // Projection
            List<Projection> Projections = dalProjection.GetProjectionsByDate(pDate);
            lstProjectionsAffiche.ItemsSource = Projections;

            // Salles
            Salles = dalSalle.ReadItems();
            Salles.ForEach(salle => ComboBoxSalles.Items.Add(salle));


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
                    InitialConfiguration(DateRecherche);
                    MessageBox.Show("La projection a été supprimée avec succès!" ,"Suppression de projection", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void lstProjectionsAffiche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Projection projection = lstProjectionsAffiche.SelectedItem as Projection;

            if (projection != null)
            {
                GroupBoxInformationsProjection.Visibility = Visibility.Visible;

                txtNomFilm.Text = projection.Film.Name;
                DatePickerDateProjectionFilm.SelectedDate = projection.DateDebut;

                Salle salle = Salles.Where(s => s.Id == projection.Salle.Id).ToList()[0];
                ComboBoxSalles.SelectedItem = salle;

            }
            else
            {
                GroupBoxInformationsProjection.Visibility = Visibility.Hidden;
            }



        }

        private void DatePickerRecherche_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date = DatePickerRecherche.SelectedDate.Value;
            InitialConfiguration(date);
        }

        private void btnModifierProjection_Click(object sender, RoutedEventArgs e)
        {
            Projection projection = lstProjectionsAffiche.SelectedItem as Projection;

            if (projection != null)
            {
                dalProjection.UpdateItem(projection);
            }
        }
    }
}
