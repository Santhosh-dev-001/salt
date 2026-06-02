using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace SaltAnalysis.Interaction
{
    public class ClickInputController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] Camera    _raycastCamera;

        [Header("Layers")]
        [SerializeField] LayerMask _clickableLayer;

       

        [Header("References")]
        [SerializeField] InputActionReference _pressAction;

        void OnEnable()
        {
            _pressAction.action.performed += OnPressed;
            _pressAction.action.Enable();
        }

        void OnDisable()
        {
            _pressAction.action.performed -= OnPressed;
            _pressAction.action.Disable();
        }

        void OnPressed(InputAction.CallbackContext ctx)
        {
           
            if (_raycastCamera == null) return;
           // Debug.Log("Pressed1");
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
           // Debug.Log("Pressed2");

            Ray ray = _raycastCamera.ScreenPointToRay(PointerPosition());
           // Debug.Log("Pressed3");

           // if (Physics.Raycast(ray, out RaycastHit _hit, 200f))
               // Debug.Log("Hit (no mask): " + _hit.collider.gameObject.name);
          //  else
              //  Debug.Log("No hit (no mask)");
            // check if a clickable was hit
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, _clickableLayer))
            {
                //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 5f);
                
                Clickable clickable = hit.collider.GetComponent<Clickable>();
                if (clickable == null || !clickable.IsInteractable) return;

                if (clickable._zone == null)
                {
                   // Debug.Log("[ClickInputController] No matching zone found.");
                    return;
                }

                clickable.OnClicked(clickable._zone);
               // Debug.Log("Pressed4");
            }


        }

        Vector2 PointerPosition()
        {
            return _pressAction.action.ReadValue<Vector2>();
        }
    }
}
