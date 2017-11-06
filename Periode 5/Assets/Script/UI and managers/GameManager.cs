using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Plugins.ObjectPool;
using Game.UI;
using Game.Character.Pickup;
using Game.Scriptable;
using Game.Event;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour {

        [SerializeField]
        private Transform m_UI;

        [SerializeField]
        [Tooltip("prefab of the UI fields to show playerinfo")]
        private GameObject m_PlayerstatsPrefab;

        [SerializeField]
        [Tooltip("player prefab to spawn into the game")]
        private GameObject m_playerPrefab;
        public GameObject GetPlayerPrefab
        {
            get
            {
                return m_playerPrefab;
            }
        }

        [SerializeField][Tooltip("Pickup Prefab")]
        private GameObject m_Pickup;
        [SerializeField] [Tooltip("Possible Powerups")]
        private ScriptablePowerUp[] m_PossiblePowerups;

        [SerializeField] [Tooltip("Points where people can empty there fishing nets")]
        private GameObject[] m_ScorePoints;
        public GameObject[] GetScorePoints
        {
            get
            {
                return m_ScorePoints;
            }
        }

        [SerializeField]
        [Tooltip("Boards where cought fish will display")]
        private Text[] m_ScoreBoards;

        internal List<GameObject> M_Players { get; private set; }
        private List<PlayerScore> m_Scores;

        //used to refrence how many fish can spawn
        [SerializeField]
        internal int SpawnLimit = 25;

        [SerializeField] [Tooltip("score that will be required to win")]
        private int m_WinRequiredScore;
        [SerializeField] [Tooltip("Object to spawn when a player wins")]
        private GameObject m_WinScreen;


        [SerializeField]
        [Tooltip("Background music that will play for the duration of the game")]
        private AudioClip m_BackgroundMusic;
        private AudioSource m_AudioSource;

        private List<WorldEvent> m_ActiveWorldEvents;

        [SerializeField]
        private GameObject m_SpinningWheel;
        private SpinningWheel m_SpinningWheelScript;
        public SpinningWheel SpinningWheel
        {
            get
            {
                return m_SpinningWheelScript;
            }
        }

        [SerializeField]
        private GameObject m_LevelCenter;
    
        //a refrence of the game manager to call from other classes
        public static GameManager Singelton { get; private set; }

        //function will make singleton, and will make new lists
        protected void Awake()
        {
            if (Singelton == null)
                Singelton = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            M_Players = new List<GameObject>();
            m_Scores = new List<PlayerScore>();

            foreach (Text i in m_ScoreBoards)
                i.gameObject.transform.parent.gameObject.SetActive(false);

            foreach (GameObject i in m_ScorePoints)
                i.SetActive(false);

            m_ActiveWorldEvents = new List<WorldEvent>();
        }

        private void Start()
        {
            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_AudioSource.clip = m_BackgroundMusic;
            m_AudioSource.loop = true;
            m_AudioSource.Play();

            Pool.Singleton.LoadExtraItems(
                new Poolobj()
                {
                    m_Amount = 1,
                    m_Prefab = m_WinScreen
                });

            Pool.Singleton.LoadExtraItems(new Poolobj()
            {
                m_Amount = 15,
                m_Prefab = m_Pickup
            });

            m_SpinningWheelScript = m_SpinningWheel.GetComponent<SpinningWheel>();

            m_SpinningWheel.SetActive(false);
        }

        //function to call when creating a new player
        public void RegisterPlayer(CharacterControl Player)
        {

            //making a new score system for the player
            m_Scores.Add(new PlayerScore(Player));
            PlayerScore score = m_Scores[m_Scores.Count -1];
            m_ScorePoints[m_Scores.Count - 1].SetActive(true);
            m_ScorePoints[m_Scores.Count - 1].AddComponent<ScorePoint>();
            m_ScoreBoards[m_Scores.Count - 1].gameObject.transform.parent.gameObject.SetActive(true);
            M_Players.Add(Player.gameObject);

            //spawning the UI element
            PoolObject playerstatspool = Pool.Singleton.Spawn(m_PlayerstatsPrefab, m_UI);
            Pool.Singleton.CreatePoolBasedOnObject(m_ScorePoints[m_Scores.Count - 1].GetComponent<PlayerStats>());


            GameObject playerstats = playerstatspool.gameObject;
            RectTransform playerstatstransform = playerstats.GetComponent<RectTransform>();

            switch(M_Players.Count){
                case 1:
                    playerstatstransform.localPosition = new Vector3(6, -2, 0);
                    Player.ModifyControls("Controller1AButton", "Controller1XButton", "Controller1LeftBumper", "Controller1RightBumper", "Controller1Horizontal", "Controller1Vertical");
                    RuntimeAnimatorController animator0 = Resources.Load<RuntimeAnimatorController>("CharacterAnimations/Flint/FlintAnimator");
                    Sprite dobberSprite0 = Resources.Load<Sprite>("Sprites/Dobbers/FlintDobber");
                    m_ScorePoints[0].GetComponent<ScorePoint>().m_PlayerID = 1;
                    Player.SetAnimator(dobberSprite0, animator0);
                    break;

                case 2:
                    playerstatstransform.localPosition = new Vector3(-6, -2, 0);
                    Player.ModifyControls("Controller2AButton", "Controller2XButton", "Controller2LeftBumper", "Controller2RightBumper", "Controller2Horizontal", "Controller2Vertical");
                    RuntimeAnimatorController animator1 = Resources.Load<RuntimeAnimatorController>("CharacterAnimations/Gladianus/GladianusAnimator");
                    Sprite dobberSprite1 = Resources.Load<Sprite>("Sprites/Dobbers/GladianusDobber");
                    m_ScorePoints[1].GetComponent<ScorePoint>().m_PlayerID = 2;
                    Player.SetAnimator(dobberSprite1, animator1);
                    break;

                case 3:
                    playerstatstransform.localPosition = new Vector3(6, 2, 0);
                    Player.ModifyControls("Controller3AButton", "Controller3XButton", "Controller3LeftBumper", "Controller3RightBumper", "Controller3Horizontal", "Controller3Vertical");
                    RuntimeAnimatorController animator2 = Resources.Load<RuntimeAnimatorController>("CharacterAnimations/Hermes/HermesAnimator");
                    Sprite dobberSprite2 = Resources.Load<Sprite>("Sprites/Dobbers/HermesDobber");
                    m_ScorePoints[2].GetComponent<ScorePoint>().m_PlayerID = 3;
                    Player.SetAnimator(dobberSprite2, animator2);
                    break;

                case 4:
                    playerstatstransform.localPosition = new Vector3(-6, 2, 0);
                    Player.ModifyControls("Controller4AButton", "Controller4XButton", "Controller4LeftBumper", "Controller4RightBumper", "Controller4Horizontal", "Controller4Vertical");
                    RuntimeAnimatorController animator3 = Resources.Load<RuntimeAnimatorController>("CharacterAnimations/Ming/MingAnimator");
                    Sprite dobberSprite3 = Resources.Load<Sprite>("Sprites/Dobbers/MingDobber");
                    m_ScorePoints[3].GetComponent<ScorePoint>().m_PlayerID = 4;
                    Player.SetAnimator(dobberSprite3, animator3);
                    break;
            }

            //call the registerfunction in the UI, this will setup all fields inside of them
            PlayerStats stats = playerstats.GetComponent<PlayerStats>();
            stats.UpdateID((byte)M_Players.Count, score, m_WinRequiredScore, m_ScoreBoards[M_Players.Count - 1]);

            //update playerID in the character controller
            Player.PlayerID = (byte)M_Players.Count;
        }

        //function that will instantiate a new world event into the game
        public void RegisterWorldEvent(ScriptableWorldEvent NewEvent)
        {
            WorldEvent actual = NewEvent.CreateInstance(NewEvent.m_TimeActive, new RemoveWorldEventPoolDelegate(DestroyWorldEvent));
            m_ActiveWorldEvents.Add(actual);
            EditorUtility.SetDirty(NewEvent);

        }

        //function will destroy and remove a world event
        private void DestroyWorldEvent(WorldEvent EndEvent)
        {
            m_ActiveWorldEvents.Remove(EndEvent);
        }

        //function will spawn the winscreen with winner and button to go back to menu
        private void EndGame(PlayerScore player)
        {
            PoolObject screen = Pool.Singleton.Spawn(m_WinScreen, m_UI);
            screen.GetComponent<WinScherm>().Text = "Player " + player.M_PlayerController.PlayerID;
        }

        private void Update()
        {

            for (int i = 0; i < m_ActiveWorldEvents.Count; i++)
            {
                m_ActiveWorldEvents[i].Update();
            }

            for (int i = 0; i < m_Scores.Count; i++)
            {
                if (m_Scores[i].Score >= m_WinRequiredScore)
                    EndGame(m_Scores[i]);
            }

            //spawn powerups at a random interval
            if(Random.Range(0, 1000) > 999)
            {
                Vector3 postion = m_LevelCenter.transform.position + (Random.insideUnitSphere * Random.Range(-9, 9));
                postion.y = -5;

                GameObject pickupobj = Pool.Singleton.Spawn(m_Pickup, postion).gameObject;

                Pickup pickup = pickupobj.GetComponent<Pickup>();

                ScriptablePowerUp powerup = m_PossiblePowerups[Random.Range(0, Mathf.RoundToInt(m_PossiblePowerups.Length - 1))];

                pickup.SetPower = powerup;
                pickup.Image = powerup.m_Image;
            }

            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetSceneByName("Menu Test").buildIndex);
        }

    }
}

