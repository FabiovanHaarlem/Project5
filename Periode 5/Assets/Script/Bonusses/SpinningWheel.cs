using UnityEngine;
using Game.Scriptable;

namespace Game.Character.Pickup
{
    public class SpinningWheel : MonoBehaviour {

        [SerializeField][Tooltip("total duration that the wheel wil spin")]
        float m_duration;

        [SerializeField][Tooltip("Powerups that can be rolled on this wheel")]
        private ScriptableWorldEvent[] m_Events;

        //speed of which the wheel is spinning
        private float m_Speed;
        //speed of which the wheel will be slowed by
        private float m_SlowingSpeed;

        //variable that records how much times the wheel has spinned
        private float m_amountspinned;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Spin();

            //amount of rotation to spin with
            float TotalSpin = m_Speed * Time.deltaTime;

            //rotating the wheel
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, transform.GetChild(0).transform.rotation.eulerAngles.z + TotalSpin);
            //adding amount of spin to recording
            m_amountspinned += TotalSpin;

            //check if the speed is low enough to stop spinning
            if (m_Speed >= -5 && m_Speed <= 5 && m_amountspinned != 0)
            {
                m_Speed = 0;
                m_SlowingSpeed = 0;
                
                //checking where the wheel stopped at
                int index = Mathf.RoundToInt(m_amountspinned / (360 / m_Events.Length));
                while(index > m_Events.Length - 1)
                {
                    index -= m_Events.Length;
                }

                m_amountspinned = 0;

                //invoking the callbackk function
                GameManager.Singelton.RegisterWorldEvent(m_Events[index]);
            }
            else
            {
                //slowwing the wheel
                m_Speed -= m_SlowingSpeed * Time.deltaTime;
            }


        }

        /// <summary>
        /// Spin the wheel, callback will be executed upon completion
        /// </summary>
        /// <param name="callback"></param>
        public void Spin(int minspinspeed = 200 , int maxspinspeed = 1000)
        {

            m_amountspinned = 0;

            float velocity = Random.Range(minspinspeed, maxspinspeed);

            m_SlowingSpeed = velocity / m_duration;

            m_Speed = velocity;

            //setting the transform equal to current spin
            m_amountspinned = transform.GetChild(0).transform.rotation.eulerAngles.z;

        }
    }
}


