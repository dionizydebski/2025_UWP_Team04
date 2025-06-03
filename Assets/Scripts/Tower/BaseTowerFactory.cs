using UnityEngine;

namespace Tower
{
    public abstract class BaseTowerFactory : MonoBehaviour
    {
       public abstract BaseTower CreateTower(Transform transform);
    }
}