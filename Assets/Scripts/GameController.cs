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
    public GameObject gameOverPanel;// game over panel 
    public GameObject mainBoardPanel;//main Tic Tac Toe board
    public GameObject mainMenuPanel;//main menu panel(The FIrst Panel Visible)
    public GameObject gameModeSelectionPanel;
    public GameObject firstPlayerPanel;//first player panel
    public GameObject activePlayerPanel;//current active player panel
    public GameObject AIDifficultyPanel;

    public Image playerStartImage;
    public Sprite spritePlayer1;
    public Sprite spritePlayer2;
    //icons and images for current active player
    public Image Player1Img;
    public Image Player2Img;
    public Image Player1Icon;
    public Image Player2Icon;

    public Image winnerImage;//used when there is a winner and not draw
    public Image winnerPlayerImage;
    public Sprite winnerSprite;
    public Sprite drawSprite;
    public Sprite player1WinnerSprite;//sprite references used to displayer winenr p1
    public Sprite player2WinnerSprite;//sprite references used to displayer winenr p2

    private ButtonState gameWonPlayer;
    private ButtonState PlayerSide;

    //enum for AI Difficulty
    public enum AIDifficulty
    {
        Easy,
        Medium,
        Hard
    }
    //enum for AI Difficulty end

    private void Awake()
    {
        SetAllPanelsInactive();
        SetGameControlelrReferenceButtons();
        PlayerSide = (ButtonState)Random.Range(1, System.Enum.GetValues(typeof(ButtonState)).Length);
    }

    //On Game Start
    private void Start()
    {
        mainMenuPanel.SetActive(true);
    }

    //SeControllerReference to each GridSpace buttons 
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

    //returns current player state (player 1 or 2)
    public ButtonState GetPlayerSide()
    {
        return PlayerSide;
    }

    //function called on gridSpace after player completes turn
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

    //Check if Game Over
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

    //CHeck if game over
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
        mainBoardPanel.SetActive(false);
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
            winnerImage.sprite = winnerSprite;
            winnerImage.enabled = true;
            winnerPlayerImage.sprite = player1WinnerSprite;
            winnerPlayerImage.enabled = true;
            //gameOverText.text = "X Wins!!";
        }
        else if(gameWonPlayer == ButtonState.Player2)
        {
            winnerImage.sprite = winnerSprite;
            winnerImage.enabled = true;
            winnerPlayerImage.sprite = player2WinnerSprite;
            winnerPlayerImage.enabled = true;
            // gameOverText.text = "O Wins!!";
        }
        else if(gameWonPlayer == ButtonState.None)
        {
            winnerImage.sprite = drawSprite;
            winnerImage.enabled = true;
            winnerPlayerImage.enabled = false;

           // gameOverText.text = "Draw!!";
        }
        
    }

    //On Quit Btn Pressed
    public void OnQuiGameBtnClicked()
    {
        Application.Quit();
    }

    //On Restart Btn pressed
    public void OnRestartBtnClicked()
    {
        ResetMainBoard();
        SetAllPanelsInactive();
        OnGameModeCoOpClicked();
    }

    //reloads whole game level
    public void OnMainMenuBtnClicked()
    {
        SceneManager.LoadScene(0);
    }

    //close first player panel
    public void CloseFirstPlayerPanel()
    {
        //enable player turn images
        Player1Img.enabled = true;
        Player2Img.enabled = true;
        Player1Icon.enabled = true;
        Player2Icon.enabled = true;
        firstPlayerPanel.SetActive(false);

        //enable main board panel
        mainBoardPanel.SetActive(true);
    }

    //show current player turn on the sides of the board
    void SetActivePlayerImage()
    {
        if(PlayerSide==ButtonState.Player1)
        {
            Player1Img.color = Color.white;
            Player1Icon.color = Color.white;
            Player2Img.color = new Color(0.6f, 0.6f, 0.6f);
            Player2Icon.color = new Color(0.6f, 0.6f, 0.6f);
        }
        else
        {
            Player2Img.color = Color.white;
            Player2Icon.color = Color.white;
            Player1Img.color = new Color(0.6f, 0.6f, 0.6f);
            Player1Icon.color = new Color(0.6f, 0.6f, 0.6f);
        }
    }

    void SetAllPanelsInactive()
    {
        gameOverPanel.SetActive(false);
        mainBoardPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        gameModeSelectionPanel.SetActive(false);
        firstPlayerPanel.SetActive(false);
        activePlayerPanel.SetActive(false);
        //disable player turn images
        Player1Img.enabled = false;
        Player2Img.enabled = false;
        Player1Icon.enabled = false;
        Player2Icon.enabled = false;
    }

    public void OnMainMenuPlayBtnClicked()
    {
        mainMenuPanel.SetActive(false);
        gameModeSelectionPanel.SetActive(true);    
    }

    public void OnGameModeCoOpClicked()
    {
        gameModeSelectionPanel.SetActive(false);
        playerStartImage.sprite = PlayerSide == ButtonState.Player1 ? spritePlayer1 : PlayerSide == ButtonState.Player2 ? spritePlayer2 : null;
        SetActivePlayerImage();
        firstPlayerPanel.SetActive(true);
    }

    public void OnGameModeBackBtnClicked()
    {
        gameModeSelectionPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void ResetMainBoard()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                GridSpace tempGridSpace = gameGrid[row].gridSpaces[col];
                tempGridSpace.state = ButtonState.None;
                tempGridSpace.playerImage.sprite = null;
                tempGridSpace.button.interactable = true;
                //reset color
                Color color = tempGridSpace.playerImage.color;
                color.a = 0f;
                tempGridSpace.playerImage.color = color;

            }
        }
    }
}
