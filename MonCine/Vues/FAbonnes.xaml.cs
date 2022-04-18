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
    public partial class FAbonnes : Page
    {
        private List<Abonne> abonnes;
        private DALAbonne Dal { get; set; }


        public FAbonnes(DALAbonne dal)
        {
            InitializeComponent();
            abonnes = dal.ReadItems();
            Dal = dal;
            LstAbonnes.ItemsSource = abonnes;

        }



        private void BtnReturn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new Accueil());

        }


        private void LstAbonnes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Abonne abonne = (Abonne)LstAbonnes.SelectedItem;
            //NameField.Text = abonne?.Username;

            //BtnDelete.IsEnabled = abonne != null;

            Abonne unAbonne = LstAbonnes.SelectedItem as Abonne;

            FAbonne fabonne;
            fabonne = new FAbonne(Dal, unAbonne);
            this.NavigationService.Navigate(fabonne);
        }


        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (LstAbonnes.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez choisir un Abonne pour le modifier", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {

                //Abonne abonne = (Abonne)LstAbonnes.SelectedItem;
                //UpdateAbonne(abonne);
                //var result = await Dal.UpdateAbonne(abonne);

                //if (result)
                //{
                //    NameField.Text = "";
                //    UpdateItems();
                //    MessageBox.Show($"Le film {abonne.Username} a été mis à jour avec succès !", "Modification", MessageBoxButton.OK, MessageBoxImage.None);
                //}

            }
        }

        private void UpdateAbonne(Abonne pAbonne)
        {
            //pAbonne.Username = NameField.Text;
        }

        /// <summary>
        /// Permet de mettre à jour les données des éléments affichés
        /// </summary>
        private void UpdateItems()
        {
           // LstAbonnes.ItemsSource = Dal.ReadAbonnes();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        public override string ToString()
        {
            return $"{Name}";
        }

        private void NameField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}