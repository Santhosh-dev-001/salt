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

        [SerializeField] ExperimentZone _zone;

        [Header("References")]
        [SerializeField] InputActionReference _pressAction;

        void OnEnable()
        {
            _pressAction.action.started += OnPressed;
            _pressAction.action.Enable();
        }

        void OnDisable()
        {
            _pressAction.action.started -= OnPressed;
            _pressAction.action.Disable();
        }

        void OnPressed(InputAction.CallbackContext ctx)
        {
            if (_raycastCamera == null) return;
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = _raycastCamera.ScreenPointToRay(PointerPosition());

            // check if a clickable was hit
            if (!Physics.Raycast(ray, out RaycastHit hit, 200f, _clickableLayer)) return;

            Clickable clickable = hit.collider.GetComponent<Clickable>();
            if (clickable == null || !clickable.IsInteractable) return;

            if (_zone == null)
            {
                Debug.Log("[ClickInputController] No matching zone found.");
                return;
            }

            clickable.OnClicked(_zone);
        }

        Vector2 PointerPosition()
        {
            var mouse = Mouse.current;
            if (mouse != null) return mouse.position.ReadValue();

            var touch = Touchscreen.current;
            if (touch != null && touch.primaryTouch.press.isPressed)
                return touch.primaryTouch.position.ReadValue();

            return Vector2.zero;
        }
    }
}
