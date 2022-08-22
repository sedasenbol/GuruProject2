using System;
using System.Collections;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressTracker : MonoBehaviour
    {
        [SerializeField] private Transform finishTransform;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Slider progressSlider;

        private float distanceBetween;
        
        private void OnEnable()
        {
            LevelManager.OnNewLevelLoaded += OnNewLevelLoaded;
        }
        
        private void OnDisable()
        {
            LevelManager.OnNewLevelLoaded -= OnNewLevelLoaded;
        }

        private void OnNewLevelLoaded()
        {
            progressSlider.value = 0f;

            StartCoroutine(SetDistanceBetweenNextFrame());
        }

        private IEnumerator SetDistanceBetweenNextFrame()
        {
            yield return null;
            
            distanceBetween = finishTransform.position.z - playerTransform.position.z;
        }

        private void Update()
        {
            if (!LevelManager.Instance.IsGameActive) {return;}
            
            progressSlider.value = 1 - (finishTransform.position.z - playerTransform.position.z) / distanceBetween;
        }
    }
}