using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleFoxSpirit : Enemy
{

    public List<int> playerLocation = new List<int>();
    public List<int> enemyLocationStart = new List<int>();

    public List<gameState> allActionsSequence = new List<gameState>(); // Does not change no matter what, used as a failsafe!

    public gameState curr;

    public bool justWokeUp = true;

    public class gameState
    {
        public gameState(int row, int col, int h, int g, gameState pred, List<gameState> closed, List<gameState> open)
        {
            enemyRow = row;
            enemyCol = col;
            gScore = g;
            heuristic = h;
            predecessor = pred;

            openList = open;
            closedList = closed;

        }

        public int heuristic;
        public int gScore;
        public int enemyRow;
        public int enemyCol;

        public gameState predecessor;

        public List<gameState> openList; // All the squares that are being considered to find shortest path
        public List<gameState> closedList; // Sqaures that do not have to be considered again


    }

    private void Awake()
    {
        setTimer(1.5f); // Changing the AI Timer, it should take a lot longer for this spirit to get moving
        playerLocation.Add(-1);
        playerLocation.Add(-1);
        justWokeUp = true;

        curr = new gameState(-1, -1, -1, -1, null, new List<gameState>(), new List<gameState>());

    }
    override public void Move() // A* Movement 
    {
        if (!pm.startedMoving) // Fox doesn't move until player has started to move
        {
            return;
        }


        // Have a record of where the player is, what are the walkable paths, and heuristic
        // f(n) = g(n) + h(n)
        // g(n) - Movement cost to move from starting point to any given square on the grid, following the path generated from there
        // h(n) - Estimated movement cost to move from given square on grid to final destination.
        
        // Happens if the player moves, if they don't that's great we keep going
        if (PlayerLocationChange(playerLocation) || justWokeUp){
            ResetEverything();
        }

        // Find open slot with the lowest score
        int irecord = 0;
        int minFScore = int.MaxValue;
        gameState recordBest = null;

        // Calculation loop
        for (int i = 0; i < curr.openList.Count; i++)
        {
            gameState consider = curr.openList[i];
            int newHScore = Mathf.Abs(consider.enemyRow - playerLocation[0]) + Mathf.Abs(consider.enemyCol - playerLocation[1]);
            if (newHScore + consider.gScore < minFScore)
            {
                minFScore = newHScore + consider.gScore;
                irecord = i; // We find our best open slot like this!!!
                recordBest = consider;
            }
        }

        curr.closedList.Add(curr.openList[irecord]); // Move best spot to closed list in curr
        curr.openList.RemoveAt(irecord); // Remove it from the open list in curr
        recordBest.closedList.Add(recordBest); // Add self to recordBest's closed list
        recordBest.predecessor = curr; // Just in case this isn't done lol



        // For each square T in the best spot's walkable adjacent tiles
        // If T is in closed list we continue
        // If T is not in open list we add it
        // If T is already in the open list we 

        // Down
        if (walkableTiles.Contains(manager.gridStatus[recordBest.enemyRow + 1][recordBest.enemyCol]))
        {
            bool continueBool = false;
            // Check if T is in closed list
            for (int i = 0; i < recordBest.closedList.Count; i++)
            {
                if((recordBest.closedList[i].enemyRow == recordBest.enemyRow + 1) && (recordBest.closedList[i].enemyCol == recordBest.enemyCol))
                {
                    continueBool = true;
                    //Debug.Log("here1");
                }
            }

            // It's not let's continue
            if(continueBool == false)
            {
                bool continueBool2 = false;
                int irecord2 = 0;
                // Check if T is in open list
                for (int i = 0; i < recordBest.openList.Count; i++)
                {
                    if ((recordBest.openList[i].enemyRow == recordBest.enemyRow + 1) && (recordBest.openList[i].enemyCol == recordBest.enemyCol))
                    {
                        continueBool2 = true;
                        irecord2 = i;
                    }
                }

                if (continueBool2) // is in open list, check to see if f score can be lower + update its parent potentially!
                {
                    gameState temp2State = recordBest.openList[irecord2];
                    int fScore = temp2State.heuristic + temp2State.gScore;
                    int otherfScore = temp2State.heuristic + recordBest.gScore + 1;

                    if(fScore > otherfScore)
                    {
                        recordBest.openList[irecord2].gScore = recordBest.gScore + 1;
                        recordBest.openList[irecord2].predecessor = recordBest;
                    }

                } else // not in open list, so we record its scores and add it
                {
                    int newHeuristicScore = Mathf.Abs(recordBest.enemyRow + 1 - playerLocation[0]) + Mathf.Abs(recordBest.enemyCol - playerLocation[1]); // always be an estimate
                    gameState temp2State = new gameState(recordBest.enemyRow + 1, recordBest.enemyCol, newHeuristicScore, recordBest.gScore + 1, recordBest, new List<gameState>(), new List<gameState>());
                    temp2State.closedList.Add(recordBest);
                    recordBest.openList.Add(temp2State);
                }

            }
        }

        // Up
        if (walkableTiles.Contains(manager.gridStatus[recordBest.enemyRow - 1][recordBest.enemyCol]))
        {
            bool continueBool = false;
            // Check if T is in closed list
            for (int i = 0; i < recordBest.closedList.Count; i++)
            {
                if ((recordBest.closedList[i].enemyRow == recordBest.enemyRow - 1) && (recordBest.closedList[i].enemyCol == recordBest.enemyCol))
                {
                    continueBool = true;
                    //Debug.Log("here1");
                }
            }

            // It's not let's continue
            if (continueBool == false)
            {
                bool continueBool2 = false;
                int irecord2 = 0;
                // Check if T is in open list
                for (int i = 0; i < recordBest.openList.Count; i++)
                {
                    if ((recordBest.openList[i].enemyRow == recordBest.enemyRow - 1) && (recordBest.openList[i].enemyCol == recordBest.enemyCol))
                    {
                        continueBool2 = true;
                        irecord2 = i;
                    }
                }

                if (continueBool2) // is in open list, check to see if f score can be lower + update its parent potentially!
                {
                    gameState temp2State = recordBest.openList[irecord2];
                    int fScore = temp2State.heuristic + temp2State.gScore;
                    int otherfScore = temp2State.heuristic + recordBest.gScore + 1;

                    if (fScore > otherfScore)
                    {
                        recordBest.openList[irecord2].gScore = recordBest.gScore + 1;
                        recordBest.openList[irecord2].predecessor = recordBest;
                    }

                }
                else // not in open list, so we record its scores and add it
                {
                    int newHeuristicScore = Mathf.Abs(recordBest.enemyRow - 1 - playerLocation[0]) + Mathf.Abs(recordBest.enemyCol - playerLocation[1]); // always be an estimate
                    gameState temp2State = new gameState(recordBest.enemyRow - 1, recordBest.enemyCol, newHeuristicScore, recordBest.gScore + 1, recordBest, new List<gameState>(), new List<gameState>());
                    temp2State.closedList.Add(recordBest);
                    recordBest.openList.Add(temp2State);
                }

            }
        }

        // Left
        if (walkableTiles.Contains(manager.gridStatus[recordBest.enemyRow][recordBest.enemyCol - 1]))
        {
            bool continueBool = false;
            // Check if T is in closed list
            for (int i = 0; i < recordBest.closedList.Count; i++)
            {
                if ((recordBest.closedList[i].enemyRow == recordBest.enemyRow) && (recordBest.closedList[i].enemyCol == recordBest.enemyCol - 1))
                {
                    continueBool = true;
                    //Debug.Log("here1");
                }
            }

            // It's not let's continue
            if (continueBool == false)
            {
                bool continueBool2 = false;
                int irecord2 = 0;
                // Check if T is in open list
                for (int i = 0; i < recordBest.openList.Count; i++)
                {
                    if ((recordBest.openList[i].enemyRow == recordBest.enemyRow) && (recordBest.openList[i].enemyCol == recordBest.enemyCol - 1))
                    {
                        continueBool2 = true;
                        irecord2 = i;
                    }
                }

                if (continueBool2) // is in open list, check to see if f score can be lower + update its parent potentially!
                {
                    gameState temp2State = recordBest.openList[irecord2];
                    int fScore = temp2State.heuristic + temp2State.gScore;
                    int otherfScore = temp2State.heuristic + recordBest.gScore + 1;

                    if (fScore > otherfScore)
                    {
                        recordBest.openList[irecord2].gScore = recordBest.gScore + 1;
                        recordBest.openList[irecord2].predecessor = recordBest;
                    }

                }
                else // not in open list, so we record its scores and add it
                {
                    int newHeuristicScore = Mathf.Abs(recordBest.enemyRow - playerLocation[0]) + Mathf.Abs(recordBest.enemyCol - 1 - playerLocation[1]); // always be an estimate
                    gameState temp2State = new gameState(recordBest.enemyRow, recordBest.enemyCol - 1, newHeuristicScore, recordBest.gScore + 1, recordBest, new List<gameState>(), new List<gameState>());
                    temp2State.closedList.Add(recordBest);
                    recordBest.openList.Add(temp2State);
                }

            }
        }

        // Right
        if (walkableTiles.Contains(manager.gridStatus[recordBest.enemyRow][recordBest.enemyCol + 1]))
        {
            bool continueBool = false;
            // Check if T is in closed list
            for (int i = 0; i < recordBest.closedList.Count; i++)
            {
                if ((recordBest.closedList[i].enemyRow == recordBest.enemyRow) && (recordBest.closedList[i].enemyCol == recordBest.enemyCol + 1))
                {
                    continueBool = true;
                    //Debug.Log("here1");
                }
            }

            // It's not let's continue
            if (continueBool == false)
            {
                bool continueBool2 = false;
                int irecord2 = 0;
                // Check if T is in open list
                for (int i = 0; i < recordBest.openList.Count; i++)
                {
                    if ((recordBest.openList[i].enemyRow == recordBest.enemyRow) && (recordBest.openList[i].enemyCol == recordBest.enemyCol + 1))
                    {
                        continueBool2 = true;
                        irecord2 = i;
                    }
                }

                if (continueBool2) // is in open list, check to see if f score can be lower + update its parent potentially!
                {
                    gameState temp2State = recordBest.openList[irecord2];
                    int fScore = temp2State.heuristic + temp2State.gScore;
                    int otherfScore = temp2State.heuristic + recordBest.gScore + 1;

                    if (fScore > otherfScore)
                    {
                        recordBest.openList[irecord2].gScore = recordBest.gScore + 1;
                        recordBest.openList[irecord2].predecessor = recordBest;
                    }

                }
                else // not in open list, so we record its scores and add it
                {
                    int newHeuristicScore = Mathf.Abs(recordBest.enemyRow - playerLocation[0]) + Mathf.Abs(recordBest.enemyCol + 1 - playerLocation[1]); // always be an estimate
                    gameState temp2State = new gameState(recordBest.enemyRow, recordBest.enemyCol + 1, newHeuristicScore, recordBest.gScore + 1, recordBest, new List<gameState>(), new List<gameState>());
                    temp2State.closedList.Add(recordBest);
                    recordBest.openList.Add(temp2State);
                }

            }
        }

        // Let's Move!!! We're taking recordBest's values and adding them to our own thing!

        if(recordBest.openList.Count == 0)
        {
            if(curr.predecessor != null)
            {
                recordBest = curr.predecessor;
            } else
            {
                recordBest = allActionsSequence[allActionsSequence.Count - 1];
            }

            allActionsSequence.RemoveAt(allActionsSequence.Count - 1);
            
        } else
        {
            allActionsSequence.Add(recordBest);
        }

        int movRow = row - recordBest.enemyRow;
        int movCol = col - recordBest.enemyCol;


        if (movRow == 0){ // Horizontal Movement
            //Debug.Log("Moving Hori");
            if (recordBest.enemyCol < col) // Left
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(-0.64f, 0, 0);
            } else if (recordBest.enemyCol > col) // Right
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0.64f, 0, 0);
            }
            col = recordBest.enemyCol;

        } else if (movCol == 0){ // Vertical Movement
            //Debug.Log("Moving Verti");
            if (recordBest.enemyRow < row) // Up
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.64f, 0);
            }
            else if (recordBest.enemyRow > row) // Down
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0, -0.64f, 0);
            }
            row = recordBest.enemyRow;
        }

        curr = recordBest;

        //Debug.Log(row);
        //Debug.Log(col);
        //Debug.Log("");

        return;
    }


    public void ResetEverything()
    {
        curr.openList.Clear();
        curr.closedList.Clear();
        enemyLocationStart.Add(row);
        enemyLocationStart.Add(col);
        // Get player location
        playerLocation = FindPlayer(playerLocation);


        // We haven't moved yet so our score defaults are as follows
        int heuristicScore = Mathf.Abs(enemyLocationStart[0] - playerLocation[0]) + Mathf.Abs(enemyLocationStart[1] - playerLocation[1]); // always be an estimate
        int g = 0; // + 1 from pred

        // Initial gameState
        gameState tempState = new gameState(row, col, heuristicScore, g, null, new List<gameState>(), new List<gameState>());
        curr = tempState;
        allActionsSequence.Add(curr);
        curr.closedList.Add(curr);

        // Add all walkable tiles to open list
        if (walkableTiles.Contains(manager.gridStatus[row + 1][col]))
        {
            int newHeuristicScore = Mathf.Abs(enemyLocationStart[0] + 1 - playerLocation[0]) + Mathf.Abs(enemyLocationStart[1] - playerLocation[1]); // always be an estimate
            gameState temp2State = new gameState(row + 1, col, newHeuristicScore, g + 1, curr, new List<gameState>(), new List<gameState>());
            temp2State.closedList.Add(curr);
            curr.openList.Add(temp2State);
        }
        if (walkableTiles.Contains(manager.gridStatus[row - 1][col]))
        {
            int newHeuristicScore = Mathf.Abs(enemyLocationStart[0] - 1 - playerLocation[0]) + Mathf.Abs(enemyLocationStart[1] - playerLocation[1]); // always be an estimate
            gameState temp2State = new gameState(row - 1, col, newHeuristicScore, g + 1, curr, new List<gameState>(), new List<gameState>());
            temp2State.closedList.Add(curr);
            curr.openList.Add(temp2State);
        }
        if (walkableTiles.Contains(manager.gridStatus[row][col + 1]))
        {
            int newHeuristicScore = Mathf.Abs(enemyLocationStart[0] - playerLocation[0]) + Mathf.Abs(enemyLocationStart[1] + 1 - playerLocation[1]); // always be an estimate
            gameState temp2State = new gameState(row, col + 1, newHeuristicScore, g + 1, curr, new List<gameState>(), new List<gameState>());
            temp2State.closedList.Add(curr);
            curr.openList.Add(temp2State);
        }
        if (walkableTiles.Contains(manager.gridStatus[row][col - 1]))
        {
            int newHeuristicScore = Mathf.Abs(enemyLocationStart[0] - playerLocation[0]) + Mathf.Abs(enemyLocationStart[1] - 1 - playerLocation[1]); // always be an estimate
            gameState temp2State = new gameState(row, col - 1, newHeuristicScore, g + 1, curr, new List<gameState>(), new List<gameState>());
            temp2State.closedList.Add(curr);
            curr.openList.Add(temp2State);
        }
    }

    public List<int> FindPlayer(List<int> playerLocation)
    {
        playerLocation = manager.getPlayerLocation(); // returns a size 2 array of row and col of player location
        return playerLocation;
    }

    public bool PlayerLocationChange(List<int> playerLocation)
    {

        if (justWokeUp == true)
        {
            justWokeUp = false;
            return true;
        }

        List<int> tmpList = new List<int>();
        tmpList.Add(-1);
        tmpList.Add(-1);
        tmpList = manager.getPlayerLocation();

        if ((tmpList[0] != playerLocation[0]) || (tmpList[1] != playerLocation[1])){
            return true;
        }
        return false;

    }
}
