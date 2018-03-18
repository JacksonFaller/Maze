using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class MenuNavigator : MonoBehaviour
    {
        [SerializeField]
        private Button[] _backButtons;

        void Start()
        { 
        }

        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                InvokeClickOnButton();
            }
        }

        public void InvokeClickOnButton()
        {
            foreach (var backButton in _backButtons)
            {
                if (backButton.isActiveAndEnabled)
                {
                    backButton.onClick.Invoke();
                    return;
                }
            }
        }
    }
}
