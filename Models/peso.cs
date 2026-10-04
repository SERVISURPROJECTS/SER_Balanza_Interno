using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("peso")]
    public class peso
    {
        [Key]
        [Column("NroConsec")]
        public int NroConsec { get; set; }
        [Column("NroPesaje")]
        public int NroPesaje { get; set; }
        [Column("FechaIngreso")]
        public DateTime FechaIngreso { get; set; }
        [Column("FechaSalida")]
        public DateTime FechaSalida { get; set; }
        [Column("Bruto")]
        public double Bruto { get; set; }
        [Column("Tara")]
        public double Tara { get; set; }
        [Column("Neto")]
        public double Neto { get; set; }
        [Column("UnidadPrimaria")]
        public string UnidadPrimaria { get; set; } = string.Empty;
        [Column("unidadSecundaria")]
        public string unidadSecundaria { get; set; } = string.Empty;
        [Column("PesoUnidSec")]
        public double? PesoUnidSec { get; set; }
        [Column("Id_usuarioIng")]
        public int Id_usuarioIng { get; set; }
        [Column("id_Proveedor")]
        public int? id_Proveedor { get; set; }
        [Column("id_Chofer")]
        public int? id_Chofer { get; set; }
        [Column("id_Vehiculo")]
        public int? id_Vehiculo { get; set; }
        [Column("Id_producto")]
        public int? Id_producto { get; set; }
        [Column("Id_UsuarioSal")]
        public int Id_UsuarioSal { get; set; }
        [Column("Observacion")]
        public string Observacion { get; set; } = string.Empty;
        [Column("Nulo")]
        public bool Nulo { get; set; }
        [Column("Id_cliente")]
        public int? Id_cliente { get; set; }
        [Column("Importe")]
        public decimal? Importe { get; set; }
        [Column("peso_manual")]
        public bool peso_manual { get; set; }
        [Column("NroTicket")]
        public string NroTicket { get; set; } = string.Empty;
        [Column("PesoLiquido")]
        public double? PesoLiquido { get; set; }
        [Column("TipoTara")]
        public int? TipoTara { get; set; }
        [Column("PesoTara")]
        public double? PesoTara { get; set; }
        [Column("id_origen")]
        public int? id_origen { get; set; }
        [Column("id_destino")]
        public int? id_destino { get; set; }
        [Column("Credito")]
        public bool? Credito { get; set; }
        [Column("Lote")]
        public string Lote { get; set; } = string.Empty;
        [Column("TotalDesc")]
        public double? TotalDesc { get; set; }
        [Column("ModeService")]
        public bool? ModeService { get; set; }
        [Column("produccion")]
        public bool? produccion { get; set; }
        [Column("id_campania")]
        public int? id_campania { get; set; }
        [Column("id_documento")]
        public int id_documento { get; set; }
        [Column("Id_balanza")]
        public int? Id_balanza { get; set; }
        [Column("PesoInKey")]
        public int? PesoInKey { get; set; }
        [Column("Id_Hacienda")]
        public int? Id_Hacienda { get; set; }
        [Column("cultivo")]
        public string cultivo { get; set; } = string.Empty;
    }
}
