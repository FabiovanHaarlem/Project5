using System.Collections.Generic;
using UnityEngine;
using System;
using Plugins.ObjectPool;
using Game.Character.Ai;
using Game.Character.player.Powerups;
using Game;
using Game.Character.Pickup;

public class CharacterControl : PoolObject
{
    [SerializeField]
    private GameObject m_SelectedSprite;
    [SerializeField]
    private LineRenderer m_FishingLine;

    private Walking m_WalkingState;
    private Fishing m_FishingState;
    private CarryingFish m_CarryingFishState;
    private Rigidbody m_Rigidbody;

    private ICharacterStates m_CurrentState;

    [SerializeField]
    private float m_HorMoveSpeed;
    [SerializeField]
    private float m_VerMoveSpeed;

    private string[] m_Inputs;

    public Action<IFish> M_Catched { get; set; }
    public Action<PowerUp> M_AddPowerup { get; set; }

    private byte m_PlayerID;
    public byte SetPlayerID { set { m_PlayerID = value; } }


    public void ModifyControls(string toFishingButton, string attackButton, string switchFishLeftButton, string switchFishRightButton, string horizontalAxis, string verticalAxis)
    {
        //Pass all axis in array
        m_Inputs[0] = toFishingButton;
        m_Inputs[1] = attackButton;
        m_Inputs[2] = switchFishLeftButton;
        m_Inputs[3] = switchFishRightButton;
        m_Inputs[4] = horizontalAxis;
        m_Inputs[5] = verticalAxis;


        m_WalkingState.UpdateControls(m_Inputs);
        m_FishingState.UpdateControls(m_Inputs);
        m_CarryingFishState.UpdateControls(m_Inputs);
    }

    public void GetSelectedPlayerSprites()
    {
        //Hier moeten alle sprites en animaties worden doorgegeven die deze speler moet gebruiken
    }

    private void Start()
    {
        if (m_Inputs == null)
        {
            m_Inputs = new string[6];
        }

        if (m_Inputs[0] == null)
        {
            m_Inputs[0] = "Controller1AButton";
            m_Inputs[1] = "Controller1XButton";
            m_Inputs[2] = "Controller1LeftBumper";
            m_Inputs[3] = "Controller1RightBumper";
            m_Inputs[4] = "Controller1JoystickHorizontal";
            m_Inputs[5] = "Controller1JoystickVertical";
        }

        m_WalkingState = new Walking(this, ref m_HorMoveSpeed, ref m_VerMoveSpeed);
        m_FishingState = new Fishing(this, m_SelectedSprite);
        m_CarryingFishState = new CarryingFish(this, ref m_HorMoveSpeed, ref m_VerMoveSpeed);
        m_CurrentState = m_WalkingState;

        m_WalkingState.UpdateControls(m_Inputs);
        m_FishingState.UpdateControls(m_Inputs);
        m_CarryingFishState.UpdateControls(m_Inputs);
    }

    internal override void Initialize(PoolObjectInfo Info)
    {
        SetMoveSpeed();
        m_Inputs = new string[6];

        m_WalkingState = new Walking(this, ref m_HorMoveSpeed, ref m_VerMoveSpeed);
        m_FishingState = new Fishing(this, m_SelectedSprite);
        m_CarryingFishState = new CarryingFish(this, ref m_HorMoveSpeed, ref m_VerMoveSpeed);
        m_CurrentState = m_WalkingState;

        m_Rigidbody = GetComponent<Rigidbody>();

        m_SelectedSprite = Pool.Singleton.Spawn(m_SelectedSprite).gameObject;
        GameObject fishingLine = Pool.Singleton.Spawn(m_FishingLine.gameObject).gameObject;
        m_FishingLine = fishingLine.GetComponent<LineRenderer>();

        m_SelectedSprite.SetActive(false);
        m_FishingLine.gameObject.SetActive(false);
        
        GameManager.Singelton.RegisterPlayer(this);
        base.Initialize(Info);
    }

    private void SetMoveSpeed()
    {
        if (m_HorMoveSpeed == 0f)
        {
            m_HorMoveSpeed = 2f;
        }

        if (m_VerMoveSpeed == 0f)
        {
            m_VerMoveSpeed = 1f;
        }
    }

    public void ShowCurrentSelectedfish(GameObject currentSelectedFish)
    {
        m_SelectedSprite.transform.position = new Vector3(currentSelectedFish.transform.position.x, currentSelectedFish.transform.position.y + 1f, currentSelectedFish.transform.position.z);
    }


    void Update ()
    {
        m_CurrentState.UpdateState();
    }

    public void DropFish()
    {
        if (m_CurrentState == m_CarryingFishState)
        {
            m_CarryingFishState.DropFish();
        }
    }

    public void SwitchToWalkingState()
    {
        m_CurrentState = m_WalkingState;
        m_WalkingState.InitializeState();
        m_SelectedSprite.SetActive(false);
    }

    public void SwitchToFishingState()
    {
        m_CurrentState = m_FishingState;
        m_FishingState.InitializeState();
        m_FishingState.SetCurrentSelecetedFish();
        m_SelectedSprite.SetActive(true);
    }

    public void SwitchToCarryingState(List<IFish> caughtFish)
    {
        m_CurrentState = m_CarryingFishState;
        m_CarryingFishState.InitializeState();
        m_CarryingFishState.GetCaughtFish(caughtFish);
        m_SelectedSprite.SetActive(false);
    }

    public void HitByAttack()
    {
        if (m_CurrentState == m_WalkingState)
        {

        }
        else if (m_CurrentState == m_FishingState)
        {
            List<IFish> caughtFish = new List<IFish>();
            caughtFish = m_FishingState.GetCaughtFish();

            if (caughtFish.Count >= 1)
            {
                m_CarryingFishState.DropFish();

                if (caughtFish.Count >= 1)
                {
                    SwitchToCarryingState(caughtFish);
                }
            }
            else
            {
                SwitchToWalkingState();
            }
        }
        else if (m_CurrentState == m_CarryingFishState)
        {
            m_CarryingFishState.DropFish();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        m_CurrentState.OnTriggerStay(other);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (m_CurrentState == m_FishingState)
        {
            m_FishingState.OnTriggerEnter(other);
        }

        if (other.CompareTag("Scorepoint"))
        {
            if(other.gameObject.GetComponent<ScorePoint>().m_PlayerID == m_PlayerID)
            {
                if (m_CurrentState == m_CarryingFishState)
                {
                    m_CarryingFishState.DropFishInScorepoint();
                }
            }

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (m_CurrentState == m_FishingState)
        {
            m_FishingState.OnTriggerExit(other);
        }
    }

    private void PickUp(ScriptablePowerUp power)
    {
        PowerUp powerup = new PowerUp(power.stats, new RemovePowerupEffectDelegate(AddRemovePowerup), power.m_Image);

        M_AddPowerup.Invoke(powerup);
    }

    public void ActivateFishingLine(GameObject selectedFish)
    {
        m_FishingLine.gameObject.SetActive(true);
        m_FishingLine.SetPosition(0, transform.position);
        m_FishingLine.SetPosition(1, selectedFish.transform.position);
    }

    public void DeactivateFishingLine()
    {
        m_FishingLine.gameObject.SetActive(false);
    }

    public void UpdateFishingLine(GameObject fishCoughtStartPosition)
    {
        m_FishingLine.SetPosition(0, transform.position);
        m_FishingLine.SetPosition(1, fishCoughtStartPosition.transform.position);
    }

    private void AddRemovePowerup(PowerupStats stats)
    {
        m_HorMoveSpeed *= stats.m_AddSpeed;
        m_VerMoveSpeed *= stats.m_AddSpeed;
    }
}
