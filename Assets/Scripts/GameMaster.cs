using Assets.Scripts.Menu;
using Assets.Scripts._3DMaze;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(DrawLab), typeof(AudioSource))]
    public class GameMaster : MonoBehaviour
    {
        private MazeGenerator.Flags[,,] _mazeA;
        private MazeGenerator.Flags[,,] _mazeB;
        private Transform mazeATransform;
        private Transform mazeBTransform;

        private MazeGenerator _mazeGenerator;
        private DrawLab _drawLab;
        private DrawMap _drawMap;
        private AudioSource _audioSource;
        
        [SerializeField]
        private PauseMenu _pauseMenu;

        [SerializeField]
        private AddMazeElements _mazeElements;

        [SerializeField]
        private MazeConstructor _mazeConstructor;

        [SerializeField]
        private GameObject _mainCamera;

        [SerializeField]
        private GameObject _constructorCamera;

        [SerializeField]
        private bool _autoGenerate;

        [SerializeField]
        private Transform _character;

        [SerializeField]
        private Transform _mazeMap;

        [SerializeField]
        [Range(1, 20)]
        private int MazeSizeX;

        [SerializeField]
        [Range(1, 20)]
        private int MazeSizeY;

        [Range(1, 20)]
        private int MazeSizeZ = 1;

        void Awake()
        {
            _drawLab = GetComponent<DrawLab>();
            _drawMap = _mazeMap.GetComponent<DrawMap>();
            _audioSource = GetComponent<AudioSource>();
        }

        // Use this for initialization
        void Start ()
        {
            _pauseMenu.PauseEvent += PauseEvent;
            mazeBTransform = new GameObject("MazeB").transform;
            if (!_autoGenerate)
            {
                DrawGrid();
                _mazeElements.SetMaze(_mazeB);
                _mazeConstructor.SetMaze(_mazeB);
                return;
            }

            _mazeGenerator = new MazeGenerator();
            _mazeA = _mazeGenerator.GenerateMaze(MazeSizeX, MazeSizeY, MazeSizeZ);
            _mazeB = _mazeGenerator.GenerateMaze(MazeSizeX, MazeSizeY, MazeSizeZ);
            mazeATransform = new GameObject("MazeA").transform;
            _drawLab.Draw(_mazeA, mazeATransform);
        
            DrawMazeMap();
        }

        private void PauseEvent()
        {
            Debug.Log("paused");
        }

        // Update is called once per frame
        void Update ()
        {
      
        }

        public void StartGame()
        {
            _constructorCamera.SetActive(false);
            _mainCamera.SetActive(true);
            //_mazeMap.gameObject.SetActive(true);
            Destroy(mazeBTransform.gameObject);
            DrawMazeMap();
        }

        private void DrawGrid()
        {
            _mazeB = MazeGenerator.GetEmptyMaze(MazeSizeX, MazeSizeY, MazeSizeZ);
            _drawLab.DrawGrid(MazeSizeX, MazeSizeY, MazeSizeZ, mazeBTransform);
        }

        private void DrawMazeMap()
        {
            _drawMap.DrawMap2D(_mazeB);
        }

        public void PlaySound(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}
