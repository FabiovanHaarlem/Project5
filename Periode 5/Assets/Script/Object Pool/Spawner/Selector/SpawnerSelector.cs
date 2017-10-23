using System.Collections.Generic;
using UnityEngine;


namespace Plugins.ObjectPool.Spawners
{
    public enum SpawnerType
    {
        Procentage = 1,
        Procentage_multible_spawnpoints,
        Wave,
        Wave_multible_spawnpoints
    }

    public class SpawnerSelector : MonoBehaviour
    {

        #region Global
        [HideInInspector]
        public int M_SpawnerType;

        [HideInInspector]
        public ISpawner m_Spawner;

        [HideInInspector]
        public List<SpawnerInfo> M_SpawnerList = new List<SpawnerInfo>();

        [HideInInspector]
        public List<Transform> M_Spawnlocation = new List<Transform>();
        #endregion

        #region Wave
        public float m_MaxTimer = 10;
        #endregion

        private void Start()
        {
            if (M_SpawnerType != 0)
                MakeSpawner();
        }

        public void MakeSpawner()
        {
            switch ((SpawnerType)M_SpawnerType)
            {
                case SpawnerType.Procentage:
                    m_Spawner = gameObject.AddComponent<ProcentageSpawner>();
                    m_Spawner.M_Spawners = M_SpawnerList;
                    ProcentageSpawner p_spawner = (ProcentageSpawner)m_Spawner;
                    p_spawner.M_SpawnPosition = M_Spawnlocation.ToArray();
                    break;

                case SpawnerType.Procentage_multible_spawnpoints:
                    m_Spawner = gameObject.AddComponent<ProcentageSpawnerMultipeSpawnPoints>();
                    m_Spawner.M_Spawners = M_SpawnerList;
                    ProcentageSpawnerMultipeSpawnPoints pm_spawner = (ProcentageSpawnerMultipeSpawnPoints)m_Spawner;
                    pm_spawner.M_SpawnPosition = M_Spawnlocation.ToArray();
                    break;

                case SpawnerType.Wave:
                    m_Spawner = gameObject.AddComponent<WaveSpawner>();
                    m_Spawner.M_Spawners = M_SpawnerList;
                    WaveSpawner w_spawner = (WaveSpawner)m_Spawner;
                    w_spawner.M_MaxTimer = m_MaxTimer;
                    w_spawner.M_SpawnPosition = M_Spawnlocation.ToArray();
                    break;

                case SpawnerType.Wave_multible_spawnpoints:
                    m_Spawner = gameObject.AddComponent<WaveSpawnerMultipeSpawnPoints>();
                    m_Spawner.M_Spawners = M_SpawnerList;
                    WaveSpawnerMultipeSpawnPoints wm_spawner = (WaveSpawnerMultipeSpawnPoints)m_Spawner;
                    wm_spawner.M_MaxTimer = m_MaxTimer;
                    wm_spawner.M_SpawnPosition = M_Spawnlocation.ToArray();
                    break;
            }

            m_Spawner.Initialize();

        }

        public void Update()
        {
            Transform[] positions = M_Spawnlocation.ToArray();

            if (m_Spawner != null
                && m_Spawner.M_SpawnPosition != positions)
            {
                m_Spawner.M_SpawnPosition = positions;
                m_Spawner.Initialize();
            }
        }
    }
}
