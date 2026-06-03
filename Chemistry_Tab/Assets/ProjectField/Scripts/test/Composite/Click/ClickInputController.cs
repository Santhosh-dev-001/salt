using SaltAnalysis.Core;
using SaltAnalysis.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SaltAnalysis.Interaction
{
    public class ClickInputController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] Camera    _raycastCamera;

        [Header("Layers")]
        [SerializeField] LayerMask _clickableLayer;
        [SerializeField] SessionManager _session;

        Clickable currentclick;
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

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;


            Ray ray = _raycastCamera.ScreenPointToRay(PointerPosition());
 
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, _clickableLayer))
            {
        
                
                Clickable clickable = hit.collider.GetComponent<Clickable>();
                if (clickable == null || !clickable.IsInteractable) return;

                currentclick = clickable;
                if (currentclick.saltType != SaltType.None)
                    _session.TrySetSalt(currentclick.saltType);

                if (clickable._zone == null)
                {

                    return;
                }

                clickable.OnClicked(clickable._zone);

            }


        }

        Vector2 PointerPosition()
        {
            return _pressAction.action.ReadValue<Vector2>();
        }
    }
}
