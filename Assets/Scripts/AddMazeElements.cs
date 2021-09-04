using Assets.Scripts._3DMaze;
using Assets.Scripts.Classes;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class AddMazeElements : MonoBehaviour
    {
        private MazeGenerator.Flags[,,] _maze;

        private Transform _elementsContainer;

        private Camera _camera;

        private Elements _selectedElementsItem;
        private MazeGenerator.Flags _selectedElementFlag;

        private readonly MazeGenerator.Flags _elements = MazeGenerator.Flags.FAKE_TREASURE | 
            MazeGenerator.Flags.KEY | MazeGenerator.Flags.PIT | 
            MazeGenerator.Flags.TRAP | MazeGenerator.Flags.TREASURE;

        [SerializeField] 
        private LayerMask _cellsMask;

        [SerializeField]
        private LayerMask _outerWallsMask;

        [SerializeField]
        private MazeElement _key;

        [SerializeField]
        private MazeElement _pit;

        [SerializeField]
        private MazeElement _treasure;

        [SerializeField]
        private MazeElement _fakeTreasure;

        [SerializeField]
        private MazeElement _trap;

        [SerializeField]
        private MazeElement _exit;

        public Elements Keys { get; set; }
        public Elements Pits { get; set; }
        public Elements Treasures { get; set; }
        public Elements FakeTreasures { get; set; }
        public Elements Traps { get; set; }
        public Elements Exits { get; set; }

        void Start ()
        {
            _camera = Camera.main;
            _elementsContainer = new GameObject("Elements").transform;
            //GroupUpElements(_elementsContainer, Exit, Key, FakeTreasure, Treasure, Trap, Pit);
            _selectedElementFlag = 0;
            Keys = new Elements(_key.Count, _key.Transform, _key.Counter);
            Treasures = new Elements(_treasure.Count, _treasure.Transform, _treasure.Counter);
            Exits = new Elements(_exit.Count, _exit.Transform, _exit.Counter);
            FakeTreasures = new Elements(_fakeTreasure.Count, _fakeTreasure.Transform, _fakeTreasure.Counter);
            Traps = new Elements(_trap.Count, _trap.Transform, _trap.Counter);
            Pits = new Elements(_pit.Count, _pit.Transform, _pit.Counter);
            GroupUpElements(_elementsContainer, Exits, Keys, Treasures, FakeTreasures, Traps, Pits);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray =  _camera.ScreenPointToRay(Input.mousePosition);
                if (_selectedElementsItem == null) return;
                if (_selectedElementFlag == MazeGenerator.Flags.EXIT)
                {
                    if (Physics.Raycast(ray, out hit, 100, _outerWallsMask))
                        AffectExitElement(hit.transform);
                }
                else
                {
                    if (Physics.Raycast(ray, out hit, 100, _cellsMask))
                        AffectElement(hit.transform);
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
            bool exitExists = false;
            // Right and left walls
            if (objectHit.rotation.y == 0)
            {
                if(position.Y == 0)
                {
                    if (Exits.Any(x => x.Position?.Y == 0)) exitExists = true;
                    flag = MazeGenerator.Flags.WALL_LEFT;
                }
                else
                {
                    if (Exits.Any(x => x.Position?.Y == _maze.GetLength(1) - 1)) exitExists = true;
                    flag = MazeGenerator.Flags.WALL_RIGHT;
                }
            }
            // Up and down walls
            else
            {
                if (position.X == 0)
                {
                    if (Exits.Any(x => x.Position?.X == 0)) exitExists = true;
                    flag = MazeGenerator.Flags.WALL_DOWN;
                }
                else
                {
                    if (Exits.Any(x => x.Position?.X == _maze.GetLength(0) - 1)) exitExists = true;
                    flag = MazeGenerator.Flags.WALL_UP;
                }
            }

            // Add exit
            if ((_maze[position.X, position.Y, 0] & MazeGenerator.Flags.EXIT) == 0)
            {
                if (_selectedElementsItem.ElementsCount <= 0 || exitExists) return;
                
                _selectedElementsItem.DecrementCounter();
                _maze[position.X, position.Y, 0] ^= flag;
                _maze[position.X, position.Y, 0] |= MazeGenerator.Flags.EXIT;

                var element = _selectedElementsItem.First(x => x.Position == null);
                element.Transform.position = objectHit.position;
                element.Transform.gameObject.SetActive(true);
                objectHit.GetComponent<MeshRenderer>().enabled = false;
                element.Position = new Point(position.X, position.Y);
            }
            // Remove exit
            else
            {
                _maze[position.X, position.Y, 0] |= flag;
                _maze[position.X, position.Y, 0] ^= MazeGenerator.Flags.EXIT;

                var point = new Point(position.X, position.Y);
                var element = _selectedElementsItem.First(x => point.Equals(x.Position));

                element.Transform.position = objectHit.position;
                element.Transform.gameObject.SetActive(false);
                objectHit.GetComponent<MeshRenderer>().enabled = true;
                element.Position = null;
                _selectedElementsItem.IncrementCounter();
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

            if ((_maze[position.X, position.Y, 0] & _selectedElementFlag) == 0)
            {
                if((_maze[position.X, position.Y, 0] & _elements) != 0) return;

                if (_selectedElementsItem.ElementsCount <= 0) return;
                _selectedElementsItem.DecrementCounter();
                _maze[position.X, position.Y, 0] |= _selectedElementFlag;

                var element = _selectedElementsItem.First(x => x.Position == null);
                element.Transform.position = objectHit.position;
                element.Transform.gameObject.SetActive(true);
                element.Position = new Point(position.X, position.Y);
            }
            else
            {
                _selectedElementsItem.IncrementCounter();
                _maze[position.X, position.Y, 0] ^= _selectedElementFlag;

                Point point = new Point(position.X, position.Y);
                var element = _selectedElementsItem.First(x => point.Equals(x.Position));

                element.Transform.position = objectHit.position;
                element.Transform.gameObject.SetActive(false);
                element.Position = null;
            }
            //Debug.DrawRay(ray.origin, ray.direction * 500, Color.green, 15f); 
        }

        private Elements GetSelectedElement(MazeGenerator.Flags elementFlag)
        {
            Elements selectedElement;

            switch (elementFlag)
            {
                case MazeGenerator.Flags.EXIT:
                {
                    selectedElement = Exits;
                    break;
                }
                case MazeGenerator.Flags.TRAP:
                {
                    selectedElement = Traps;
                    break;
                }
                case MazeGenerator.Flags.TREASURE:
                {
                    selectedElement = Treasures;
                    break;
                }
                case MazeGenerator.Flags.FAKE_TREASURE:
                {
                    selectedElement = FakeTreasures;
                    break;
                }
                case MazeGenerator.Flags.PIT:
                {
                    selectedElement = Pits;
                    break;
                }
                case MazeGenerator.Flags.KEY:
                {
                    selectedElement = Keys;
                    break;
                }
                default:
                {
                    return null;
                }
            }
            return selectedElement;
        }

        public void UpdateSelectedElement(MazeGenerator.Flags flag)
        {
            _selectedElementFlag = flag;
            _selectedElementsItem = GetSelectedElement(_selectedElementFlag);
        }

        public void SetMaze(MazeGenerator.Flags[,,] maze)
        {
            _maze = maze;
        }

        private void GroupUpElements(Transform container, params Elements[] elements)
        {
            foreach (var element in elements)
            {
                foreach (var transform in element.Select(x => x.Transform))
                {
                    transform.SetParent(container);
                }
            }
        }

        public void CleanUp()
        {
            Destroy(_elementsContainer.gameObject);
        }

        public void SelectElement(MazeGenerator.Flags flags)
        {

        }
    }
}