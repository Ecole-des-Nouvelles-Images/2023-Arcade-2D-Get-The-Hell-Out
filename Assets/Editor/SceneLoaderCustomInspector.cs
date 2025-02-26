using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

using Master.Scripts.Managers;

namespace Editor
{
    [CustomEditor(typeof(SceneLoader))]
    public class SceneLoaderCustomInspector : UnityEditor.Editor
    {
        private SerializedProperty _reloadSceneOnOverflow;
        private SerializedProperty _transitionController;
        private SerializedProperty _additionnalDuration;
        
        private int _currentSceneBuildIndex;
        private int _lastSceneBuildIndex;

        private void OnEnable()
        {
            _currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            _lastSceneBuildIndex = SceneManager.sceneCountInBuildSettings - 1;
            _reloadSceneOnOverflow = serializedObject.FindProperty("_reloadSceneOnOverflow");
            _transitionController = serializedObject.FindProperty("_transitionController");
            _additionnalDuration = serializedObject.FindProperty("_additionnalDuration");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_currentSceneBuildIndex == 0 || _currentSceneBuildIndex == _lastSceneBuildIndex)
            {
                EditorGUILayout.PropertyField(_reloadSceneOnOverflow, new GUIContent("Reload Scene on Overflow",
                    "Shall the current scene be reloaded if the next scene to load should overflow the buildIndex count.\n" + 
                    "If unchecked, any loads attempts that should overflow will do nothing instead."
                ));
            }
            
            EditorGUILayout.PropertyField(_transitionController);
            EditorGUILayout.PropertyField(_additionnalDuration);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
