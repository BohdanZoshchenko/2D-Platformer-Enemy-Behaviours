namespace PlatformerEnemyBehaviours2D.Qualities
{
    using UnityEngine;
    using UnityEngine.Events;
    using PlatformerEnemyBehaviours2D.Internal;

    /// <summary>
    /// Calls OnDamage event when colliding with Player tagged object, with delay.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Qualities/Damager", 1)]
    public class Damager : MonoBehaviour
    {
        public bool hitPlayerTaggedObjectAutomatically = false;
        [Header("Works if hitPlayerTaggedObjectAutomatically is on only.")]
        public float autoDamage = 0.2f;
        
        [RangeAttribute(0f, 100f)]
        [Header("Delay between two hits if player is colliding.")]
        public float hitDelay = 1f;
        [Header("Event called when Damager and the player are colliding.")]
        public UnityEvent OnDamage;
       
        private float delayRest = 0f;
        private Collider2D coll = null;

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<Collider2D>() == null)
                Debug.LogWarning(name + ": Damager must have Collider2D (trigger or not).");
            if (GameObject.FindWithTag("Player") == null)
                Debug.LogWarning(name + ": Scene have to contain object with tag Player, which will collide with Damager.");
            
            // delay
            if (delayRest > 0)
            {
                delayRest -= Time.deltaTime;
                if (delayRest < 0)
                    delayRest = 0;
            }

            // check fo collision, if True - send event
            if (coll != null)
                CheckDelayEnded();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                coll = other;
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
                coll = other.collider;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                coll = null; 
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
                coll = null;
        }

        void CheckDelayEnded()
        {
            if (delayRest == 0)
            {
                if (hitPlayerTaggedObjectAutomatically)
                {
                    GameObject.FindWithTag("Player").GetComponent<Player>().LoseHealth(autoDamage);
                }

                delayRest = hitDelay;
                OnDamage.Invoke();
            }
        }
    } 
}
