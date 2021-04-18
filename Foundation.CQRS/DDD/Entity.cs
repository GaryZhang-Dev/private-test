using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Foudation.CQRS.DDD
{
    public interface IEntity { 
        DateTime LastModifiedOn { get; set; }

        DateTime CreatedOn { get; set; }
    }
    public abstract class Entity<TKey> : IEquatable<Entity<TKey>>,IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }

        public virtual bool Equals(Entity<TKey>obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return obj.Id.Equals(Id);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((Entity<TKey>)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ GetType().GetHashCode();
        }
        public DateTime CreatedOn { get; set; }

        public DateTime LastModifiedOn { get; set; }
    }
}
