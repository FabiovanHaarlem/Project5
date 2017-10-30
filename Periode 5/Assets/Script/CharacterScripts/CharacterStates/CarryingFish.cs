using System.Collections.Generic;
using UnityEngine;
using Game.Character.Ai;
using Game.Event;

public class CarryingFish : ICharacterStates
{
    private List<IFish> m_CaughtFish;
    private Animator m_Animator;
    private Transform m_Transform;

    private string[] m_Inputs;

    private CharacterControl m_CharacterControl;

    private float m_HorMoveSpeed;
    private float m_VerMoveSpeed;
    private float m_PlayerScale;
    private int m_AnimatorLayer;


    public CarryingFish(CharacterControl characterController, ref float horMoveSpeed, ref float verMoveSpeed, int layer, Transform transform, Animator animator)
    {
        m_Inputs = new string[6];
        m_CaughtFish = new List<IFish>();
        m_CharacterControl = characterController;
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
    }

    public void GetCaughtFish(List<IFish> caughtFish)
    {
        if (caughtFish.Count == 0)
        {
            m_CharacterControl.SwitchToWalkingState();
        }
        else
        {
            for (int i = 0; i < caughtFish.Count; i++)
            {
                m_CaughtFish.Add(caughtFish[i]);
            }
        }
    }

    public void UpdateState()
    {
        Debug.Log("Carry Fish State");
        //m_HorMoveSpeed = m_HorMoveSpeed * (1f / m_CaughtFish.Count);
        //m_VerMoveSpeed = m_VerMoveSpeed * (1f / m_CaughtFish.Count);

        if (m_CaughtFish.Count <= 0)
        {
            ToWalking();
        }

        m_CharacterControl.gameObject.transform.position += new Vector3(-Input.GetAxis(m_Inputs[4]), 0, Input.GetAxis(m_Inputs[5])) * Time.deltaTime;

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

    public void DropFish()
    {
        m_CaughtFish.RemoveAt(0);
    }

    public void  DropFishInScorepoint()
    {
        foreach (IFish i in m_CaughtFish)
        {
            m_CharacterControl.M_Catched.Invoke(i);
            Debug.Log("Yeahhh");
        }
    }

    public void ToWalking()
    {
        m_CharacterControl.SwitchToWalkingState();
    }

    public void ToFishing()
    {
        m_CharacterControl.SwitchToFishingState();
    }

    public void OnTriggerStay(Collider other)
    {

    }

    public void AddPowerUp(PowerUp Power)
    {
        m_CharacterControl.M_AddPowerup.Invoke(Power);
    }
}
