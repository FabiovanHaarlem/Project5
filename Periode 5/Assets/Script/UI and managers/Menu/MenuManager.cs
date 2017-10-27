using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Plugins.ObjectPool;

namespace Game.UI
{
    //starting the menu manager and updating it
    public partial class MenuManager
    {
        [SerializeField]
        private Text m_TextPlayerCount;

        [SerializeField]
        private string m_Level;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
            m_PlayerCount = 2;
        }

        private void Update()
        {
            m_TextPlayerCount.text = m_PlayerCount.ToString();
            m_animator.SetInteger("CurrentScreen", m_CurrentMenuScreen);
        }
    }

    //setting Settings
    public partial class MenuManager : MonoBehaviour {

        private int m_FishSpawningLimit;

        private byte m_PlayerCount;

        //sets a limit of fish that can spawn during the game
        public void SetSpawningLimit(int Limit)
        {
            m_FishSpawningLimit = Limit;
        }

        //increases the playercount
        public void AddPlayer()
        {
            if (m_PlayerCount != 4)
                m_PlayerCount++;
        }

        //lowers the playercount
        public void RemovePlayer()
        {
            if (m_PlayerCount > 2)
                m_PlayerCount--;
        }

    }

    //starting the game
    public partial class MenuManager
    {
        private string m_CurrentSceneName;

        public void StartGame()
        {
            //search for active scene
            m_CurrentSceneName = SceneManager.GetActiveScene().name;

            //add game load function to action when the new level has been loaded
            SceneManager.sceneLoaded += OnGameLoad;

            //load the main level scene
            SceneManager.LoadScene(m_Level, LoadSceneMode.Additive);
        }

        //needs to be worked on
        private void OnGameLoad(Scene Level, LoadSceneMode setting)
        {
            //remove this function from the action
            SceneManager.sceneLoaded -= OnGameLoad;

            //get root objects of newly loaded scene
            GameObject[] rootobjects = Level.GetRootGameObjects();

            GameManager mymanager = null;
            GameObject[] SpawnPoints = null;

            //try and find the object pool in the new level
            foreach (GameObject i in rootobjects)
            {
                if(i.GetComponent<GameManager>() != null)
                {
                    mymanager = i.GetComponent<GameManager>();
                    SpawnPoints = mymanager.GetScorePoints;
                }
            }

            //spawns an amount of players according to setting in this manager
            for (int i = 0; i < m_PlayerCount; i++)
            {
                Pool.Singleton.Spawn(mymanager.GetPlayerPrefab, SpawnPoints[i].transform.position);
            }

            //unload current scene
            SceneManager.UnloadSceneAsync(m_CurrentSceneName);
        }
    }

    //animating the menu
    public partial class MenuManager
    {
        private Animator m_animator;

        private byte m_CurrentMenuScreen;

        public void Switchscreen(int index)
        {
            m_CurrentMenuScreen = (byte)index;
        }


    }
}

