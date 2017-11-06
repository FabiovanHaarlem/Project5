using System.Collections.Generic;
using UnityEngine;
using Game.Character.Ai;
using Game.Event;

namespace Game.UI
{
    public class PlayerScore
    {
        //struct that will store all nessecary info for the score
        private PlayerCurrentScore M_Struct;

        //function to get the score of this scoremanager
        internal float Score
        {
            get
            {
                return M_Struct.Score;
            }
        }
        internal PowerUp[] Powerups
        {
            get
            {
                return M_Struct.CurrentPowerups.ToArray();
            }
        }

        //refrence to the object that is being observed
        public CharacterControl M_PlayerController { private set; get; }

        //a list to check what fish have been cought to check when to add a point to player
        private List<IFish> CoughtFish;

        public PlayerScore(CharacterControl observer)
        {
            M_PlayerController = observer;

            //making new actions in the observer and add functions of this class to be called on a alert
            observer.M_Catched = new System.Action<IFish>(Catch);
            observer.M_AddPowerup = new System.Action<PowerUp>(AddPowerup);

            //making a new struct with the details of score and active powerups
            M_Struct = new PlayerCurrentScore()
            {
                CurrentPowerups = new List<PowerUp>()
            };

            CoughtFish = new List<IFish>();
        }

        //function being called when player catches a fish
        private void Catch(IFish fish)
        {
            CoughtFish.Add(fish);
            Fish actualfish = (Fish)fish;
            string Actualname = actualfish.name.Split('(')[0];

            if (CheckScore(Actualname, 4))
                M_Struct.Score += 1;

            if (Random.Range(0, 100) > 90)
                GameManager.Singelton.SpinningWheel.Spin();
        }

        //this function will check if there is the nessecary amount of the same fish in inventory
        private bool CheckScore(string input_name, byte requirement)
        {
            for (int i = 0; i < CoughtFish.Count; i++)
            {
                if (CoughtFish[i].M_Name == input_name)
                    requirement--;
            }

            if (requirement <= 0)
                return true;

            return false;
        }

        //called when player picks up an powerup
        private void AddPowerup(PowerUp Power)
        {
            //making a callback to call when time runs out
            Power.M_RemovePoolCallback += RemovePowerup;
            M_Struct.CurrentPowerups.Add(Power);
        }

        //called when powerup has been depleted (time active less than 0)
        public void RemovePowerup(PowerUp power)
        {
            M_Struct.CurrentPowerups.Remove(power);

            Debug.Log("Powerup Removed");
        }
    }

    internal struct PlayerCurrentScore
    {
        internal float Score;
        internal List<PowerUp> CurrentPowerups;
    }
}

