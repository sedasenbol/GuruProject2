using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
    public class PlayerSettingsScriptableObject : ScriptableObject
    {
        [SerializeField] private float smoothDampSmoothTime = 0.02f;
        [SerializeField] private float smoothDampMaxSpeed = 10f;
        [SerializeField] private float horizontalSpeed = 1.2f;

        public float SmoothDampSmoothTime => smoothDampSmoothTime;
        public float SmoothDampMaxSpeed => smoothDampMaxSpeed;
        public float HorizontalSpeed => horizontalSpeed;
    }
}