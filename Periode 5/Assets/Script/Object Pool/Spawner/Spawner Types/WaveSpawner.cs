using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Plugins.ObjectPool.Spawners
{
    internal class WaveSpawner : Spawner, IWaveSpawner
    {

        public float M_Timer { get; set; }

        public float M_MaxTimer { get; set; }

        public List<SpawnItem> M_Wave { get; set; }

        private void Awake()
        {
            M_Wave = new List<SpawnItem>();
            M_Timer = M_MaxTimer;
        }

        protected override void FixedUpdate()
        {

            M_Timer -= Time.deltaTime;

            if (M_Timer < 0 && !IsSpawning)
            {
                StopCoroutine(WaveSpawn());
                StartCoroutine(WaveSpawn());
            }

            for (int i = 0; i < M_Spawners.Count; i++)
            {
                try
                {
                    Debug.Log(M_Spawners[i].m_Obj.m_Prefab);
                    if (UnityEngine.Random.Range(0, 100) < M_Spawners[i].m_Obj.m_SpawnProcentage)
                    {
                        SpawnItem spawnItem = new SpawnItem()
                        {
                            M_prefab = M_Spawners[i].m_Obj.m_Prefab,
                            m_Pool = M_Spawners[i].m_Pool,
                            M_SpawnPosition = M_SpawnPosition[0]
                        };
                        M_Wave.Add(spawnItem);
                        
                    }
                }
                catch (NullReferenceException) { continue; }

            }
        }

        protected bool IsSpawning = false;
        protected IEnumerator WaveSpawn()
        {
            IsSpawning = true;

            SpawnItem[] tempwave = M_Wave.ToArray();

            M_Wave.Clear();
            M_Timer = M_MaxTimer;

            for (int i = 0; i < tempwave.Length; i++)
            {
                if (tempwave[i].M_SpawnPosition == null)
                {
                    Debug.LogWarning("Spawning Aborted: Missing Spawnpoint", transform);
                    continue;
                }


                bool success = Spawn(i, tempwave[i].m_Pool, tempwave[i].M_prefab, tempwave[i].M_SpawnPosition.position);
                if (!success)
                {
                    yield return new WaitForEndOfFrame();
                    success = Spawn(i, tempwave[i].m_Pool, tempwave[i].M_prefab, tempwave[i].M_SpawnPosition.position);
                    if (!success)
                        Debug.LogWarning("Spawn function failed", transform);
                }
                yield return new WaitForSecondsRealtime(0.005f);
            }

            IsSpawning = false;
        }
    }


    /// <summary>
    /// stores information about the item to spawn and contains a refrence to the desired pool
    /// </summary>
    [Serializable]
    public struct SpawnItem
    {
        public GameObject M_prefab { get; internal set; }
        internal List<PoolObject> m_Pool;
        public Transform M_SpawnPosition { get; internal set; }
    }
}
