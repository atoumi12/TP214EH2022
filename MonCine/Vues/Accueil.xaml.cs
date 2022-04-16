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
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Page
    {
        public Accueil()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO: A changer
            FAbonnes frmAbonnes = new FAbonnes(new DALFilm());

            NavigationService?.Navigate(frmAbonnes);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FProjections frmProjections = new FProjections();

            NavigationService?.Navigate(frmProjections);
        }

        private void BtnFilm_Click(object sender, RoutedEventArgs e)
        {
            FFilms frmFilms = new FFilms(new DALFilm());
            NavigationService?.Navigate(frmFilms);
        }

    }
}
