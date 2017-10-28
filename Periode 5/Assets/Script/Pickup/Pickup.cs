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

        private void Start()
        {
            GetComponent<Collider>().isTrigger = true;
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


