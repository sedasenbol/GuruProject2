using UnityEngine;

namespace Input
{
    public interface IDraggable
    {
        //TouchController.cs OnPlayerDragged event handler that rotates the platform groups and the pick ups.
        void OnPlayerDragged(Vector3 dragVector);
    }
}