using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("indicador")]
    public class indicador
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("comando")]
        public string comando { get; set; } = string.Empty;
        [Column("cant_caracter")]
        public int? cant_caracter { get; set; }
        [Column("caracter_ini")]
        public string caracter_ini { get; set; } = string.Empty;
        [Column("longitud_dato")]
        public int? longitud_dato { get; set; }
        [Column("posicion")]
        public int? posicion { get; set; }
    }
}
