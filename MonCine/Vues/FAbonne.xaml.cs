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
using System.Windows.Shapes;
using MonCine.Data;

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour FAbonnes.xaml
    /// </summary>
    public partial class FAbonne : Page
    {

        public FAbonne(Abonne pAbonne)
        {
            InitializeComponent();
            InitialConfiguration(pAbonne);
        }



        /// <summary>
        /// Définit l'état inital du form
        /// </summary>
        private void InitialConfiguration(Abonne pAbonne)
        {
            UserInfos.Text = $"Nom et Prénom : {pAbonne.FirstName} {pAbonne.LastName}" +
                $"\n" +
                $"\n" +
                $"Acteur favorie : {pAbonne.ActeurFavorie}" +
                $"\n" +
                $"\n" +
                $"Realisateur favorie : {pAbonne.RealisateurFavorie}" +
                $"\n" +
                $"\n" +
                $"Seance assister : {pAbonne.nbSeanceAssistees}";

            DisableButtons();

        }

        private void DisableButtons()
        {
            BtnDelete.IsEnabled = false;
            BtnUpdate.IsEnabled = false;
            BtnOffrirReconpense.IsEnabled = false;
        }


        private void BtnReturn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new FAbonnes(new DALAbonne()));
        }

    }
}