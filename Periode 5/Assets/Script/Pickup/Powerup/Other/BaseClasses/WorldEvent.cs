using UnityEngine;
using Game.Scriptable;

namespace Game.Event
{
    public class WorldEvent
    {
        //used for update
        private float m_TimeActive;

        internal RemoveWorldEventPoolDelegate M_RemovePoolCallback;

        internal WorldEvent(float TimeActive, RemoveWorldEventPoolDelegate callback)
        {
            m_TimeActive = TimeActive;
            M_RemovePoolCallback = callback;
        }

        public virtual void Update()
        {
            m_TimeActive -= Time.deltaTime;

            if (m_TimeActive <= 0)
            {
                M_RemovePoolCallback.Invoke(this);
                Debug.Log("Event Ended");
            }
        }
    }
}
