using UnityEngine;

namespace Input
{
    public interface ITapPositionReceiver
    {
        //TouchController.cs OnPlayerTapped event handler.
        void OnPlayerTapped(Vector3 currentTapPosition);
    }
}