﻿using Assets.Scripts._3DMaze;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DrawLab : MonoBehaviour
    {
        private int _wallsLayer;
        private int _floorLayer;
        private int _outerWallsLayer;

        [SerializeField]
        private Material GridMaterial;

        [SerializeField]
        private Transform LeftRightWall;

        [SerializeField]
        private Transform UpDownWall;

        [SerializeField]
        private Transform AboveBelowWall;

        public bool IsCeilingNeeded = true;
        public bool IsFloorNeeded = true;

        private float _cellSize;
        private float _wallWidth;
        private float _wallHeight;
        private float _outerWallWidth;

        // Use this for initialization
        void Start ()
        {
            _wallsLayer = LayerMask.NameToLayer("Walls");
            _floorLayer = LayerMask.NameToLayer("Cells");
            _outerWallsLayer = LayerMask.NameToLayer("OuterWalls");

            _wallWidth = LeftRightWall.lossyScale.x;
            _outerWallWidth = _wallWidth;
            _cellSize = LeftRightWall.lossyScale.z;
            _wallHeight = LeftRightWall.lossyScale.y + AboveBelowWall.lossyScale.y;
            UpDownWall.localScale = LeftRightWall.lossyScale;
            AboveBelowWall.localScale = 
                new Vector3(_cellSize - _wallWidth, AboveBelowWall.lossyScale.y, _cellSize - _wallWidth);
        }

        /// <summary>
        /// Draws maze
        /// </summary>
        /// <param name="maze">maze matrix</param>
        /// <param name="mazeObject">object to contain maze walls</param>
        /// <returns>Position of center of the maze</returns>
        public void Draw (MazeGenerator.Flags[,,] maze, Transform mazeObject)
        {
            float currentY = 0, currentX = 0, currentZ = 0;
            int xSize = maze.GetLength(0), ySize = maze.GetLength(1), zSize = maze.GetLength(2);
            Transform WallsContainer = new GameObject("wallsContainer").transform;

            for (int z = 0; z < zSize; z++)
            {
                currentZ = 0;
                for (int x = 0; x < xSize; x++)
                {
                    currentX = 0;
                    for (int y = 0; y < ySize; y++)
                    {
                        if ((maze[x, y, z] & MazeGenerator.Flags.WALL_UP) == MazeGenerator.Flags.WALL_UP && x != xSize - 1)
                        {
                            Transform wall = Instantiate(UpDownWall);
                            wall.position = new Vector3(currentX, currentY, currentZ + _cellSize / 2 - _wallWidth / 2);
                            wall.SetParent(WallsContainer);
                        }
                        if ((maze[x, y, z] & MazeGenerator.Flags.WALL_RIGHT) == MazeGenerator.Flags.WALL_RIGHT && y != ySize - 1)
                        {
                            Transform wall = Instantiate(LeftRightWall);
                            wall.position = new Vector3(currentX + _cellSize / 2 - _wallWidth / 2, currentY, currentZ);
                            wall.SetParent(WallsContainer);
                        }
                        if ((maze[x, y, z] & MazeGenerator.Flags.WALL_ABOVE) == MazeGenerator.Flags.WALL_ABOVE && z != zSize - 1)
                        {
                            Transform wall = Instantiate(AboveBelowWall);
                            wall.position = new Vector3(currentX, currentY + _wallHeight / 2, currentZ);
                            wall.SetParent(WallsContainer);
                        }
                        currentX += _cellSize - _wallWidth;
                        //Debug.Log(String.Format("{0}, {1}, {2}", x, y, z) + maze[x,y,z]);
                    }
                    currentZ += _cellSize - _wallWidth;
                }
                currentY += _wallHeight;
            }
            mazeObject.position = DrawOuterWalls(currentX, currentY, currentZ, xSize, ySize, zSize, WallsContainer);
            WallsContainer.SetParent(mazeObject);
        }


        /// <summary>
        /// Draw outer walls
        /// </summary>
        /// <param name="currentX">currentX position of drawing maze</param>
        /// <param name="currentY">currentY position of drawing maze</param>
        /// <param name="currentZ">currentZ position of drawing maze</param>
        /// <param name="xSize">x size of maze</param>
        /// <param name="ySize">y size of maze</param>
        /// <param name="zSize">z size of maze</param>
        /// <param name="wallsContainer">container for walls</param>
        /// <returns>Center of maze</returns>
        public Vector3 DrawOuterWalls(float currentX, float currentY, float currentZ, int xSize, int ySize, int zSize, Transform wallsContainer)
        {
            var left = Instantiate(AboveBelowWall);
            left.position = new Vector3(-_cellSize / 2 + _outerWallWidth / 2, currentY / 2 - _wallHeight / 2, 
                currentZ / 2 - _cellSize / 2 + _wallWidth / 2);
            left.localScale = new Vector3(_outerWallWidth, currentY + _outerWallWidth, 
                (_cellSize - _wallWidth) * xSize + _wallWidth);
            left.parent = wallsContainer;

            var right = Instantiate(left);
            right.position = new Vector3(currentX - _cellSize / 2 + _outerWallWidth / 2, left.position.y, left.position.z);
            right.parent = wallsContainer;

            var down = Instantiate(AboveBelowWall);
            down.position = new Vector3(currentX / 2 - _cellSize / 2 + _wallWidth / 2, currentY / 2 - _wallHeight / 2,
                -_cellSize / 2 + _outerWallWidth / 2);
            down.localScale = new Vector3(left.lossyScale.z, 
                left.lossyScale.y, left.lossyScale.x);
            down.parent = wallsContainer;

            var up = Instantiate(down);
            up.position = new Vector3(down.position.x, down.position.y, currentZ - _cellSize / 2 + _wallWidth / 2);
            up.parent = wallsContainer;

            float zPos = currentZ / 2 - _cellSize / 2 + _wallWidth / 2;

            if (IsFloorNeeded)
            {
                var bottom = Instantiate(AboveBelowWall);
                bottom.position = new Vector3(down.position.x, -_wallHeight / 2, zPos);
                bottom.localScale = new Vector3((_cellSize - _wallWidth) * ySize + _wallWidth,
                    _outerWallWidth, (_cellSize - _wallWidth) * xSize + _wallWidth);
                bottom.parent = wallsContainer;
            }

            /*if (IsCeilingNeeded)
        {
            var top = Instantiate(bottom);
            top.position = new Vector3(bottom.position.x, currentY - _wallHeight / 2, bottom.position.z);
            top.parent = wallsContainer;
            top.name = "top";
        }*/

            return new Vector3(down.position.x, left.position.y, zPos);
        }

        public void DrawGrid(int xSize, int ySize, int zSize, Transform mazeObject)
        {
            float currentY = 0, currentX = 0, currentZ = 0;
            Transform WallsContainer = new GameObject("wallsContainer").transform;

            Transform upWall = Instantiate(UpDownWall);
            upWall.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GridMaterial;
            upWall.gameObject.layer = _wallsLayer;
            //upWall.localScale = new Vector3(upWall.lossyScale.x * 2, upWall.lossyScale.y, upWall.lossyScale.z);

            Transform rightWall = Instantiate(LeftRightWall);
            rightWall.gameObject.GetComponent<MeshRenderer>().sharedMaterial = GridMaterial;
            rightWall.gameObject.layer = _wallsLayer;
            //rightWall.localScale = new Vector3(rightWall.lossyScale.x * 2, rightWall.lossyScale.y, rightWall.lossyScale.z);

            Transform floor = Instantiate(AboveBelowWall);
            floor.gameObject.layer = _floorLayer;

            Transform outerWall = Instantiate(AboveBelowWall);
            outerWall.gameObject.layer = _outerWallsLayer;
            outerWall.localScale = rightWall.lossyScale;

            for (int z = 0; z < zSize; z++)
            {
                currentZ = 0;
                for (int x = 0; x < xSize; x++)
                {

                    currentX = 0;
                    for (int y = 0; y < ySize; y++)
                    {
                        Vector2 position = new Vector2(x, y);

                        Transform wallUp;
                        if (x != xSize - 1)
                        {
                            wallUp = Instantiate(upWall);
                        }
                        else
                        {
                            wallUp = Instantiate(outerWall);
                            wallUp.rotation = Quaternion.AngleAxis(90f, Vector3.up);
                        }
                        wallUp.position = new Vector3(currentX, currentY, currentZ + _cellSize / 2 - _wallWidth / 2);
                        wallUp.GetComponent<CellPosition>().Position = position;
                        if (y == 0)
                        {
                            wallUp.localScale =
                                new Vector3(wallUp.lossyScale.x, wallUp.lossyScale.y, wallUp.lossyScale.z - _outerWallWidth);
                            Transform wallLeft = Instantiate(outerWall);
                            wallLeft.position = new Vector3(currentX - _cellSize / 2 + _wallWidth / 2, currentY, currentZ);
                            wallLeft.GetComponent<CellPosition>().Position = position;
                            wallLeft.SetParent(WallsContainer);
                        }
                        wallUp.SetParent(WallsContainer);

                        Transform wallRight = Instantiate(y != ySize - 1 ? rightWall : outerWall);
                        wallRight.position = new Vector3(currentX + _cellSize / 2 - _wallWidth / 2, currentY, currentZ);
                        wallRight.GetComponent<CellPosition>().Position = position;
                        if (x == 0)
                        {
                            wallRight.localScale = new Vector3(wallRight.lossyScale.x, wallRight.lossyScale.y,
                                wallRight.lossyScale.z - _outerWallWidth);
                            Transform wallDown = Instantiate(outerWall);
                            wallDown.rotation = Quaternion.AngleAxis(90f, Vector3.up);
                            wallDown.position = new Vector3(currentX, currentY, currentZ - _cellSize / 2 + _wallWidth / 2);
                            wallDown.GetComponent<CellPosition>().Position = position;
                            wallDown.SetParent(WallsContainer);
                        }
                        wallRight.SetParent(WallsContainer);

                        Transform wall = Instantiate(floor);
                        wall.position = new Vector3(currentX, currentY - _wallHeight / 2, currentZ);
                        wall.GetComponent<CellPosition>().Position = position;
                        wall.SetParent(WallsContainer);

                        currentX += _cellSize - _wallWidth;
                    }
                    currentZ += _cellSize - _wallWidth;
                }
                currentY += _wallHeight;
            }
            //mazeObject.position = DrawOuterWalls(currentX, currentY, currentZ, xSize, ySize, zSize, WallsContainer);
            mazeObject.position = new Vector3(currentX / 2 - _cellSize / 2 + _wallWidth / 2, currentY / 2 - _wallHeight / 2, currentZ / 2 - _cellSize / 2 + _wallWidth / 2);
            WallsContainer.SetParent(mazeObject);

            // Destroying excess walls
            Destroy(upWall.gameObject);
            Destroy(rightWall.gameObject);
            Destroy(floor.gameObject);
            Destroy(outerWall.gameObject);
        }
    }
}
