using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("documento")]
    public class documento
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("activo")]
        public bool activo { get; set; }
        [Column("fecha_creacion")]
        public DateTime fecha_creacion { get; set; }
        [Column("id_usuario")]
        public int id_usuario { get; set; }
        [Column("modo_transaccion")]
        public string modo_transaccion { get; set; } = string.Empty;
        [Column("Id_balanza")]
        public int? Id_balanza { get; set; }
        [Column("IdProducto")]
        public int? IdProducto { get; set; }
    }
}
