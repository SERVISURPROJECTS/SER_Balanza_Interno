using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("activo")]
        public bool activo { get; set; }
        [Column("HabilitarParametro")]
        public bool HabilitarParametro { get; set; }
        [Column("Humedad")]
        public double? Humedad { get; set; }
        [Column("FDHumedad")]
        public double? FDHumedad { get; set; }
        [Column("Impureza")]
        public double? Impureza { get; set; }
        [Column("FDImpureza")]
        public double? FDImpureza { get; set; }
        [Column("Partido")]
        public double? Partido { get; set; }
        [Column("FDPartido")]
        public double? FDPartido { get; set; }
        [Column("Danado")]
        public double? Danado { get; set; }
        [Column("FDDanado")]
        public double? FDDanado { get; set; }
        [Column("OtroColor")]
        public double? OtroColor { get; set; }
        [Column("FDOtroColor")]
        public double? FDOtroColor { get; set; }
        [Column("DanadoPorCalor")]
        public double? DanadoPorCalor { get; set; }
        [Column("FDDanadoPorCalor")]
        public double? FDDanadoPorCalor { get; set; }
        [Column("Enfermo")]
        public double? Enfermo { get; set; }
        [Column("FDEnfermo")]
        public double? FDEnfermo { get; set; }
        [Column("Verde")]
        public double? Verde { get; set; }
        [Column("FDVerde")]
        public double? FDVerde { get; set; }
        [Column("UnidadOpcional")]
        public string UnidadOpcional { get; set; } = string.Empty;
        [Column("FactorUO")]
        public double? FactorUO { get; set; }
        [Column("HabilitadoUO")]
        public bool? HabilitadoUO { get; set; }
        [Column("id_producto_padre")]
        public int? id_producto_padre { get; set; }
        [Column("factor_correccion")]
        public double? factor_correccion { get; set; }
    }
}
