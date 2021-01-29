namespace PlatformerEnemyBehaviours2D.Movement
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Falls down.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Movement/Faller", 4)]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Faller: MonoBehaviour
    {
        [RangeAttribute(0f, 10f)]
        [Header("Speed of falling.")]
        public float speed = 1;

        [Header("Event called when surface is achieved.")]
        public UnityEvent OnAchieveSurface;
        
        private Rigidbody2D rb2D;

        /// <summary>
        /// Resets values and repeat movement.
        /// </summary>
        void Init()
        {
            if (GetComponent<Collider2D>() == null)
            Debug.LogWarning(name + ": Faller: There is no 2D collider.");
            rb2D = GetComponent<Rigidbody2D>();
            rb2D.gravityScale = 0;
            rb2D.velocity = Vector3.down * speed;
        }

        void OnEnable()
        {
            // initialize
            Init();
        }

        void Update()
        {
            rb2D.velocity = Vector3.down * speed;
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            // check if collision is not with ceiling
            if (other.collider.transform.position.y < transform.position.y)
                OnAchieveSurface.Invoke();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // check if collision is not with ceiling
            if (other.transform.position.y < transform.position.y)
                OnAchieveSurface.Invoke();
        }
    }
}