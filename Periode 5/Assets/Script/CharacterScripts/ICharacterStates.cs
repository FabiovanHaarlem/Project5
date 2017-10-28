using UnityEngine;
using Game.Event;

public interface ICharacterStates
{
    void InitializeState();

    void UpdateControls(string[] keyCodes);

    void UpdateState();

    void OnTriggerStay(Collider collider);

    void AddPowerUp(PowerUp Power);
}
