using UnityEngine;
using System;

namespace Plugins.ObjectPool.Spawners
{

    internal sealed class ProcentageSpawnerMultipeSpawnPoints : ProcentageSpawner
    {

        protected override void FixedUpdate()
        {
            for (int i = 0; i < M_Spawners.Count; i++)
            {
                if (M_Spawners[i].m_Obj != null && M_Spawners[i].m_Obj.m_SpawnProcentage > 0)
                {
                    if (UnityEngine.Random.Range(0, 100) < M_Spawners[i].m_Obj.m_SpawnProcentage)
                    {
                        try
                        {
                            int spawnpointindex = UnityEngine.Random.Range(0, M_SpawnPosition.Length);
                            Spawn(i, M_Spawners[i].m_Pool, M_Spawners[i].m_Obj.m_Prefab, M_SpawnPosition[spawnpointindex].position);
                        }
                        catch (NullReferenceException) { Debug.LogWarning("Missing variable while spawning check settings ", transform); }
                        catch (IndexOutOfRangeException) { Debug.LogWarning("Missing variable while spawning check settings ", transform); }
                    }

                }
                else if (M_Spawners[i].m_Obj == null)
                    Debug.LogWarning("Can't Spawn, object struct has not been create yet, aborting", transform);
            }
        }
    }
}
