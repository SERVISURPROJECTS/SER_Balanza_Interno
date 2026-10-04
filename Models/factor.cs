using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("factor")]
    public class factor
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; } = string.Empty;
        [Column("Aud_Anulado")]
        public byte? Aud_Anulado { get; set; }
        [Column("Aud_FechaReg")]
        public DateTime? Aud_FechaReg { get; set; }
        [Column("Aud_UsuarioReg")]
        public string Aud_UsuarioReg { get; set; } = string.Empty;
        [Column("Aud_IpReg")]
        public string Aud_IpReg { get; set; } = string.Empty;
        [Column("Aud_FechaMod")]
        public DateTime? Aud_FechaMod { get; set; }
        [Column("Aud_UsuarioMod")]
        public string Aud_UsuarioMod { get; set; } = string.Empty;
        [Column("Aud_IpMod")]
        public string Aud_IpMod { get; set; } = string.Empty;
    }
}
