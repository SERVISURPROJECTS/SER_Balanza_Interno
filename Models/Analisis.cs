using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("Analisis")]
    public class Analisis
    {
        [Key]
        [Column("DocEntry")]
        public int DocEntry { get; set; }
        [Column("DocNum")]
        public int? DocNum { get; set; }
        [Column("PesoId")]
        public int? PesoId { get; set; }
        [Column("ProductoId")]
        public int? ProductoId { get; set; }
        [Column("ClienteId")]
        public int? ClienteId { get; set; }
        [Column("Cancelado")]
        public string Cancelado { get; set; } = string.Empty;
        [Column("Estado")]
        public string Estado { get; set; } = string.Empty;
        [Column("UsuarioIdReg")]
        public int UsuarioIdReg { get; set; }
        [Column("FechaDoc")]
        public DateTime FechaDoc { get; set; }
        [Column("FechaReg")]
        public DateTime FechaReg { get; set; }
        [Column("FechaAct")]
        public DateTime? FechaAct { get; set; }
        [Column("UsuarioIdAct")]
        public int? UsuarioIdAct { get; set; }
        [Column("PHumedad")]
        public double? PHumedad { get; set; }
        [Column("PImpureza")]
        public double? PImpureza { get; set; }
        [Column("PPartido")]
        public double? PPartido { get; set; }
        [Column("PDanado")]
        public double? PDanado { get; set; }
        [Column("POtroColor")]
        public double? POtroColor { get; set; }
        [Column("PDCalor")]
        public double? PDCalor { get; set; }
        [Column("PEnfermo")]
        public double? PEnfermo { get; set; }
        [Column("PVerde")]
        public double? PVerde { get; set; }
        [Column("Humedad")]
        public double? Humedad { get; set; }
        [Column("Impureza")]
        public double? Impureza { get; set; }
        [Column("Partido")]
        public double? Partido { get; set; }
        [Column("Danado")]
        public double? Danado { get; set; }
        [Column("OtroColor")]
        public double? OtroColor { get; set; }
        [Column("DCalor")]
        public double? DCalor { get; set; }
        [Column("Enfermo")]
        public double? Enfermo { get; set; }
        [Column("Verde")]
        public double? Verde { get; set; }
        [Column("Desc_Humedad")]
        public double? Desc_Humedad { get; set; }
        [Column("Desc_Impureza")]
        public double? Desc_Impureza { get; set; }
        [Column("Desc_Partido")]
        public double? Desc_Partido { get; set; }
        [Column("Desc_Danado")]
        public double? Desc_Danado { get; set; }
        [Column("Desc_OtroColor")]
        public double? Desc_OtroColor { get; set; }
        [Column("Desc_DCalor")]
        public double? Desc_DCalor { get; set; }
        [Column("Desc_Enfermo")]
        public double? Desc_Enfermo { get; set; }
        [Column("Desc_Verde")]
        public double? Desc_Verde { get; set; }
        [Column("DescWeightHumedad")]
        public double? DescWeightHumedad { get; set; }
        [Column("DescWeightImpureza")]
        public double? DescWeightImpureza { get; set; }
        [Column("DescWeightPartido")]
        public double? DescWeightPartido { get; set; }
        [Column("DescWeightDanado")]
        public double? DescWeightDanado { get; set; }
        [Column("DescWeightOtroColor")]
        public double? DescWeightOtroColor { get; set; }
        [Column("DescWeightDCalor")]
        public double? DescWeightDCalor { get; set; }
        [Column("DescWeightEnfermo")]
        public double? DescWeightEnfermo { get; set; }
        [Column("DescWeightVerde")]
        public double? DescWeightVerde { get; set; }
        [Column("TotalDescPorcent")]
        public double? TotalDescPorcent { get; set; }
        [Column("TotalDescPeso")]
        public double? TotalDescPeso { get; set; }
        [Column("Unidad")]
        public string Unidad { get; set; } = string.Empty;
        [Column("PesoHectolitrico")]
        public double? PesoHectolitrico { get; set; }
    }
}
