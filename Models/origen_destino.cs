using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("origen_destino")]
    public class origen_destino
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("es_destino")]
        public bool es_destino { get; set; }
        [Column("activo")]
        public bool activo { get; set; }
    }
}
