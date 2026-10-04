using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("rol")]
    public class rol
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Column("activo")]
        public bool activo { get; set; }
        [Column("eliminado")]
        public bool eliminado { get; set; }
    }
}
