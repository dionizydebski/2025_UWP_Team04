using UnityEngine;
using UnityEngine.Serialization;

namespace UI.BaseClasses
{
    public abstract class BasePresenter<T> : MonoBehaviour where T : BaseModel<T>
    { 
        [SerializeField] protected T model; 
        [SerializeField] protected BaseView<T> view;

        private void Awake()
        {
            model.ModelChanged += OnModelChanged;
            OnModelChanged(model);
        }

        protected virtual void OnModelChanged(T data)
        {
            view.UpdateView(data);
        }
    }
}