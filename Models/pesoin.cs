using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("pesoin")]
    public class pesoin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PesoInKey")]
        public int PesoInKey { get; set; }
        [Column("NroPesaje")]
        public int NroPesaje { get; set; }
        [Column("id_Vehiculo")]
        public int? id_Vehiculo { get; set; }
        [Column("Id_cliente")]
        public int? Id_cliente { get; set; }
        [Column("Id_producto")]
        public int? Id_producto { get; set; }
        [Column("pesoin")]
        public DateTime FechaPesoIn { get; set; }
        [Column("peso")]
        public double peso { get; set; }
        [Column("unidad_primaria")]
        public string unidad_primaria { get; set; } = string.Empty;
        [Column("unidad_secundaria")]
        public string unidad_secundaria { get; set; } = string.Empty;
        [Column("PesoUnidSec")]
        public double? PesoUnidSec { get; set; }
        [Column("notas")]
        public string notas { get; set; } = string.Empty;
        [Column("id_Chofer")]
        public int? id_Chofer { get; set; }
        [Column("id_Proveedor")]
        public int? id_Proveedor { get; set; }
        [Column("peso_manual")]
        public bool peso_manual { get; set; }
        [Column("id_usuario")]
        public int id_usuario { get; set; }
        [Column("Importe")]
        public decimal? Importe { get; set; }
        [Column("NroTicket")]
        public string NroTicket { get; set; } = string.Empty;
        [Column("id_origen")]
        public int? id_origen { get; set; }
        [Column("id_destino")]
        public int? id_destino { get; set; }
        [Column("Credito")]
        public bool? Credito { get; set; }
        [Column("Lote")]
        public string Lote { get; set; } = string.Empty;
        [Column("ModeService")]
        public bool? ModeService { get; set; }
        [Column("produccion")]
        public bool? produccion { get; set; }
        [Column("id_campania")]
        public int? id_campania { get; set; }
        [Column("id_documento")]
        public int? id_documento { get; set; }
        [Column("Id_balanza")]
        public int? Id_balanza { get; set; }
        [Column("Id_Hacienda")]
        public int? Id_Hacienda { get; set; }
        [Column("cultivo")]
        public string cultivo { get; set; } = string.Empty;
    }
}
