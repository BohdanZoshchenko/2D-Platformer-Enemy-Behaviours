namespace PlatformerEnemyBehaviours2D.Movement
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Walks for translation units and back, and so on, with speed.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Movement/Walker", 1)]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Walker : MonoBehaviour
    {
        [RangeAttribute(0f, 10f)]
        [Header("Speed of walking.")] 
        public float speed = 3f;

        [Header("Do you want to apply gravity?")] 
        public bool gravityOn = false;

        [Header("Distance to move, right if value is positive, left if value is negative.")]
        public float horizontalTranslation = 3f;

        [Header("Flip character when point is achieved.")]
        public bool flipAtPoint = true;

        [Header("Flip character on start (if its facing direction is wrong).")]
        public bool flipOnStart = false;

        [Header("Event called when start or finish point is achieved.")] 
        public UnityEvent OnAchievePointAOrB;

        [Header("Event called when start point is achieved.")]
        public UnityEvent OnAchievePointA;

        [Header("Event called when finish point is achieved.")]
        public UnityEvent OnAchievePointB;

        // start and finish points
        private Vector2 pointA, pointB;
        // gravity scale stored here
        private float gravity;
        // this object's rigid body
        private Rigidbody2D rb2D;
        // goal point (start or finish)
        private Vector2 goalPoint;

        // Awake is used because we want to enable/disable gravity very early
        void Awake()
        {
            // initialize values
            rb2D = GetComponent<Rigidbody2D>();
            rb2D.freezeRotation = true;
            gravity = rb2D.gravityScale;
            
            if (!gravityOn) // turn off gravity (by default)
            {
                rb2D.gravityScale = 0;
                var velocity = rb2D.velocity;
                velocity.y = 0;
                rb2D.velocity = velocity;
            }
            pointA = rb2D.position;
            goalPoint = pointB = pointA + new Vector2(horizontalTranslation, 0);
            // flip before beginning
            if (flipOnStart)
                Flip();
        }

        // Update is called once per frame
        void Update()
        {
            // change object's y (for case when gravity is on)
            goalPoint.y = pointA.y = pointB.y = rb2D.position.y;
            // calculate and apply velocity
            var velocity = rb2D.velocity;
            velocity.x = Mathf.Sign(goalPoint.x - rb2D.position.x) * speed;
            if (gravityOn)
                rb2D.gravityScale = gravity;
            else
            {
                rb2D.gravityScale = 0;
                velocity.y = 0;
            }
            rb2D.velocity = velocity;

            // Calculate movement step to check achieving (or closeness to) goal point
            float step = Time.deltaTime * speed;

            // Events called on achieving (or closeness to) start and/or finish point(s) 
            if (Mathf.Abs(goalPoint.x - rb2D.position.x) < step)
            {
                if (goalPoint == pointA)
                {
                    // Walk to another point
                    goalPoint = pointB;
                    OnAchievePointA.Invoke();
                }
                else if (goalPoint == pointB)
                {
                    // Walk to another point
                    goalPoint = pointA;
                    OnAchievePointB.Invoke();
                }

                OnAchievePointAOrB.Invoke();
                if (flipAtPoint)
                    Flip();
            }
        }

        /// <summary>
        /// Flips object by multiplying scale x by -1.
        /// </summary>
        void Flip()
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }   
}