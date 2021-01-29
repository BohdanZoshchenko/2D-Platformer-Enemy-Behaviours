namespace PlatformerEnemyBehaviours2D.Internal 
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Simple ground check.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Internal/GroundCheck", 9)]
    [RequireComponent(typeof(Collider2D))]
    public class GroundCheck : MonoBehaviour
    {
        private List<Collider2D> potentialGrounds = new List<Collider2D>();

        public bool IsGrounded()
        {
            if (potentialGrounds.Count == 0)
                return false;
            return true;
        }

        void OnEnable()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other != transform.parent.GetComponent<Collider2D>())
                potentialGrounds.Add(other);  
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (potentialGrounds.Contains(other))
                potentialGrounds.Remove(other);    
        }
    }
}
