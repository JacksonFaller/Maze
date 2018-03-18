using Assets.Scripts._3DMaze;
using UnityEngine;

namespace Assets.Scripts
{
    public class AddMazeElements : MonoBehaviour
    {
        private MazeGenerator.Flags[,,] _maze;

        private Transform _elementsContainer;

        private Camera _camera;

        private Element _selectedElement;

        private MazeGenerator.Flags _elements = MazeGenerator.Flags.FAKE_TREASURE | MazeGenerator.Flags.KEY |
                                               MazeGenerator.Flags.PIT | MazeGenerator.Flags.TRAP |
                                               MazeGenerator.Flags.TREASURE;
        [SerializeField] 
        private LayerMask _cellsMask;

        [SerializeField]
        private LayerMask _outerWallsMask;

        [SerializeField]
        private Transform _key;

        [SerializeField]
        private Transform _pit;

        [SerializeField]
        private Transform _treasure;

        [SerializeField]
        private Transform _fakeTreasure;

        [SerializeField]
        private Transform _trap;

        [SerializeField]
        private Transform _exit;

        public Element Key { get; set; }
        public Element Pit { get; set; }
        public Element Treasure { get; set; }
        public Element FakeTreasure { get; set; }
        public Element Trap { get; set; }
        public Element Exit { get; set; }

        public MazeGenerator.Flags SelectedElementFlag { get; set; }

        void Start ()
        {
            _camera = Camera.main;
            _elementsContainer = new GameObject("Elements").transform;
            //GroupUpElements(_elementsContainer, Exit, Key, FakeTreasure, Treasure, Trap, Pit);
            SelectedElementFlag = 0;
            Key = new Element(2, _key);
            Treasure = new Element(2, _treasure);
            Exit = new Element(4, _exit);
            GroupUpElements(_elementsContainer, Exit, Key, Treasure);
        }

        void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray =  _camera.ScreenPointToRay(Input.mousePosition);

                if(_selectedElement == null) return;
                if (SelectedElementFlag == MazeGenerator.Flags.EXIT)
                {
                    if (Physics.Raycast(ray, out hit, 100, _outerWallsMask))
                    {
                        AffectExitElement(hit.transform);
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out hit, 100, _cellsMask))
                    {
                        AffectElement(hit.transform);
                    }
                }
            }
        }

        /// <summary>
        /// Add or remove exit element
        /// </summary>
        /// <param name="objectHit">object that was hit by raycast</param>
        private void AffectExitElement(Transform objectHit)
        {
            Point position = objectHit.GetComponent<CellPosition>().Position;
            MazeGenerator.Flags flag;

            // Right and left walls
            if (objectHit.rotation.y == 0)
            {
                flag = position.y == 0 ? MazeGenerator.Flags.WALL_LEFT : MazeGenerator.Flags.WALL_RIGHT;
            }
            // Up and down walls
            else
            {
                flag = position.x == 0 ? MazeGenerator.Flags.WALL_DOWN : MazeGenerator.Flags.WALL_UP;
            }
            // Add exit
            if ((_maze[position.x, position.y, 0] & flag) == flag)
            {
                if (_selectedElement.ElementsNumber <= 0) return;
                _selectedElement.ElementsNumber--;
                _maze[position.x, position.y, 0] ^= flag;

                int index = _selectedElement.Positions.IndexOf(null);
                _selectedElement[index].position = objectHit.position;
                _selectedElement[index].gameObject.SetActive(true);
                objectHit.GetComponent<MeshRenderer>().enabled = false;
                _selectedElement.Positions[index] = new Point(position.x, position.y);
            }
            // Remove exit
            else
            {
                _selectedElement.ElementsNumber++;
                _maze[position.x, position.y, 0] |= flag;

                Point point = new Point(position.x, position.y);
                int index = _selectedElement.Positions.IndexOf(point);

                _selectedElement[index].position = objectHit.position;
                _selectedElement[index].gameObject.SetActive(false);
                objectHit.GetComponent<MeshRenderer>().enabled = true;
                _selectedElement.Positions[index] = null;
            }
            //Debug.DrawRay(ray.origin, ray.direction * 500, Color.green, 15f);
        }

        /// <summary>
        /// Add or remove any element (except for exit)
        /// </summary>
        /// <param name="objectHit">object that was hit by raycast</param>
        private void AffectElement(Transform objectHit)
        {
            Point position = objectHit.GetComponent<CellPosition>().Position;

            if ((_maze[position.x, position.y, 0] & SelectedElementFlag) == 0)
            {
                if((_maze[position.x, position.y, 0] & _elements) != 0) return;

                if (_selectedElement.ElementsNumber <= 0) return;
                _selectedElement.ElementsNumber--;
                _maze[position.x, position.y, 0] |= SelectedElementFlag;

                int index = _selectedElement.Positions.IndexOf(null);
                _selectedElement[index].position = objectHit.position;
                _selectedElement[index].gameObject.SetActive(true);
                _selectedElement.Positions[index] = new Point(position.x, position.y);
            }
            else
            {
                _selectedElement.ElementsNumber++;
                _maze[position.x, position.y, 0] ^= SelectedElementFlag;

                Point point = new Point(position.x, position.y);
                int index = _selectedElement.Positions.IndexOf(point);

                _selectedElement[index].position = objectHit.position;
                _selectedElement[index].gameObject.SetActive(false);
                _selectedElement.Positions[index] = null;
            }
            //Debug.DrawRay(ray.origin, ray.direction * 500, Color.green, 15f); 
        }

        private Element GetSelectedElement(MazeGenerator.Flags elementFlag)
        {
            Element selectedElement;

            switch (elementFlag)
            {
                case MazeGenerator.Flags.EXIT:
                {
                    selectedElement = Exit;
                    break;
                }
                case MazeGenerator.Flags.TRAP:
                {
                    selectedElement = Trap;
                    break;
                }
                case MazeGenerator.Flags.TREASURE:
                {
                    selectedElement = Treasure;
                    break;
                }
                case MazeGenerator.Flags.FAKE_TREASURE:
                {
                    selectedElement = FakeTreasure;
                    break;
                }
                case MazeGenerator.Flags.PIT:
                {
                    selectedElement = Pit;
                    break;
                }
                case MazeGenerator.Flags.KEY:
                {
                    selectedElement = Key;
                    break;
                }
                default:
                {
                    return null;
                }
            }
            return selectedElement;
        }

        public void UpdateSelectedElement()
        {
            _selectedElement = GetSelectedElement(SelectedElementFlag);
        }

        public void SetMaze(MazeGenerator.Flags[,,] maze)
        {
            _maze = maze;
        }

        private void GroupUpElements(Transform container, params Element[] elements)
        {
            foreach (var element in elements)
            {
                foreach (var transform in element.ElementTransforms)
                {
                    transform.SetParent(container);
                }
            }
        }

        public void CleanUp()
        {
            Destroy(_elementsContainer.gameObject);
        }
    }
}