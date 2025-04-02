using UnityEngine;

namespace UI.BaseClasses
{
    public abstract class BaseView<T> : MonoBehaviour where T : BaseModel<T>
    {
        public abstract void UpdateView(T model);
    }
}