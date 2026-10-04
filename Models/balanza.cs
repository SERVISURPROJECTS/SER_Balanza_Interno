using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("balanza")]
    public class balanza
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("tipo_conexion")]
        public int tipo_conexion { get; set; }
        [Column("ip")]
        public string ip { get; set; } = string.Empty;
        [Column("puerto")]
        public int? puerto { get; set; }
        [Column("puerto_serial")]
        public string puerto_serial { get; set; } = string.Empty;
        [Column("baudRate")]
        public int? baudRate { get; set; }
        [Column("dataBits")]
        public int? dataBits { get; set; }
        [Column("parity")]
        public int? parity { get; set; }
        [Column("stopBits")]
        public int? stopBits { get; set; }
        [Column("nro_balanza")]
        public int? nro_balanza { get; set; }
        [Column("activo")]
        public bool? activo { get; set; }
        [Column("id_indicador")]
        public int id_indicador { get; set; }
        [Column("semaforo")]
        public bool? semaforo { get; set; }
        [Column("rfid")]
        public bool? rfid { get; set; }
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        [Column("Direccion")]
        public string Direccion { get; set; } = string.Empty;
        [Column("Telefono")]
        public string Telefono { get; set; } = string.Empty;
        [Column("email")]
        public string email { get; set; } = string.Empty;
    }
}
