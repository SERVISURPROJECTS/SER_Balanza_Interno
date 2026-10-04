using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("Permiso")]
    public class Permiso
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("es_grupo")]
        public bool? es_grupo { get; set; }
        [Column("grupo")]
        public int? grupo { get; set; }
    }
}
