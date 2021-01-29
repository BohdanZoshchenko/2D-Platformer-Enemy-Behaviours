    
namespace PlatformerEnemyBehaviours2D.Internal
{
    using UnityEngine;

    [AddComponentMenu("2D Platformer Enemy Behaviours/Internal/SmoothCamera2D", 6)]
    public class SmoothCamera2D : MonoBehaviour { 
        public Transform hero;
        public float smooth = 2.5F;
        public float distance = -10.0F;
        public float minHorizontal = -1000.0F;
        public float maxHorizontal = 1000.0F;
        public float minVertical = -5.0F;
        public float maxVertical = 1000.0F;
            
        Vector3 position;
            
            
        void Start() {
            position = transform.position;
        }
            
            
        // You would use Update()/LateUpdate() if your game is completely frame based
        void FixedUpdate() {
            
            position = Vector3.Lerp(position, hero.position, (smooth * Time.fixedDeltaTime));
            position = new Vector3(
                (Mathf.Clamp(position.x, minHorizontal, maxHorizontal)),
                (Mathf.Clamp(position.y, minVertical, maxVertical)),
                distance);
            transform.position = position;
        }
    }
}       
    