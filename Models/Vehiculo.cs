using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("Vehiculo")]
    public class Vehiculo
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Placa")]
        public string Placa { get; set; } = string.Empty;
        [Column("Marca")]
        public string Marca { get; set; } = string.Empty;
        [Column("Modelo")]
        public string Modelo { get; set; } = string.Empty;
        [Column("Tipo")]
        public string Tipo { get; set; } = string.Empty;
        [Column("color")]
        public string color { get; set; } = string.Empty;
        [Column("Propietario")]
        public string Propietario { get; set; } = string.Empty;
        [Column("Activo")]
        public bool Activo { get; set; }
        [Column("Tara")]
        public double? Tara { get; set; }
        [Column("TipoTara")]
        public int? TipoTara { get; set; }
        [Column("TaraAdquirida")]
        public DateTime? TaraAdquirida { get; set; }
        [Column("taraExpira")]
        public DateTime? taraExpira { get; set; }
    }
}
