using UnityEngine;
using System;

namespace Game.Scriptable
{
    [Serializable]
    internal class PowerupStats : TimedStates
    {

        //var for other objects to use
        [SerializeField]
        internal float m_AddSpeed;
        [SerializeField]
        internal float m_AddCatchSpeed;
    }

    [Serializable]
    internal class TimedStates
    {
        //time active
        [SerializeField]
        internal float m_TimeActive;
    }
}

