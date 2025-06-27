using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Database.Models
{
    public class Federation : BaseEntity
    {
        public required string Id { get; init; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public bool Enabled { get; set; }

        public ICollection<FederationContact> Contacts { get; init; } = [];
        public ICollection<Member> Members { get; init; } = [];
    }
}