using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class MainMenu : MonoBehaviour
    {

        public void PlayGame()
        {
       
        }

        public void Settings()
        {
        
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
