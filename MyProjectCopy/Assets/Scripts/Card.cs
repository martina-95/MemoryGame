using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int cardID; //id na karta
    public GameManagerCard gameManager;
    public bool isflipped;
    public UnityEngine.UI.Image cardImage;

    void Start()
    {
        isflipped = false;
        cardImage.sprite = GameManagerCard.Instance.cardBack; 
    }

    public void FlipCard()
    {
        if ((!isflipped && gameManager.firstCard == null) || (!isflipped && gameManager.secondCard == null))
        {
            isflipped = true;
            cardImage.sprite = gameManager.cardFaces[cardID];
            gameManager.CardFlipped(this);
        }
    }
        
    public void HideCard()
    {
        isflipped=false;
        cardImage.sprite = gameManager.cardBack; 
    }
    public void ShowFace()
    {
        isflipped = true;
        cardImage.sprite = gameManager.cardFaces[cardID];
        
       
    }


}
