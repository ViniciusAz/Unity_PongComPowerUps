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

    //novo sistema de locomoção
    private double origem = 0, destino = 0;
    private bool chegou;
    private float posZ = 0;

    private void Awake() {
        defaultSpeed = speed;
        timeCounter = 0;
    }

    // Reconhecimento de voz
    public string[] keywords = new string[] { "a", "b", "c", "d", "1", "2", "3", "4"};
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
        //salva a palavra falada na variavel word
        word = args.text;
        //redefine os controles de movimento
        chegou = false;
        origem = destino;
        switch (word) {
            case "1":
            case "a":
                destino = 7.125;
                break;
            case "2":
            case "b":
                destino = 2.375;
                break;
            case "3":
            case "c":
                destino = -2.375;
                break;
            case "4":
            case "d":
                destino = -7.125;
                break;
        }
        if(destino == origem) chegou = true;
        //results.text = "Você disse: <b>" + word + "</b>";
    }

    private void Update() {


        speed = defaultSpeed;
        //get the collider around the paddle
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.z / 2);

        //for each collider found
        foreach (Collider col in colliders) {

//print(message: gameObject.name + " - [ " + word + " ] | Origem: " + origem + " | Destino: " + destino + " | Z: " + gameObject.transform.position.z +
//" --- DIR " + dir);// + (Vector3 * 7.125f));

            // if (speed != 0) {
		        //     switch (word) {
            //
		        //         case "1":
            //         case "a":
            //
            //             //if ()
            //
		        //             break;
		        //         case "2":
		        //         case "b":
            //         case "3":
            //         case "c":
            //             if (dir < 18) dir = dir + speed;
            //             else if (dir > -23) dir = dir - speed;
            //             else dir = dir;
		        //             break;
            //         case "4":
		        //         case "d":
		        //             if (dir > -23) dir = dir - speed;
		        //             else dir = dir;
		        //             break;
		        //         case "para":
		        //             dir = dir;
		        //             break;
            //     }
            // }

            if(destino != origem){
                if(destino > origem) {
                    if (dir < 18) dir = dir + speed;
                } else if (destino < origem /**/) {
                    if (dir > -23) dir = dir - speed;
                }

                if(!chegou){
                    posZ = (transform.position + Vector3.forward * speed * dir * Time.fixedDeltaTime).z;
                    if(destino > origem /* subindo */){
                        if(posZ > destino){
                            posZ = (float) destino;
                            chegou = true;
                            dir = 0;
                            speed = 0;
                            break;
                        }
                    } else { // destino < origem *descendo
                        if(posZ < destino){
                            posZ = (float) destino;
                            chegou = true;
                            dir = 0;
                            speed = 0;
                            break;
                        }
                    }
                }
            }
//print(message: "Preview >  " + posZ + (transform.position + Vector3.forward * speed * dir * Time.fixedDeltaTime));

            // //if it is Border Top and it is going up, stop
            // if (col.gameObject.name == "Border Top" && dir > 0) {
            //     print(message: word + "BATI NO TOP posição " + dir);
            //     speed = 0;
            //     break;
            // }
            //
            // //if it is Border Bottom and it is going down, stop
            // if (col.gameObject.name == "Border Bottom" && dir < 0) {
            //     print(message: word + "BATI NO BOT posição " + dir);
            //     speed = 0;
            //     break;
            // }
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
        //if(transform.position + Vector3.forward * speed * dir * Time.fixedDeltaTime)
        //transform.position += Vector3.forward * speed * dir * Time.fixedDeltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
    }

    //timer to reset paddle to original size
    public void StartTimer() {
        timeCounter = Time.deltaTime;
    }

    private void ResetScale() {
        transform.localScale = new Vector3(1, 1, 3);
    }
}
