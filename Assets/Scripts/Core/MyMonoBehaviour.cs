using System;
using UnityEngine;

namespace Core
{
    public class MyMonoBehaviour : MonoBehaviour
    {
        private GameObject _gameObject;
        private Transform _transform;

        public GameObject GameObject
        {
            get
            {
                if (_gameObject != null)
                {
                    return _gameObject;
                }

                if (base.gameObject != null)
                {
                    _gameObject = base.gameObject;
                }

                return _gameObject;
            }
        }

        public Transform Transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = base.transform;
                }

                return _transform;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                _transform.position = value.position;
                _transform.rotation = value.rotation;
                _transform.localScale = value.localScale;
            }
        }

        [Obsolete] public new Transform transform => throw new InvalidOperationException();
        [Obsolete] public new GameObject gameObject => throw new InvalidOperationException();
    }
}
