﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Driver;

namespace MonCine.Data
{
    public class DALProjection : DAL, ICRUD<Projection>
    {
        public string CollectionName { get; set; }

        public DALProjection(IMongoClient client = null) : base(client)
        {
            CollectionName = "Projection";
        }


        /// <summary>
        /// Récupère l'ensemble des projections de la BD
        /// </summary>
        /// <returns>Liste de Projection</returns>
        public List<Projection> ReadItems()
        {
            throw new NotImplementedException();
        }


        public bool AddItem(Projection pProjection)
        {
            if (pProjection is null)
            {
                throw new ArgumentNullException("pProjection", "La projection ne peut pas être null");
            }

            try
            {
                var collection = database.GetCollection<Projection>(CollectionName);
                collection.InsertOneAsync(pProjection);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d'ajouter la projection {pProjection.Id} dans la collection {ex.Message}",
                    "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }

            return true;
        }

        public List<Projection> GetProjectionsOfFilm(Film pFilm)
        {
            if (pFilm is null)
            {
                throw new ArgumentNullException("pFilm", "Le film ne peut pas être null");
            }

            List<Projection> projections = new List<Projection>();
            try
            {
                var collection = database.GetCollection<Projection>(CollectionName);
                projections = collection.Find(Builders<Projection>.Filter.Eq(x => x.Film.Id, pFilm.Id)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d'ajouter la projection {pFilm.Id} dans la collection {ex.Message}",
                    "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }

            return projections;
        }

        public List<Projection> GetProjectionsByDate(DateTime pDate)
        {
            if (pDate == DateTime.MinValue)
            {
                throw new ArgumentNullException("pDate", "La date de recherche ne peut pas être null");
            }

            List<Projection> projections = new List<Projection>();
            try
            {
                var collection = database.GetCollection<Projection>(CollectionName);

                var filter = Builders<Projection>.Filter.Where(p => p.DateDebut == pDate);

                projections = collection.Find(filter).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d'effectuer la recherche selon la date suivante {pDate.ToShortDateString()} | Erreur: {ex.Message}",
                    "Erreur de recherche", MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }

            return projections;
        }




        public bool UpdateItem(Projection pProjection)
        {
            if (pProjection is null)
            {
                throw new ArgumentNullException("pProjection ", "La projection ne peut pas être null");
            }

            try
            {
                var collection = database.GetCollection<Projection>(CollectionName);
                collection.ReplaceOne(x=>x.Id == pProjection.Id , pProjection);

                // Mettre à jour le film associé à la projection
                //Film film = pProjection.Film;
                //film.DatesProjection
                //DALFilm dalFilm = new DALFilm();
                //dalFilm.UpdateItem(film);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible de mettre à jour la projection de la collection {ex.Message}",
                    "Erreur de mise à jour", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            return true;
        }

        public bool DeleteItem(Projection pProjection)
        {
            if (pProjection is null)
            {
                throw new ArgumentNullException("pProjection ", "La projection ne peut pas être null");
            }

            try
            {
                var collection = database.GetCollection<Projection>(CollectionName);
                collection.DeleteOne(Builders<Projection>.Filter.Eq(x => x.Id, pProjection.Id));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible de supprimer la projection de la collection {ex.Message}",
                    "Erreur de mise à jour", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            return true;
        }
    }
}