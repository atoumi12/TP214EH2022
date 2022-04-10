using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace MonCine.Data
{
    public class Abonne
    {
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public DateTime DateAdhesion { get; set; }
        
        // A compléter pour mardi

         
    }
}
