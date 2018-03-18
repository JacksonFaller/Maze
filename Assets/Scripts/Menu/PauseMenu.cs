using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        private bool _isPaused;

        [SerializeField]
        private List<GameObject> _UIElements;

        [SerializeField]
        private GameObject _pauseMenuUI;

        [SerializeField]
        private GameObject _background;

        public event Action PauseEvent;

        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (_isPaused)
                    Resume();
                else
                    Pause();
            }
        }

        public void Pause()
        {
            _isPaused = true;
            Time.timeScale = 0f;
            PauseEvent?.Invoke();

            _UIElements.RemoveAll(x => x.activeSelf == false);

            foreach (var element in _UIElements)
            {
                element.SetActive(false);
            }
            _pauseMenuUI.SetActive(true);
            _background.SetActive(true);
        }

        public void Resume()
        {
            _isPaused = false;
            Time.timeScale = 1f;

            foreach (var element in _UIElements)
            {
                element.SetActive(true);
            }
            _pauseMenuUI.SetActive(false);
            _background.SetActive(false);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(0);
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

