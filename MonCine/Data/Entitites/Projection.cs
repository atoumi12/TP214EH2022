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


        public Projection(Salle salle, Film film, DateTime pDate)
        {
            Salle = salle;
            Film = film;
            DateDebut = pDate;
        }



        public Projection selectionnerProjection()
        {
            return this;
        }

        public bool encorePlaceDisponible()
        {
            throw new NotImplementedException();
        }

        public (DateTime , DateTime) consulterHoraire()
        {
            return (DateDebut, DateFin);
        }

        public override string ToString()
        {
            return $"Salle : {Salle} - {DateDebut.ToShortDateString()}";
        }
    }
}
