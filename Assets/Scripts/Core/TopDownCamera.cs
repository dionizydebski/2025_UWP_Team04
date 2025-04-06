using UnityEngine;

namespace Core
{
    public class TopDownCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private float cameraDistance;

        private void Awake()
        {
            SetTopDown();
        }

        private void SetTopDown()
        {
            if (!target) return;
            transform.position = new Vector3(target.position.x, cameraDistance, target.position.z);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
