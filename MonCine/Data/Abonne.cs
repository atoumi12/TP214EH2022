using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MonCine.Data
{
    public class Abonne : Personne
    {
        //private ObjectId Id { get; set; }
        public string Username { get; set; }
        private DateTime DateAdhesion { get; set; }

        private String ActeurFavorie { get; set; }

        private String RealisateurFavorie { get; set; }

        private String Categorie { get; set; }

        private bool Recomprenses { get; set; }

        private bool Reservation { get; set; }

        private int nbSeanceAssistees { get; set; }



        public Abonne(string pUsername)
        {
            Username = pUsername;
        }


        public void NoterFilm()
        {

        }

        public bool AimeCategorie()
        {
            return false;
        }

        public bool EstPrioriaitaire()
        {
            return false;
        }




        public override string ToString()
        {
            return $"{Username}";
        }
    }


}