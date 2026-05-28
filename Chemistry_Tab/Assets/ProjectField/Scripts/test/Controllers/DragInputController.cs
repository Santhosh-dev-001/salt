using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using SaltAnalysis.Interface;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace SaltAnalysis.Interaction
{
    [RequireComponent(typeof(Camera))]
    public class DragInputController : MonoBehaviour
    {
        [SerializeField] LayerMask       _draggableLayer;
        [SerializeField] LayerMask       _dropZoneLayer;
        [SerializeField] DragDropHandler _drophandler;
        [SerializeField] float           _dragPlaneZ = 0f;

        Camera        _cam;
        SaltDraggable _held;
        Vector3       _offset;

        void Awake()
        {
            _cam = GetComponent<Camera>();
            EnhancedTouchSupport.Enable();
        }

        void OnDestroy()
        {
            EnhancedTouchSupport.Disable();
        }

        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            HandleMouse();
#else
            HandleTouch();
#endif
        }

        // ── Mouse (PC / Editor / WebGL) ──────────────────────────────────────

        void HandleMouse()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            Vector2 screenPos = mouse.position.ReadValue();

            if (mouse.leftButton.wasPressedThisFrame)
                TryPickUp(screenPos);
            else if (mouse.leftButton.isPressed && _held != null)
                _held.OnDrag(ScreenToWorld(screenPos));
            else if (mouse.leftButton.wasReleasedThisFrame && _held != null)
                TryDrop(screenPos);
        }

        // ── Touch (Mobile) ───────────────────────────────────────────────────

        void HandleTouch()
        {
            if (Touch.activeTouches.Count == 0) return;

            var touch = Touch.activeTouches[0];

            switch (touch.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    TryPickUp(touch.screenPosition);
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    if (_held != null)
                        _held.OnDrag(ScreenToWorld(touch.screenPosition));
                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    if (_held != null)
                        TryDrop(touch.screenPosition);
                    break;
            }
        }

        // ── Pick up / Drop ───────────────────────────────────────────────────

        void TryPickUp(Vector2 screenPos)
        {
            Ray ray = _cam.ScreenPointToRay(screenPos);
            if (!Physics.Raycast(ray, out var hit, 100f, _draggableLayer)) return;

            var draggable = hit.collider.GetComponent<SaltDraggable>();
            if (draggable == null) return;

            _held  = draggable;
            _offset = hit.point - _held.transform.position;
            _held.OnPickUp();
        }

        void TryDrop(Vector2 screenPos)
        {
            Ray          ray  = _cam.ScreenPointToRay(screenPos);
            IExperimentZone zone = null;

            if (Physics.Raycast(ray, out var hit, 100f, _dropZoneLayer))
                zone = hit.collider.GetComponent<IExperimentZone>();

            if (zone != null)
                _drophandler.OnDrop(_held, zone);
            else
                _held.OnSnapBack();

            _held = null;
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        Vector3 ScreenToWorld(Vector2 screenPos)
        {
            float z   = _cam.WorldToScreenPoint(new Vector3(0f, 0f, _dragPlaneZ)).z;
            var   pos = new Vector3(screenPos.x, screenPos.y, z);
            return _cam.ScreenToWorldPoint(pos) - _offset;
        }
    }
}
