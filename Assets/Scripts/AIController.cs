using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GameController;
using static GridSpace;

public struct MoveResult
{
    public int row;
    public int col;

    public MoveResult(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
}

public class AIController
{
    MoveResult minMaxResult;
    public static MoveResult AiMove(GridRow[] gameGrid, ButtonState playerSide)
    {
        ///call functions based on requirements
        /// this will control the AI difficult
        /// Eg use all random for easy and random with min max sometimes and only min max function based on difficulty
        /// remember to configure later

        if (IsFirstPlayer(gameGrid))
        {
            return RandomAvailableMove(gameGrid, playerSide);
        }
        return AIBestMove(gameGrid, playerSide);
    }

    public static MoveResult AIBestMove(GridRow[] gameGrid, ButtonState playerSide)
    {
        GridRow[] tempGrid = gameGrid;
        MoveResult BestMove = new MoveResult();
        int bestScore = -1000;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (tempGrid[row].gridSpaces[col].state == ButtonState.None)
                {
                    tempGrid[row].gridSpaces[col].state = ButtonState.Player2;
                    int tempscore = MinMaxAlgorithm(tempGrid, 0, playerSide == ButtonState.Player1 ? ButtonState.Player2 : ButtonState.Player1);//change this later i think best move will be called only once so just need to send player 2 here
                    tempGrid[row].gridSpaces[col].state = ButtonState.None;

                    if (tempscore > bestScore)
                    {
                        bestScore = tempscore;
                        BestMove.row = row;
                        BestMove.col = col;
                    }
                }
            }
        }
        return BestMove;
    }

    //Min Max Algorithm 
    public static int MinMaxAlgorithm(GridRow[] tempGrid, int depth, ButtonState playerSide)
    {
        GameOutcome gameOutcome = GameOverRules.CheckGameOver(tempGrid, playerSide);

        if (gameOutcome == GameOutcome.Draw) return 0;
        if (gameOutcome == GameOutcome.Win) return playerSide == ButtonState.Player1 ? -1 : 1;

        //minimising player
        if (playerSide == ButtonState.Player1)
        {
            int bestScore = 1000;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (tempGrid[row].gridSpaces[col].state == ButtonState.None)
                    {
                        tempGrid[row].gridSpaces[col].state = ButtonState.Player1;
                        int tempscore = MinMaxAlgorithm(tempGrid, depth + 1, ButtonState.Player2);
                        tempGrid[row].gridSpaces[col].state = ButtonState.None;

                        bestScore = Math.Min(bestScore, tempscore);
                    }
                }
            }
            return bestScore;
        }


        // maximising player
        if (playerSide == ButtonState.Player2)
        {
            int bestScore = -1000;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (tempGrid[row].gridSpaces[col].state == ButtonState.None)
                    {
                        tempGrid[row].gridSpaces[col].state = ButtonState.Player2;
                        int tempscore = MinMaxAlgorithm(tempGrid, depth + 1, ButtonState.Player1);
                        tempGrid[row].gridSpaces[col].state = ButtonState.None;

                        bestScore = Math.Max(bestScore, tempscore);

                    }
                }
            }
            return bestScore;
        }
        return 0;
    }

    // the below code is to give the first available space
    public static MoveResult FirstAvailableMove(GridRow[] gameGrid, ButtonState playerSide)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gameGrid[row].gridSpaces[col].state == ButtonState.None)
                {
                    return new MoveResult(row, col);
                }
            }
        }
        Debug.LogError("No MoreMoves Available");
        return new MoveResult(-1, -1);
    }


    //Random Available Spots
    public static MoveResult RandomAvailableMove(GridRow[] gameGrid, ButtonState playerSide)
    {
        List<MoveResult> availableMoves = new List<MoveResult>();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gameGrid[row].gridSpaces[col].state == ButtonState.None)
                {
                    availableMoves.Add(new MoveResult(row, col));
                }
            }
        }

        if (availableMoves == null || availableMoves.Count == 0)
        {
            return new MoveResult(-1, -1);
        }

        int index = UnityEngine.Random.Range(0, availableMoves.Count);
        return availableMoves[index];
    }

    public static bool IsFirstPlayer(GridRow[] gameGrid)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gameGrid[row].gridSpaces[col].state != ButtonState.None)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
