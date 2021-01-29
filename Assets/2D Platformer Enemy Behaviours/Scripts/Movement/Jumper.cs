namespace PlatformerEnemyBehaviours2D.Movement
{
    using UnityEngine;
    using UnityEngine.Events;
    using PlatformerEnemyBehaviours2D.Internal;

    /// <summary>
    /// Jumps.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Movement/Jumper", 6)]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Jumper: MonoBehaviour
    {
        public Vector2 jumpSpeed = Vector2.one;
        public bool jumpOnEnable = true;

        [Header("Autochange jump direction (to player tagged position).")]
        public bool jumpToPlayer = false;

        [Header("Called when ground is achieved.")]
        public UnityEvent OnGround;

        private bool inJump = false;
        private Rigidbody2D rb2D;
        private Collider2D coll2D;
        private GroundCheck groundCheck;
        private GameObject groundCheckGO;

        void OnEnable()
        {
            rb2D = GetComponent<Rigidbody2D>();
            coll2D = GetComponent<Collider2D>();
            
            if (groundCheckGO != null)
                Destroy(groundCheckGO);
            var yHalfExtents = coll2D.bounds.extents.y;
            var yCenter = coll2D.bounds.center.y;
            float yLower = transform.position.y + (yCenter - yHalfExtents);
            var go = new GameObject();
            groundCheckGO = Instantiate(go, new Vector3(transform.position.x, yLower, transform.position.z), Quaternion.identity);
            Destroy(go);
            groundCheckGO.name = "GroundCheck";
            groundCheckGO.AddComponent<BoxCollider2D>().isTrigger = true;
            groundCheck = groundCheckGO.AddComponent<GroundCheck>();
            groundCheckGO.transform.SetParent(transform);
            groundCheckGO.GetComponent<BoxCollider2D>().size = new Vector2(0.2f, 0.2f);

            if (jumpOnEnable)
                Jump();
        }


        /// <summary>
        /// Jumping method. Can be called from Inspector.
        /// </summary>
        public void Jump()
        {
            if (rb2D.gravityScale <= 0)
                rb2D.gravityScale = 1;
            rb2D.freezeRotation = true;
            if (jumpToPlayer)
            {
                var sign = (int)Mathf.Sign(GameObject.FindWithTag("Player").transform.position.x - transform.position.x);
                jumpSpeed.x = Mathf.Abs(jumpSpeed.x) * 
                    sign;
                Flip(-sign);
            }
            rb2D.velocity += jumpSpeed;
            inJump = true;
        }

        /// <summary>
        /// Stops body (to avoid skidding). Can be called from Inspector.
        /// </summary>
        public void Stop()
        {
            rb2D.velocity = Vector2.zero;
        }

        void FixedUpdate()
        {
            if (!(coll2D is BoxCollider2D || coll2D is CircleCollider2D || coll2D is CapsuleCollider2D))
            {
                Debug.LogWarning(name + ": Jumper: BoxCollider2D, CircleCollider2D and CapsuleCollider2D are supported only. Fix this and re-enable script.");
                return;
            }

            if (inJump && groundCheck.IsGrounded())
            {
                inJump = false;
                Stop();
                OnGround.Invoke();
            } 
        }

        /// <summary>
        /// Flips object.
        /// </summary>
        void Flip(int dir = 1)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= Mathf.Sign(theScale.x);
            if (Mathf.Sign(dir) == -1)
               theScale.x *= -1; 
            transform.localScale = theScale;
        }
    } 
}