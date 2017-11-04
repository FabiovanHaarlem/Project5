using UnityEngine;
using Sjabloon;

public class InputTest : MonoBehaviour
{
    private InputManager m_InputManger;

    private void Start()
    {
        m_InputManger = InputManager.Instance;

        //Buttons
        //Controller1
        m_InputManger.BindButton("Controller1AButton", 0, ControllerButtonCode.A, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller1XButton", 0, ControllerButtonCode.X, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller1LeftBumper", 0, ControllerButtonCode.LeftShoulder, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller1RightBumper", 0, ControllerButtonCode.RightShoulder, InputManager.ButtonState.OnPress);
        //Controller2
        m_InputManger.BindButton("Controller2AButton", 1, ControllerButtonCode.A, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller2XButton", 1, ControllerButtonCode.X, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller2LeftBumper", 1, ControllerButtonCode.LeftShoulder, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller2RightBumper", 1, ControllerButtonCode.RightShoulder, InputManager.ButtonState.OnPress);
        //Controller3
        m_InputManger.BindButton("Controller3AButton", 2, ControllerButtonCode.A, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller3XButton", 2, ControllerButtonCode.X, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller3LeftBumper", 2, ControllerButtonCode.LeftShoulder, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller3RightBumper", 2, ControllerButtonCode.RightShoulder, InputManager.ButtonState.OnPress);
        //Controller4
        m_InputManger.BindButton("Controller4AButton", 3, ControllerButtonCode.A, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller4XButton", 3, ControllerButtonCode.X, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller4LeftBumper", 3, ControllerButtonCode.LeftShoulder, InputManager.ButtonState.OnPress);
        m_InputManger.BindButton("Controller4RightBumper", 3, ControllerButtonCode.RightShoulder, InputManager.ButtonState.OnPress);

        //Axis
        //Controller1
        m_InputManger.BindAxis("Controller1Horizontal", 0, ControllerAxisCode.LeftStickX);
        m_InputManger.BindAxis("Controller1Vertical", 0, ControllerAxisCode.LeftStickY);
        //Controller2
        m_InputManger.BindAxis("Controller2Horizontal", 1, ControllerAxisCode.LeftStickX);
        m_InputManger.BindAxis("Controller2Vertical", 1, ControllerAxisCode.LeftStickY);
        //Controller1
        m_InputManger.BindAxis("Controller3Horizontal", 2, ControllerAxisCode.LeftStickX);
        m_InputManger.BindAxis("Controller3Vertical", 2, ControllerAxisCode.LeftStickY);
        //Controller1
        m_InputManger.BindAxis("Controller4Horizontal", 3, ControllerAxisCode.LeftStickX);
        m_InputManger.BindAxis("Controller4Vertical", 3, ControllerAxisCode.LeftStickY);

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
        //if (m_InputManger.GetButton("ControllerOne"))
        //{
        //    Debug.Log("One!");
        //}

        //if (InputManager.Instance.GetButton("ControllerOne"))
        //{
        //    Debug.Log("Two!");
        //}

        //Debug.Log(InputManager.Instance.GetAxis("Horizontal"));

        //...
    }
}
