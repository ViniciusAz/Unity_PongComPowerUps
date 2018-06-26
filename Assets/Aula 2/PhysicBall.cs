using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicBall : MonoBehaviour {

    public Vector3 startForce;
    public ForceMode mode;
    private GameManager gm;
    private int pf = 30;
    private int bateu = 0; // 1 == jogador branco bateu na bolinha, 2 == jogador  bateu na bolinha, 0 == ninguem bateu
    private GameObject aux;
    // Use this for initialization
    void Start () {
        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        GetComponent<Rigidbody>().AddForce(startForce, mode);
	}
    private void OnTriggerExit(Collider other)
    {
        startForce = new Vector3(5, 0, 10); //Coloca a bolinha com a força original

        //if leaves the boundary collider, game over
        if (other.gameObject.tag == "Boundary")
        {
          transform.position = Vector3.zero;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        startForce = new Vector3(5, 0, 10); //Coloca a bolinha com a força original

        if (other.gameObject.tag == "PowerUp")
        {
            if (bateu == 1) // Branco bateu na bola por último e pegou uma moeda
            {
                gm.UpdateScore(2);
                other.gameObject.active = false;
                if (gm.GetScore() >= pf)
                {
                    gm.endGame(true);
                }
            }
            else if (bateu == 2) // Preto bateu na bola por último e pegou uma moeda
            {
                gm.UpdateScoreR(2);
                other.gameObject.active = false;
                if (gm.GetScore() >= pf)
                {
                    gm.endGame(false);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        startForce = new Vector3(5, 0, 10); //Coloca a bolinha com a força original
        // Se o menu está ativo, a "bolinha" fica travada
        if (collision.transform.gameObject.tag == "Bolinha")
        {
            transform.position = Vector3.zero;
        }
        else if (collision.transform.gameObject.tag == "Paddle")
        {
            gm.UpdateScore(1);
            bateu = 1;
            if (gm.GetScore() >= pf)
            {
                gm.endGame(true);
            }
        }
        else if ((collision.transform.gameObject.tag == "PaddleR") || (collision.transform.gameObject.tag == "PaddleIA"))
        {
            gm.UpdateScoreR(1);
            bateu = 2;
            if (gm.GetScoreR() >= pf)
            {
                gm.endGame(false);
            }
        }

        // Se ir para atrás dos gols adiciona ponto para o rival
        if (collision.gameObject.tag == "FundoEsquerda")
        {
            gm.UpdateScoreR(10);
            bateu = 0;
            if (gm.GetScoreR() >= pf)
            {
                gm.endGame(false);
            }
            transform.position = Vector3.zero;

        }
        if (collision.gameObject.tag == "FundoDireita")
        {
            gm.UpdateScore(10);
            bateu = 0;
            if (gm.GetScore() >= pf)
            {
                gm.endGame(true);
            }
            transform.position = Vector3.zero;

        }
    }
}
