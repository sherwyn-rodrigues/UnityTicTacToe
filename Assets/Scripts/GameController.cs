using System.Collections;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameController;
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
    public GridRow[] gameGrid = new GridRow[3];
    //game over panel
    public GameObject gameOverPanel;// game over panel 
    public GameObject mainBoardPanel;//main Tic Tac Toe board
    public GameObject mainMenuPanel;//main menu panel(The FIrst Panel Visible)
    public GameObject gameModeSelectionPanel;
    public GameObject firstPlayerPanel;//first player panel
    public GameObject activePlayerPanel;//current active player panel
    public GameObject AIDifficultyPanel;
    public GameObject YouAreP1Panel;

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

    private AIDifficulty singlePlayerDifficulty;// variable to keeptrack of the single players difficulty
    private bool bIsSinglePlayer = false;

    //enum for AI Difficulty
    public enum AIDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    //enum for game state
    public enum GameOutcome
    {
        InProgress,
        Win,
        Draw
    }

    private void Awake()
    {
        SetAllPanelsInactive();
        SetGameControlelrReferenceButtons();
    }

    // randomise the player that plays first move (do it evevery time a new game is started or restarted) 
    private void RandomisePlayerToStart()
    {
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
                gameGrid[row].gridSpaces[col].SetGameControllerReference(this);
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
        GameOutcome outcome  =  GameOverRules.CheckGameOver(gameGrid, PlayerSide);

        if(outcome == GameOutcome.Win)
        {
            gameWonPlayer = PlayerSide;
            GameOver();
            return;
        }

        if(outcome == GameOutcome.Draw)
        {
            gameWonPlayer = ButtonState.None;
            GameOver();
            return;
        }

        PlayerSide = (PlayerSide == ButtonState.Player1) ? ButtonState.Player2 : ButtonState.Player1;
        SetActivePlayerImage();
        //AI Players turn
        if (bIsSinglePlayer && PlayerSide == ButtonState.Player2)
        {
            StartCoroutine(AIPlayersTurn());
        }
    }

    // when game over disable all buttons and display won player
    public void GameOver()
    {
        mainBoardPanel.SetActive(false);
        disablePlayerInput();

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
        RandomisePlayerToStart();
        ResetMainBoard();
        SetAllPanelsInactive();

        //reset board start to appropriate mode 
        if (bIsSinglePlayer) OnGameModeSinglePlayerClicked(); else OnGameModeCoOpClicked();

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
        //run this code only if its a single player mode

        if(bIsSinglePlayer && PlayerSide == ButtonState.Player2)
        {
            StartCoroutine(AIPlayersTurn());
        }
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
        AIDifficultyPanel.SetActive(false);
        YouAreP1Panel.SetActive(false);
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
        RandomisePlayerToStart();
        gameModeSelectionPanel.SetActive(false);
        playerStartImage.sprite = PlayerSide == ButtonState.Player1 ? spritePlayer1 : PlayerSide == ButtonState.Player2 ? spritePlayer2 : null;
        SetActivePlayerImage();
        firstPlayerPanel.SetActive(true);
        bIsSinglePlayer = false;
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

    //move into single player mode
    public void OnGameModeSinglePlayerClicked()
    {
        RandomisePlayerToStart();
        gameModeSelectionPanel.SetActive(false);
        AIDifficultyPanel.SetActive(true);
        bIsSinglePlayer = true;

    }

    /// 3 different functions to set 3 different difficulty levels for single player mode
    public void SetEasyDifficulty() => SetDifficulty(AIDifficulty.Easy);
    public void SetMediumDifficulty() => SetDifficulty(AIDifficulty.Medium);
    public void SetHardDifficulty() => SetDifficulty(AIDifficulty.Hard);

    /// End Of Difficulty mode buttons 

    private void SetDifficulty(AIDifficulty difficulty)
    {
        AIDifficultyPanel.SetActive(false);
        singlePlayerDifficulty = difficulty;
        YouAreP1Panel.SetActive(true);

    }

    //Play button of the you are player 1
    public void OnYouAreP1PlayBtnClicked()
    {

        playerStartImage.sprite = PlayerSide == ButtonState.Player1 ? spritePlayer1 : PlayerSide == ButtonState.Player2 ? spritePlayer2 : null;
        SetActivePlayerImage();

        YouAreP1Panel.SetActive(false);
        firstPlayerPanel.SetActive(true);
    }

    //function called when AI 
    private IEnumerator AIPlayersTurn()
    {
        disablePlayerInput();
        MoveResult aiMoveResult = AIController.AiMove(gameGrid, PlayerSide);//this will get the Ai Move 
        //Debug.Log(aiMoveResult.row + ":" + aiMoveResult.col);

        // disable player from moving and add delay before showing Ai Move
        yield return new WaitForSeconds(1f);
        gameGrid[aiMoveResult.row].gridSpaces[aiMoveResult.col].SetSpace();
        enablePlayerInput();
    }

    private void disablePlayerInput()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                gameGrid[row].gridSpaces[col].button.interactable = false;
            }
        }
    }

    private void enablePlayerInput()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if(gameGrid[row].gridSpaces[col].state == ButtonState.None)
                    gameGrid[row].gridSpaces[col].button.interactable=true;
            }
        }
    }
}
