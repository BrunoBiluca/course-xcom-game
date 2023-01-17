using System;
using System.Collections.Generic;

namespace GameAssets.Tests
{
    public abstract class BilucaBuilder<T>
    {
        private bool WasBuilt = false;

        private readonly Dictionary<Type, object> innerObjects = new();

        public T Build()
        {
            WasBuilt = true;
            return OnBuild();
        }

        public TObj Get<TObj>()
        {
            if(!WasBuilt) throw new InvalidOperationException("Object was not built");
            return (TObj)innerObjects[typeof(TObj)];
        }

        protected void AddToObjects<TObj>(TObj newObject)
        {
            innerObjects.Add(typeof(TObj), newObject);
        }

        protected abstract T OnBuild();
    }
}