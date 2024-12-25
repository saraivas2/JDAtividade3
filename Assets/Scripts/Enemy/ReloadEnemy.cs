using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadEnemy : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject currentEnemy;
    public NPCMovement enemyScript; 
    private float respawnDelay = 45f;
    private bool isRespawning = false;
    public GameObject barraVida;


    void Start()
    {
        
        if (currentEnemy == null)
        {
            Debug.LogError("GameObject com a tag 'enemies' não foi encontrado!");
            return;
        }

        if (enemyScript == null)
        {
            Debug.LogError("O script NPCMovement não foi encontrado no inimigo!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Nenhum objeto com a tag 'point' foi encontrado!");
            return;
        }
       
    }

    void Update()
    {
        // Verifica se o inimigo está desativado e inicia o respawn
        if (!currentEnemy.activeSelf && !isRespawning)
        {
            isRespawning = true;
            StartCoroutine(RespawnEnemy());
        }
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnDelay); 

        if (enemyScript != null)
        {
            enemyScript.isDead = false;
            enemyScript.timeDeath = 0.2f;
            enemyScript.vida = 100;
            enemyScript.attackTimer = 0f;
            enemyScript.jumpBool = false;
            enemyScript.jump = 2;
            enemyScript.count = 500;
            enemyScript.move = false;
            enemyScript.time = false;
            enemyScript.timeNew = 0.5f;
            enemyScript.attackCooldown = 3.0f;
        }

        currentEnemy.transform.position = spawnPoint.position;

        currentEnemy.SetActive(true);
        barraVida.SetActive(true);
        Debug.Log("Inimigo reativado!");

        isRespawning = false; 
    }
}

