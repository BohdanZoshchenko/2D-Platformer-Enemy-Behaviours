namespace PlatformerEnemyBehaviours2D.Internal 
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Control for demo level.
    /// </summary>
    [AddComponentMenu("2D Platformer Enemy Behaviours/Internal/DemoLevel", 1)]
    public class DemoLevel : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
