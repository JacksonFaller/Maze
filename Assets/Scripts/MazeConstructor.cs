using Assets.Scripts._3DMaze;
using UnityEngine;

namespace Assets.Scripts
{
    public class MazeConstructor : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField]
        private Material WallGridMaterial;

        [SerializeField]
        private Material WallSelectedMaterial;

        [SerializeField]
        private LayerMask _wallsMask;
        // Use this for initialization

        private MazeGenerator.Flags[,,] _maze;

        void Start ()
        {
            _camera = Camera.main;
        }

        void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 500, _wallsMask))
                {
                    Transform objectHit = hit.transform;

                    if (objectHit.GetComponent<MeshRenderer>().sharedMaterial == WallSelectedMaterial)
                    {
                        objectHit.GetComponent<MeshRenderer>().sharedMaterial = WallGridMaterial;
                        Point position = objectHit.GetComponent<CellPosition>().Position;
                        if (objectHit.rotation == Quaternion.Euler(0, 0, 0))
                            _maze[position.x, position.y, 0] ^= MazeGenerator.Flags.WALL_RIGHT;
                        else
                            _maze[position.x, position.y, 0] ^= MazeGenerator.Flags.WALL_UP;
                        //Debug.Log(_maze[position.x, position.y, 0]);
                        //Debug.Log(position);
                    }
                    else
                    {
                        objectHit.GetComponent<MeshRenderer>().sharedMaterial = WallSelectedMaterial;
                        Point position = objectHit.GetComponent<CellPosition>().Position;
                        if (objectHit.rotation == Quaternion.Euler(0, 0, 0))
                            _maze[position.x, position.y, 0] |= MazeGenerator.Flags.WALL_RIGHT;
                        else
                            _maze[position.x, position.y, 0] |= MazeGenerator.Flags.WALL_UP;
                        //Debug.Log(_maze[position.x, position.y, 0]);
                    }
                    //Destroy(objectHit.gameObject);
                }
            }
        }

        public void SetMaze(MazeGenerator.Flags[,,] maze)
        {
            _maze = maze;
        }
    }
}
