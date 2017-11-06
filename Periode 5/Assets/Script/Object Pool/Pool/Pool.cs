using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Plugins.ObjectPool.Spawners;

namespace Plugins.ObjectPool
{
    public sealed class Pool : MonoBehaviour {

        private static Pool m_Pool;
        public static Pool Singleton
        {
            get
            {
                if(m_Pool == null)
                {
                    GameObject obj = Instantiate(new GameObject("ObjectPool"));
                    m_Pool = obj.AddComponent<Pool>();
                }

                return m_Pool;

            }
        }
        private List<KeyValuePair<List<PoolObject>, string>> m_Pools;

        public Poolobj[] ObjectList;

        #region Startup

        public void Inspector()
        {
            if(ObjectList == null)
                ObjectList = new Poolobj[0];
            if (m_Tags   == null)
                m_Tags = new string[0];
        }

        private void Awake()
        {
            //making pool static instance
            if (m_Pool == null)
                m_Pool = this;
            else
            {
                Destroy(gameObject);
                return;
            }


            if (m_Tags.Length == 0)
                Debug.LogWarning("No tags are associated with spawners, add them, its strongly suggested. if you do not, it will cost some performace on startup",transform);

            //loading default assets in pool
            m_Pools = new List<KeyValuePair<List<PoolObject>, string>>();
            for (int i = 0; i < ObjectList.Length; i++)
            {
                if (ObjectList[i].m_Prefab != null)
                {
                    m_Pools.Add(new KeyValuePair<List<PoolObject>, string>(new List<PoolObject>(), ObjectList[i].m_Prefab.name));
                    GameObject Parrent = new GameObject(ObjectList[i].m_Prefab.name);
                    Parrent.transform.SetParent(transform);
                    List<PoolObject> pool = GetPool(ObjectList[i].m_Prefab.name);
                    for (int j = 0; j < ObjectList[i].m_Amount; j++)
                    {
                        SpawnInPool(Parrent.transform, ObjectList[i].m_Prefab, pool);
                    }
                }
                else
                    Debug.LogError("No object attached a default object", transform);
            }

            M_RemappingRequest = true;
        }
        #endregion


        #region PoolChanges

            #region Spawning
            /// <summary>
            /// Spawn object by prefab named that is already in the pool
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(string name)
            {
                List<PoolObject> Poollist = GetPool(name);

                if (Poollist != null)
                {
                    if (Poollist.Count == 0)
                        return null;
                }
                else
                    return null;

                return RemoveFromPool(Poollist);
            }
            /// <summary>
            /// Spawn object by prefab named that is already in the pool
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(string name, Quaternion rotation)
            {
                PoolObject obj = Spawn(name);

                if(obj != null)
                {
                    obj.transform.rotation = rotation;
                }

                return obj;
            }
            /// <summary>
            /// Spawn object by prefab named that is already in the pool and will spawn it at the spawnpoint given
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(string name, Vector3 SpawnPosition)
            {

                PoolObject obj = Spawn(name);
                if (obj != null)
                    obj.transform.position = SpawnPosition;
                return obj;
            }
            /// <summary>
            /// Spawn object by prefab named that is already in the pool and will spawn it at the spawnpoint given
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(string name, Vector3 SpawnPosition, Quaternion Rotation)
            {

                PoolObject obj = Spawn(name);
                if (obj != null)
                {
                    obj.transform.position = SpawnPosition;
                    obj.transform.rotation = Rotation;
                }

                return obj;
            }
            /// <summary>
            /// Spawn object by prefab named that is already in the pool and will parrent it under transform given
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(string name, Transform Parrent)
            {
                PoolObject obj = Spawn(name);

                if (obj != null)
                    obj.transform.SetParent(Parrent);

                return obj;
            }
            /// <summary>
            /// Spawn object by prefab named that is already in the pool and will parrent it under transform given
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(string name, Transform Parrent, Quaternion Rotation)
            {
                PoolObject obj = Spawn(name);

                if (obj != null)
                {
                    obj.transform.SetParent(Parrent);
                    obj.transform.rotation = Rotation;
                }

                return obj;
            }
            /// <summary>
            /// Spawn object with the given prefab, if no object avalible pool will make more instances
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(GameObject prefab)
            {
                if (prefab == null)
                    return null;


                PoolObject obj = RemoveFromPool(GetPool(prefab.name));

                if (obj == null)
                {
                    obj = RemoveFromPool(LoadExtraItems(new Poolobj() { m_Amount = 2, m_Prefab = prefab }));
                    Debug.LogWarning("Not enough " + prefab.name + " in objectpool, " + 2 + " new " + prefab.name + " has been instantiated.");
                }


            return obj;
            }
            /// <summary>
            /// Spawn object with the given prefab, if no object avalible pool will make more instances
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(GameObject prefab, Quaternion rotation)
            {
                PoolObject obj = Spawn(prefab);

                if (obj != null)
                    obj.transform.rotation = rotation;

                return obj;
            }
            /// <summary>
            /// Spawn object with the given prefab, if no object avalible pool will make more instances, object will spawn at given position
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(GameObject prefab, Vector3 SpawnPosition)
            {

                PoolObject obj = Spawn(prefab);

                if (obj != null)
                    obj.transform.localPosition = SpawnPosition;

                return obj;
            }
            /// <summary>
            /// Spawn object with the given prefab, if no object avalible pool will make more instances, object will spawn at given position
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(GameObject prefab, Vector3 SpawnPosition, Quaternion rotation)
            {

                PoolObject obj = Spawn(prefab);

                if (obj != null)
                {
                    obj.transform.position = SpawnPosition;
                    obj.transform.rotation = rotation;
                }


                return obj;
            }
            /// <summary>
            /// Spawn object with the given prefab, if no object avalible pool will make more instances, object will be parrented under transform given
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(GameObject prefab, Transform Parrent)
            {
                PoolObject obj = Spawn(prefab);

                if (obj != null)
                    obj.transform.SetParent(Parrent);

                return obj;
            }
            /// <summary>
            /// Spawn object with the given prefab, if no object avalible pool will make more instances, object will be parrented under transform given
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(GameObject prefab, Transform Parrent, Quaternion rotation)
            {
                PoolObject obj = Spawn(prefab);

                if (obj != null)
                {
                    obj.transform.SetParent(Parrent);
                    obj.transform.rotation = rotation;
                }


                return obj;
            }
            /// <summary>
            /// Spawn object by giving inputing desired pool to spawn object from
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool)
            {
                return RemoveFromPool(objectpool);
            }
            /// <summary>
            /// Spawn object by giving inputing desired pool to spawn object from
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, Quaternion rotation)
            {
                PoolObject obj = Spawn(objectpool);

                if (obj != null)
                    obj.transform.rotation = rotation;

                return obj;
            }
            /// <summary>
            /// Spawn instance from a desired pool, if None avalible then spawn in new ones
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="prefab"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, GameObject prefab)
            {

                PoolObject obj = Spawn(objectpool);

                if (obj == null)
                    Spawn(prefab);

                return obj;
            }
            /// <summary>
            /// Spawn instance from a desired pool, if None avalible then spawn in new ones
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="prefab"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, GameObject prefab, Quaternion rotation)
            {

                PoolObject obj = Spawn(objectpool);

                if (obj == null)
                    Spawn(prefab);

                if (obj != null)
                    obj.transform.rotation = rotation;

                return obj;
            }
            /// <summary>
            /// Spawn object by given a pool and spawn at spawnpoint location
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="SpawnPosition"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, Vector3 SpawnPosition)
            {
                PoolObject obj = Spawn(objectpool);

                if(obj != null)
                    obj.transform.position = SpawnPosition;

                return obj;
            }
            /// <summary>
            /// Spawn object by given a pool and spawn at spawnpoint location
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="SpawnPosition"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, Vector3 SpawnPosition, Quaternion rotation)
            {
                PoolObject obj = Spawn(objectpool);

                if (obj != null)
                {
                    obj.transform.position = SpawnPosition;
                    obj.transform.rotation = rotation;
                }


                return obj;
            }
            /// <summary>
            /// Spawn Object and parrent it to given transform, will return NULL if no object are avalible in the pool
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="Parrent"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, Transform Parrent)
            {
                PoolObject obj = Spawn(objectpool);

                if (obj != null)
                    obj.transform.SetParent(Parrent);

                return obj;
            }
            /// <summary>
            /// Spawn Object and parrent it to given transform, will return NULL if no object are avalible in the pool
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="Parrent"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, Transform Parrent, Quaternion rotation)
            {
                PoolObject obj = Spawn(objectpool);

                if (obj != null)
                {
                    obj.transform.SetParent(Parrent);
                    obj.transform.rotation = rotation;
                }

                return obj;
            }
            /// <summary>
            /// Spawn using a pool, if no object avalible then spawn in new ones, object will spawn at given spawnpoint. will return NULL if no object are avalible in the pool
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="prefab"></param>
            /// <param name="SpawnPosition"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, GameObject prefab , Vector3 SpawnPosition)
            {
                PoolObject obj = Spawn(objectpool);

                if (obj == null)
                    Spawn(prefab);

                if (obj != null)
                    obj.transform.position = SpawnPosition;
                return obj;
            }
            /// <summary>
            /// Spawn using a pool, if no object avalible then spawn in new ones, object will spawn at given spawnpoint. will return NULL if no object are avalible in the pool
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="prefab"></param>
            /// <param name="SpawnPosition"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, GameObject prefab, Vector3 SpawnPosition, Quaternion rotation)
            {
                PoolObject obj = Spawn(objectpool);

                if (obj == null)
                    Spawn(prefab);

                if (obj != null)
                {
                    obj.transform.position = SpawnPosition;
                    obj.transform.rotation = rotation;
                }

                return obj;
            }
            /// <summary>
            /// Spawn using a pool, if no object avalible then spawn in new ones, object will be parrented at given transform
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="prefab"></param>
            /// <param name="Parrent"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, GameObject prefab, Transform Parrent)
            {
                PoolObject obj = Spawn(objectpool);

                if (obj == null)
                    Spawn(prefab);

                if (obj != null)
                    obj.transform.SetParent(Parrent);

                return obj;
            }
            /// <summary>
            /// Spawn using a pool, if no object avalible then spawn in new ones, object will be parrented at given transform
            /// </summary>
            /// <param name="objectpool"></param>
            /// <param name="prefab"></param>
            /// <param name="Parrent"></param>
            /// <returns></returns>
            public PoolObject Spawn(List<PoolObject> objectpool, GameObject prefab, Transform Parrent, Quaternion rotation)
            {
                PoolObject obj = Spawn(objectpool);

                if (obj == null)
                    Spawn(prefab);

                if (obj != null)
                {
                    obj.transform.SetParent(Parrent);
                    obj.transform.rotation = rotation;
                }


                return obj;
            }

            /// <summary>
            /// will return a the pool list of the newly created pool and returns the name it can be found under
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public KeyValuePair<List<PoolObject>, string> CreatePoolBasedOnObject(PoolObject obj)
            {
                //create new pool
                if(obj != null)
                {
                    KeyValuePair<List<PoolObject>, string> newpool = new KeyValuePair<List<PoolObject>, string>(new List<PoolObject>(), obj.gameObject.name);

                    m_Pools.Add(newpool);

                    return newpool;
                }

                return new KeyValuePair<List<PoolObject>, string>();
            }
        /// <summary>
        /// will return a the pool list of the newly created pool and returns the name it can be found under, will also take a number of how many you already want to instantiate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
            public KeyValuePair<List<PoolObject>, string> CreatePoolBasedOnObject(PoolObject obj, int Amount)
            {
                KeyValuePair<List<PoolObject>, string> newpool = CreatePoolBasedOnObject(obj);

                //create a spawnitem
                Poolobj spawnobj = new Poolobj()
                {
                    m_Amount = 1,
                    m_Prefab = CreateClonePrefab(obj.gameObject)
                };

                //spawn item in pool
                LoadExtraItems(spawnobj);

                return newpool;
            }

            private GameObject CreateClonePrefab(GameObject input)
            {
                GameObject new_obj = new GameObject(input.gameObject.name);
                foreach (Component j in input.gameObject.GetComponents<Component>())
                {
                    Component newcomponent = new_obj.AddComponent<Component>();
                    newcomponent = j;
                }
                return new_obj;
            }

            public void PlaceInPool(PoolObject input)
            {
                List<PoolObject> pool = GetPool(input.name);
                if (pool == null)
                    CreatePoolBasedOnObject(input).Key.Add(input);
                else
                    pool.Add(input);

                LoadExtraItems(new Poolobj() { m_Amount = 0, m_Prefab = input.gameObject });
                Transform Parrent = transform.Find(input.name);
                SpawnInPool(Parrent, input.gameObject, pool);
            }

        #endregion

        /// <summary>
        /// PoolObject callback when object gets deactivated they will call this function to place them back in their pool
        /// </summary>
        /// <param name="m_Pool"></param>
        /// <param name="Script"></param>
        /// <param name="parrent"></param>
        private void AddToPool(List<PoolObject> m_Pool, PoolObject Script, Transform parrent)
        {
            m_Pool.Add(Script);
            Script.gameObject.transform.SetParent(parrent);
        }

        /// <summary>
        /// removes object from pool that is given, if none is avalible function returns NULL
        /// </summary>
        /// <param name="Poollist"></param>
        /// <returns></returns>
        private PoolObject RemoveFromPool(List<PoolObject> Poollist)
        {
            if(Poollist != null
                && Poollist.Count > 0)
            {
                PoolObject poolobject = Poollist[0];
                Poollist.Remove(poolobject);
                poolobject.gameObject.SetActive(true);
                poolobject.Activate();
                return poolobject;
            }
            return null;
        }

        /// <summary>
        /// spawns instances in a pool
        /// </summary>
        /// <param name="Parrent"></param>
        /// <param name="prefab"></param>
        /// <param name="pool"></param>
        /// <returns></returns>
        private PoolObject SpawnInPool(Transform Parrent, GameObject prefab, List<PoolObject> pool)
        {
            GameObject NewObj = Instantiate(prefab,Parrent);
            PoolObject obj = NewObj.GetComponent<PoolObject>();
            if(obj == null)
            {
                NewObj.AddComponent<PoolObject>();
                obj = NewObj.GetComponent<PoolObject>();
            }
            pool.Add(obj);
            PoolObjectInfo info = new PoolObjectInfo
            {
                Pool = GetPool(prefab.name),
                Parrent = Parrent.transform,
                Callback = new PoolObjectCallback(AddToPool)
            };

            pool[pool.Count - 1].Initialize(info);
            return obj;
        }

        /// <summary>
        /// Creates new object and automaticly places them in the right pool, if this is a new object for the pool it will automaticly create a new pool
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="Prefab"></param>
        /// <returns></returns>
        internal List<PoolObject> LoadExtraItems(Poolobj Input)
        {

            List<PoolObject> output = GetPool(Input.m_Prefab.name);

            if (GetPool(Input.m_Prefab.name) == null)
            {

                m_Pools.Add(new KeyValuePair<List<PoolObject>, string>(new List<PoolObject>(), Input.m_Prefab.name));
                GameObject Parrent = new GameObject(Input.m_Prefab.name);
                Parrent.transform.SetParent(transform);
                output = GetPool(Input.m_Prefab.name);
                for (int j = 0; j < Input.m_Amount; j++)
                {
                    SpawnInPool(Parrent.transform, Input.m_Prefab, output);
                }

                M_RemappingRequest = true;
                Debug.Log("New objectpool created, poolname: " + Input.m_Prefab.name);
            }
            else
            {
                Transform Parrent = transform.Find(Input.m_Prefab.name);
                for (int j = 0; j < Input.m_Amount; j++)
                {
                    SpawnInPool(Parrent, Input.m_Prefab, output);
                }
            }

            return output;

        }

        #endregion


        #region Other

        internal bool M_IsRemapping { get; private set; }
        internal bool M_RemappingRequest { get; private set; }
        public string[] m_Tags;

        /// <summary>
        /// this function will rearange all spawners with there spawnobjects to match the right pools
        /// </summary>
        /// <returns></returns>
        private IEnumerator RemapPoolData()
        {
            if (!M_IsRemapping)
            {
                M_IsRemapping = true;
                M_RemappingRequest = false;
                yield return null;
                foreach (string tagname in m_Tags)
                {
                    foreach (GameObject i in GameObject.FindGameObjectsWithTag(tagname))
                    {
                        foreach (ISpawner j in i.GetComponents<ISpawner>())
                            j.Initialize();
                    }
                }
                M_IsRemapping = false;
                Debug.Log("Remapping Pool Completed");
            }
            else
                Debug.LogWarning("Remapping already running, exiting");
        }

        /// <summary>
        /// Finds a pool based on string as a input, String must match prefab name
        /// </summary>
        /// <param name="PoolName"></param>
        /// <returns></returns>
        public List<PoolObject> GetPool(string PoolName)
        {
            for (int i = 0; i < m_Pools.Count; i++)
            {
                if (m_Pools[i].Value == PoolName)
                    return m_Pools[i].Key;
            }

            return null;
        }

        public void FixedUpdate()
        {
            if (M_RemappingRequest && !M_IsRemapping)
                StartCoroutine(RemapPoolData());
        }

        #endregion

    }
    /// <summary>
    /// adds amount of how many must be spawned in pool
    /// </summary>
    [Serializable]
    public sealed class Poolobj : PoolObjBase
    {
        public int m_Amount = 1;
    }
    /// <summary>
    /// base class of spawninformation
    /// </summary>
    public class PoolObjBase {
        public GameObject m_Prefab;
    }
}

