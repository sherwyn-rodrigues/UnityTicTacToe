using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GridSpace;


/// <summary>
/// this class is for creating a row of 3 elemts since 2d arrays cant be serialised in inspector
/// </summary>

[System.Serializable]
public class GridRow
{
    public GridSpace[] gridSpaces = new GridSpace[3]; // One row of 3 buttons
}

// end button row class 
public class GameController : MonoBehaviour
{
    //public GridSpace[,] GridList = new GridSpace[3,3];
    public GridRow[] uiGrid = new GridRow[3];

    private ButtonState PlayerSide;

    private void Awake()
    {
        SetGameControlelrReferenceButtons();
        PlayerSide = ButtonState.Player1;
    }

    void SetGameControlelrReferenceButtons()
    {
        for(int row = 0; row < 3; row++)
        {
            for(int col = 0; col < 3; col++)
            {
                GridSpace tempGridSpace = uiGrid[row].gridSpaces[col];
                tempGridSpace.SetGameControllerReference(this);
            }
        }
    }

    public ButtonState GetPlayerSide()
    {
        return PlayerSide;
    }

    public void EndTurn()
    {
        if(PlayerSide == ButtonState.Player1)
        {
            PlayerSide = ButtonState.Player2;
            return;
        }
        else if(PlayerSide == ButtonState.Player2)
        {
            PlayerSide = ButtonState.Player1;
            return;
        }
    }
}
