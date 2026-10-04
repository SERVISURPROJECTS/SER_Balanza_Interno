using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SER_Balanza_Interno.Models
{
    [Table("permiso_rol")]
    [PrimaryKey(nameof(id_permiso), nameof(id_rol))]
    public class permiso_rol
    {
        [Column("id_permiso", Order = 0)]
        public int id_permiso { get; set; }
        [Column("id_rol", Order = 1)]
        public int id_rol { get; set; }
    }
}
