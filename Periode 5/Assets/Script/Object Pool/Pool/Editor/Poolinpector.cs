using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Plugins.ObjectPool
{
    [CustomEditor(typeof(Pool))]
    sealed class Poolinpector : Editor
    {

        private List<string> m_TagList;

        private Pool m_target;
        private List<Poolobj> m_ObjList;

        private void OnEnable()
        {
            m_target = (Pool)target;
            m_target.Inspector();
            m_ObjList = new List<Poolobj>();
            for (int i = 0; i < m_target.ObjectList.Length; i++)
                m_ObjList.Add(m_target.ObjectList[i]);
            m_TagList = new List<string>();
            for (int i = 0; i < m_target.m_Tags.Length; i++)
                m_TagList.Add(m_target.m_Tags[i]);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (Application.isPlaying)
                GUI.enabled = false;

            ObjectPool();
            TagForSpawners();

            if (GUI.changed && !Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            EditorUtility.SetDirty(m_target);

            if (Application.isPlaying)
                GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }

        private int selectedtag = 0;
        private void TagForSpawners()
        {
            GUILayout.Label("Tags associated with Spawners");

            for (int i = 0; i < m_TagList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                m_TagList[i] = EditorGUILayout.TextField(m_TagList[i]);
                if (GUILayout.Button("Remove"))
                    m_TagList.Remove(m_TagList[i]);

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(5);
            GUILayout.Label("Add new tag");
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
            selectedtag = EditorGUILayout.Popup(selectedtag, tags);
            if (GUILayout.Button("Add Tag"))
                m_TagList.Add(tags[selectedtag]);
            m_target.m_Tags = m_TagList.ToArray();
        }

        private void ObjectPool()
        {
            GUILayout.Label("Default objects");
            if (m_ObjList.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Prefab");
                GUILayout.Label("Amount");
                EditorGUILayout.EndHorizontal();
            }

            for (int i = 0; i < m_ObjList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                m_ObjList[i].m_Prefab = (GameObject)EditorGUILayout.ObjectField(m_ObjList[i].m_Prefab, typeof(GameObject), true);
                m_ObjList[i].m_Amount = EditorGUILayout.IntField(m_ObjList[i].m_Amount);
                if (GUILayout.Button("Remove"))
                    m_ObjList.Remove(m_ObjList[i]);

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Object"))
                m_ObjList.Add(new Poolobj());

            m_target.ObjectList = m_ObjList.ToArray();
        }
    }
}
