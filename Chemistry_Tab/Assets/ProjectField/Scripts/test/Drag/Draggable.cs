using UnityEngine;
using System.Collections;
using SaltAnalysis.Core;

namespace SaltAnalysis.Interaction
{
    public class Draggable : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] public SaltType saltType;
        [SerializeField] public string stringId;

        [Header("Drag Settings")]
        [SerializeField] float dragSmooth = 15f;
        [SerializeField] float heldDepth = 5f;

        [Header("Drop Settings")]
        [SerializeField] float dropDuration = 0.5f;
        [SerializeField] bool reparentOnDrop;
        [SerializeField] bool rotateOnDrop;
        [SerializeField] Vector3 dropRotationEuler;
        [SerializeField] bool dropRotationRelativeToZone;

        [Header("Return Settings")]
        [SerializeField] float returnDuration = 0.5f;

        [Header("Collider")]
        [SerializeField] bool disableColliderOnPickup = true;
        [SerializeField] bool enableColliderOnDrop = true;

        // ── Public ───────────────────────────────────────────────────────────

        public static Draggable CurrentlyDragged { get; private set; }

        public bool IsInteractable => _isInteractable;
        public float HeldDepth => heldDepth;
        public Vector3 DragStartPos => _dragStartPos;
        public Quaternion DragStartRot => _dragStartRot;

        // ── Private ──────────────────────────────────────────────────────────

        Collider _col;
        Coroutine _activeRoutine;
        Vector3 _dragStartPos;
        Quaternion _dragStartRot;
        bool _isDragging;
        bool _isInteractable = true;

        void Awake()
        {
            _col = GetComponent<Collider>();
        }

        // ── Drag ─────────────────────────────────────────────────────────────

        public void BeginDrag(Vector3 hitPoint)
        {
            CurrentlyDragged = this;
            _dragStartPos = transform.position;
            _dragStartRot = transform.rotation;
            _isDragging = true;

            if (disableColliderOnPickup && _col != null)
                _col.enabled = false;
        }

        public void Drag(Vector3 worldPosition)
        {
            if (!_isDragging) return;
            transform.position = Vector3.Lerp(
                transform.position, worldPosition, Time.deltaTime * dragSmooth);
        }

        public void EndDrag()
        {
            _isDragging = false;
        }

        // ── Drop ─────────────────────────────────────────────────────────────

        public void Attach(Transform zoneTrans)
        {
            _isDragging = false;
            StopActive();
            _activeRoutine = StartCoroutine(DropLerp(zoneTrans));
        }

        public void ReturnToStart()
        {
            _isDragging = false;
            StopActive();
            _activeRoutine = StartCoroutine(ReturnLerp(_dragStartPos, _dragStartRot));
        }

        public void SetInteractable(bool state)
        {
            _isInteractable = state;
            if (_col != null) _col.enabled = state;
        }

        // ── Coroutines ───────────────────────────────────────────────────────

        IEnumerator DropLerp(Transform zoneTrans)
        {
            if (reparentOnDrop && zoneTrans != null)
                transform.SetParent(zoneTrans.parent, true);

            Vector3 startPos = transform.position;
            Vector3 endPos = zoneTrans != null ? zoneTrans.position : startPos;
            Quaternion startRot = transform.rotation;
            Quaternion endRot = ComputeDropRotation(zoneTrans);

            float elapsed = 0f;
            while (elapsed < dropDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / dropDuration;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                if (rotateOnDrop)
                    transform.rotation = Quaternion.Slerp(startRot, endRot, t);
                yield return null;
            }

            transform.position = endPos;
            if (rotateOnDrop) transform.rotation = endRot;

            if (enableColliderOnDrop && _col != null)
                _col.enabled = true;

            CurrentlyDragged = null;
        }

        IEnumerator ReturnLerp(Vector3 targetPos, Quaternion targetRot)
        {
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;

            float elapsed = 0f;
            while (elapsed < returnDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / returnDuration;
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
                yield return null;
            }

            transform.position = targetPos;
            transform.rotation = targetRot;

            if (_col != null) _col.enabled = true;

            CurrentlyDragged = null;
        }

        Quaternion ComputeDropRotation(Transform zoneTrans)
        {
            if (!rotateOnDrop) return transform.rotation;
            if (dropRotationRelativeToZone && zoneTrans != null)
                return zoneTrans.rotation * Quaternion.Euler(dropRotationEuler);
            return Quaternion.Euler(dropRotationEuler);
        }

        void StopActive()
        {
            if (_activeRoutine != null)
            {
                StopCoroutine(_activeRoutine);
                _activeRoutine = null;
            }
        }
    }
}


//using UnityEngine;
//using System.Collections;
//using SaltAnalysis.Core;

//namespace SaltAnalysis.Interaction
//{
//    public class Draggable : MonoBehaviour
//    {
//        [Header("Identity")]
//        [SerializeField] public SaltType saltType;
//        [SerializeField] public string stringId;

//        [Header("Drag Settings")]
//        [SerializeField] float dragSmooth = 15f;
//        [SerializeField] float heldDepth = 5f;

//        [Header("Drop Settings")]
//        [SerializeField] float dropDuration = 0.5f;
//        [SerializeField] bool reparentOnDrop;
//        [SerializeField] bool rotateOnDrop;
//        [SerializeField] Vector3 dropRotationEuler;
//        [SerializeField] bool dropRotationRelativeToZone;

//        [Header("Return Settings")]
//        [SerializeField] float returnDuration = 0.5f;

//        // NEW CHECKBOX
//        [SerializeField] bool returnToOriginalAfterDrop = false;

//        [Header("Collider")]
//        [SerializeField] bool disableColliderOnPickup = true;
//        [SerializeField] bool enableColliderOnDrop = true;

//        // ── Public ───────────────────────────────────────────────────────────

//        public static Draggable CurrentlyDragged { get; private set; }

//        public bool IsInteractable => _isInteractable;
//        public float HeldDepth => heldDepth;
//        public Vector3 DragStartPos => _dragStartPos;
//        public Quaternion DragStartRot => _dragStartRot;

//        // ── Private ──────────────────────────────────────────────────────────

//        Collider _col;
//        Coroutine _activeRoutine;
//        Vector3 _dragStartPos;
//        Quaternion _dragStartRot;
//        bool _isDragging;
//        bool _isInteractable = true;

//        void Awake()
//        {
//            _col = GetComponent<Collider>();
//        }

//        // ── Drag ─────────────────────────────────────────────────────────────

//        public void BeginDrag(Vector3 hitPoint)
//        {
//            CurrentlyDragged = this;
//            _dragStartPos = transform.position;
//            _dragStartRot = transform.rotation;
//            _isDragging = true;

//            if (disableColliderOnPickup && _col != null)
//                _col.enabled = false;
//        }

//        public void Drag(Vector3 worldPosition)
//        {
//            if (!_isDragging) return;

//            transform.position = Vector3.Lerp(
//                transform.position,
//                worldPosition,
//                Time.deltaTime * dragSmooth);
//        }

//        public void EndDrag()
//        {
//            _isDragging = false;
//        }

//        // ── Drop ─────────────────────────────────────────────────────────────

//        public void Attach(Transform zoneTrans)
//        {
//            _isDragging = false;
//            StopActive();
//            _activeRoutine = StartCoroutine(DropLerp(zoneTrans));
//        }

//        public void ReturnToStart()
//        {
//            _isDragging = false;
//            StopActive();
//            _activeRoutine = StartCoroutine(ReturnLerp(_dragStartPos, _dragStartRot));
//        }

//        public void SetInteractable(bool state)
//        {
//            _isInteractable = state;

//            if (_col != null)
//                _col.enabled = state;
//        }

//        // ── Coroutines ───────────────────────────────────────────────────────

//        IEnumerator DropLerp(Transform zoneTrans)
//        {
//            if (reparentOnDrop && zoneTrans != null)
//                transform.SetParent(zoneTrans.parent, true);

//            Vector3 startPos = transform.position;
//            Vector3 endPos = zoneTrans != null ? zoneTrans.position : startPos;

//            Quaternion startRot = transform.rotation;
//            Quaternion endRot = ComputeDropRotation(zoneTrans);

//            float elapsed = 0f;

//            while (elapsed < dropDuration)
//            {
//                elapsed += Time.deltaTime;
//                float t = elapsed / dropDuration;

//                transform.position = Vector3.Lerp(startPos, endPos, t);

//                if (rotateOnDrop)
//                    transform.rotation = Quaternion.Slerp(startRot, endRot, t);

//                yield return null;
//            }

//            transform.position = endPos;

//            if (rotateOnDrop)
//                transform.rotation = endRot;

//            if (enableColliderOnDrop && _col != null)
//                _col.enabled = true;

//            // NEW LOGIC
//            if (returnToOriginalAfterDrop)
//            {
//                yield return StartCoroutine(
//                    ReturnLerp(_dragStartPos, _dragStartRot));
//                yield break;
//            }

//            CurrentlyDragged = null;
//        }

//        IEnumerator ReturnLerp(Vector3 targetPos, Quaternion targetRot)
//        {
//            Vector3 startPos = transform.position;
//            Quaternion startRot = transform.rotation;

//            float elapsed = 0f;

//            while (elapsed < returnDuration)
//            {
//                elapsed += Time.deltaTime;
//                float t = elapsed / returnDuration;

//                transform.position = Vector3.Lerp(startPos, targetPos, t);
//                transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

//                yield return null;
//            }

//            transform.position = targetPos;
//            transform.rotation = targetRot;

//            if (_col != null)
//                _col.enabled = true;

//            CurrentlyDragged = null;
//        }

//        Quaternion ComputeDropRotation(Transform zoneTrans)
//        {
//            if (!rotateOnDrop)
//                return transform.rotation;

//            if (dropRotationRelativeToZone && zoneTrans != null)
//                return zoneTrans.rotation * Quaternion.Euler(dropRotationEuler);

//            return Quaternion.Euler(dropRotationEuler);
//        }

//        void StopActive()
//        {
//            if (_activeRoutine != null)
//            {
//                StopCoroutine(_activeRoutine);
//                _activeRoutine = null;
//            }
//        }
//    }
//}