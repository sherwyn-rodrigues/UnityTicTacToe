using UnityEngine;
using static GameController;
using static GridSpace;

public struct MoveResult
{
    public int row;
    public int col;
    public int score;

    public MoveResult(int row, int col, int score)
    {
        this.row = row;
        this.col = col;
        this.score = score;
    }
}

    public class AIController
{
    public static MoveResult AiMove(GridRow[] gameGrid, ButtonState playerSide)
    {

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gameGrid[row].gridSpaces[col].state == ButtonState.None)
                {
                    return new MoveResult(row,col,0);
                }
            }
        }
        Debug.LogError("No MoreMoves Available");
        return new MoveResult(-1,-1,0);
    }
}
