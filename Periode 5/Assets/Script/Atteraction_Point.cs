using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Ai
{
    public class Atteraction_Point : MonoBehaviour
    {

        public Atteraction_Point()
        {
            //get all objects in area
            RaycastHit[] Hits = Physics.CapsuleCastAll(transform.position, transform.position, 5, transform.forward);

            //make list to store the fish into
            List<IFish> fishes = new List<IFish>();

            //check if any objects in area are fish
            foreach(RaycastHit i in Hits)
            {
                IFish tempfish = i.collider.gameObject.GetComponent<IFish>();
                if (tempfish != null)
                    fishes.Add(tempfish);

            }

            //each fish will be atteracted to this location
            foreach (IFish i in fishes)
                i.Atteract(transform.position);

            //destroy this objects after 5 seconds
            Destroy(gameObject, 5f);
        }
    }
}


