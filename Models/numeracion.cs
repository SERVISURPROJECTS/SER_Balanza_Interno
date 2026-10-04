using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("numeracion")]
    public class numeracion
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("numeracion_inicial")]
        public int numeracion_inicial { get; set; }
        [Column("prefijo")]
        public string prefijo { get; set; } = string.Empty;
        [Column("valor")]
        public int valor { get; set; }
        [Column("id_documento")]
        public int? id_documento { get; set; }
        [Column("activo")]
        public bool? activo { get; set; }
    }
}
