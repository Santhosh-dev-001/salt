using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SaltAnalysis.Interaction
{
    public class ClickableStepIndex : MonoBehaviour
    {
        [SerializeField] int _stepIndex;

        public int       StepIndex  => _stepIndex;
        public Clickable Clickable  { get; private set; }

        void Awake()
        {
            Clickable = GetComponent<Clickable>();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ClickableStepIndex))]
    public class ClickableStepIndexEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ClickableStepIndex stepIndex = (ClickableStepIndex)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Register to Clickable Registry"))
                RegisterToRegistry(stepIndex);

            if (GUILayout.Button("Unregister from Clickable Registry"))
                UnregisterFromRegistry(stepIndex);
        }

        void RegisterToRegistry(ClickableStepIndex stepIndex)
        {
            ClickableRegistry registry = FindObjectOfType<ClickableRegistry>();
            if (registry == null)
            {
                Debug.LogWarning("[ClickableStepIndex] No ClickableRegistry found in scene.");
                return;
            }

            SerializedObject   serializedRegistry = new SerializedObject(registry);
            SerializedProperty entriesProp        = serializedRegistry.FindProperty("_entries");

            for (int i = 0; i < entriesProp.arraySize; i++)
            {
                if (entriesProp.GetArrayElementAtIndex(i).objectReferenceValue == stepIndex)
                {
                    Debug.Log($"[ClickableStepIndex] {stepIndex.gameObject.name} already registered.");
                    return;
                }
            }

            entriesProp.arraySize++;
            entriesProp.GetArrayElementAtIndex(entriesProp.arraySize - 1).objectReferenceValue = stepIndex;

            serializedRegistry.ApplyModifiedProperties();
            EditorUtility.SetDirty(registry);

            Debug.Log($"[ClickableStepIndex] {stepIndex.gameObject.name} registered to ClickableRegistry.");
        }

        void UnregisterFromRegistry(ClickableStepIndex stepIndex)
        {
            ClickableRegistry registry = FindObjectOfType<ClickableRegistry>();
            if (registry == null)
            {
                Debug.LogWarning("[ClickableStepIndex] No ClickableRegistry found in scene.");
                return;
            }

            SerializedObject   serializedRegistry = new SerializedObject(registry);
            SerializedProperty entriesProp        = serializedRegistry.FindProperty("_entries");

            for (int i = 0; i < entriesProp.arraySize; i++)
            {
                if (entriesProp.GetArrayElementAtIndex(i).objectReferenceValue == stepIndex)
                {
                    entriesProp.DeleteArrayElementAtIndex(i);
                    serializedRegistry.ApplyModifiedProperties();
                    EditorUtility.SetDirty(registry);
                    Debug.Log($"[ClickableStepIndex] {stepIndex.gameObject.name} unregistered.");
                    return;
                }
            }

            Debug.Log($"[ClickableStepIndex] {stepIndex.gameObject.name} not found in registry.");
        }
    }
#endif
}
