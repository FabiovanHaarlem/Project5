using UnityEngine;
using Game.Event;

public class Walking : ICharacterStates
{
    private CharacterControl m_CharacterController;
    private Animator m_Animator;
    private Transform m_Transform;

    private string[] m_Inputs;
    private float m_HorMoveSpeed;
    private float m_VerMoveSpeed;
    private float m_AttackCooldown;
    private float m_PlayerScale;
    private int m_AnimatorLayer;

    public Walking(CharacterControl characterController, ref float horMoveSpeed, ref float verMoveSpeed, int layer, Transform transform, Animator animator)
    {
        m_Inputs = new string[6];
        m_CharacterController = characterController;
        m_HorMoveSpeed = horMoveSpeed;
        m_VerMoveSpeed = verMoveSpeed;
        m_AnimatorLayer = layer;
        m_Transform = transform;
        m_PlayerScale = m_Transform.localScale.x;
        m_Animator = animator;
    }

    public void UpdateControls(string[] inputs)
    {
        m_Inputs = inputs;
    }

    public void InitializeState()
    {
        m_AttackCooldown = 0;
    }

    public void UpdateState()
    {
        Debug.Log("Walking State");
        if (Input.GetButtonDown(m_Inputs[0]))
        {
            ToFishing();
        }

        m_CharacterController.gameObject.transform.position += new Vector3(-Input.GetAxis(m_Inputs[4]), 0, Input.GetAxis(m_Inputs[5])) * Time.deltaTime;

        if (-Input.GetAxis(m_Inputs[4]) < -0.2f)
        {
            //m_Animator.SetInteger("State", 3);
            m_Animator.Play(3, m_AnimatorLayer);
            m_Transform.localScale = new Vector3(-m_PlayerScale, m_Transform.localScale.y, m_Transform.localScale.z);
        }
        else if (-Input.GetAxis(m_Inputs[4]) > 0.2f)
        {
            m_Animator.Play(4, m_AnimatorLayer);
            //m_Animator.SetInteger("State", 4);
            m_Transform.localScale = new Vector3(m_PlayerScale, m_Transform.localScale.y, m_Transform.localScale.z);
        }

        if (Input.GetAxis(m_Inputs[5]) < -0.2f)
        {
            m_Animator.Play(1, m_AnimatorLayer);
            //m_Animator.SetInteger("State", 1);
            m_Transform.localScale = new Vector3(m_PlayerScale, m_Transform.localScale.y, m_Transform.localScale.z);
        }
        else if (Input.GetAxis(m_Inputs[5]) > 0.2f)
        {
            m_Animator.Play(2, m_AnimatorLayer);
            //m_Animator.SetInteger("State", 2);
            m_Transform.localScale = new Vector3(m_PlayerScale, m_Transform.localScale.y, m_Transform.localScale.z);
        }
        else
        {
            m_Animator.Play(5, m_AnimatorLayer);
            //m_Animator.SetInteger("State", 5);
            m_Transform.localScale = new Vector3(m_PlayerScale, m_Transform.localScale.y, m_Transform.localScale.z);
        }
    }

    public void ToFishing()
    {
        m_CharacterController.SwitchToFishingState();
    }

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown(m_Inputs[1]) && m_AttackCooldown >= 2)
        {
        //    Vector3 explosionPos = new Vector3 = m_CharacterController.transform.position;
        //    Collider[] hitObjects = Physics.OverlapSphere(explosionPos, 2f);

        //    foreach (Collider hit in hitObjects)
        //    {
        //        if (other.CompareTag("Player"))
        //        {
        //            Rigidbody hitRigidbody = hit.gameObject.GetComponent<Rigidbody>();

        //            hitRigidbody.AddExplosionForce(2f, explosionPos, 1f);
        //        }
        //    }
        //}

        if (other.CompareTag("Player"))
            {
                other.gameObject.SendMessage("HitByAttack");
                
            }
        }
    }

    public void AddPowerUp(PowerUp Power)
    {
        m_CharacterController.M_AddPowerup.Invoke(Power);
    }
}
