using UnityEngine;
using Sjabloon;

public class InputTest : MonoBehaviour
{
    private InputManager m_InputManger;

    private void Start()
    {
        m_InputManger = InputManager.Instance;

        //Button
        m_InputManger.BindButton("ControllerOne", 0, ControllerButtonCode.X, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("ControllerTwo", 1, ControllerButtonCode.X, InputManager.ButtonState.OnPress);

        //Axis
        m_InputManger.BindAxis("Horizontal", 1, ControllerAxisCode.LeftStickX);

        //Keyboard
        m_InputManger.BindButton("Keyboard_Submit", KeyCode.Return, InputManager.ButtonState.OnRelease);
        m_InputManger.BindButton("Keyboard_Cancel", KeyCode.Escape, InputManager.ButtonState.OnRelease);

        //Rumble, NIET VERGETEN UIT TE ZETTEN
        //ControllerInput.SetVibration(0, 1, 0, 1);
    }

    private void OnDestroy()
    {
        if (m_InputManger == null)
            return;

        m_InputManger.UnbindButton("Keyboard_Submit");
        m_InputManger.UnbindButton("Controller_Submit");

        m_InputManger.UnbindButton("Keyboard_Cancel");
        m_InputManger.UnbindButton("Controller_Cancel");

        ControllerInput.SetVibration(0, 0, 0, 1);
    }

    private void Update()
    {
        if (m_InputManger.GetButton("ControllerOne"))
        {
            Debug.Log("One!");
        }

        if (InputManager.Instance.GetButton("ControllerTwo"))
        {
            Debug.Log("Two!");
        }

        //Debug.Log(InputManager.Instance.GetAxis("Horizontal"));

        //...
    }
}
