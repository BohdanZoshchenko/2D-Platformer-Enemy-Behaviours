namespace PlatformerEnemyBehaviour2D.Abilities
{  
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Grows or shrinks to given scale, anchoring to bottom.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Abilities/Grower_Shrinker", 1)]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Grower_Shrinker : MonoBehaviour
    {
        [RangeAttribute(0f, 100f)]
        [Header("Speed of growing/shrinking.")]
        public float speed = 1;

        [RangeAttribute(0f, 100f)]
        [Header("Scale by Y we are growing/shrinking to.")]
        public float goalYScale = 1f;

        [Header("How close size should be to goal size to stop.")]
        public float threshold = 0.05f;

        [Header("Grow/shrink X scale proportionally. Or not.")]
        public bool useProportionToX = true;

        [Header("If true, move back to start size and so on.")]
        public bool loop = false;

        // event on achieve goal Y scale
        public UnityEvent OnAchieveHeight;

        // stores previous Use Proportion setting (to check for change)
        private bool prevUseProportionValue;
        
        // start scale
        private Vector3 oldScale;

        // goal scale
        private Vector3 newScale;

        // X scale / Y scale
        private float proportion;

        // cached transform
        private Transform tr;

        // cached SpriteRenderer
        private SpriteRenderer rend;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        void OnEnable()
        {
            Init();
        }

        void Init()
        {
            tr = transform;

            // rotation should be default to make script to work properly
            if (tr.rotation != Quaternion.identity)
            {
                Debug.LogWarning(name + ": Grower_Shrinker: Rotation was not default. Fixed.");
                tr.rotation = Quaternion.identity;
            }

            rend = GetComponent<SpriteRenderer>();

            // rotation should be default to make script to work properly
            if (rend == null || rend.sprite == null)
            {
                Debug.LogWarning(name + ": Grower_Shrinker: There is no sprite.");
                return;
            }

            prevUseProportionValue = useProportionToX;

            // init new scale value
            oldScale = newScale = tr.lossyScale;

            // set proportion
            proportion = Mathf.Abs(tr.lossyScale.x / tr.lossyScale.y);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            // rotation should be default to make script to work properly
            if (tr.rotation != Quaternion.identity)
            {
                Debug.LogWarning(name + ": Grower_Shrinker: Rotation was not default. Fixed.");
                tr.rotation = Quaternion.identity;
            }

            if (rend == null)
                rend = GetComponent<SpriteRenderer>();

            // rotation should be default to make script to work properly
            if (rend == null || rend.sprite == null)
            {
                Debug.LogWarning(name + ": Grower_Shrinker: There is no sprite.");
                return;
            }

            if (Mathf.Abs(tr.lossyScale.y - goalYScale) <= threshold)
            {
                if (loop)
                    Resize();
            }
            else 
            {
                Resize();
            }
        }

        /// <summary>
        /// Resizes object by Y lossy scale (and X if mentioned) with anchoring to bottom.
        /// </summary>
        void Resize()
        {
            // change parent to null to change lossy scale
            var oldParent = tr.parent;
            tr.parent = null;

            // if Use Proportion is on
            if (useProportionToX && !prevUseProportionValue)
                // set proportion
                proportion = tr.lossyScale.x / tr.lossyScale.y;

            // calculate new scale
            newScale.y = goalYScale;
            if (useProportionToX)
            {
                newScale.x = newScale.y * proportion * Mathf.Sign(tr.localScale.x);
            }

            float h0 = tr.localScale.y;
            float y0 = tr.position.y;

            // smothly apply new scale
            tr.localScale = Vector3.Lerp(tr.localScale, newScale, Time.deltaTime * speed);


            float h1 = tr.localScale.y;

            // anchor to bottom
            Anchor(h0, h1, y0);

            // if loop, go back to start size
            if (Mathf.Abs(tr.lossyScale.y - goalYScale) <= threshold)
            {
                OnAchieveHeight.Invoke();

                if (loop)
                {
                    var tmp = oldScale;
                    oldScale = newScale;
                    newScale = tmp;
                    goalYScale = newScale.y;
                }
            }

            // attach to parent
            tr.parent = oldParent;

            // set last Use Proportion value
            prevUseProportionValue = useProportionToX;

        }

        /// <summary>
        /// Anchors object to bottom.
        /// </summary>
        void Anchor(float h0, float h1, float y0)
        {
            var mult = rend.sprite.rect.height / rend.sprite.pixelsPerUnit;
            var y1 = y0 + mult * (h1 - h0) / 2f;

            var pos = tr.position;
            pos.y = y1;
            tr.position = pos;
        }
    }
}