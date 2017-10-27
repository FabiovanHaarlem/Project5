using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ObjectPool
{
    internal delegate void PoolObjectCallback(List<PoolObject> Pool, PoolObject Script, Transform Parrent);
    internal struct PoolObjectInfo { internal List<PoolObject> Pool; internal Transform Parrent; internal PoolObjectCallback Callback; }

    public class PoolObject :  MonoBehaviour
    {

        private PoolObjectInfo m_Info;

        internal virtual void Initialize(PoolObjectInfo Info)
        {
            m_Info = Info;
            gameObject.SetActive(false);
        }

        protected internal virtual void Activate()
        {
            transform.SetParent(null);
        }

        protected virtual void Deactivate()
        {
            if (m_Info.Callback != null)
                m_Info.Callback.Invoke(m_Info.Pool, this, m_Info.Parrent);
            else
            {
                Pool.Singleton.PlaceInPool(this);
                if(m_Info.Callback == null)
                    Debug.LogError("Callback can't be found", transform);
                else
                    m_Info.Callback.Invoke(m_Info.Pool, this, m_Info.Parrent);
            }

        
            gameObject.SetActive(false);
        }
    }
}


