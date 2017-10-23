using UnityEngine;
using System.Collections.Generic;
using System;

public interface ICharacterStates
{
    void InitializeState();

<<<<<<< HEAD
<<<<<<< HEAD
        void UpdateControls(KeyCode[] keyCodes);
=======
    void UpdateControls(string[] keyCodes);
>>>>>>> Fabio
=======
    void UpdateControls(string[] keyCodes);
>>>>>>> f260879072728bc89455d28b421450bc13595c3f

    void UpdateState();

    void OnTriggerStay(Collider collider);

    void AddPowerUp(PowerUp Power);
}
