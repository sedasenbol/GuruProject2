using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace GameCore
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        
        private AudioClip[] audioClips;

        private void Start()
        {
            audioClips = Resources.LoadAll<AudioClip>("AudioFiles");
        }
        
        private void OnEnable()
        {
            StackManager.OnStackMatch += OnStackMatch;
        }
        
        private void OnDisable()
        {
            StackManager.OnStackMatch -= OnStackMatch;
        }

        private void OnStackMatch(int stackMatchCounter)
        {
            stackMatchCounter %= audioClips.Length;

           audioSource.PlayOneShot(audioClips[stackMatchCounter]);
        }
    }
}