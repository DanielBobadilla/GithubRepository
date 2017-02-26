using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GenoomTree.Models
{
    public class People
    {
        [Key, Column(Order = 1)]
        public int PersonId { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Parent1Id { get; set; }
        public int? Parent2Id { get; set; }
    }
}
