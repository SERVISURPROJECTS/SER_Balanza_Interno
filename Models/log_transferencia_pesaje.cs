using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("log_transferencia_pesaje")]
    public class log_transferencia_pesaje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdLog")]
        public int IdLog { get; set; }
        [Column("FechaHora")]
        public DateTime FechaHora { get; set; }
        [Column("PesoInKey")]
        public int PesoInKey { get; set; }
        [Column("Placa")]
        public string Placa { get; set; } = string.Empty;
        [Column("Producto")]
        public string Producto { get; set; } = string.Empty;
        [Column("Peso")]
        public decimal? Peso { get; set; }
        [Column("FechaIngreso")]
        public DateTime? FechaIngreso { get; set; }
        [Column("Cliente")]
        public string Cliente { get; set; } = string.Empty;
        [Column("Proveedor")]
        public string Proveedor { get; set; } = string.Empty;
        [Column("Chofer")]
        public string Chofer { get; set; } = string.Empty;
        [Column("Remito")]
        public string Remito { get; set; } = string.Empty;
        [Column("DocumentoOrigen")]
        public string DocumentoOrigen { get; set; } = string.Empty;
        [Column("HaciendaOrigen")]
        public string HaciendaOrigen { get; set; } = string.Empty;
        [Column("BalanzaOrigen")]
        public string BalanzaOrigen { get; set; } = string.Empty;
        [Column("BalanzaDestino")]
        public string BalanzaDestino { get; set; } = string.Empty;
        [Column("DocumentoDestino")]
        public string DocumentoDestino { get; set; } = string.Empty;
        [Column("HaciendaDestino")]
        public string HaciendaDestino { get; set; } = string.Empty;
        [Column("Usuario")]
        public string Usuario { get; set; } = string.Empty;
        [Column("Equipo")]
        public string Equipo { get; set; } = string.Empty;
        [Column("Resultado")]
        public string Resultado { get; set; } = string.Empty;
    }
}
