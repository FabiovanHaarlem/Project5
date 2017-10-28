using UnityEngine;
using Game.Event;

namespace Game.Scriptable
{
    //callback that will be called upon depleetment of the timer
    internal delegate void RemovePowerupEffectDelegate(PowerupStats stats);
    //callback to remove powerup in UI
    internal delegate void RemovePowerupPoolDelegate(PowerUp powerUp);

    [CreateAssetMenu(fileName = "NewHat", menuName = "Scriptable/Hat", order = 1)]
    public class ScriptablePowerUp : ScriptableObject
    {
        //info of object is stored here
        [SerializeField]
        [Tooltip("Power Stats go in here")]
        internal PowerupStats m_Stats;

        //image used for UI will go here
        [SerializeField]
        [Tooltip("image used for UI will go here")]
        internal Sprite m_Image;
    }
}
