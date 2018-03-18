using System;
using Assets.Scripts._3DMaze;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(RectTransform))]
    public class DrawMap : MonoBehaviour
    {
        private float _cellSize;
        private readonly float _heightToWidthRatio = 3f;
        private readonly float _cellsizeToWidthRatio = 210;
        private RectTransform _mapTransform;

        [SerializeField]
        private float _wallWidth;

        [SerializeField]
        private float _wallHeight;

        private PrefabUtility pref;

        [SerializeField]
        private bool _autoFit;

        [SerializeField]
        private RectTransform _upWall;

        [SerializeField]
        private RectTransform _outerUpWall;

        [SerializeField]
        private Sprite _wallSprite;

        [SerializeField]
        private Transform _key;

        [SerializeField]
        private Transform _pit;

        [SerializeField]
        private Transform _trap;

        [SerializeField]
        private Transform _treasure;

        [SerializeField]
        private Transform _fakeTreasure;

        [SerializeField]
        private Transform _exit;

        private Transform _outerRightWall;
        private Transform _rightWall;
        private Transform _elements;
        private Transform _wallsContainer;
        private Transform _mazeB;

        void Awake()
        {
            _mapTransform = GetComponent<RectTransform>();
            _outerRightWall = Instantiate(_outerUpWall);
            _outerRightWall.Rotate(Vector3.forward, 90);
            _rightWall = Instantiate(_upWall);
            _rightWall.Rotate(Vector3.forward, 90);
            _mazeB = new GameObject("MazeB").transform;
            //_upWall = Instantiate(_upWall);
        }

        void Start()
        {
            
        }

        /// <summary>
        /// Auto fits maze map to the panel
        /// </summary>
        /// <param name="fieldSize">map panel size</param>
        /// <param name="mazeSizeX">maze x size</param>
        /// <param name="mazeSizeY">maze y size</param>
        private void AutoFit(Vector2 fieldSize, float mazeSizeX, float mazeSizeY)
        {
            float sizeX = fieldSize.x / mazeSizeX;
            float sizeY = fieldSize.y / mazeSizeY;

            _cellSize = sizeX > sizeY ? sizeY : sizeX;

            _wallWidth = _cellSize / _cellsizeToWidthRatio;
            _wallHeight = _wallWidth * _heightToWidthRatio;
        }

        public void DrawMap2D(MazeGenerator.Flags[,,] maze)
        {
            if (_autoFit)
            {
                AutoFit(_mapTransform.sizeDelta, maze.GetLength(0), maze.GetLength(1));
                _upWall.localScale = new Vector3(_wallHeight, _wallWidth, 0);
                _rightWall.localScale = _upWall.localScale;
                _outerUpWall.localScale = _upWall.localScale;
                _outerRightWall.localScale = _outerUpWall.localScale;
            }

            float currentY = 0, currentX = 0;
            int xSize = maze.GetLength(0), ySize = maze.GetLength(1);
            _wallsContainer = new GameObject("WallsContainer").transform;
            _elements = new GameObject("Elements").transform;
            //Transform _rightWall = Instantiate(_upWall);
            //_rightWall.Rotate(Vector3.forward, 90);

            float cellSize = _upWall.sizeDelta.x * (_wallHeight - _wallWidth);

            cellSize += _wallWidth;

            for (int x = 0; x < xSize; x++)
            {
                currentX = 0;
                for (int y = 0; y < ySize; y++)
                {
                    if (x == 0 && (maze[x, y, 0] & MazeGenerator.Flags.WALL_DOWN) == MazeGenerator.Flags.WALL_DOWN)
                    {
                        Transform wall = Instantiate(_outerUpWall).transform;
                        wall.position = new Vector3(currentX, currentY - cellSize / 2 + _wallWidth / 2, 0);
                        wall.SetParent(_wallsContainer);
                    }
                    if (y == 0 && (maze[x, y, 0] & MazeGenerator.Flags.WALL_LEFT) == MazeGenerator.Flags.WALL_LEFT)
                    {
                        Transform wall = Instantiate(_outerRightWall).transform;
                        wall.position = new Vector3(currentX - cellSize / 2 + _wallWidth / 2, currentY, 0);
                        wall.SetParent(_wallsContainer);
                    }

                   if ((maze[x, y, 0] & MazeGenerator.Flags.WALL_UP) == MazeGenerator.Flags.WALL_UP)
                    {
                        Transform wall = x < xSize - 1 ? Instantiate(_upWall).transform : Instantiate(_outerUpWall);
                        wall.position = new Vector3(currentX, currentY + cellSize / 2 - _wallWidth / 2, 0);
                        wall.SetParent(_wallsContainer);
                    }
                    if ((maze[x, y, 0] & MazeGenerator.Flags.WALL_RIGHT) == MazeGenerator.Flags.WALL_RIGHT)
                    {
                        Transform wall = y < xSize - 1 ? Instantiate(_rightWall).transform : Instantiate(_outerRightWall);
                        wall.position = new Vector3(currentX + cellSize / 2 - _wallWidth / 2, currentY, 0);
                        wall.SetParent(_wallsContainer);
                    }
                    Transform tr = GetObjectByFlag(maze[x, y, 0]);
                    if (tr != null)
                    {
                        tr.position = new Vector3(currentX, currentY, 0);
                        tr.SetParent(_elements);
                    }
                    currentX += cellSize - _wallWidth;
                }
                currentY += cellSize - _wallWidth;
            }
            _mazeB.position = new Vector3(currentX / 2 + _wallWidth - cellSize / 2, currentY / 2 + _wallWidth / 2 - cellSize / 2);
            _wallsContainer.SetParent(_mazeB);
            _elements.SetParent(_mazeB);
            _mazeB.position = _mapTransform.position;
            _mazeB.SetParent(_mapTransform);
        }

        private Transform GetObjectByFlag(MazeGenerator.Flags objectFlag)
        {
            if ((objectFlag & MazeGenerator.Flags.KEY) != 0)
            {
                return Instantiate(_key);
            }
            if ((objectFlag & MazeGenerator.Flags.EXIT) != 0)
            {
                return Instantiate(_exit);
            }
            if ((objectFlag & MazeGenerator.Flags.TRAP) != 0)
            {
                return Instantiate(_trap);
            }
            if ((objectFlag & MazeGenerator.Flags.TREASURE) != 0)
            {
                return Instantiate(_treasure);
            }
            if ((objectFlag & MazeGenerator.Flags.FAKE_TREASURE) != 0)
            {
                return Instantiate(_fakeTreasure);
            }
            if ((objectFlag & MazeGenerator.Flags.PIT) != 0)
            {
                return Instantiate(_pit);
            }
            return null;
        }
    }
}
