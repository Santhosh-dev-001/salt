using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SaltAnalysis.Interaction
{
    public class DraggableStepIndex : MonoBehaviour
    {
        [SerializeField] List<int> _stepIndex;

        public List<int> StepIndex => _stepIndex;
        public Draggable Draggable { get; private set; }

        void Awake()
        {
            Draggable = GetComponent<Draggable>();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DraggableStepIndex))]
    public class DraggableStepIndexEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DraggableStepIndex stepIndex = (DraggableStepIndex)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Register to Draggable Registry"))
            {
                RegisterToRegistry(stepIndex);
            }

            if (GUILayout.Button("Unregister from Draggable Registry"))
            {
                UnregisterFromRegistry(stepIndex);
            }
        }

        void RegisterToRegistry(DraggableStepIndex stepIndex)
        {
            DraggableRegistry registry = FindObjectOfType<DraggableRegistry>();
            if (registry == null)
            {
                Debug.LogWarning("[DraggableStepIndex] No DraggableRegistry found in scene.");
                return;
            }

            SerializedObject serializedRegistry = new SerializedObject(registry);
            SerializedProperty entriesProp = serializedRegistry.FindProperty("_entries");

            // check if already registered
            for (int i = 0; i < entriesProp.arraySize; i++)
            {
                SerializedProperty entry = entriesProp.GetArrayElementAtIndex(i);
                if (entry.objectReferenceValue == stepIndex)
                {
                    Debug.Log($"[DraggableStepIndex] {stepIndex.gameObject.name} already registered.");
                    return;
                }
            }

            // add to registry
            entriesProp.arraySize++;
            entriesProp.GetArrayElementAtIndex(entriesProp.arraySize - 1).objectReferenceValue = stepIndex;

            serializedRegistry.ApplyModifiedProperties();
            EditorUtility.SetDirty(registry);

            Debug.Log($"[DraggableStepIndex] {stepIndex.gameObject.name} registered to DraggableRegistry.");
        }

        void UnregisterFromRegistry(DraggableStepIndex stepIndex)
        {
            DraggableRegistry registry = FindObjectOfType<DraggableRegistry>();
            if (registry == null)
            {
                Debug.LogWarning("[DraggableStepIndex] No DraggableRegistry found in scene.");
                return;
            }

            SerializedObject serializedRegistry = new SerializedObject(registry);
            SerializedProperty entriesProp = serializedRegistry.FindProperty("_entries");

            for (int i = 0; i < entriesProp.arraySize; i++)
            {
                SerializedProperty entry = entriesProp.GetArrayElementAtIndex(i);
                if (entry.objectReferenceValue == stepIndex)
                {
                    entriesProp.DeleteArrayElementAtIndex(i);
                    serializedRegistry.ApplyModifiedProperties();
                    EditorUtility.SetDirty(registry);
                    Debug.Log($"[DraggableStepIndex] {stepIndex.gameObject.name} unregistered.");
                    return;
                }
            }

            Debug.Log($"[DraggableStepIndex] {stepIndex.gameObject.name} not found in registry.");
        }
    }
#endif
}