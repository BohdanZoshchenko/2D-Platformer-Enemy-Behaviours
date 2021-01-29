namespace PlatformerEnemyBehaviour2D.Abilities
{  
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Emits bullets or another enemies.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Abilities/Emitter", 2)]
    public class Emitter : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public float bulletSpeed = 10;
        public Transform bulletStartPoint;
        public Transform bulletTargetPoint;
        public float delayBetweenBullets = 0.5f;

        [Header("Event called when bullet is emitted.")]
        public UnityEvent OnEmit;

        void OnEnable()
        {
            Emit();
        }

        void OnDisable()
        {
            CancelInvoke();
        }

        void Emit()
        {
            var b = Instantiate(bulletPrefab, bulletStartPoint.position, bulletPrefab.transform.rotation) as GameObject;
            if (b.GetComponent<Rigidbody2D>() == null)
            {
                b.AddComponent<Rigidbody2D>();
            }
            b.GetComponent<Rigidbody2D>().gravityScale = 0;
            b.GetComponent<Rigidbody2D>().velocity = bulletSpeed * 
                (bulletTargetPoint.position - bulletStartPoint.position).normalized;

            if (b.GetComponent<Collider2D>() == null)
            {
                b.AddComponent<CircleCollider2D>().isTrigger = true;
            }

            Invoke("Emit", delayBetweenBullets);
            OnEmit.Invoke();
        }
    }
}