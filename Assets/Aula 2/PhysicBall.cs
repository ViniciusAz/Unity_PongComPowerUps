using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicBall : MonoBehaviour {

    public Vector3 startForce;
    public ForceMode mode;
    private GameManager gm;
    private int pf = 30;
    // Use this for initialization
    void Start () {
        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        GetComponent<Rigidbody>().AddForce(startForce, mode);
	}
    private void OnTriggerExit(Collider other)
    {

        // Antiga classe de bola fora!

        //if leaves the boundary collider, game over
        if (other.gameObject.tag == "Boundary")
        {
          //Debug.Log("Game Over! AQUI");
           //reset the scene
          //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
          transform.position = Vector3.zero;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if it is some powerup
        if (other.gameObject.tag == "PowerUp")
        {
            other.gameObject.GetComponent<PowerUp>().DoStuff();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {   // Se o menu está ativo, a "bolinha" fica travada
        if (collision.transform.gameObject.tag == "Bolinha")
        {
            //transform.position = Vector3.zero; // ISSO AQUI TA BUGANDO A BOLINHA, ai ela só se mexe para uma posição
            // uma solução é tentar levantar as barreiras ao invés de sumir, vou ver o que fzer
            // mas ´´e aqui o bug da bolinha!
        }
        else if (collision.transform.gameObject.tag == "Paddle")
        { 
            gm.UpdateScore(1);
            if (gm.GetScore() >= pf)
            {
                
                gm.endGame("ROXO");
            }
        }
        else if (collision.transform.gameObject.tag == "PaddleR"){
            gm.UpdateScoreR(1);
            if (gm.GetScoreR() >= pf)
            {
                gm.endGame("AZUL");
            }
        }

        // Se ir para atrás dos gols adiciona ponto para o rival
        if (collision.gameObject.tag == "FundoEsquerda")
        {
            gm.UpdateScoreR(10);
            if (gm.GetScoreR() >= pf)
            {
                gm.endGame("AZUL");
            }
            transform.position = Vector3.zero;

        }
        if (collision.gameObject.tag == "FundoDireita")
        {
            gm.UpdateScore(10);
            if (gm.GetScore() >= pf)
            {
                gm.endGame("ROXO");
            }
            transform.position = Vector3.zero;

        }
    }
}
