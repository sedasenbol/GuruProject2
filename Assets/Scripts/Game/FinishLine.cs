using System;
using GameCore;
using UnityEngine;

namespace Game
{
    public class FinishLine : MonoBehaviour
    {
        private int playerLayer;

        private void Start()
        {
            playerLayer = LayerMask.NameToLayer("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != playerLayer) {return;}
            
            LevelManager.Instance.CompleteLevel();
        }
    }
}