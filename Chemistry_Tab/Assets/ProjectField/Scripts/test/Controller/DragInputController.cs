using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using SaltAnalysis.Interface;
using SaltAnalysis.Data;
using SaltAnalysis.Core;
using SaltAnalysis.Zones;

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
            _pressAction.action.started += OnDragStarted;
            _pressAction.action.canceled += OnDragCanceled;
            _pressAction.action.Enable();
            _dragAction.action.Enable();
        }

        void OnDisable()
        {
            _pressAction.action.started -= OnDragStarted;
            _pressAction.action.canceled -= OnDragCanceled;
            _pressAction.action.Disable();
            _dragAction.action.Disable();
        }

        void Update() => Drag();

        // ── Input callbacks ──────────────────────────────────────────────────

        void OnDragStarted(InputAction.CallbackContext ctx) => TryPick();
        void OnDragCanceled(InputAction.CallbackContext ctx) => Drop();

        Vector2 PointerPosition() => _dragAction.action.ReadValue<Vector2>();

        // ── Pick ─────────────────────────────────────────────────────────────

        void TryPick()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = _raycastCamera.ScreenPointToRay(PointerPosition());
            if (!Physics.Raycast(ray, out RaycastHit hit, 200f, _draggableLayer)) return;

            Draggable draggable = hit.collider.GetComponent<Draggable>();
            if (draggable == null || !draggable.IsInteractable) return;

            _current = draggable;
            _depth = _current.HeldDepth;

            if (_current.draggableType == DraggableType.Salt)
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

        void Drop()
        {
            if (_current == null) return;

            Ray ray = _raycastCamera.ScreenPointToRay(PointerPosition());
            bool droppedOnZone = false;

            if (Physics.Raycast(ray, out RaycastHit hit, 200f, _dropZoneLayer))
            {
                ExperimentZoneBase zone = hit.collider.GetComponent<ExperimentZoneBase>();
                if (zone != null)
                {
                    if (!zone.IsStepAllowed(_session))
                    {
                        Debug.Log("[DragInput] Complete the previous experiment first.");
                    }
                    else
                    {
                        droppedOnZone = true;
                        // pass draggable ID and zone ID — zone validates and fires effects
                        zone.OnStepDropped(_current.Id, zone.ZoneId);
                        _current.Attach(hit.transform);
                    }
                }
            }

            if (!droppedOnZone)
                _current.ReturnToStart();

            _current.EndDrag();
            _current = null;
        }
    }
}