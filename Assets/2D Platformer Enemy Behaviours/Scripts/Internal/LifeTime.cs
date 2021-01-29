namespace PlatformerEnemyBehaviours2D.Internal
{
    using UnityEngine;

    [AddComponentMenu("2D Platformer Enemy Behaviours/Internal/LifeTime", 7)]
    public class LifeTime : MonoBehaviour
    {
        public float time = 3f;
        
        void OnEnable()
        {
            Invoke("Die", time);
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}