using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Data
{
    public class EmergencyContact
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        // Assuming fk_users is a foreign key to a Users table
        [Column("fk_users")]
        public Guid fk_user { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("relationship")]
        public string? Relationship { get; set; }
    }
}
