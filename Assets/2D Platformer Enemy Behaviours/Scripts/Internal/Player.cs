namespace PlatformerEnemyBehaviours2D.Internal
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;

    /// <summary>
    /// Player main script.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Internal/Player", 3)]
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        public float speed = 0.1f;  // walk speed

        public float HP = 1f;   // max and start health
        public float visualDamageTime = 0.5f;
        public Color damageColor = Color.red;

        [Header("Event called when Player dies.")]
        public UnityEvent OnDie;

        private bool jump = false;
        private Animator animator;
        private CharacterController2D controller2D;
        private SpriteRenderer spriteRend;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            controller2D = GetComponent<CharacterController2D>();
            spriteRend = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            float move = 0;
            bool crouch = false;

            var h_input = Input.GetAxisRaw("Horizontal");
            var v_input = Input.GetAxisRaw("Vertical");
            
            if (!jump)
            {
                if (v_input < 0)
                {
                    // crouch
                    animator.SetBool("jump", false);
                    animator.SetBool("crouch", true);
                    animator.SetBool("move", false);
                    crouch = true;
                }
                else if (v_input > 0)
                {
                    animator.SetBool("jump", true);
                    animator.SetBool("crouch", false);
                    animator.SetBool("move", false);
                    jump = true;
                }
                else if (v_input == 0)
                {
                    animator.SetBool("crouch", false);
                }
            }

            if (h_input > 0)
            {
                // right
                if (!jump&&!crouch)
                    animator.SetBool("move", true);
                
                move = speed;
            }
            else if (h_input < 0)
            {
                // left
                if (!jump&&!crouch)
                    animator.SetBool("move", true);
                
                move = -speed;
            }
            else
            {
                animator.SetBool("move", false);
                
                move = 0;
            }
            controller2D.Move(move, crouch, jump);
        }

        public void Land()
        {
            animator.SetBool("jump", false);
            jump = false;
        }

        public void LoseHealth(float value)
        {
            StartCoroutine(VisualizeDamage());
            HP -= value;
            if (HP <= 0)
            {
                HP = 0;
                OnDie.Invoke();
            }
        }

        IEnumerator VisualizeDamage()
        {
            spriteRend.color = damageColor;
            yield return new WaitForSeconds(visualDamageTime);
            spriteRend.color = Color.white;
        }

        public float GetHealth()
        {
            return HP;
        }
    }
}