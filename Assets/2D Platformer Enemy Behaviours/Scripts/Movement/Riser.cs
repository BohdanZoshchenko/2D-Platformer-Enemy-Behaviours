namespace PlatformerEnemyBehaviours2D.Movement
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Rises from zero (ground) with speed.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Movement/Riser", 2)]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Riser : MonoBehaviour
    {
        [RangeAttribute(0f, 10f)]
        [Header("Speed of rising.")]
        public float speed = 1;

        [Header("Event called when whole height is achieved.")]
        public UnityEvent OnAchieveHeight;

        // cached transform
        private Transform tr;

        // cached SpriteRenderer
        private SpriteRenderer spriteRend;

        // y value were we start
        private float startY;

        /// <summary>
        /// Resets values and repeat movement.
        /// </summary>
        void Init()
        {
            tr = transform;
            
            // rotation should be default for script to work properly 
            if (tr.rotation != Quaternion.identity)
            {
                Debug.LogWarning(name + ": Riser or Ducker rotation have to be (0, 0, 0)");
                tr.rotation = Quaternion.identity;
            }

            spriteRend = GetComponent<SpriteRenderer>();
            startY = tr.position.y;
            ChangeVisibility(0);
        }

        void OnEnable()
        {
            // initialize
            Init();
        }

        void Start()
        {
            // initialize
            Init();
        }
 
        void Update()
        {
            // rotation should be default for script to work properly 
            if (tr.rotation != Quaternion.identity)
            {
                Debug.LogWarning(name + ": Riser or Ducker rotation have to be (0, 0, 0)");
                tr.rotation = Quaternion.identity;
            }
            CheckHeight();
        }

        /// <summary>
        /// Checks achieving height, moves and stops movement.
        /// </summary>
        void CheckHeight()
        {
            var height = spriteRend.sprite.rect.height / spriteRend.sprite.pixelsPerUnit * tr.lossyScale.y; // height
            var visibleH = tr.position.y - startY; // visible height

            // rise
            if (height > visibleH)
            {
                ChangeVisibility(speed * Time.deltaTime);
            }
            else
            // stop in appropriate point and invoke event
            {
                tr.position = new Vector3(tr.position.x, startY, tr.position.z) + Vector3.up * height;

                // invoke event
                OnAchieveHeight.Invoke();
            }
        }

        /// <summary>
        /// Unmasks/masks object's sprite's part.
        /// </summary>
        void ChangeVisibility(float heightChange)
        {
            tr.position += heightChange * Vector3.up; // new position
        }
    }
}