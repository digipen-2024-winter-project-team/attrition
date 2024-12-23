using System;

namespace Attrition.Common.Registration
{
    public interface IRegistry<TEntity>
    {
        void Register(TEntity entity);
        void Deregister(TEntity entity);

        event Action<TEntity> Registered;
        event Action<TEntity> Deregistered;
    }
}
