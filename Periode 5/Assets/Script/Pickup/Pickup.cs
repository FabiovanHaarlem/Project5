using UnityEngine;
using Plugins.ObjectPool;
using Game.Scriptable;

namespace Game.Character.Pickup
{
    [RequireComponent(typeof(Collider))]
    public class Pickup : PoolObject
    {

        [SerializeField]
        private ScriptableObject m_ScriptableObj;

        private Sprite m_Image;
        public Sprite Image
        {
            set
            {
                m_Image = value;
            }
        }

        private SpriteRenderer m_Renderer;

        public ScriptableObject SetPower
        {
            set
            {
                m_ScriptableObj = value;
            }
        }


        private void Start()
        {
            GetComponent<Collider>().isTrigger = true;
            m_Renderer = gameObject.AddComponent<SpriteRenderer>();
        }

        private void Update()
        {
            m_Renderer.sprite = m_Image;
            transform.LookAt(Camera.main.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (m_ScriptableObj.GetType() == typeof(ScriptablePowerUp))
                    other.SendMessage("PickUp", (ScriptablePowerUp)m_ScriptableObj, SendMessageOptions.RequireReceiver);

                Deactivate();
            }
        }
    }
}


