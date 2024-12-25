using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.EventSystems;

public class NPCMovement : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
    public GameObject Fire;
    public GameObject PontoFire;
    public float vel = 2f;
    public bool move,time = false;
    public float vida = 100;
    private Animator animator;
    private Rigidbody2D rb;
    private int walkEHash = Animator.StringToHash("walkE");
    private int dieEHash = Animator.StringToHash("dieE");
    private int jumpEHash = Animator.StringToHash("jumpE");
    private int attackEHash = Animator.StringToHash("attackE");
    private int idleEHash = Animator.StringToHash("idle");
    public int count = 500;
    private int jumpForce = 90;
    public int jump = 2;
    private bool canAttack = false;
    public float attackCooldown = 3.0f;
    private float moveRange = 8f;
    public float attackTimer = 0f;
    public bool jumpBool = false;
    private float distance;
    public float timeNew = 0.5f;
    public float timeDeath = 0.2f;
    public bool isDead = false;
    public GameObject barraVida;
    public Recompensas recompenas;
    private float timeAttack = 0.5f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            
            if (vida <= 0)
            {
                isDead = true;
                dieNPC();
            }

            vira();

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
                attackTimer = AttackNPC(attackTimer);
                time = true;
            }
            else
            {
                canAttack = false;
            }

            if (time)
            {
                timeNew = ObjectFire(timeNew);
            }
            
        }
        else
        {
            dieNPC();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("floor"))
        {
            jumpBool = true;           
        }

        if (collision.CompareTag("AttackFire"))
        {
            tomarDano(5);
        }

        if (collision.CompareTag("damage"))
        {
            tomarDano(2);
            jumpBool = true;
            if (canAttack)
            {
                JumpForce();
            }
            MoveNPC();
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("damage"))
        {
            timeAttack -= Time.deltaTime;
            if (timeAttack < 0)
            {
                vida -= 2;
                timeAttack = 0.5f;
            }
        }

        if (collision.CompareTag("floor"))
        {
            jumpBool = true;
            JumpForce();
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
        }
    }

    private void JumpForce()
    {
        animator.SetBool(jumpEHash, jumpBool);
        animator.SetBool(walkEHash, false);
        animator.SetBool(attackEHash, false);
        animator.SetBool(idleEHash, false); 
        rb.AddForce(new Vector2(0, jumpForce*Time.deltaTime), ForceMode2D.Impulse);
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

    private void dieNPC()
    {
        animator.SetBool(dieEHash, true);
        animator.SetBool(jumpEHash, false);
        animator.SetBool(walkEHash, false);
        animator.SetBool(attackEHash, false);
        move = false;
        
        timeDeath-=0.02f;
        if (timeDeath < 0)
        {
            OnEnemyDeath(); 
            recompenas.InstantiateLife();
            recompenas.InstantiateFoods();
        }
    }

    public void OnEnemyDeath()
    {
        barraVida.SetActive(false);
        Enemy.SetActive(false);   
    }

    void vira()
    {
        
        if (transform.position.x < Player.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (transform.position.x > Player.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    public void MoveNPC()
    {
        float moveDirection = 0;
        if (transform.position.x < Player.transform.position.x)
        {
            moveDirection += 1;
        }
        else if (transform.position.x > Player.transform.position.x)
        {
            moveDirection -= 1;
        }

        rb.velocity = new Vector3(moveDirection*vel, rb.velocity.y, 0f);
    }
    

    private float CalcDistance()
    {
        distance = Vector2.Distance(Enemy.transform.position, Player.transform.position);
        return distance;
    }
    private float AttackNPC(float attackTimer)
    {
        attackTimer -= 0.02f;
        if (attackTimer <= 0)
        {
            animator.SetBool(jumpEHash, false);
            animator.SetBool(walkEHash, false);
            animator.SetBool(attackEHash, canAttack);
            animator.SetBool(idleEHash, false);
            attackTimer = 6.0f;
            NewMethod();
        }
       
        void NewMethod()
        {
            Instantiate(Fire, PontoFire.transform.position, PontoFire.transform.rotation);
        }
        return attackTimer;
    }

    private float ObjectFire(float timeNew)
    {
        timeNew -= 0.0001f;
        if (timeNew <= 0)
        {
            Destroy(Fire);
            timeNew = 0.5f;
            time=false;
        }
        return timeNew;
    }
}
