using System;
using System.Collections.Generic;

namespace Attrition.Common.Registration
{
    public class Registry<TEntity> : IRegistry<TEntity>
    {
        private readonly HashSet<TEntity> entities = new();

        public event Action<TEntity> Registered;
        public event Action<TEntity> Deregistered;

        public void Register(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (this.entities.Add(entity))
            {
                this.Registered?.Invoke(entity);
            }
        }

        public void Deregister(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (this.entities.Remove(entity))
            {
                this.Deregistered?.Invoke(entity);
            }
        }
    }
}
