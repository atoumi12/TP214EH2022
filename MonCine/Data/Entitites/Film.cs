using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MonCine.Data
{
    public class Film
    {
        [BsonId] public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<Categorie> Categories { get; set; }
        public DateTime DateSortie { get; set; }
        public DateTime DateProjection { get; set; }
        public DateTime DerniereProjection { get; set; }
        public bool SurAffiche { get; set; }
        public List<Acteur> Acteurs { get; set; }
        public List<Realisateur> Realisateurs { get; set; }
        private List<int> Notes { get; set; }
        public double NoteMoyenne { get; set; }


        [Range(0, 2, ErrorMessage = "Le nombre de projection d'un film ne peut pas dépasser 2 projections pas années")]
        private int NbProjection { get; set; }


        public Film(string pName, List<Categorie> pCategories = null, List<Acteur> pActeurs = null,
            List<Realisateur> pRealisateurs = null, bool pSurAffiche = false)
        {
            Name = pName;
            Categories = pCategories ?? GenerateCategories();
            Acteurs = pActeurs ?? new List<Acteur>();
            Realisateurs = pRealisateurs ?? new List<Realisateur>();
            SurAffiche = pSurAffiche;

            Notes = GenerateNotes();
            NoteMoyenne = CalculerMoyennesNotes();
        }


        #region DEV

        private List<int> GenerateNotes()
        {
            Random random = new Random();
            List<int> notes = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                notes.Add(random.Next(1, 11));
            }

            return notes;
        }

        private List<Categorie> GenerateCategories()
        {
            Random random = new Random();

            List<String> enumNames = typeof(Categorie).GetEnumNames().ToList();
            List<Categorie> categories = new List<Categorie>();

            for (int i = 0; i < 5; i++)
            {
                int indiceCat = random.Next(enumNames.Count);
                Categorie cat = (Categorie)indiceCat;

                if (!categories.Contains(cat))
                {
                    categories.Add(cat);
                }
            }

            return categories;
        }

        #endregion

        private double CalculerMoyennesNotes()
        {
            Notes ??= new List<int>();
            int taille = Notes.Count > 0 ? Notes.Count : 1;

            return Notes.Sum(x => x) / taille;
        }

        public void Noter(int note)
        {
            if (note < 0 || note > 10)
            {
                throw new ArgumentOutOfRangeException("note", "La notre attribué doit être comprise entre 1 et 10");
            }

            Notes.Add(note);
        }

        public Film SelectionnerFilm()
        {
            return this;
        }

        public bool EstAdmissibleAReprojecter()
        {
            return NbProjection <= 1;
        }

        public override string ToString()
        {
            return $"{Name} - ({NoteMoyenne}/10)";
        }
    }
}