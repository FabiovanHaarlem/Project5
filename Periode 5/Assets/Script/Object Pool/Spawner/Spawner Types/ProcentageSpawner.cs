using UnityEngine;
using System;

namespace Plugins.ObjectPool.Spawners
{
    internal class ProcentageSpawner : Spawner
    {

        protected void Start()
        {
            for (int i = 0; i < M_SpawnPosition.Length; i++)
            {
                if (M_SpawnPosition[i] == null)
                {
                    Debug.LogWarning("Not all spawnposition fields are assigned", transform);
                }

            }

            if (M_SpawnPosition == null)
                Debug.LogError("No spawnpoints are created on this object", transform);
        }

        protected override void MakePoolReference()
        {
            //get pool reference
            for (int i = 0; i < M_Spawners.Count; i++)
            {
                if (M_Spawners[i].m_Obj == null)
                    continue;

                else if (M_SpawnPosition.Length != 0)
                {
                    try
                    {
                        M_Spawners[i] = new SpawnerInfo()
                        {
                            m_Obj = M_Spawners[i].m_Obj,
                            m_Pool = Pool.Singleton.GetPool(M_Spawners[i].m_Obj.m_Prefab.name),
                            m_SpawnPoint = M_SpawnPosition[0]
                        };
                    }
                    catch (NullReferenceException) { }

                }

                else
                {
                    if (M_Spawners[i].m_Obj.m_SpawnProcentage > 0)
                    {
                        Debug.LogWarning("Removed non attached gameobject from pool", transform);
                        M_Spawners.Remove(M_Spawners[i]);
                    }
                }
            }
        }

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
                            Spawn(i, M_Spawners[i].m_Pool, M_Spawners[i].m_Obj.m_Prefab, M_Spawners[i].m_SpawnPoint.position);
                        }
                        catch (NullReferenceException)
                        {
                            Debug.LogWarning("Missing variable while spawning check settings ", transform);
                            Initialize();
                        }
                    }

                }
                else if (M_Spawners[i].m_Obj == null)
                    Debug.LogWarning("Can't Spawn, object struct has not been create yet, aborting", transform);
            }
        }
    }
}
