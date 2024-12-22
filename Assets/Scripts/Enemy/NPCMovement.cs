using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
    public GameObject Fire;
    public GameObject PontoFire;
    public Transform spawnPoint;
    private float vel = 2f;
    private bool move,time = false;
    public float vida = 100;
    private Animator animator;
    private Rigidbody2D rb;
    private int walkEHash = Animator.StringToHash("walkE");
    private int dieEHash = Animator.StringToHash("dieE");
    private int jumpEHash = Animator.StringToHash("jumpE");
    private int attackEHash = Animator.StringToHash("attackE");
    private int idleEHash = Animator.StringToHash("idle");
    private int count = 500;
    private int jumpForce = 12;
    private int jump = 2;
    private bool canAttack = false;
    private float attackCooldown = 1.5f;
    private float moveRange = 8f;
    private float attackTimer = 0f;
    private bool jumpBool = false;
    private float distance;
    private float timeNew = 0.5f;
    public float timeDeath = 1.5f;
    public bool isDead = false;
    public GameObject barraVida;
    public Recompensas recompenas;


    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (!isDead)
        {
            if (jump == 0)
            {
                count--;
                count = count_jump(count);
            }
            if (jumpBool & !canAttack)
            {
                JumpForce();
                MoveNPC();
            }

            dieNPC(vida);

            distance = CalcDistance();

            if (distance > attackCooldown & distance < moveRange)
            {
                move = true;

                if (move & !jumpBool & !canAttack)
                {
                    MoveNPC();
                    animator.SetBool(jumpEHash, false);
                    animator.SetBool(walkEHash, move);
                    animator.SetBool(attackEHash, false);
                    animator.SetBool(idleEHash, false);
                }
            }
            else
            {
                move = false;
                animator.SetBool(jumpEHash, false);
                animator.SetBool(walkEHash, move);
                animator.SetBool(attackEHash, false);
                animator.SetBool(idleEHash, true);
            }

            if (distance < attackCooldown)
            {
                canAttack = true;
            }
            else
            {
                canAttack = false;
            }

            if (canAttack)
            {
                AttackNPC();
                time = true;

            }

            if (time)
            {
                timeNew = ObjectFire(timeNew);
            }
            
        }
        else
        {
            dieNPC(vida);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("floor"))
        {
            jumpBool = true;
            animator.SetBool(jumpEHash, jumpBool);
            animator.SetBool(walkEHash, false);
            animator.SetBool(attackEHash, false);
            animator.SetBool(idleEHash, false);
        }

        if (collision.CompareTag("AttackFire"))
        {
            tomarDano(5);
        }
    }

    public float GetVida()
    {
        return vida;
    }


    public void tomarDano(float dano)
    {
        vida-= dano;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            jumpBool = false;
            animator.SetBool(jumpEHash, jumpBool);
            animator.SetBool(walkEHash, false);
            animator.SetBool(attackEHash, false);
            animator.SetBool(idleEHash, true);
        }
        
    }
    private void JumpForce()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
    }

    private int count_jump(int count)
    {
        if (count <= 0)
        {
            count = 500;
            jump = 2;
        }
        return count;
    }


    private void dieNPC(float vida)
    {
        if (vida <= 0)
        {
            animator.SetBool(dieEHash, true);
            animator.SetBool(jumpEHash, false);
            animator.SetBool(walkEHash, false);
            animator.SetBool(attackEHash, false);
            move = false;
            isDead = true;
            timeDeath-=Time.deltaTime;
            if (timeDeath < 0)
            {
                OnEnemyDeath(); 
                recompenas.InstantiateLife();
                recompenas.InstantiateFoods();
            }
            
            return;
        }
    }

    public void OnEnemyDeath()
    {
        barraVida.SetActive(false);
        Enemy.SetActive(false);   
    }

    public void MoveNPC()
    {
        
        if (transform.position.x < Player.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            transform.Translate(new Vector2(vel * Time.deltaTime, 0));
            
        }
        else if (transform.position.x > Player.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            transform.Translate(new Vector2(vel * Time.deltaTime, 0));
        }
    }
    

    private float CalcDistance()
    {
        distance = Vector2.Distance(Enemy.transform.position, Player.transform.position);
        return distance;
    }
    private void AttackNPC()
    {
        if (canAttack)
        {
            attackTimer = attackTimer - Time.deltaTime;
            if (attackTimer <= 0)
            {
                Instantiate(Fire, PontoFire.transform.position, PontoFire.transform.rotation);
                animator.SetBool(jumpEHash, false);
                animator.SetBool(walkEHash, false);
                animator.SetBool(attackEHash, canAttack);
                animator.SetBool(idleEHash, false);
                attackTimer = attackCooldown;
            }
        }
    }

    private float ObjectFire(float timeNew)
    {
        timeNew = timeNew - Time.deltaTime;
        if (timeNew <= 0)
        {
            Destroy(Fire);
            timeNew = 0.5f;
            time=false;
        }
        return timeNew;
    }
}
