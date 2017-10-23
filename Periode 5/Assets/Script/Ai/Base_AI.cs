using UnityEngine;
using UnityEngine.AI;
using Plugins.ObjectPool;

namespace Game.Character.Ai
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Base_AI : PoolObject {

        protected NavMeshAgent m_Agent;

        private float m_UpdateTimer;

        protected float m_speed;

        protected virtual void Awake()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_UpdateTimer = (float)Random.Range(0, 100) / 100;
            m_speed = m_Agent.speed;
        }

        protected virtual void FixedUpdate()
        {
            m_UpdateTimer += Time.deltaTime;
            if (m_UpdateTimer >= 1)
                AiUpdater();
        }

        protected internal override void Activate()
        {
            base.Activate();
            m_Agent.SetDestination(CreateWanderTarget(50, transform.position));
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            m_Agent.SetDestination(transform.position);
        }

        protected virtual void AiUpdater()
        {
            if (m_Agent.remainingDistance < m_Agent.stoppingDistance)
                 m_Agent.SetDestination(CreateWanderTarget(50,transform.position));
            m_UpdateTimer = 0;
        }

        protected virtual Vector3 CreateWanderTarget(float Radius, Vector3 Pivot )
        {
            Vector3 newtarget = new Vector3(999,999,999);
            NavMeshPath newpath = new NavMeshPath();

            int Retrys = 0;

            while (!m_Agent.CalculatePath(newtarget, newpath) && Retrys < 10)
            {
                newtarget = Pivot + new Vector3(Random.insideUnitSphere.x * Radius, transform.position.y , Random.insideUnitSphere.z * Radius);
                Retrys++;
            }

            return newtarget;
        }

    }
}

