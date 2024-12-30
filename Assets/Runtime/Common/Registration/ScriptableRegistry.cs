using System;
using UnityEngine;

namespace Attrition.Common.Registration
{
    public abstract class ScriptableRegistry<TEntity> : ScriptableObject, IRegistry<TEntity>
    {
        private IRegistry<TEntity> registry;
        
        private IRegistry<TEntity> Registry
        {
            get
            {
                this.registry ??= new Registry<TEntity>();
                
                return this.registry;
            }
        }
        
        public void Register(TEntity entity)
        {
            this.Registry.Register(entity);
        }

        public void Deregister(TEntity entity)
        {
            this.Registry.Deregister(entity);
        }

        public event Action<TEntity> Registered
        {
            add => this.Registry.Registered += value;
            remove => this.Registry.Registered -= value;
        }
        public event Action<TEntity> Deregistered
        {
            add => this.Registry.Deregistered += value;
            remove => this.Registry.Deregistered -= value;
        }
    }
}
