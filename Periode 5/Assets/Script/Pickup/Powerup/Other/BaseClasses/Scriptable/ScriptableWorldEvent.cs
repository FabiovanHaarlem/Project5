using UnityEngine;
using Game.Event;

namespace Game.Scriptable
{
    //callback to remove powerup in UI
    internal delegate void RemoveWorldEventPoolDelegate(WorldEvent Event);

    public class ScriptableWorldEvent : ScriptableObject
    {
        //info of object is stored here
        [SerializeField]
        [Tooltip("Power Stats go in here")]
        internal float m_TimeActive;

        protected WorldEvent m_Event;

        internal virtual WorldEvent CreateInstance(float m_TimeActive, RemoveWorldEventPoolDelegate callback)
        {
            return null;
        }
    }

    [CreateAssetMenu(fileName = "NewWorldEvent", menuName = "Scriptable/WorldEvents/Tornado", order = 1)]
    public class ScriptableTornado : ScriptableWorldEvent
    {
        //script of the event
        [SerializeField]
        [Tooltip("EventScript that will be ran")]
        protected new Tornado m_Event;

        internal override WorldEvent CreateInstance(float m_TimeActive, RemoveWorldEventPoolDelegate callback)
        {
            return new Tornado(m_TimeActive, callback);
        }
    }

}
