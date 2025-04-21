using UnityEngine;
using static GridSpace;

public class GameOverRules
{
    //Check if Game Over
    public static bool CheckGameOver(GridRow[] gameGrid, ButtonState playerSide)
    {
        return WinCheckVertical(gameGrid, playerSide) || WinCheckHorizontal(gameGrid, playerSide) || WinCheckDiagonally(gameGrid, playerSide) || GameDraw(gameGrid);
    }




    //check for win vertically
    public static bool WinCheckVertical(GridRow[] gameGrid, ButtonState playerSide)
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameGrid[0].gridSpaces[i].state == playerSide && gameGrid[1].gridSpaces[i].state == playerSide && gameGrid[2].gridSpaces[i].state == playerSide)
            {
                return true;
            }
        }
        return false;
    }

    //check for win horizontally
    public static bool WinCheckHorizontal(GridRow[] gameGrid, ButtonState playerSide)
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameGrid[i].gridSpaces[0].state == playerSide && gameGrid[i].gridSpaces[1].state == playerSide && gameGrid[i].gridSpaces[2].state == playerSide)
            {
                //gameWonPlayer = playerSide;
                return true;
            }
        }
        return false;
    }

    //check for win diagonally
    public static bool WinCheckDiagonally(GridRow[] gameGrid, ButtonState playerSide)
    {
        if (gameGrid[0].gridSpaces[0].state == playerSide && gameGrid[1].gridSpaces[1].state == playerSide && gameGrid[2].gridSpaces[2].state == playerSide)
        {
           // gameWonPlayer = playerSide;
            return true;
        }

        if (gameGrid[2].gridSpaces[0].state == playerSide && gameGrid[1].gridSpaces[1].state == playerSide && gameGrid[0].gridSpaces[2].state == playerSide)
        {
            //gameWonPlayer = playerSide;
            return true;
        }
        return false;
    }

    //CHeck if game over
    public static bool GameDraw(GridRow[] gameGrid)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gameGrid[row].gridSpaces[col].state == ButtonState.None)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
