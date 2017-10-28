using UnityEngine;
using Game.Scriptable;

namespace Game.Event
{

    public class PowerUp
    {
        //used for update
        private PowerupStats M_States;

        private RemovePowerupEffectDelegate m_RemoveCallBack;
        internal RemovePowerupPoolDelegate M_RemovePoolCallback;

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
                M_RemovePoolCallback.Invoke(this);
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


