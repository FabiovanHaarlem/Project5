using UnityEngine;
using UnityEngine.AI;
using Plugins.ObjectPool;
using Game.Character.player;

namespace Game.Character.Ai
{
    public class Fish : Base_AI , IFish{

        private enum State
        {
            Swimming,
            beingcatched,
            catched
        }

        private byte m_State;

        private SpriteRenderer m_Image;

        public string M_Name { get; private set; }

        public GameObject GetGameObject { get { return gameObject; } }

        internal override void Initialize(PoolObjectInfo Info)
        {
            transform.GetChild(0).transform.localPosition += ((Vector3.down * Random.Range(1, 10)) / 4);
            base.Initialize(Info);
            M_Name = name.Split('(')[0];
            m_Agent.updateRotation = false;
            m_Image = GetComponentInChildren<SpriteRenderer>();
        }

        protected internal override void Activate()
        {
            base.Activate();
            m_State = (byte)State.Swimming;
            if (GameManager.Singelton.SpawnLimit > 0)
                GameManager.Singelton.SpawnLimit -= 1;
            else Deactivate();
        }

        protected override void AiUpdater()
        {
            transform.LookAt(Camera.main.transform);

            if(CheckDistancePlayer())
            {
                byte retry = 0;
                while(Vector3.Distance(m_Agent.destination, transform.position) < 3 && retry < 5)
                {
                    m_Agent.SetDestination(CreateWanderTarget(10, transform.position));
                    retry++;
                }

            }

            if(m_Agent.velocity.x > 0)
            {
                m_Image.flipX = false;
            }
            else
            {
                m_Image.flipX = true;
            }


            if (m_State == (byte)State.Swimming)
                base.AiUpdater();

        }

        public void Atteract(Vector3 destination)
        {
            m_Agent.destination = destination;
        }

        public void Catched()
        {
            m_State = (byte)State.catched;
            Deactivate();
        }
    
        public void BeingCatched()
        {
            m_State = (byte)State.beingcatched;
            m_Agent.speed = 0;
        }

        public void Escaped()
        {
            m_State = (byte)State.Swimming;
            m_Agent.speed = m_speed;
            byte retry = 0;
            while (Vector3.Distance(m_Agent.destination, transform.position) < 3 && retry < 5)
            {
                m_Agent.SetDestination(CreateWanderTarget(10, transform.position));
                retry++;
            }
        }

        protected override Vector3 CreateWanderTarget(float Radius, Vector3 Pivot)
        {
            Vector3 newtarget = new Vector3(999, 999, 999);
            NavMeshPath newpath = new NavMeshPath();

            NavMeshHit Hit = new NavMeshHit();

            int Retrys = 0;
            while (!m_Agent.CalculatePath(newtarget, newpath) && Retrys < 10) 
            {
                newtarget = Pivot + new Vector3(Random.insideUnitSphere.x * Radius, transform.position.y, Random.insideUnitSphere.z * Radius);
                NavMesh.FindClosestEdge(newtarget, out Hit, 0);
                Retrys++;
            }

            return newtarget;
        }

        private bool CheckDistancePlayer()
        {

            foreach (GameObject i in GameManager.Singelton.M_Players)
            {
                CharacterControl controller = i.GetComponent<CharacterControl>();
                if(controller != null)
                {
                    Transform player = controller.transform;

                    if (Vector3.Distance(transform.position, player.position) < 2)
                        return false;
                }
            }

            return true;
        }
    }
}


