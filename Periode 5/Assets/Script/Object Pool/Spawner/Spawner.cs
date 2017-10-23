using System.Collections.Generic;
using UnityEngine;
using System;

namespace Plugins.ObjectPool.Spawners
{
    internal class Spawner : MonoBehaviour, ISpawner {

        public List<SpawnerInfo> M_Spawners { get; set; }

        public Transform[] M_SpawnPosition { get; set; }

        #region Initialization

        /// <summary>
        /// function that will make refrences of the spawning objects to the pools of the objectpool and will also check if successfull
        /// </summary>
        public void Initialize()
        {
            MakePoolReference();
            Checkreferences();
        }

        /// <summary>
        /// makes refrences from a spawning object in list to the desired pool in the objectpool
        /// </summary>
        protected virtual void MakePoolReference()
        {
            //get pool reference
            for (int i = 0; i < M_Spawners.Count; i++)
            {
                try
                {
                    M_Spawners[i] = new SpawnerInfo()
                    {
                        m_Obj = M_Spawners[i].m_Obj,
                        m_Pool = Pool.Singleton.GetPool(M_Spawners[i].m_Obj.m_Prefab.name),
                    };
                }
                catch (NullReferenceException) { }
            }
        }

        /// <summary>
        /// checks if all refrences are successfully made
        /// </summary>
        /// <returns></returns>
        protected virtual bool Checkreferences()
        {
            //check if all items have a refrence
            for (int i = 0; i < M_Spawners.Count; i++)
            {
                try
                {
                    if (M_Spawners[i].m_Pool == null && M_Spawners[i].m_Obj.m_Prefab != null)
                    {
                        Pool.Singleton.LoadExtraItems(new Poolobj() { m_Amount = 25, m_Prefab = M_Spawners[i].m_Obj.m_Prefab });
                        Debug.LogWarning("Spawned 25 " + M_Spawners[i].m_Obj.m_Prefab.name + " because non spawned on default, check objectpool", transform);
                        return true;
                    }
                }
                catch (NullReferenceException) { }

            }
            return false;
        }

        #endregion

        #region Spawning

        /// <summary>
        /// Spawns object with the input
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pool"></param>
        /// <param name="prefab"></param>
        /// <param name="Spawnpoint"></param>
        /// <returns></returns>
        protected bool Spawn(int index, List<PoolObject> pool, GameObject prefab, Vector3 Spawnpoint)
        {
            if (pool != null && prefab != null)
            {
                if (Pool.Singleton.Spawn(pool, prefab, Spawnpoint) == null)
                    return false;
            }
            else
            {
                Initialize();
                if (prefab == null)
                    Debug.LogWarning("Trying to spawn Prefab but no prefab is given, aborting spawn", transform);
                else
                    Debug.LogWarning("No Pool could be found for " + prefab.name + " remapping ObjectPools for Spawner", transform);
                return false;
            }

            return true;
        }

        #endregion

        protected virtual void FixedUpdate() { }
    }

    #region Structs and classes

    /// <summary>
    /// used to customize a spawner in the inspector and contains info of the objectpool
    /// </summary>
    [Serializable]
    public struct SpawnerInfo
    {
        public SpawnObj m_Obj;
        internal List<PoolObject> m_Pool;
        public Transform m_SpawnPoint;
    }

    /// <summary>
    /// addes a procentage value mainly used to make a spawn chance if an item needs to spawn
    /// </summary>
    [Serializable]
    public sealed class SpawnObj : PoolObjBase
    {
        public float m_SpawnProcentage;
    }

    #endregion
}


