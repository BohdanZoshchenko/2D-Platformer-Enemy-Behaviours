namespace PlatformerEnemyBehaviours2D.Internal
{
    using UnityEngine;
    
    [AddComponentMenu("2D Platformer Enemy Behaviours/Internal/HealthBar", 4)]
    public class HealthBar : MonoBehaviour {
        public float barDisplay; //current health
        public Vector2 pos = new Vector2(20, 40);
        public Vector2 size = new Vector2(100, 40);
        private Texture2D emptyTex, fullTex;
        private GUIStyle emptyStyle, fullStyle;

        void InitStyles()
        {
            emptyTex = MakeTex(2, 2, Color.gray);
            fullTex = MakeTex(2, 2, Color.red);

            emptyStyle = new GUIStyle(GUI.skin.box);
            emptyStyle.normal.background = emptyTex;

            fullStyle = new GUIStyle(GUI.skin.box);
            fullStyle.normal.background = fullTex;
        }

        void OnGUI() {
            InitStyles();

            //draw the background:
            GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
                GUI.Box(new Rect(0,0, size.x, size.y), "", emptyStyle);
             
                //draw the filled-in part:
                GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
                    GUI.Box(new Rect(0,0, size.x, size.y), "", fullStyle);
                GUI.EndGroup();
            GUI.EndGroup();
        }
         
        void Update() 
        {
            barDisplay = GetComponent<Player>().GetHealth();
        }

        Texture2D MakeTex( int width, int height, Color col )
        {
            Color[] pix = new Color[width * height];
            for( int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D( width, height );
            result.SetPixels( pix );
            result.Apply();
            return result;
        }
    }
}