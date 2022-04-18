using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MonCine.Data
{
    public class Projection
    {
        public ObjectId Id { get; set; }
        public Salle Salle { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public Film Film { get; set; }
        

        public Projection(Salle salle, Film film)
        {
            Salle = salle;
            Film = film;
            DateDebut = DateTime.Now;
        }



        public Projection selectionnerProjection()
        {
            return null;
        }

        public bool encorePlaceDisponible()
        {
            return false;
        }

        public DateTime consulterHoraire()
        {
            return DateTime.Now;
        }

        public override string ToString()
        {
            return $"Salle : {Salle} - {DateDebut.ToShortTimeString()}";
        }
    }
}
