using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectPool;

public class CharacterControl : PoolObject
{
<<<<<<< HEAD
<<<<<<< HEAD
    public class CharacterControl : PoolObject
    {
        private Walking m_WalkingState;
        private Fishing m_FishingState;
        private CarryingFish m_CarryingFishState;
        private ICharacterStates m_CurrentState;
        [SerializeField]
        private GameObject m_SelectedSprite;
=======
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
    [SerializeField]
    private GameObject m_SelectedSprite;
    [SerializeField]
    private LineRenderer m_FishingLine;

    private Walking m_WalkingState;
    private Fishing m_FishingState;
    private CarryingFish m_CarryingFishState;
    private Rigidbody m_Rigidbody;

    private ICharacterStates m_CurrentState;
<<<<<<< HEAD
>>>>>>> Fabio
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f

    [SerializeField]
    private float m_HorMoveSpeed;
    [SerializeField]
    private float m_VerMoveSpeed;

<<<<<<< HEAD
<<<<<<< HEAD
        [SerializeField]
        private float m_HorMoveSpeed;
        [SerializeField]
        private float m_VerMoveSpeed;
=======
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
    private string[] m_Inputs;

    public Action<IFish> M_Catched { get; set; }
    public Action<PowerUp> M_AddPowerup { get; set; }
<<<<<<< HEAD
>>>>>>> Fabio
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f

    private byte m_PlayerID;
    public byte SetPlayerID { set { m_PlayerID = value; } }

<<<<<<< HEAD
<<<<<<< HEAD
        private byte m_PlayerID;
        public void SetPlayerID(byte input) { m_PlayerID = input; }

        public void ModifyControls(KeyCode upKey, KeyCode downKey, KeyCode leftKey, KeyCode rightKey, KeyCode toFishingKey, KeyCode attackKey)
        {
            m_KeyCodes = new KeyCode[6];
            m_KeyCodes[0] = upKey;
            m_KeyCodes[1] = downKey;
            m_KeyCodes[2] = leftKey;
            m_KeyCodes[3] = rightKey;
            m_KeyCodes[4] = toFishingKey;
            m_KeyCodes[5] = attackKey;

            m_WalkingState.UpdateControls(m_KeyCodes);
            m_FishingState.UpdateControls(m_KeyCodes);
            m_CarryingFishState.UpdateControls(m_KeyCodes);

        }
=======
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f

    public void ModifyControls(string toFishingButton, string attackButton, string switchFishLeftButton, string switchFishRightButton, string horizontalAxis, string verticalAxis)
    {
        //Pass all axis in array
        m_Inputs[0] = toFishingButton;
        m_Inputs[1] = attackButton;
        m_Inputs[2] = switchFishLeftButton;
        m_Inputs[3] = switchFishRightButton;
        m_Inputs[4] = horizontalAxis;
        m_Inputs[5] = verticalAxis;

        if (m_Inputs[0] == null)
        {
            m_Inputs[0] = "Controller1AButton";
            m_Inputs[1] = "Controller1XButton";
            m_Inputs[2] = "Controller1LeftBumper";
            m_Inputs[3] = "Controller1RightBumper";
            m_Inputs[4] = "Controller1JoystickHorizontal";
            m_Inputs[5] = "Controller1JoystickVertical";
        }

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
        m_Inputs = new string[6];
        m_Inputs[0] = "Controller1AButton";
        m_Inputs[1] = "Controller1XButton";
        m_Inputs[2] = "Controller1LeftBumper";
        m_Inputs[3] = "Controller1RightBumper";
        m_Inputs[4] = "Controller1JoystickHorizontal";
        m_Inputs[5] = "Controller1JoystickVertical";

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
        m_SelectedSprite = Pool.Singleton.Spawn(m_FishingLine.gameObject).gameObject;

        m_SelectedSprite.SetActive(false);
        m_FishingLine.gameObject.SetActive(false);
        
        GameManager.Singelton.RegisterPlayer(this);
        base.Initialize(Info);
    }
<<<<<<< HEAD
>>>>>>> Fabio
=======

    private void SetMoveSpeed()
    {
        if (m_HorMoveSpeed == 0f)
        {
            m_HorMoveSpeed = 2f;
        }
>>>>>>> f260879072728bc89455d28b421450bc13595c3f

        if (m_VerMoveSpeed == 0f)
        {
            m_VerMoveSpeed = 1f;
        }
    }

<<<<<<< HEAD
<<<<<<< HEAD
        void Update ()
        {
            m_CurrentState.UpdateState();
	    }
=======
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
    public void ShowCurrentSelectedfish(GameObject currentSelectedFish)
    {
        m_SelectedSprite.transform.position = new Vector3(currentSelectedFish.transform.position.x, currentSelectedFish.transform.position.y + 1f, currentSelectedFish.transform.position.z);
    }


    void Update ()
    {
        m_CurrentState.UpdateState();
    }
<<<<<<< HEAD
>>>>>>> Fabio
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f

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

<<<<<<< HEAD
<<<<<<< HEAD
        public void SwitchToCarryingState(List<IFish> caughtFish)
        {
            m_CurrentState = m_CarryingFishState;
            m_CarryingFishState.InitializeState();
            m_CarryingFishState.GetCaughtFish(caughtFish);
            m_SelectedSprite.SetActive(false);
        }
=======
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
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
<<<<<<< HEAD
>>>>>>> Fabio
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f

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

<<<<<<< HEAD
<<<<<<< HEAD
        private void AddRemovePowerup(PowerupStats stats)
        {
            m_HorMoveSpeed *= stats.m_AddSpeed;
            m_VerMoveSpeed *= stats.m_AddSpeed;
        }
=======
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
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
<<<<<<< HEAD
>>>>>>> Fabio
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
    }
}
