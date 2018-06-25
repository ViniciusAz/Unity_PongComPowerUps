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

    // Bolinha
    private GameObject bola;

    //Todas as váriaveis para o MENU
    private GameObject menu;
    private GameObject instrucoes;
    private GameObject pause;
    bool menuVisivel = true;
    bool pauseVisivel = false;
    bool instrucoesVisivel = false;

    // Travas da Bolinha
    public GameObject[] bolinha;

    //controle de paddles
    private GameObject paddleIa;
    private GameObject paddleHu;

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
        instrucoes = GameObject.FindGameObjectWithTag("Instrucoes");
        paddleIa = GameObject.FindGameObjectWithTag("PaddleIA");
        paddleHu = GameObject.FindGameObjectWithTag("PaddleR");
        instrucoes.active = false;
        pause = GameObject.FindGameObjectWithTag("Pause");
        pause.active = false;
        bola = GameObject.FindGameObjectWithTag("Bola");
    }

    // Reconhecimento de voz
    public string[] keywords = new string[] { "jogar solo", "jogar contra", "pause", "seguir", "menu", "Controle", "voltar" };
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
            case "jogar solo":   // Inicia o jogo zerado IA vs Humano
                if (menuVisivel) //Se estiver com o menu aberto
                {
                    //tem que ativar o paddle da IA
                    paddleIa.active = true;
                    //desativar o paddle HUMANO
                    paddleHu.active = false;

                    // Desativa todos os menus
                    menu.active = false;
                    instrucoes.active = false;
                    menuVisivel = false;
                    pauseVisivel = false;
                    desativaBloqueio();
                    limpaScores();
                }
                break;
            case "jogar contra":   // Inicia o jogo zerado IA vs Humano
                if (menuVisivel) //Se estiver com o menu aberto
                {
                    //ATivar o paddle humano
                    paddleIa.active = false;
                    //Desativar o paddle da IA
                    paddleHu.active = true;

                    // Desativa todos os menus
                    menu.active = false;
                    instrucoes.active = false;
                    menuVisivel = false;
                    pauseVisivel = false;
                    desativaBloqueio();
                    limpaScores();
                }
                break;
            case "pause":    //Ativa o menu, e "pausa" o jogo!
                if ((!menuVisivel) && (!pauseVisivel)) // Se estiver jogando
                {
                    pause.active = true;
                    pauseVisivel = true;
                    ativaBloqueio();
                }
                break;
            case "seguir":    //Continua o jogo com os mesmos scores
                if (pauseVisivel) // Se estiver pausado
                {
                    pause.active = false;
                    pauseVisivel = false;
                    desativaBloqueio();
                }
                break;
            case "menu":    //Volta para o menu principal
                if (pauseVisivel) // Se estiver pausado
                {
                    //Desativa o menu de pause
                    pause.active = false;
                    pauseVisivel = false;

                    //Garante que as instruções estão off 
                    instrucoes.active = false;

                    // Ativa o menu principal
                    menu.active = true;
                    menuVisivel = true;

                    ativaBloqueio();
                    limpaScores();
                }
                break;
            case "controle": //Instruções == Controles ( reconhecimento foi melhor com controles)
                if (menuVisivel) // Se um menu estiver ativo
                {
                    menu.active = false; // Destiva o menu
                    instrucoes.active = true; // ativa o menu de instruções
                    instrucoesVisivel = true;
                }else if (pauseVisivel)
                {
                       pause.active = false;
                       instrucoes.active = true; // ativa o menu de instruções
                       instrucoesVisivel = true;
                }
                break;
            case ("voltar"):
                if (instrucoesVisivel)
                {
                    //desativa as instruções
                    instrucoes.active = false;
                    instrucoesVisivel = false;
                    if (menuVisivel)   //Se entrou nas instruções pelo menu, volta ao menu
                    {
                        //ativa o menu
                        menu.active = true;
                    }
                    else if (pauseVisivel) // se entrou nas instruções pelo pause, volta ao pause
                    {
                        // ativa a pause
                        pause.active = true;
                    }
                }
                break;
        

        }
    }

    private void ativaBloqueio()
    {
        bola.transform.position = Vector3.zero;
        for (int i = 0; i < bolinha.Length; i++)
        {
            bolinha[i].active = true;
        }
    }

    private void desativaBloqueio()
    {
        for (int i = 0; i < bolinha.Length; i++)
        {
            bolinha[i].active = false;
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
