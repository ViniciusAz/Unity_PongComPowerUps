using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class PlayerPaddle : MonoBehaviour {

    public float speed = 1;
    //default speed
    private float defaultSpeed;
    //direction
    private float dir;
    //time counter
    private float timeCounter;

    private void Awake() {
        defaultSpeed = speed;
        timeCounter = 0;
    }

    // Reconhecimento de voz
    public string[] keywords = new string[] { "a", "b", "c", "d",
	 																						"1", "2", "3", "4"};
    // ConfidenceLevel confidence = ConfidenceLevel.Medium;
    ConfidenceLevel confidence = ConfidenceLevel.Low;

    public Text results;
    public Image target;

    protected PhraseRecognizer recognizer;
    protected string word = "para";

    private void Start() {
        if (keywords != null) {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args) {
        word = args.text;
        //results.text = "Você disse: <b>" + word + "</b>";
    }

    private void Update() {

        speed = defaultSpeed;
        //get the collider around the paddle
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.z / 2);

        //for each collider found
        foreach (Collider col in colliders) {
            if (speed != 0) {
		            print(message: word + " posição " + dir);
		            switch (word) {

                    //AQUI TA O PROBLEMA, O METODO DE PARADA ATUAL
                    //É VIA COLLIDER TOP/Bottom, TEM QUE MUDAR
                    //COMECEI A VERIFICAR O POSICIONAMENTO PELO
                    //transform.position.z MAS ATE ENTAO ELE NAO
                    //TA PARANDO 100%, O RESTO ESTA INTERFERINDO...
                    //TEM QUE REESCREVER ESSA PARTE TODA
                    //PRA ELE N USAR MAIS COLLIDERS PRO PADDLE
                    // E CONSEGUIR POSICIONAR NOS LUGARES PREDEFINIDOS
                    // 7.125 | 2.375 | -2.375 | -7.125
                    //  1/A     2/B      3/C     4/D

                    //DIFICULDADE DE RECONHECIMENTO PARA
                    // VOGAIS "e" E "i" (QUASE SEMPRE CAPTA "i")


		                case "1":
                    case "a":

                        if(transform.position.z < 7.125) dir = dir + speed;
                        else if (transform.position.z < 7.125) dir = dir + speed;
                        // if (dir < 18) dir = dir + speed;
		                    else dir = dir;
		                    break;
		                case "2":
		                case "b":
                    case "3":
                    case "c":
                        if (dir < 18) dir = dir + speed;
                        else if (dir > -23) dir = dir - speed;
                        else dir = dir;
		                    break;
                    case "4":
		                case "d":
                        if(transform.position.z > -7.125) dir -= speed;
		                    // if (dir > -23) dir = dir - speed;
		                    else dir = dir;
		                    break;
                    default:
                        dir = dir;
                        break;
		                case "para":
		                    dir = dir;
		                    break;
                }
            }

            print(message: word + " posição " + transform.position);

            // if (transform.position.z < 7.225 && transform.position.z > 7.025) {
            //   speed = 0;
            //   break;
            // }

            //if it is Border Top and it is going up, stop
            if (col.gameObject.name == "Border Top" && dir > 0) {
                print(message: word + "BATI NO TOP posição " + dir);
                speed = 0;
                break;
            }

            //if it is Border Bottom and it is going down, stop
            if (col.gameObject.name == "Border Bottom" && dir < 0) {
                print(message: word + "BATI NO BOT posição " + dir);
                speed = 0;
                break;
            }
        }

        //if timer is set, paddle returns to normal after 5 seconds
        if(timeCounter > 0) {
            timeCounter += Time.deltaTime;
            if(timeCounter >= 5) {
                timeCounter = 0;
                ResetScale();
            }
        }
    }

    void FixedUpdate() {
        transform.position += Vector3.forward * speed * dir * Time.fixedDeltaTime;
    }

    //timer to reset paddle to original size
    public void StartTimer() {
        timeCounter = Time.deltaTime;
    }

    private void ResetScale() {
        transform.localScale = new Vector3(1, 1, 3);
    }
}
