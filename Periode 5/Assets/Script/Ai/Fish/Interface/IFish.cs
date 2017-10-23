using UnityEngine;

namespace Game.Character.Ai
{
    public interface IFish
    {
        string M_Name { get; }


        GameObject GetGameObject { get; }
        void Atteract(Vector3 destination);
        void Catched();
        void BeingCatched();
        void Escaped();
    }
}


