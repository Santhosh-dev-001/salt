using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using SaltAnalysis.Core;
using SaltAnalysis.Data;

namespace SaltAnalysis.Interaction
{
    public class DragInputController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] Camera _raycastCamera;

        [Header("Layers")]
        [SerializeField] LayerMask _draggableLayer;
        [SerializeField] LayerMask _dropZoneLayer;

        [Header("References")]
        [SerializeField] SessionManager _session;
        [SerializeField] InputActionReference _dragAction;
        [SerializeField] InputActionReference _pressAction;

        Draggable _current;
        float _depth;

        // ── Enable / Disable ─────────────────────────────────────────────────

        void OnEnable()
        {
            _pressAction.action.started += OnPressStarted;
            _pressAction.action.canceled += OnPressCanceled;
            _pressAction.action.Enable();
            _dragAction.action.Enable();
        }

        void OnDisable()
        {
            _pressAction.action.started -= OnPressStarted;
            _pressAction.action.canceled -= OnPressCanceled;
            _pressAction.action.Disable();
            _dragAction.action.Disable();
        }

        void Update() => Drag();

        // ── Input ─────────────────────────────────────────────────────────────

        void OnPressStarted(InputAction.CallbackContext ctx) => TryPick();
        void OnPressCanceled(InputAction.CallbackContext ctx) => TryDrop();

        Vector2 PointerPosition() => _dragAction.action.ReadValue<Vector2>();

        // ── Pick ─────────────────────────────────────────────────────────────

        void TryPick()
        {
            if (_raycastCamera == null) return;
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Ray ray = _raycastCamera.ScreenPointToRay(PointerPosition());
            if (!Physics.Raycast(ray, out RaycastHit hit, 200f, _draggableLayer)) return;

            Draggable draggable = hit.collider.GetComponent<Draggable>();
            if (draggable == null || !draggable.IsInteractable) return;

            _current = draggable;
            _depth = _current.HeldDepth;

            // notify session if salt draggable
            if (_current.saltType != SaltType.None)
                _session.TrySetSalt(_current.saltType);

            _current.BeginDrag(hit.point);
        }

        // ── Drag ─────────────────────────────────────────────────────────────

        void Drag()
        {
            if (_current == null) return;

            Vector2 screenPos = PointerPosition();
            Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, _depth);
            Vector3 worldPos = _raycastCamera.ScreenToWorldPoint(screenPoint);

            _current.Drag(worldPos);
        }

        // ── Drop ─────────────────────────────────────────────────────────────

        void TryDrop()
        {
            if (_current == null) return;
            if (_raycastCamera == null) return;

            Ray ray = _raycastCamera.ScreenPointToRay(PointerPosition());
            bool droppedOnZone = false;

            if (Physics.Raycast(ray, out RaycastHit hit, 200f, _dropZoneLayer))
            {
                ExperimentZone zone = hit.collider.GetComponent<ExperimentZone>();
                if (zone != null)
                {
                    Debug.Log("TryDrop enterd");
                    // zone validates everything — salt type, string id, step
                    bool accepted = zone.TryDrop(_current);

                    Debug.Log("calling drop zone");
                    if (accepted)
                    {
                        droppedOnZone = true;
                        _current.Attach(hit.transform);
                    }
                    Debug.Log(accepted);
                }
            }

            if (!droppedOnZone)
                _current.ReturnToStart();

            _current.EndDrag();
            _current = null;
        }
    }
}