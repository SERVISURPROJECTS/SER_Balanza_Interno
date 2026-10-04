using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("Hacienda")]
    public class Hacienda
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        [Column("Direccion")]
        public string Direccion { get; set; } = string.Empty;
        [Column("Abreviatura")]
        public string Abreviatura { get; set; } = string.Empty;
        [Column("FechaCreacion")]
        public DateTime? FechaCreacion { get; set; }
        [Column("Activo")]
        public bool? Activo { get; set; }
    }
}
