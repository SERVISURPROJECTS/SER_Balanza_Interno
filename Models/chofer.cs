using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("chofer")]
    public class chofer
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("ci")]
        public string ci { get; set; } = string.Empty;
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("direccion")]
        public string direccion { get; set; } = string.Empty;
        [Column("telefono")]
        public string telefono { get; set; } = string.Empty;
        [Column("activo")]
        public bool activo { get; set; }
    }
}
