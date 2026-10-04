using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("prod_fac_rank")]
    [PrimaryKey(nameof(id), nameof(idProducto), nameof(idfactor))]
    public class prod_fac_rank
    {
        [Column("id", Order = 0)]
        public int id { get; set; }
        [Column("idProducto", Order = 1)]
        public int idProducto { get; set; }
        [Column("idfactor", Order = 2)]
        public int idfactor { get; set; }
        [Column("rank_from")]
        public double? rank_from { get; set; }
        [Column("rank_to")]
        public double? rank_to { get; set; }
        [Column("discount")]
        public double? discount { get; set; }
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
