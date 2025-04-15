using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GridSpace;
using static UnityEngine.Rendering.DebugUI.Table;


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
    public GridRow[] gameGrid = new GridRow[3];
    //game over panel
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    //first player panel
    public GameObject firstPlayerPanel;
    public Image playerStartImage;
    public Sprite spritePlayer1;
    public Sprite spritePlayer2;

    //current active player panel
    public GameObject activePlayerPanel;
    public Image Player1Img;
    public Image Player2Img;

    private ButtonState gameWonPlayer;
    private ButtonState PlayerSide;

    private void Awake()
    {
        //disable player turn images
        Player1Img.enabled = false;
        Player2Img.enabled = false;

        gameOverPanel.SetActive(false);
        SetGameControlelrReferenceButtons();
        PlayerSide = (ButtonState)Random.Range(1, System.Enum.GetValues(typeof(ButtonState)).Length);
       // PlayerSide = ButtonState.Player1;
    }

    private void Start()
    {
        firstPlayerPanel.SetActive(true);
        playerStartImage.sprite = PlayerSide == ButtonState.Player1 ? spritePlayer1 : PlayerSide == ButtonState.Player2 ? spritePlayer2 : null;
        SetActivePlayerImage();
    }

    void SetGameControlelrReferenceButtons()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                GridSpace tempGridSpace = gameGrid[row].gridSpaces[col];
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
        if(CheckGameOver())
        {
            //run Game Over code
            GameOver();
            return;
        }

        PlayerSide = (PlayerSide == ButtonState.Player1) ? ButtonState.Player2 : ButtonState.Player1;
        SetActivePlayerImage();
    }

    public bool CheckGameOver()
    {
        return WinCheckVertical() || WinCheckHorizontal() || WinCheckDiagonally() || GameDraw();
    }

    //check for win vertically
    public bool WinCheckVertical()
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameGrid[0].gridSpaces[i].state == PlayerSide && gameGrid[1].gridSpaces[i].state == PlayerSide && gameGrid[2].gridSpaces[i].state == PlayerSide)
            {
                gameWonPlayer = PlayerSide;
                return true;
            }
        }
        return false;
    }

    //check for win horizontally
    public bool WinCheckHorizontal()
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameGrid[i].gridSpaces[0].state == PlayerSide && gameGrid[i].gridSpaces[1].state == PlayerSide && gameGrid[i].gridSpaces[2].state == PlayerSide)
            {
                gameWonPlayer = PlayerSide;
                return true;
            }
        }
        return false;
    }

    //check for win diagonally
    public bool WinCheckDiagonally()
    {
        if (gameGrid[0].gridSpaces[0].state == PlayerSide && gameGrid[1].gridSpaces[1].state == PlayerSide && gameGrid[2].gridSpaces[2].state == PlayerSide)
        {
            gameWonPlayer = PlayerSide;
            return true;
        }

        if (gameGrid[2].gridSpaces[0].state == PlayerSide && gameGrid[1].gridSpaces[1].state == PlayerSide && gameGrid[0].gridSpaces[2].state == PlayerSide)
        {
            gameWonPlayer = PlayerSide;
            return true;
        }
        return false;
    }

    public bool GameDraw()
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

    // when game over disable all buttons and display won player
    public void GameOver()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                gameGrid[row].gridSpaces[col].button.interactable = false;
            }
        }

        //enable game over panel
        gameOverPanel.SetActive(true);
        if(gameWonPlayer == ButtonState.Player1)
        {
            gameOverText.text = "X Wins!!";
        }
        else if(gameWonPlayer == ButtonState.Player2)
        {
            gameOverText.text = "O Wins!!";
        }
        else if(gameWonPlayer == ButtonState.None)
        {
            gameOverText.text = "Draw!!";
        }
        
    }

    public void QuiGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    //close first player panel
    public void CloseFirstPlayerPanel()
    {
        //enable player turn images
        Player1Img.enabled = true;
        Player2Img.enabled = true;

        firstPlayerPanel.SetActive(false);
    }

    void SetActivePlayerImage()
    {
        if(PlayerSide==ButtonState.Player1)
        {
            Player1Img.color = Color.white;
            Player2Img.color = new Color(0.6f, 0.6f, 0.6f);
        }
        else
        {
            Player2Img.color = Color.white;
            Player1Img.color = new Color(0.6f, 0.6f, 0.6f);
        }
    }
}
