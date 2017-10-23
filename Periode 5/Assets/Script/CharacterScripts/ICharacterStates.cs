using UnityEngine;
using Game.Character.player.Powerups;

public interface ICharacterStates
{
    void InitializeState();

    void UpdateControls(string[] keyCodes);

    void UpdateState();

    void OnTriggerStay(Collider collider);

    void AddPowerUp(PowerUp Power);
}
