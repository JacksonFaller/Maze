using Assets.Scripts._3DMaze;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ElementSelect : MonoBehaviour
    {

        [SerializeField]
        private MazeGenerator.Flags _elementFlag;

        [SerializeField]
        private AddMazeElements _mazeElements;

        private static Button _selectedElement;

        // Use this for initialization
        void Start ()
        {
	        
        }

        public void SelectElement()
        {
            _mazeElements.UpdateSelectedElement(_elementFlag);
            var button = GetComponent<Button>();
            if (_selectedElement != null)
                _selectedElement.interactable = true;
            button.interactable = false;
            _selectedElement = button;
        }
    }
}
