using Assets.Scripts._3DMaze;
using UnityEngine;

namespace Assets.Scripts
{
    public class ElementSelect : MonoBehaviour
    {

        [SerializeField]
        private MazeGenerator.Flags _elementFlag;

        [SerializeField]
        private AddMazeElements _mazeElements;

        // Use this for initialization
        void Start ()
        {
	
        }

        public void SelectElement()
        {
            _mazeElements.SelectedElementFlag = _elementFlag;
            _mazeElements.UpdateSelectedElement();
        }
    }
}
