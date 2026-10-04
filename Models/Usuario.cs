using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Column("usuario")]
        public string usuario { get; set; } = string.Empty;
        [Column("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;
        [Column("Habilitado")]
        public bool Habilitado { get; set; }
        [Column("Eliminado")]
        public bool Eliminado { get; set; }
        [Column("id_rol")]
        public int? id_rol { get; set; }
        [Column("id_balanza")]
        public int? id_balanza { get; set; }
        [Column("ci")]
        public string? Ci { get; set; }
    }
}
