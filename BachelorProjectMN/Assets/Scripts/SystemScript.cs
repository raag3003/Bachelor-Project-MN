using UnityEngine;

public class SystemScript : MonoBehaviour
{
    public GameObject submitButton;

    public int karmaScore = 0;
    int testKarma = 0;
    public int piecesOnArticle = 0; // How many pices are on the article right now
    private int piecesNedded = 3; // How many pieces are needed to submit this article

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (testKarma != karmaScore)
        {
            Debug.Log("Karma Score: " + karmaScore);
            testKarma = karmaScore;
        }
    }

    public void AddPiece(int _karmaValue)
    {
        karmaScore += _karmaValue;
        piecesOnArticle++;

        if (piecesOnArticle == piecesNedded)
        {
            submitButton.SetActive(true); // Show the submit button if there are enough pieces on the article to submit it
            Debug.Log("You have added enough pieces to submit the article!");
        }

        Debug.Log("Added a piece with a karma value of: " + _karmaValue);
        Debug.Log("Current karma score: " + karmaScore);
    }

    public void RemovePiece(int _karmaValue)
    {
        submitButton.SetActive(false); // Hide the submit button if there are not enough pieces on the article to submit it

        karmaScore -= _karmaValue;
        piecesOnArticle--;
        Debug.Log("Removed a piece with a karma value of: " + _karmaValue);
        Debug.Log("Current karma score: " + karmaScore);
    }


    public void SubmitArticle()
    {
        /* Define the karma score thresholds for each category of article. These values can be adjusted as needed to fit the desired scoring system.
         If the karmaScore is between -... and -15 it is considered a horible article, between -14 and -7 it is considered a bad article, between -6 and 6 it is considered a neutral article,
         between 7 and 14 it is considered a solid article, and between 15 and ... it is considered a great article.
         This gives us 5 different outcomes for each article. We can change it if needed. */
        int horibleKarma = -15;
        int badKarma = -7;
        // int neutralKarma;
        int solidKarma = 7;
        int greatKarma = 15;

        
        if (karmaScore <= horibleKarma) // Fake news article
        {
            Debug.Log("This article is considered a horible article with a karma score of: " + karmaScore);
        }
        else if (karmaScore <= badKarma && karmaScore > horibleKarma) // Bad article
        {
            Debug.Log("This article is considered a bad article with a karma score of: " + karmaScore);
        }
        else if (karmaScore > badKarma && karmaScore < solidKarma) // Neutral article
        {
            Debug.Log("This article is considered a neutral article with a karma score of: " + karmaScore);
        }
        else if (karmaScore >= solidKarma && karmaScore < greatKarma) // Solid article
        {
            Debug.Log("This article is considered a solid article with a karma score of: " + karmaScore);
        }
        else if (karmaScore >= greatKarma) // All truth article
        {
            Debug.Log("This article is considered a great article with a karma score of: " + karmaScore);
        }
    }
}
