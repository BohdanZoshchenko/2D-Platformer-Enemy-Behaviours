namespace PlatformerEnemyBehaviours2D.Internal
{
    using UnityEngine;

    [AddComponentMenu("2D Platformer Enemy Behaviours/Internal/ShakeCamera2D", 5)]
    public class ShakeCamera2D : MonoBehaviour
    {
        public float shakeMagnitude = 0.7f;
        public float dampingSpeed = 1;

        void LateUpdate()
        {
            transform.localPosition += Random.insideUnitSphere * shakeMagnitude;
        }
    }
}