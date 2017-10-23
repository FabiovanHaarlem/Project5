using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : ICharacterStates
{
<<<<<<< HEAD
<<<<<<< HEAD
    public class Walking : ICharacterStates
    {
        private CharacterControl M_CharacterController { get; }

        private KeyCode[] m_KeyCodes;

        private float M_HorMoveSpeed { get; }
        private float M_VerMoveSpeed { get; }
        private float M_AttackCooldown { get; }

        public Walking(CharacterControl characterController, ref float horMoveSpeed, ref float verMoveSpeed)
=======
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
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
<<<<<<< HEAD
>>>>>>> Fabio
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
        {
            ToFishing();
        }

<<<<<<< HEAD
<<<<<<< HEAD
        public void UpdateControls(KeyCode[] keyCodes)
        {
            m_KeyCodes = keyCodes;
        }

        public void InitializeState()
        {

        }

        public void UpdateState()
        {
            Debug.Log("Walking");
            if (Input.GetKeyDown(m_KeyCodes[4]))
            {
                ToFishing();
            }

            Movement();
        }

        private void Movement()
        {
            Vector3 currentPosition = M_CharacterController.gameObject.transform.position;

            if (Input.GetKey(m_KeyCodes[1]))
            {
              M_CharacterController.gameObject.transform.position = new Vector3(M_CharacterController.gameObject.transform.position.x,
                M_CharacterController.gameObject.transform.position.y, M_CharacterController.gameObject.transform.position.z + M_VerMoveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(m_KeyCodes[0]))
            {
                M_CharacterController.gameObject.transform.position = new Vector3(M_CharacterController.gameObject.transform.position.x,
                    M_CharacterController.gameObject.transform.position.y, M_CharacterController.gameObject.transform.position.z - M_VerMoveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(m_KeyCodes[3]))
            {
                M_CharacterController.gameObject.transform.position = new Vector3(M_CharacterController.gameObject.transform.position.x - M_HorMoveSpeed * Time.deltaTime,
                    M_CharacterController.gameObject.transform.position.y, M_CharacterController.gameObject.transform.position.z);
            }
            if (Input.GetKey(m_KeyCodes[2]))
            {
                M_CharacterController.gameObject.transform.position = new Vector3(M_CharacterController.gameObject.transform.position.x + M_HorMoveSpeed * Time.deltaTime,
                    M_CharacterController.gameObject.transform.position.y, M_CharacterController.gameObject.transform.position.z);
            }
        }
=======
        m_CharacterController.gameObject.transform.position += new Vector3(Input.GetAxis(m_Inputs[4]), 0, -Input.GetAxis(m_Inputs[5])) * Time.deltaTime;
    }
>>>>>>> Fabio
=======
        m_CharacterController.gameObject.transform.position += new Vector3(Input.GetAxis(m_Inputs[4]), 0, -Input.GetAxis(m_Inputs[5])) * Time.deltaTime;
    }
>>>>>>> f260879072728bc89455d28b421450bc13595c3f

    public void ToFishing()
    {
        m_CharacterController.SwitchToFishingState();
    }

<<<<<<< HEAD
<<<<<<< HEAD
        public void OnTriggerStay(Collider collider)
        {
            if (Input.GetKey(m_KeyCodes[5])
                && M_AttackCooldown >= 2
                && collider.CompareTag("Player")
                && collider.gameObject != null)
            {
                /// fabio moet slaan en stoppen met vissen vangen
                collider.gameObject.GetComponent<CharacterControl>().DropFish();
=======
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
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
                
<<<<<<< HEAD
>>>>>>> Fabio
=======
>>>>>>> f260879072728bc89455d28b421450bc13595c3f
            }
        }
    }

    public void AddPowerUp(PowerUp Power)
    {
        m_CharacterController.M_AddPowerup.Invoke(Power);
    }
}
