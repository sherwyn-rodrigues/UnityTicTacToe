using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Drawing;


public class GridSpace : MonoBehaviour
{
    public enum ButtonState
    {
        None,
        Player1,
        Player2
    }

    public Button button;
    public TextMeshProUGUI buttonText; //currently not being used
    public Image playerImage;
    public ButtonState state;

    //sprites for player 1 and 2 resp (need to change for themes later)
    public Sprite spritePlayer1;
    public Sprite spritePlayer2;

    //game controller reference 
    private GameController gameController;

    // on player selecting that tile
    public void SetSpace()
    {
        state = gameController.GetPlayerSide();
        playerImage.color = UnityEngine.Color.white;

        if (state == ButtonState.Player1)
        {
            playerImage.sprite = spritePlayer1;
        }
        else if(state == ButtonState.Player2)
        {
            playerImage.sprite = spritePlayer2;
        }
        button.interactable = false;
        gameController.EndTurn();
    }

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
}
