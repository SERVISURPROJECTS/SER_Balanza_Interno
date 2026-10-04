using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("campania")]
    public class campania
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("sigla")]
        public string sigla { get; set; } = string.Empty;
        [Column("activo")]
        public bool activo { get; set; }
        [Column("valido_desde")]
        public DateTime valido_desde { get; set; }
        [Column("valido_hasta")]
        public DateTime valido_hasta { get; set; }
        [Column("fecha_creacion")]
        public DateTime fecha_creacion { get; set; }
        [Column("id_usuario")]
        public int id_usuario { get; set; }
    }
}
