using UnityEngine;

namespace _Scripts.Obstacle
{
    public abstract class Activatable : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public abstract void Active();
        public abstract void Inactive();
        
    }
}
