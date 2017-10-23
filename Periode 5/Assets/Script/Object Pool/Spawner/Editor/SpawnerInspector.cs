using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Plugins.ObjectPool.Spawners;

namespace Plugins.ObjectPool
{
    [CustomEditor(typeof(SpawnerSelector))]
    sealed class SpawnerInspector : Editor
    {

        private SpawnerSelector m_target;

        private int m_Type;


        private void OnEnable()
        {
            m_target = (SpawnerSelector)target;
            m_Type = m_target.M_SpawnerType;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (Application.isPlaying)
                GUI.enabled = false;

            TypeChoice();

            if (m_Type > 0)
            {
                SpawnPoint();
                ObjectSelector();
            }


            if (Application.isPlaying && (
                m_Type == (int)SpawnerType.Wave ||
                m_Type == (int)SpawnerType.Wave_multible_spawnpoints))
            {
                DrawNextWave();
            }

            if (GUI.changed && !Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            EditorUtility.SetDirty(m_target);
            if (Application.isPlaying)
                GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }

        private void TypeChoice()
        {
            int typecopy = m_Type;
            GUILayout.Label("select Spawnertype");
            string[] options = new string[]
            {
             "None", "Procentage", "Procentage multiple spawnpoints" , "Wave", "Wave multiple spawnpoints"
            };
            m_Type = EditorGUILayout.Popup("SpawnerType", m_Type, options);

            if (typecopy != m_Type && m_target.M_Spawnlocation.Count > 1)
                m_target.M_Spawnlocation = new List<Transform>() { m_target.M_Spawnlocation[0] };
            else if (typecopy != m_Type)
            {
                m_target.M_Spawnlocation = new List<Transform>() { };
            }


            m_target.M_SpawnerType = m_Type;
        }

        private void SpawnPoint()
        {
            GUILayout.Space(10);
            GUI.enabled = true;
            if (m_Type == (int)SpawnerType.Wave || m_Type == (int)SpawnerType.Procentage)
            {
                if (m_target.M_Spawnlocation.Count == 0)
                    m_target.M_Spawnlocation.Add((Transform)EditorGUILayout.ObjectField("Spawnpoint", null, typeof(Transform), true));
                else
                    m_target.M_Spawnlocation[0] = (Transform)EditorGUILayout.ObjectField("Spawnpoint", m_target.M_Spawnlocation[0], typeof(Transform), true);
                GUILayout.Space(10);
            }
            else if (m_Type == (int)SpawnerType.Wave_multible_spawnpoints || m_Type == (int)SpawnerType.Procentage_multible_spawnpoints)
            {
                if (m_target.M_Spawnlocation.Count > 0)
                {
                    GUILayout.Label("SpawnPoints");
                }
                else
                    GUILayout.Label("No Spawnpoints");

                for (int i = 0; i < m_target.M_Spawnlocation.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    m_target.M_Spawnlocation[i] = (Transform)EditorGUILayout.ObjectField("Spawnpoint", m_target.M_Spawnlocation[i], typeof(Transform), true);
                    if (GUILayout.Button("Remove"))
                        m_target.M_Spawnlocation.Remove(m_target.M_Spawnlocation[i]);

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add SpawnPoint"))
                    m_target.M_Spawnlocation.Add(null);

                GUILayout.Space(10);
            }

            if (Application.isPlaying)
                GUI.enabled = false;
        }

        private void ObjectSelector()
        {
            GUILayout.Space(10);
            GUI.enabled = true;

            //only for waves
            if (m_Type == (int)SpawnerType.Wave || m_Type == (int)SpawnerType.Wave_multible_spawnpoints)
            {
                m_target.m_MaxTimer = EditorGUILayout.FloatField("Time between waves", m_target.m_MaxTimer);
                if (Application.isPlaying)
                {
                    IWaveSpawner spawner = (IWaveSpawner)m_target.m_Spawner;
                    spawner.M_MaxTimer = m_target.m_MaxTimer;
                }

            }

            GUILayout.Space(10);

            if (m_target.M_SpawnerList.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Prefab");
                GUILayout.Label("Procentage 0 - 100% per frame");
                EditorGUILayout.EndHorizontal();
            }
            else
                GUILayout.Label("No objects to spawn");

            for (int i = 0; i < m_target.M_SpawnerList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                m_target.M_SpawnerList[i].m_Obj.m_Prefab = (GameObject)EditorGUILayout.ObjectField(m_target.M_SpawnerList[i].m_Obj.m_Prefab, typeof(GameObject), true);
                m_target.M_SpawnerList[i].m_Obj.m_SpawnProcentage = EditorGUILayout.FloatField(m_target.M_SpawnerList[i].m_Obj.m_SpawnProcentage);
                if (GUILayout.Button("Remove"))
                    m_target.M_SpawnerList.Remove(m_target.M_SpawnerList[i]);

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Object"))
                m_target.M_SpawnerList.Add(new SpawnerInfo() { m_Obj = new SpawnObj() });
            if (Application.isPlaying)
                GUI.enabled = false;
        }

        private void DrawNextWave()
        {
            GUI.enabled = false;
            GUILayout.Space(15);

            IWaveSpawner spawner = (IWaveSpawner)m_target.m_Spawner;
            List<SpawnItem> info = spawner.M_Wave;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Upcomming wave");
            GUILayout.Label("Time left " + spawner.M_Timer);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (info.Count < 1)
                GUILayout.Label("No objects for next wave");

            for (int i = 0; i < info.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.ObjectField(info[i].M_prefab, typeof(GameObject), true);
                EditorGUILayout.ObjectField(info[i].M_SpawnPosition, typeof(Transform), true);

                EditorGUILayout.EndHorizontal();
            }
            GUI.enabled = true;
        }
    }
}
