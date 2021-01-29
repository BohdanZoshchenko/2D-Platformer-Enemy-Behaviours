
namespace PlatformerEnemyBehaviours2D.Internal
{
    using UnityEngine;

    [AddComponentMenu("2D Platformer Enemy Behaviours/Internal/Dissolve", 8)]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Dissolve : MonoBehaviour
    {
        public float speed = 0f;
        
        private Material dissolveMat;
        private SpriteRenderer rend;

        void OnEnable()
        {
            rend = GetComponent<SpriteRenderer>();
            rend.material.shader = Shader.Find("Custom/2D/Dissolve");
            dissolveMat = rend.material;
        }

        void Update()
        {
            if (speed != 0 && rend != null)
            {
                float value = dissolveMat.GetFloat("_Threshold");

                dissolveMat.SetFloat("_Threshold", value + Time.deltaTime * speed);


                if (speed < 0 && value <= 0)
                {
                    dissolveMat.SetFloat("_Threshold", 0);
                    return;
                }

                if (speed > 0 && value >= 1.1f)
                {
                    dissolveMat.SetFloat("_Threshold", 1.1f);
                    return;
                }
            }
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
    }
}