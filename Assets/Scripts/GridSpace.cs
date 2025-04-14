using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GridSpace : MonoBehaviour
{
    public enum ButtonState
    {
        None,
        Player1,
        Player2
    }

    public Button button;
    public TextMeshProUGUI buttonText;
    public ButtonState state;

    //game controller reference 
    private GameController gameController;

    public void SetSpace()
    {
        state = gameController.GetPlayerSide();
        buttonText.text = "O";
        button.interactable = false;
        gameController.EndTurn();
    }

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
}
