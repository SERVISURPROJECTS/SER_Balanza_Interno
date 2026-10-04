using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("compania")]
    public class compania
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("razon_social")]
        public string razon_social { get; set; } = string.Empty;
        [Column("nit")]
        public string nit { get; set; } = string.Empty;
        [Column("direccion1")]
        public string direccion1 { get; set; } = string.Empty;
        [Column("direccion2")]
        public string direccion2 { get; set; } = string.Empty;
        [Column("telefono")]
        public string telefono { get; set; } = string.Empty;
        [Column("logo")]
        public byte[] logo { get; set; } = Array.Empty<byte>();
        [Column("email")]
        public string email { get; set; } = string.Empty;
        [Column("activo")]
        public bool activo { get; set; }
    }
}
