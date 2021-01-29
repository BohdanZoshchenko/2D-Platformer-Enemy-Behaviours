namespace PlatformerEnemyBehaviours2D.Triggers
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Activates event object (perhaps player) is within a certain distance to enemy.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Triggers/Proximity", 1)]
    public class Proximity : MonoBehaviour
    {
        [RangeAttribute(0f, 100f)]
        [Header("Distance to an object.")]
        public float distance = 1f;

        [Header("Object that triggers event when is within distance.")]
        public Transform triggeringObject;

        public UnityEvent OnWithinDistance;
        public UnityEvent OnOutsideDistance;

        private Transform tr;

        private bool prevState = false;

        void Start()
        {
            tr = transform;
        }

        void Update()
        {
            bool currSate = Vector3.Distance(triggeringObject.position, tr.position) <= distance;
            if (prevState == currSate)
                return;
            if (currSate)
            {
                OnWithinDistance.Invoke();
            }
            else
            {
                OnOutsideDistance.Invoke();
            }

            prevState = currSate;
        }
    }
}