using System;
using GameCore;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class TemplatePlayer : MonoBehaviour
    {
        private float timerDuration = 3f;
        private float timerRemainingTime;

        private bool timerIsActive;
    
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
            timerRemainingTime = timerDuration;
            timerIsActive = true;
        }

        private void Update()
        {
            if (!timerIsActive) {return;}

            if (timerRemainingTime < 0f)
            {
                timerIsActive = false;
                
                var randomWin = Random.Range(0f, 1f);

                if (randomWin > 0.5f)
                {
                    LevelManager.Instance.CompleteLevel();
                    Debug.Log("Won");
                }
                else
                {
                    LevelManager.Instance.FailLevel();
                    Debug.Log("Failed");
                }
            }
            else
            {
                timerRemainingTime -= Time.deltaTime;
            }
        }
    }
}