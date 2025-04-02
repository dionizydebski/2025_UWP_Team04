using System;

namespace UI.BaseClasses
{
    public abstract class BaseModel<T>
    {
        public event Action<T> ModelChanged;

        protected virtual void OnModelChanged(T model)
        {
            ModelChanged?.Invoke(model);
        }
    }
}