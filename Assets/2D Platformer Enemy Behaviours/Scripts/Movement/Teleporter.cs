namespace PlatformerEnemyBehaviour2D.Movement
{  
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Teleports from point A to point B.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Movement/Teleporter", 5)]
    public class Teleporter : MonoBehaviour
    {
        [Header("Sequence of goal positions.")]
        public Vector2[] goalPositions;
        public float timeBetweenTeleportations = 3f;
        public float beforeDepartEventTime = 1f;
        public bool useGlobalPosition = false;
        public bool loop = true;

        [Header("Event called before departure, before delay.")]
        public UnityEvent OnBeforeDepart;

        [Header("Event called after arriving.")]
        public UnityEvent OnAfterArrive;

        private Vector2 goal;
        private int goalNumber = 0;

        void OnEnable()
        {
            goalNumber = 0;
            if (goalPositions.Length == 0)
            {
                Debug.LogWarning(name + ": Teleporter: There are no goal positions. Add them and re-enable script.");
                return;
            }
            
            InvokeTeleportWithEvent();
        } 

        /// <summary>
        /// Invokes Teleport with OnBeforeDepart event. 
        /// </summary>
        void InvokeTeleportWithEvent()
        {
            Invoke("OnBeforeDepartMethod", timeBetweenTeleportations - beforeDepartEventTime);
            Invoke("Teleport", timeBetweenTeleportations);
        }

        /// <summary>
        /// Teleports from point A to point B with event after arriving. 
        /// </summary>
        void Teleport()
        {
            goal = goalPositions[goalNumber];
            goalNumber++;
            if (goalNumber == goalPositions.Length)
            { 
                if (loop)
                    goalNumber = 0;
                else
                    enabled = false;
            }

            var par = transform.parent;
            if (useGlobalPosition)
                transform.SetParent(null);

            var pos = transform.localPosition;
            pos = goal;
            pos.z = transform.localPosition.z;
            transform.localPosition = pos;

            if (useGlobalPosition)
                transform.SetParent(par);

            if (goalNumber < goalPositions.Length)
            {
                InvokeTeleportWithEvent();
            }

            OnAfterArrive.Invoke();
        }

        /// <summary>
        /// Invokes OnBeforeDepart event. 
        /// </summary>
        void OnBeforeDepartMethod()
        {
            OnBeforeDepart.Invoke();
        }       
    }
}