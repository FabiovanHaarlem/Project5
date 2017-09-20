﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : Spawner, IWaveSpawner {

    public float M_Timer { set; get; }

    [HideInInspector]
    public float M_MaxTimer { get; set; }


    [HideInInspector]
    public List<SpawnItem> M_Wave { get; set; }

    private void OnEnable()
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
                if (Random.Range(0, 100) < M_Spawners[i].m_Obj.m_SpawnProcentage && M_Spawners[i].M_Status)
                {
                    SpawnItem spawnItem = new SpawnItem()
                    {
                        m_prefab = M_Spawners[i].m_Obj.m_Prefab,
                        m_Pool = M_Spawners[i].m_Pool,
                        m_SpawnPosition = M_SpawnPosition[0]
                    };
                    M_Wave.Add(spawnItem);
                }
            }
            catch (System.NullReferenceException) { continue; }

        }
    }

    private bool IsSpawning;
    protected IEnumerator WaveSpawn()
    {
        IsSpawning = true;

        SpawnItem[] tempwave = M_Wave.ToArray();

        M_Wave.Clear();
        M_Timer = M_MaxTimer;

        for (int i = 0; i < tempwave.Length; i++)
        {
            if (tempwave[i].m_SpawnPosition == null)
            {
                Debug.LogWarning("Spawning Aborted: Missing Spawnpoint", transform);
                continue;
            }


            bool success = Spawn(i, tempwave[i].m_Pool, tempwave[i].m_prefab, tempwave[i].m_SpawnPosition.position);
            if (!success)
            {
                yield return new WaitForEndOfFrame();
                success = Spawn(i, tempwave[i].m_Pool, tempwave[i].m_prefab, tempwave[i].m_SpawnPosition.position);
                if(!success)
                    Debug.LogWarning("Spawn function failed" , transform);
            }
            yield return new WaitForSecondsRealtime(0.005f);
        }
        IsSpawning = false;
    }
}
