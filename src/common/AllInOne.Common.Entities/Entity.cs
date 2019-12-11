using System;

namespace AllInOne.Common.Entities
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        protected Entity() { }

        public virtual TPrimaryKey Id { get; set; }
    }

    public abstract class Entity : Entity<Guid>, IEntity
    {
        protected Entity() { }
    }
}
