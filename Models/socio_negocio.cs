using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("socio_negocio")]
    public class socio_negocio
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("direccion")]
        public string direccion { get; set; } = string.Empty;
        [Column("telefono")]
        public string telefono { get; set; } = string.Empty;
        [Column("contacto")]
        public string contacto { get; set; } = string.Empty;
        [Column("es_cliente")]
        public bool es_cliente { get; set; }
        [Column("es_proveedor")]
        public bool es_proveedor { get; set; }
        [Column("activo")]
        public bool activo { get; set; }
    }
}
