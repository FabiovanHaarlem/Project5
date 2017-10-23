using UnityEngine;

namespace Plugins.ObjectPool.Spawners
{
    internal sealed class WaveSpawnerMultipeSpawnPoints : WaveSpawner, IWaveSpawner
    {

        protected override void FixedUpdate()
        {
            M_Timer -= Time.deltaTime;
            if (M_Timer < 0 && !IsSpawning)
                StartCoroutine(WaveSpawn());

            for (int i = 0; i < M_Spawners.Count; i++)
            {
                if (M_Spawners[i].m_Obj != null && Random.Range(0, 100) < M_Spawners[i].m_Obj.m_SpawnProcentage)
                {
                    int SpawnPointIndex = Random.Range(0, M_SpawnPosition.Length);
                    if (M_SpawnPosition.Length == 0)
                        Debug.LogWarning("No Spawnpoints for Spawner found, Spawn aborted", transform);
                    else
                    {
                        SpawnItem spawnItem = new SpawnItem()
                        {
                            M_prefab = M_Spawners[i].m_Obj.m_Prefab,
                            m_Pool = M_Spawners[i].m_Pool,
                            M_SpawnPosition = M_SpawnPosition[SpawnPointIndex]
                        };
                        M_Wave.Add(spawnItem);
                    }
                }
            }
        }
    }
}
