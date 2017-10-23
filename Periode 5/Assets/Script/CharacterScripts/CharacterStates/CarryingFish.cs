using System.Collections.Generic;
using UnityEngine;
using Game.Character.Ai;
using Game.Character.player.Powerups;

public class CarryingFish : ICharacterStates
{
    private List<IFish> m_CaughtFish;

    private string[] m_Inputs;

    private CharacterControl m_CharacterControl;

    private float m_HorMoveSpeed;
    private float m_VerMoveSpeed;


    public CarryingFish(CharacterControl characterController, ref float horMoveSpeed, ref float verMoveSpeed)
    {
        m_Inputs = new string[6];
        m_CaughtFish = new List<IFish>();
        m_CharacterControl = characterController;
        m_HorMoveSpeed = horMoveSpeed;
        m_VerMoveSpeed = verMoveSpeed;
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
