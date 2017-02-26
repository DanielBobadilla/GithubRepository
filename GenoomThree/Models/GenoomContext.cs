using GenoomTree.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GenoomThree.Models
{
    public class GenoomContext : DbContext
    {
        public GenoomContext(DbContextOptions<GenoomContext> options)
            : base(options)
        { }
        public DbSet<People> People { get; set; }

    }
}
