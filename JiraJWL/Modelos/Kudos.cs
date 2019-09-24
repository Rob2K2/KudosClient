using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JiraJWL.Modelos
{
    public class Kudos
    {
        public string KudosID { get; set; } 

        public string Fuente { get; set; }

        public string Destino { get; set; }

        [Required]
        public string Tema { get; set; }

        public string Fecha { get; set; }

        [Required]
        public string Lugar { get; set; }

        [Required]
        public string Texto { get; set; }
    }
}