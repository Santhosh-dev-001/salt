using UnityEngine;
using SaltAnalysis.Core;

namespace SaltAnalysis.Interaction
{
    public class SaltDraggable : MonoBehaviour
    {
        [SerializeField] SaltType _saltType;

        public SaltType SaltType => _saltType;

        Vector3 _origin;

        public void OnPickUp()
        {
            _origin = transform.position;
        }

        public void OnDrag(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }

        public void OnSnapBack()
        {
            transform.position = _origin;
        }
    }
}
