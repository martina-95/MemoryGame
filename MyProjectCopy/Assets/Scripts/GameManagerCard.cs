using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.HID.HID;

public class GameManagerCard : MonoBehaviour
{

    private bool canFlip = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManagerCard Instance;
    public Card cardPrefab;
    public Sprite cardBack;
    public Sprite[] cardFaces;

    private List<Card> cards;
    private List<int> cardIDs;

    public Card firstCard, secondCard;
    public Transform cardHolder;
    public GameObject finalUI;
    public TextMeshProUGUI finalText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI pointsText;
    int points = 0;
    private int pairsMatched;
    private int totalPairs;
    private float timer;
    private bool isGameOver;
    private bool isLevelFinished;
    public float maxTime = 100f;



    private int currentLevel = 1;
    private float previewTime = 1.0f;
    private const float previewDecreasePerLevel = 0.2f;
    private const float timeDecreasePerLevel = 10f;
    private const int basePairs = 2;
    private const float baseTime = 100f;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartLevel();
    }

    void StartLevel()
    {
        cards = new List<Card>();
        cardIDs = new List<int>();
        pairsMatched = 0;
        totalPairs = basePairs + currentLevel - 1;
        timer = maxTime;
        isGameOver = false;
        isLevelFinished = false;
        CreateCards();
        ShuffleCards();
        finalUI.gameObject.SetActive(false);
        finalText.fontSize = 24;
        UpdatePointsText();

        maxTime = Mathf.Max(10f, baseTime - (currentLevel - 1) * timeDecreasePerLevel);
        previewTime = Mathf.Max(0.2f, 1.0f - (currentLevel - 1) * previewDecreasePerLevel);
      
    }




    void Update()
    {
        if (!isGameOver && !isLevelFinished)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                GameOver();
            }
        }
    }

    void LevelFinished()
    {
        isLevelFinished = true;
        finalUI.gameObject.SetActive(true);
        finalText.text = "Level " + currentLevel + " Finished!";
        finalText.alignment = TextAlignmentOptions.Center;

        StartCoroutine(NextLevelDelay());
    }

    IEnumerator NextLevelDelay()
    {
        yield return new WaitForSeconds(2f); 
        CleanupLevel();
        currentLevel++;
        StartLevel();
    }
    void CleanupLevel()
    {
        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();
        cardIDs.Clear();
    }


    void CreateCards()
    {
        for (int i = 0; i <totalPairs; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        foreach (int id in cardIDs)
        {
            Card newCard = Instantiate(cardPrefab, cardHolder);
            newCard.gameManager = this;
            newCard.cardID = id;
             cards.Add(newCard);
        }
       

    }

    void ShuffleCards()
    {
        for (int i = 0; i < cardIDs.Count; i++)
        {
            int randomIndex = Random.Range(i, cardIDs.Count);
            int temp = cardIDs[i];
            cardIDs[i] = cardIDs[randomIndex];
            cardIDs[randomIndex] = temp;
        }

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].cardID = cardIDs[i];
        }
    }

    public void CardFlipped(Card flippedCard)
    {
        if (!canFlip || flippedCard == firstCard) return;
        if (isGameOver) return;
        if (firstCard == null)
        {
            firstCard = flippedCard;
        }
        else if (secondCard == null)
        {
            secondCard = flippedCard;
            canFlip = false; 
            CheckMatch();
        }
    }


    void CheckMatch()
    {
        if (firstCard.cardID == secondCard.cardID)
        {
            pairsMatched++;
            points += 20;
            firstCard = null;
            secondCard = null;
            canFlip = true;

            UpdatePointsText();

            if (pairsMatched == totalPairs)
            {
                LevelFinished();
            }
        }
        else
        {
            StartCoroutine(FlipBackCards());
        }
    }

    IEnumerator FlipBackCards()
    {
        yield return new WaitForSeconds(1f);
        firstCard.HideCard();
        secondCard.HideCard();
        firstCard = null;
        secondCard = null;
        canFlip = true; 
    }


    void GameOver()
    {
        isGameOver = true;
        FinalPanel();
    }

    

    public void FinalPanel()
    {
        finalUI.gameObject.SetActive(true);
        if (isLevelFinished)
        {
            finalText.text = "Level Finished! Time Taken: " + Mathf.Round(timer) + "s";
            finalText.alignment = TextAlignmentOptions.Center;
        }
        else if (isGameOver)
        {
            finalText.text = "Game Over! Time's Up!";
            finalText.alignment = TextAlignmentOptions.Center;
           
        }
    }

    public void RestartGame()
     {
         pairsMatched = 0;
         timer = maxTime;
         isGameOver = false;
         isLevelFinished = false;
         finalUI.gameObject.SetActive(false);

         foreach (var card in cards)
         {
             Destroy(card.gameObject);
         }
         cards.Clear();
         cardIDs.Clear();

         CreateCards();
         ShuffleCards();

     }

    public void QuitNewGame()
    {
        SceneManager.LoadScene(0);
    }

  
    void UpdateTimerText()
    {
        timerText.text = "Time Left: " + Mathf.Round(timer) + "s";
    }

    void UpdatePointsText()
    {
        pointsText.text = "Points: " + points;
    }


}
