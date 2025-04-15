using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GridSpace;

public class GameController : MonoBehaviour
{
    public GridSpace[] GridList;
    private ButtonState PlayerSide;

    private void Awake()
    {
        SetGameControlelrReferenceButtons();
        PlayerSide = ButtonState.Player1;
    }

    void SetGameControlelrReferenceButtons()
    {
        for(int i = 0; i < GridList.Length; i++)
        {
            GridList[i].SetGameControllerReference(this);
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
