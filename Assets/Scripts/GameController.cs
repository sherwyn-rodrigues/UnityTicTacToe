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
        Debug.Log("End Turn Code");
    }
}
