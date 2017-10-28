using UnityEngine;
using System;

namespace Game.Character.player.Powerups
{
    //callback that will be called upon depleetment of the timer
    internal delegate void RemovePowerupEffectDelegate(PowerupStats stats);
    //callback to remove powerup in UI
    internal delegate void RemovePowerupPoolDelegate(PowerUp powerUp);

    [Serializable]
    internal struct PowerupStats
    {
        //time active
        [SerializeField]
        internal float m_TimeActive;

        //var for other objects to use
        [SerializeField]
        internal float m_AddSpeed;
        [SerializeField]
        internal float m_AddCatchSpeed;
    }

    [CreateAssetMenu(fileName = "NewHat", menuName = "Hat", order = 1)]
    public class ScriptablePowerUp : ScriptableObject
    {
        //info of object is stored here
        [SerializeField][Tooltip("Power Stats go in here")]
        internal PowerupStats stats;

        //image used for UI will go here
        [SerializeField][Tooltip("image used for UI will go here")]
        internal Sprite m_Image;
    }

    public class PowerUp
    {
        //used for update
        private PowerupStats M_States;

        private RemovePowerupEffectDelegate m_RemoveCallBack;
        internal RemovePowerupPoolDelegate m_RemovePoolCallback ;

        private Sprite M_Image;
        public Sprite GetSprite() { return M_Image; }

        internal PowerUp(PowerupStats stats, RemovePowerupEffectDelegate callback, Sprite sprite)
        {
            M_States = stats;
            m_RemoveCallBack = callback;
            M_Image = sprite;
        }

        public void Update()
        {
            M_States.m_TimeActive -= Time.deltaTime;

            if (M_States.m_TimeActive <= 0)
            {
                m_RemoveCallBack.Invoke(GetNegativeStats(M_States));
                m_RemovePoolCallback.Invoke(this);
                Debug.Log("PowerUp is depeted");
            }
        }

        private PowerupStats GetNegativeStats(PowerupStats input)
        {
            PowerupStats output = new PowerupStats()
            {
                m_AddCatchSpeed = -input.m_AddCatchSpeed,
                m_AddSpeed = -input.m_AddSpeed
            };

            return output;
        }
    }
}


