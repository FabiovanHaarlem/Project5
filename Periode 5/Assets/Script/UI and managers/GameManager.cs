using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Plugins.ObjectPool;
using Game.UI;
using Game.Character.Pickup;
using Game.Scriptable;
using Game.Event;
using UnityEditor;

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

        [SerializeField][Tooltip("Points where people can empty there fishing nets")]
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

        [SerializeField][Tooltip("score that will be required to win")]
        private int m_WinRequiredScore;
        [SerializeField][Tooltip("Object to spawn when a player wins")]
        private GameObject m_WinScreen;

        private List<WorldEvent> m_ActiveWorldEvents;
    
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
            Pool.Singleton.LoadExtraItems(
                new Poolobj()
                {
                    m_Amount = 1,
                    m_Prefab = m_WinScreen
                });
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
                    Player.ModifyControls("Controller1AButton", "Controller1XButton", "Controller1LeftBumper", "Controller1RightBumper", "Controller1JoystickHorizontal", "Controller1JoystickVertical");
                    RuntimeAnimatorController animator0 = (RuntimeAnimatorController)Resources.Load("CharacterAnimations//Flint//FlintAnimator", typeof(RuntimeAnimatorController));
                    Sprite dobberSprite0 = (Sprite)Resources.Load("Sprites//Dobbers//FlintDobber");
                    Player.SetAnimator(animator0, dobberSprite0);
                    m_ScorePoints[0].GetComponent<ScorePoint>().m_PlayerID = 1;
                    break;

                case 2:
                    playerstatstransform.localPosition = new Vector3(-6, -2, 0);
                    Player.ModifyControls("Controller2AButton", "Controller2XButton", "Controller2LeftBumper", "Controller2RightBumper", "Controller2JoystickHorizontal", "Controller2JoystickVertical");
                    RuntimeAnimatorController animator1 = (RuntimeAnimatorController)Resources.Load("CharacterAnimations//Gladianus//GladianusAnimator", typeof(RuntimeAnimatorController));
                    Sprite dobberSprite1 = (Sprite)Resources.Load("Sprites//Dobbers//GladianusDobber");
                    Player.SetAnimator(animator1, dobberSprite1);
                    m_ScorePoints[1].GetComponent<ScorePoint>().m_PlayerID = 2;
                    break;

                case 3:
                    playerstatstransform.localPosition = new Vector3(6, 2, 0);
                    Player.ModifyControls("Controller3AButton", "Controller3XButton", "Controller3LeftBumper", "Controller3RightBumper", "Controller3JoystickHorizontal", "Controller3JoystickVertical");
                    RuntimeAnimatorController animator2 = (RuntimeAnimatorController)Resources.Load("CharacterAnimations//Hermes//HermesAnimator", typeof(RuntimeAnimatorController));
                    Sprite dobberSprite2 = (Sprite)Resources.Load("Sprites//Dobbers//HermesDobber");
                    Player.SetAnimator(animator2, dobberSprite2);
                    m_ScorePoints[2].GetComponent<ScorePoint>().m_PlayerID = 3;
                    break;

                case 4:
                    playerstatstransform.localPosition = new Vector3(-6, 2, 0);
                    Player.ModifyControls("Controller4AButton", "Controller4XButton", "Controller4LeftBumper", "Controller4RightBumper", "Controller4JoystickHorizontal", "Controller4JoystickVertical");
                    RuntimeAnimatorController animator3 = (RuntimeAnimatorController)Resources.Load("CharacterAnimations//Ming//MingAnimator", typeof(RuntimeAnimatorController));
                    Sprite dobberSprite3 = (Sprite)Resources.Load("Sprites//Dobbers//MingDobber");
                    Player.SetAnimator(animator3, dobberSprite3);
                    m_ScorePoints[3].GetComponent<ScorePoint>().m_PlayerID = 4;
                    break;
            }

            //call the registerfunction in the UI, this will setup all fields inside of them
            PlayerStats stats = playerstats.GetComponent<PlayerStats>();
            stats.UpdateID((byte)M_Players.Count, score, m_WinRequiredScore, m_ScoreBoards[M_Players.Count - 1]);

            //update playerID in the character controller
            Player.SetPlayerID = (byte)M_Players.Count;
        }

        public void RegisterWorldEvent(ScriptableWorldEvent NewEvent)
        {
            WorldEvent actual = NewEvent.CreateInstance(NewEvent.m_TimeActive, new RemoveWorldEventPoolDelegate(DestroyWorldEvent));
            m_ActiveWorldEvents.Add(actual);
            EditorUtility.SetDirty(NewEvent);

        }

        private void DestroyWorldEvent(WorldEvent EndEvent)
        {
            m_ActiveWorldEvents.Remove(EndEvent);
        }

        private void EndGame(PlayerScore player)
        {
            Debug.Log("test");
            PoolObject screen = Pool.Singleton.Spawn(m_WinScreen, m_UI);
            screen.GetComponent<WinScherm>().Text = "Player " + player.M_PlayerController.name;
        }

        protected void Update()
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

            if (Input.GetKeyDown(KeyCode.Space))
                EndGame(m_Scores[0]);
        }

    }
}

