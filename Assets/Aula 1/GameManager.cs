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

    // Bolinha
    private GameObject bola;

    //Todas as váriaveis para o MENU
    private GameObject menu;
    private GameObject instrucoes;
    private GameObject pause;
    private GameObject egb; //End Game Branco
    private GameObject egp; //End Game Preto
    bool menuVisivel = true;
    bool pauseVisivel = false;
    bool instrucoesVisivel = false;
    bool fimDeJogo = false;

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
        menu = GameObject.FindGameObjectWithTag("Menu");
        bolinha = GameObject.FindGameObjectsWithTag("Bolinha");
        instrucoes = GameObject.FindGameObjectWithTag("Instrucoes");
        paddleIa = GameObject.FindGameObjectWithTag("PaddleIA");
        paddleHu = GameObject.FindGameObjectWithTag("PaddleR");
        instrucoes.active = false;
        pause = GameObject.FindGameObjectWithTag("Pause");
        pause.active = false;
        bola = GameObject.FindGameObjectWithTag("Bola");
        egb = GameObject.FindGameObjectWithTag("EndGameB");
        egb.active = false;
        egp = GameObject.FindGameObjectWithTag("EndGameP");
        egp.active = false;
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
                if (pauseVisivel && (!instrucoesVisivel)) // Se estiver pausado e não estiver nos controles!
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
                }else if (fimDeJogo)
                {
                    // Ativa o menu principal
                    menu.active = true;
                    menuVisivel = true;

                    //Tira a tela de fim de jogo
                    egb.active = false;
                    egp.active = false;
                    fimDeJogo = false;

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
            if (!(menuVisivel || pauseVisivel || fimDeJogo))
            { // Cria as moedas de forma random se não tem menu ativo
                GameObject newPowerUp = Instantiate(powerUpPrefab, new Vector3(Random.Range(-14, 14), 0, Random.Range(-9, 9)), Quaternion.identity);
            }
        }

        //if timer is set, score returns to normal after 5 seconds
        if (timeCounterReset > 0)
        {
            timeCounterReset += Time.deltaTime;
            if (timeCounterReset >= 5)
            {
                timeCounterReset = 0;
                //ResetScore();
            }
        }
    }
    // ENDGAME score >= 100

    public void endGame(bool ganhador) // True == jogador branco ganhou, false == jogador preto ganhou
    {
        limpaScores(); // Primeiro limpa o score
        ativaBloqueio(); //Ativa o bloqueio da bolinha, fazendo ela ficar parada

        //Tira qualquer tela possivel
        menu.active = false;
        instrucoes.active = false;
        pause.active = false;
        menuVisivel = false;
        pauseVisivel = false;
        instrucoesVisivel = false;

        if (ganhador) // Branco ganhou
        {

            // Ativa Tela de fim de jogo 
            egb.active = true;
            fimDeJogo = true;

            //Tira a tela de fim de jogo do adversário
            egp.active = false;
}
        else // preto Ganhou
        {
            // Ativa Tela de fim de jogo 
            egp.active = true;
            fimDeJogo = true;

            //Tira a tela de fim de jogo do adversário
            egb.active = false;
        }
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

        //update text in canvas
        scoreText.text = score.ToString();
    }

    public void UpdateScoreR(int valor)
    {
        scoreR += valor;

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
