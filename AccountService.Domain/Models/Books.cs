using Foudation.CQRS.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccountService.Domain.Models
{
    public class Books:Entity<Guid>
    {
        public string BookName { get; set; }

        public string Published { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }

        public string Auther { get; set; }
    }
}
