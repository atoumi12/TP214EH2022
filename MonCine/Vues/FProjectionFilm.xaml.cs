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
    /// Interaction logic for FProjectionFilm.xaml
    /// </summary>
    public partial class FProjectionFilm : Window
    {
        private DALProjection dalProjection { get; set; }
        private List<Projection> projections { get; set; }
        private Film FilmChoisi { get; set; } 


        public FProjectionFilm(DALProjection pDalProjection, Film pFilm)
        {
            InitializeComponent();

            dalProjection = pDalProjection;
            FilmChoisi = pFilm;

            InitialConfiguration();
        }

        private void InitialConfiguration()
        {
            txtFilmName.Text = $"{FilmChoisi.Name}";

            projections = dalProjection.GetProjectionsOfFilm(FilmChoisi);

            ProjectionsListView.ItemsSource = projections;


        }
    }
}