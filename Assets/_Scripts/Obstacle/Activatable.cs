using UnityEngine;

namespace _Scripts.Obstacle
{
    public abstract class Activatable : MonoBehaviour
    {
        private bool _isActivate = false;

        public void SetActiveActivatable(bool willActive = false)
        {
            if (!_isActivate && willActive)
            {
                _isActivate = true;
                Active();
            }
            else if (_isActivate&& !willActive )
            {
                _isActivate = false;
                Inactive();
            }
        }
        protected abstract void Active();
        protected abstract void Inactive();

        public bool IsActive()
        {
            return _isActivate;
        }
    }
}
