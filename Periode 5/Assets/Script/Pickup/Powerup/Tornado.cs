using Game.Scriptable;
using UnityEngine;

namespace Game.Event
{
    [System.Serializable]
    public class Tornado : WorldEvent
    {
        [SerializeField]
        private Vector3 m_Center;

        [SerializeField]
        private float m_TornadoRadius;

        [SerializeField]
        private float m_TornadoForce;

        [SerializeField]
        private ParticleSystem m_Particles;

        [SerializeField]
        private string[] m_TagsToSinkIn = new string[0];

        internal Tornado(float m_TimeActive, RemoveWorldEventPoolDelegate callback) : base(m_TimeActive, callback)
        {
            if(m_Particles != null)
                m_Particles.Play();
        }

        public override void Update()
        {
            base.Update();

            Ray ray = new Ray(m_Center, m_Center + Vector3.forward);

            RaycastHit[] raycastHits = Physics.SphereCastAll(ray, m_TornadoRadius);

            foreach(RaycastHit i in raycastHits)
            {
                for (int j = 0; j < m_TagsToSinkIn.Length; j++)
                {
                    if (i.collider.gameObject.tag == m_TagsToSinkIn[j])
                    {
                        Vector3 pos = i.collider.gameObject.transform.position;
                        pos = pos - m_Center / m_TornadoForce;
                        i.collider.gameObject.transform.Translate(pos);
                    }
                }
            }
        }
    }
}
