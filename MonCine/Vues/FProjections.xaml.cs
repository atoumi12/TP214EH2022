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
using System.Windows.Shapes;
using MonCine.Data;


namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Projections.xaml
    /// </summary>
    public partial class FProjections : Page
    {
        private DALProjection Dal { get; set; }
        private DALFilm DalFilm { get; set; }
        private DALSalle DalSalle { get; set; }

        private List<Film> Films = new List<Film>();
        private List<Salle> Salles = new List<Salle>();


        public FProjections(DALFilm pDalFilm, DALSalle pDalSalle, DALProjection pDal)
        {
            InitializeComponent();
            Dal = pDal;
            DalFilm = pDalFilm;
            DalSalle = pDalSalle;
            InitialConfiguration();

        }

        /// <summary>
        /// Définit l'état inital du form
        /// </summary>
        private void InitialConfiguration()
        {
            Films = DalFilm.ReadItems();
            Salles = DalSalle.ReadItems();
            PopulateCategory();
        }

        private void PopulateCategory()
        {

            foreach (Film film in Films)
            {
                FilmCombobox.Items.Add(film);
            }
            foreach (Salle salle in Salles)
            {
                SalleCombobox.Items.Add(salle);
            }
        }

        private Projection CreateProjectionToAdd()
        {
            Salle salle = (Salle)SalleCombobox.SelectedItem;
            Film film = (Film)FilmCombobox.SelectedItem;


            Projection projection = new Projection(salle,film);

            return projection;
        }

        private void FilmCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

                Projection projection = CreateProjectionToAdd();


                var result =  Dal.AddItem(projection);
                if (result)
                {
                    MessageBox.Show($"La projection a été crée avec succès !", "Création de projection", MessageBoxButton.OK, MessageBoxImage.Information);

                }

            
        }


        private void SalleCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
