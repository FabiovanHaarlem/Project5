using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Character.Ai;
using Game.Event;
using Sjabloon;

public class Fishing : ICharacterStates
{
    private IFish m_CurrentSelectedFish;
    private List<IFish> m_FishInArea;
    private List<IFish> m_CaughtFish;
    public Action<IFish> m_Catched;

    private Animator m_Animator;
    private Transform m_Transform;

    private GameObject m_SelectedFishSprite;
    private CharacterControl m_CharacterControl;

    private string[] m_Inputs;
    private int m_SelectedFishIndex;
    private float m_CatchMeter;
    private bool m_Catching;
    private int m_AnimatorLayer;


    public Fishing(CharacterControl characterController, GameObject selectedSprite, int layer, Transform transform, Animator animator)
    {
        m_Inputs = new string[6];
        m_CharacterControl = characterController;
        m_FishInArea = new List<IFish>();
        m_CaughtFish = new List<IFish>();
        m_SelectedFishSprite = selectedSprite;
        m_Catching = false;
        m_AnimatorLayer = layer;
        m_Transform = transform;
        m_Animator = animator;
    }

    public void UpdateControls(string[] inputs)
    {
        m_Inputs = inputs;
    }

    public void InitializeState()
    {
        m_CurrentSelectedFish = null;
        m_FishInArea.Clear();
        m_CaughtFish.Clear();
        GetAllFishInArea();
        m_SelectedFishIndex = 0;
        m_CatchMeter = 0;
    }


    public void UpdateState()
    {
        SwitchSelectedFish();

        if (m_CurrentSelectedFish != null)
        m_CharacterControl.ShowCurrentSelectedfish(m_CurrentSelectedFish.GetGameObject);

        if (m_CurrentSelectedFish != null)
        {
            if (InputManager.Instance.GetButton(m_Inputs[1]) && (m_Catching))
            {

                m_CharacterControl.UpdateFishingLine(m_CurrentSelectedFish.GetGameObject);
                m_CatchMeter += 20;



                if (m_CatchMeter >= 100)
                {
                    CatchFish();
                }
            }
            else if (InputManager.Instance.GetButton(m_Inputs[1]) && (!m_Catching))
            {
                m_CharacterControl.ActivateFishingLine(m_CurrentSelectedFish.GetGameObject);
                m_CurrentSelectedFish.BeingCatched();
                m_Catching = true;
            }

            if (InputManager.Instance.GetButton(m_Inputs[2]) || InputManager.Instance.GetButton(m_Inputs[3]))
            {
                m_CharacterControl.DeactivateFishingLine();

                if (m_Catching)
                {
                    m_CurrentSelectedFish.Escaped();
                    m_CatchMeter = 0;
                    m_CharacterControl.DeactivateFishingLine();
                }
            }
        }


        if (InputManager.Instance.GetButton(m_Inputs[0]))
        {
            m_Catching = false;
            if (m_CaughtFish.Count >= 1)
            {
                ToCarrying();
            }
            else
            {
                ToWalking();
            }
        }
    }

    public List<IFish> GetCaughtFish()
    {
        return m_CaughtFish;
    }

    private void SwitchSelectedFish()
    {
        if (InputManager.Instance.GetButton(m_Inputs[2]))
        {
            if (m_SelectedFishIndex == 0)
            {
                m_SelectedFishIndex = m_FishInArea.Count - 1;
            }
            else if (m_SelectedFishIndex >= 1)
            {
                m_SelectedFishIndex -= 1;
            }

            m_CatchMeter = 0;
        }

        if (InputManager.Instance.GetButton(m_Inputs[3]))
        {
            if (m_SelectedFishIndex == m_FishInArea.Count)
            {
                m_SelectedFishIndex = 0;
            }
            else if (m_SelectedFishIndex <= 1)
            {
                m_SelectedFishIndex += 1;
            }

            m_CatchMeter = 0;
        }

        if(m_FishInArea.Count >= 1)
            m_CurrentSelectedFish = m_FishInArea[m_SelectedFishIndex];
    }

    private void CatchFish()
    {
        if (m_FishInArea.Count > 0)
        {
            m_CaughtFish.Add(m_FishInArea[m_SelectedFishIndex]);
            m_CaughtFish[m_CaughtFish.Count - 1].BeingCatched();

            m_FishInArea.RemoveAt(m_SelectedFishIndex);
            m_SelectedFishIndex = 0;

            m_CurrentSelectedFish.Catched();
            //m_CaughtFish[m_CaughtFish.Count - 1].Catched();

            m_CharacterControl.DeactivateFishingLine();
            m_Catching = false;
            m_CatchMeter = 0;

            if (m_CaughtFish.Count == 3)
            {
                ToCarrying();
            }
        }
    }

    public void ToWalking()
    {
        m_CharacterControl.SwitchToWalkingState();
    }

    public void ToCarrying()
    {
        m_CharacterControl.SwitchToCarryingState(m_CaughtFish);
    }

    public void OnTriggerStay(Collider collider)
    {

    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == 8)
        {
            GetAllFishInArea();
            m_CharacterControl.DeactivateFishingLine();
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 8)
        {
            GetAllFishInArea();
        }
    }

    public void SetCurrentSelecetedFish()
    {
        if (m_FishInArea.Count >= 1)
        {
            m_CurrentSelectedFish = m_FishInArea[0];
        }
    }

    private void GetAllFishInArea()
    {
        m_FishInArea.Clear();
        if (m_CurrentSelectedFish != null)
        {
            m_FishInArea.Add(m_CurrentSelectedFish);
        }

        Collider[] hitColliders = Physics.OverlapSphere(m_CharacterControl.gameObject.transform.position, 2.5f);

        if (hitColliders.Length <= 0)
        {
            ToWalking();
        }
        else
        {
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject.layer == 8)
                {
                    m_FishInArea.Add(col.gameObject.GetComponent<IFish>());
                }
            }
        }
    }

    public void AddPowerUp(PowerUp Power)
    {
        m_CharacterControl.M_AddPowerup.Invoke(Power);
    }
}
