using UnityEngine;
using static GameController;
using static GridSpace;

public class GameOverRules
{
    //Check if Game Over
    public static GameOutcome CheckGameOver(GridRow[] gameGrid, ButtonState playerSide)
    {
        GameOutcome outcome;
        outcome = WinCheckVertical(gameGrid, playerSide);
        if (outcome != GameOutcome.InProgress) return outcome;

        outcome = WinCheckHorizontal(gameGrid, playerSide);
        if (outcome != GameOutcome.InProgress) return outcome;

        outcome = WinCheckDiagonally(gameGrid, playerSide);
        if (outcome != GameOutcome.InProgress) return outcome;

        return GameDraw(gameGrid);
    }




    //check for win vertically
    public static GameOutcome WinCheckVertical(GridRow[] gameGrid, ButtonState playerSide)
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameGrid[0].gridSpaces[i].state == playerSide && gameGrid[1].gridSpaces[i].state == playerSide && gameGrid[2].gridSpaces[i].state == playerSide)
            {
                return GameOutcome.Win;
            }
        }
        return GameOutcome.InProgress;
    }

    //check for win horizontally
    public static GameOutcome WinCheckHorizontal(GridRow[] gameGrid, ButtonState playerSide)
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameGrid[i].gridSpaces[0].state == playerSide && gameGrid[i].gridSpaces[1].state == playerSide && gameGrid[i].gridSpaces[2].state == playerSide)
            {
                //gameWonPlayer = playerSide;
                return GameOutcome.Win;
            }
        }
        return GameOutcome.InProgress;
    }

    //check for win diagonally
    public static GameOutcome WinCheckDiagonally(GridRow[] gameGrid, ButtonState playerSide)
    {
        if (gameGrid[0].gridSpaces[0].state == playerSide && gameGrid[1].gridSpaces[1].state == playerSide && gameGrid[2].gridSpaces[2].state == playerSide)
        {
            // gameWonPlayer = playerSide;
            return GameOutcome.Win;
        }

        if (gameGrid[2].gridSpaces[0].state == playerSide && gameGrid[1].gridSpaces[1].state == playerSide && gameGrid[0].gridSpaces[2].state == playerSide)
        {
            //gameWonPlayer = playerSide;
            return GameOutcome.Win;
        }
        return GameOutcome.InProgress;
    }

    //CHeck if game over
    public static GameOutcome GameDraw(GridRow[] gameGrid)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gameGrid[row].gridSpaces[col].state == ButtonState.None)
                {
                    return GameOutcome.InProgress;
                }
            }
        }
        return GameOutcome.Draw;
    }
}
