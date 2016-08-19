using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MazePuzzle : MonoBehaviour {

    private CanvasGroup puzzleGroup;
    
    private const int MAZE_WIDTH = 29;
    private const int MAZE_HEIGHT = 23;

    private Image[,] mazeWallImages;
    private byte[,] mazeWalls;
    private List<Vector3> pathMazes = new List<Vector3>();
    private Stack<Vector2> _tiletoTry = new Stack<Vector2>();
    private List<Vector2> offsets = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
    private System.Random rnd = new System.Random();
    private int _width, _height;

    // wall items
    private float wallScaleX = .25f;
    private float wallScaleY = .25f;
    private float wallOffsetX = -352;
    private float wallOffsetY = -277;

    // maze runner stuff
    private MazeRunner mazeRunner;
    ArrayList validStartingSpots = new ArrayList();

    private Vector2 _currentTile;
    public Vector2 CurrentTile {
        get { return _currentTile; }
        private set {
            if (value.x < 1 || value.x >= MAZE_WIDTH - 1 || value.y < 1 || value.y >= MAZE_HEIGHT - 1) {
                throw new ArgumentException("CurrentTile must be within the one tile border all around the maze");
            }
            if (value.x % 2 == 1 || value.y % 2 == 1) { _currentTile = value; } else {
                throw new ArgumentException("The current square must not be both on an even X-axis and an even Y-axis, to ensure we can get walls around all tunnels");
            }
        }
    }

    // Use this for initialization
    void Start() {
        puzzleGroup = this.GetComponent<CanvasGroup>();
        generateMaze();
        createRunner();
    }

    // generate maze
    private void generateMaze() {
        mazeWalls = new byte[MAZE_WIDTH, MAZE_HEIGHT];

        for(int i = 0; i < MAZE_WIDTH; i++) {
            for(int j = 0; j < MAZE_HEIGHT; j++) {
                mazeWalls[i, j] = 1;
            }
        }

        CurrentTile = Vector2.one;
        _tiletoTry.Push(CurrentTile);
        mazeWalls = createMaze();
        fillMaze();
    }

    public byte[,] createMaze() {
        //local variable to store neighbors to the current square
        //as we work our way through the maze
        List<Vector2> neighbors;
        //as long as there are still tiles to try
        while (_tiletoTry.Count > 0) {
            //excavate the square we are on
            mazeWalls[(int)CurrentTile.x, (int)CurrentTile.y] = 0;

            //get all valid neighbors for the new tile
            neighbors = GetValidNeighbors(CurrentTile);

            //if there are any interesting looking neighbors
            if (neighbors.Count > 0) {
                //remember this tile, by putting it on the stack
                _tiletoTry.Push(CurrentTile);
                //move on to a random of the neighboring tiles
                CurrentTile = neighbors[rnd.Next(neighbors.Count)];
            } else {
                //if there were no neighbors to try, we are at a dead-end
                //toss this tile out
                //(thereby returning to a previous tile in the list to check).
                CurrentTile = _tiletoTry.Pop();
            }
        }

        return mazeWalls;
    }

    /// Get all the prospective neighboring tiles
    private List<Vector2> GetValidNeighbors(Vector2 centerTile) {
        List<Vector2> validNeighbors = new List<Vector2>();

        //Check all four directions around the tile
        foreach (var offset in offsets) {
            //find the neighbor's position
            Vector2 toCheck = new Vector2(centerTile.x + offset.x, centerTile.y + offset.y);

            //make sure the tile is not on both an even X-axis and an even Y-axis
            //to ensure we can get walls around all tunnels
            if (toCheck.x % 2 == 1 || toCheck.y % 2 == 1) {
                //if the potential neighbor is unexcavated (==1)
                //and still has three walls intact (new territory)
                if (mazeWalls[(int)toCheck.x, (int)toCheck.y] == 1 && HasThreeWallsIntact(toCheck)) {
                    //add the neighbor
                    validNeighbors.Add(toCheck);
                }
            }
        }

        return validNeighbors;
    }

    /// Counts the number of intact walls around a tile
    private bool HasThreeWallsIntact(Vector2 Vector2ToCheck) {
        int intactWallCounter = 0;

        //Check all four directions around the tile
        foreach (var offset in offsets) {
            //find the neighbor's position
            Vector2 neighborToCheck = new Vector2(Vector2ToCheck.x + offset.x, Vector2ToCheck.y + offset.y);

            //make sure it is inside the maze, and it hasn't been dug out yet
            if (IsInside(neighborToCheck) && mazeWalls[(int)neighborToCheck.x, (int)neighborToCheck.y] == 1) {
                intactWallCounter++;
            }
        }

        //tell whether three walls are intact
        return intactWallCounter == 3;
    }

    private bool IsInside(Vector2 p) {
        return p.x >= 0 && p.y >= 0 && p.x < MAZE_WIDTH && p.y < MAZE_HEIGHT;
    }

    private void fillMaze() {
        GameObject mazeWall = Resources.Load("Maze/Maze Wall") as GameObject;
        BoxCollider2D mazeCollider;
        float wallPieceWidth = -1;
        float wallPieceHeight = -1;

        for (int i = 0; i < mazeWalls.GetLength(0); i++) {
            for (int j = 0; j < mazeWalls.GetLength(1); j++) {
                if(mazeWalls[i, j] == 1) {
                    //mazeWall.transform.position = ;
                    GameObject builtWall = Instantiate(mazeWall) as GameObject;
                    builtWall.transform.SetParent(transform);
                    RectTransform wallPiece = builtWall.GetComponent<RectTransform>();
                    
                    wallPiece.localScale = new Vector3(wallScaleX, wallScaleY);
                    builtWall.transform.localPosition = new Vector3(i * (wallPiece.rect.width * wallScaleX) + wallOffsetX, j * (wallPiece.rect.height * wallScaleY) + wallOffsetY);

                    mazeCollider = builtWall.GetComponent<BoxCollider2D>();
                    mazeCollider.size = new Vector2(wallPiece.rect.width, wallPiece.rect.height);

                    if (wallPieceWidth == -1) {
                        wallPieceWidth = wallPiece.rect.width;
                        wallPieceHeight = wallPiece.rect.height;
                    }
                } else if(i == 1) {
                    Vector3 validStartingSpot = new Vector3(i * (wallPieceWidth * wallScaleX) + wallOffsetX, j * (wallPieceHeight * wallScaleY) + wallOffsetY);
                    validStartingSpots.Add(validStartingSpot);
                }
            }
        }
    }

    private void createRunner() {
        GameObject mazeRunnerObj = Instantiate(Resources.Load("Maze/Maze Runner")) as GameObject;
        mazeRunnerObj.transform.SetParent(transform);

        Vector3 chosenStart = (Vector3)validStartingSpots[rnd.Next(validStartingSpots.Count)];
        mazeRunnerObj.transform.localScale = new Vector3(.15f, .15f);

        mazeRunner = mazeRunnerObj.AddComponent<MazeRunner>();
        mazeRunner.setLocation(chosenStart);

        RectTransform mazeRunnerRect = mazeRunnerObj.GetComponent<RectTransform>();
        BoxCollider2D runnerCollider = mazeRunnerObj.GetComponent<BoxCollider2D>();
        runnerCollider.size = new Vector2(mazeRunnerRect.rect.width, mazeRunnerRect.rect.height);
    }

	// Update is called once per frame
	void Update () {
        if (puzzleGroup.alpha == 1) {
            mazeRunner.updateRunner();
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "prop_powerCube") {
            Destroy(col.gameObject);
        }
    }
}
