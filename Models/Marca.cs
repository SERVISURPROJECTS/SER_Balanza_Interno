using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("Marca")]
    public class Marca
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_marca")]
        public int id_marca { get; set; }
        [Column("SD")]
        public string SD { get; set; } = string.Empty;
        [Column("S")]
        public string S { get; set; } = string.Empty;
        [Column("F")]
        public string F { get; set; } = string.Empty;
        [Column("E")]
        public string E { get; set; } = string.Empty;
        [Column("FA")]
        public string FA { get; set; } = string.Empty;
    }
}
