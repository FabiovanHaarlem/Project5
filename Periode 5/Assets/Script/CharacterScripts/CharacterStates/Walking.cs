using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Character.player.Powerups;

public class Walking : ICharacterStates
{
    private CharacterControl m_CharacterController;

    private string[] m_Inputs;
    private float m_HorMoveSpeed;
    private float m_VerMoveSpeed;
    private float m_AttackCooldown;

    public Walking(CharacterControl characterController, ref float horMoveSpeed, ref float verMoveSpeed)
    {
        m_Inputs = new string[6];
        m_CharacterController = characterController;
        m_HorMoveSpeed = horMoveSpeed;
        m_VerMoveSpeed = verMoveSpeed;

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
