using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class WinScherm : MonoBehaviour {

        [SerializeField]
        private Text m_PlayerWin;
        public string Text
        {
            set
            {
                m_PlayerWin.text = value;
            }
        }


        public void ReturnToMenu()
        {
            SceneManager.LoadScene(SceneManager.GetSceneByName("Menu Test").buildIndex);
        }
    }
}


