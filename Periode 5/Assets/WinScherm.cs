using UnityEngine;
using UnityEngine.UI;

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
    }
}


