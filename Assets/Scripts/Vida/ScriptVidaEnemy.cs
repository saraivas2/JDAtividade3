using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;


public class ScriptVidaEnemy : MonoBehaviour
{
    private NPCMovement enemyScript;
    public GameObject game;
    public Transform barraVida;
    public float valor;
    public Vector3 diferenca;


    private void Start()
    {
        Vector3 vector3 = transform.position;
        Vector3 posIdle = game.transform.position;
        Vector3 diferenca = posIdle - vector3;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = game.transform.position - new Vector3(diferenca.x + 0.15f, diferenca.y - 0.3f, diferenca.z);

        enemyScript = GameObject.Find("Enemy").GetComponent<NPCMovement>();
        
        float vida = enemyScript.GetVida();
        float barra = barraVida.transform.localScale.x;

        valor = (0.5f * vida) / 100;
        if (valor < 0)
        {
            valor = 0;
        }

        barraVida.transform.localScale = new Vector3(valor, barraVida.transform.localScale.y, barraVida.transform.localScale.z);
    }
}
