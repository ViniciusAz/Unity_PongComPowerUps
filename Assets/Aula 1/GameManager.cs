using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class GameManager : MonoBehaviour {

    //how many points the player gets every time it saves the ball
    public int pointsToGrant;
    //power up prefab
    public GameObject powerUpPrefab;
    //power up list
    public List<string> powerUpList;

    //score
    private int score;
    private int scoreR;
    //canvas text
    private Text scoreText;
    private Text scoreTextR;
    //time counter
    private float timeCounter;
    //time counter for reset power up
    private float timeCounterReset;

    // Painel de final de jogo
    private Text fim;

    //Menu
    private GameObject menu;

    // Bolinha
    public GameObject[] bolinha;


    private void Awake()
    {
        //start zeroed
        score = 0;
        timeCounter = timeCounterReset = 0;
        scoreTextR = GameObject.FindGameObjectWithTag("ScoreR").GetComponent<Text>();
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        fim = GameObject.FindGameObjectWithTag("ENDGAME").GetComponent<Text>();
        menu = GameObject.FindGameObjectWithTag("Menu");
        bolinha = GameObject.FindGameObjectsWithTag("Bolinha");
    }

    // Reconhecimento de voz
    public string[] keywords = new string[] { "jogar", "menu" };
    // ConfidenceLevel confidence = ConfidenceLevel.Medium;
    ConfidenceLevel confidence = ConfidenceLevel.Low;

    public Text results;
    public Image target;

    protected PhraseRecognizer recognizer;
    protected string word = "para";


    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        //salva a palavra falada na variavel word
        word = args.text;
        switch (word)
        {
            case "jogar": 
                menu.active = false;
                for (int i = 0; i < bolinha.Length; i++)
                {
                    bolinha[i].active = false;
                }
                limpaScores();
                break;
            case "menu":
                menu.active = true;
                for (int i = 0; i < bolinha.Length; i++)
                {
                    bolinha[i].active = true;
                }
                break;
        }
    }


    // Use this for initialization
    void Start () {
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
    }
	

	// Update is called once per frame
	void Update () {
        //each 5 seconds, pop up a power up
        timeCounter += Time.deltaTime;

        if(timeCounter >= 2)
        {
            //reset the time counter
            timeCounter = 0;

            //powerUPS  --- ( Retirado por causa dos bugs <Derick> )
           // GameObject newPowerUp = Instantiate(powerUpPrefab, new Vector3(Random.Range(-14, 14), 0, Random.Range(-9, 9)), Quaternion.identity);
            //set the type as to make paddles bigger
           // newPowerUp.GetComponent<PowerUp>().powerUpType = powerUpList[Random.Range(0, powerUpList.Count)];
        }

        //if timer is set, score returns to normal after 5 seconds
        if (timeCounterReset > 0)
        {
            timeCounterReset += Time.deltaTime;
            if (timeCounterReset >= 5)
            {
                timeCounterReset = 0;
                ResetScore();
            }
        }
    }
    // ENDGAME score >= 100

    public void endGame(string ganhador)
    {
        limpaScores(); // Primeiro limpa o score
        fim.enabled = true;
        fim.text = "FIM DE JOGO!  \n O jogador " + ganhador + " é o vencedor";
    }

    // Limpa os dois scores score = 0, scoreR = 0
    public void limpaScores()
    {
        SetScore(0);
        SetScoreR(0);
        scoreText.text = score.ToString();
        scoreTextR.text = scoreR.ToString();

    }

    //update the score
    public void UpdateScore(int valor)
    {
        score += valor;
        // Tira o texto de final de jogo
        fim.enabled = false;
        //update text in canvas
        scoreText.text = score.ToString();
    }

    public void UpdateScoreR(int valor)
    {
        scoreR += valor;
        // Tira o texto de final de jogo
        fim.enabled = false;
        //update text in canvas
        scoreTextR.text = scoreR.ToString();
    }

    //timer to reset score to original value
    public void StartTimer()
    {
        timeCounterReset = Time.deltaTime;
    }

    private void ResetScore()
    {
        pointsToGrant = 10;
    }

    //getters and setters
    public int GetScore()
    {
        return score;
    }
    public int GetScoreR()
    {
        return scoreR;
    }
    public void SetScore(int value)
    {
        score = value;
    }
    public void SetScoreR(int value)
    {
        scoreR = value;
    }
}
