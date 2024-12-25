using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Rendering;


public class player : MonoBehaviour
{

    private int vel = 4;
    private Vector3 posicao;
    private Rigidbody2D rb;
    private int jump = 2;
    public int forca = 80;
    private bool move, espada,death = false;
    private bool bool_jump, bool_jumpUp = false;
    private Animator animator;
    private int runPlayerHash = Animator.StringToHash("runPlayer");
    private int attack1Hash = Animator.StringToHash("Attack1");
    private int deathHash = Animator.StringToHash("death");
    private int jumpUpHash = Animator.StringToHash("jumpUp");
    private int jumpDownHash = Animator.StringToHash("jumpDown");
    private int idleHash = Animator.StringToHash("idle");
    private int attackfireplyer = Animator.StringToHash("attackfire");
    private int vida = 100;
    private float valorUp = 0;
    private bool damage = false;
    public GameObject Player;
    public GameObject Fire;
    public float radius;
    public LayerMask EnemyLayer;
    private float timeAttack = 0.2f;
    private float timeAttackFire = 0f;
    private float altura;
    public GameObject AttackFire;
    public GameObject Enemy;
    public GameObject point;
    public GameObject barraVida;
    private bool chuvafogo = false;
    private int valor = 10;
    private float tempoVidaFire = 0.3f;
    private float Updamage = 0;
    public GameOverScript gameover;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fire"))
        {
            damage = true;
            Destroy(collision.gameObject);
        }
        
        if (collision.gameObject.CompareTag("damage"))
        {
            vida -= 2;  
        }
        
        if (collision.gameObject.CompareTag("floor"))
        {
            jump = 2;
            bool_jump = false;

        }

        if (collision.gameObject.CompareTag("food"))
        {
            if (Updamage<20)
            { 
                Updamage += 1f;
                Destroy(collision.gameObject); 
            }
        }
        if (collision.gameObject.CompareTag("life"))
        {
            if (vida < 100)
            {
                vida += 20;
                Destroy(collision.gameObject);
                if (vida > 100)
                {
                    vida = 100;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
          bool_jump = true;
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
                timeAttack = 0.2f;
            }
            
        }
        
        if (collision.gameObject.CompareTag("floor"))
        {
            jump = 2; 
            bool_jump = false;
        }

    }

    private void FixedUpdate()
    {
        if (!death)
        {
            movimentPlayer();
        }
   
    }
    // Update is called once per frame
    void Update()
    {
        if (!death)
        {
            comando();
            
            altura = rb.velocity.y;

            if (bool_jump)
            {
                bool_jumpUp = jumpUp(altura);
                JumpPlayer();
            }

            
            if (Updamage > 20)
            {
                Updamage = 20;
            }
            
            if (damage)
            {
                vida = vida - 10;
                damage = false;
            }

            if (vida <= 0)
            {
                death = true;
            }

            if (bool_jump == false & move == false & espada == false & death == false)
            {
                IdlePlayer();
            }

            if (chuvafogo)
            {
                timeAttackFire -= Time.deltaTime;
                if (timeAttackFire < 0)
                {
                    valor = ChuvaFogo(valor);
                    timeAttackFire = 0.2f;
                }

                if (valor < 2)
                {   
                    chuvafogo = false;
                    valor = 8;
                    timeAttackFire = 0f;
}
            }
        }
        else
        {
            DeathPlayer();
        }
    }

    private void movePlayer()
    {
        animator.SetBool(runPlayerHash, move==true && espada==false && bool_jump==false);
        animator.SetBool(attack1Hash, false);
        animator.SetBool(jumpUpHash, false);
        animator.SetBool(jumpDownHash, false);
        animator.SetBool(deathHash, false);
        animator.SetBool(idleHash, false);
        animator.SetBool(attackfireplyer, false);
        
    }

    public float GetVida()
    {
        return vida;
    }

    private void JumpPlayer()
    {
        animator.SetBool(runPlayerHash, false);
        animator.SetBool(attack1Hash, false);
        animator.SetBool(jumpUpHash, bool_jump==true && espada==false && bool_jumpUp==true);
        animator.SetBool(jumpDownHash, bool_jump==true && espada== false && bool_jumpUp == false);
        animator.SetBool(deathHash, false);
        animator.SetBool(idleHash, false);
        animator.SetBool(attackfireplyer, false);
    }


    private void AttackPlayer()
    {
        animator.SetBool(runPlayerHash, false);
        animator.SetBool(attack1Hash, espada==true && bool_jump==false);
        animator.SetBool(jumpUpHash, false);
        animator.SetBool(jumpDownHash, false);
        animator.SetBool(deathHash, false);
        animator.SetBool(idleHash, false);
        animator.SetBool(attackfireplyer, false);

    }


    private void DeathPlayer()
    {
        barraVida.SetActive(false); 
        animator.SetBool(runPlayerHash, false);
        animator.SetBool(attack1Hash, false);
        animator.SetBool(jumpUpHash, false);
        animator.SetBool(jumpDownHash, false);
        animator.SetBool(deathHash, death==true);
        animator.SetBool(idleHash, false);
        animator.SetBool(attackfireplyer, false);

        bool resp = gameover.ShowTelaGameOver(true);

        Invoke("ReloadScene", 3f);
        
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void IdlePlayer()
    {
        animator.SetBool(runPlayerHash, false);
        animator.SetBool(attack1Hash, false);
        animator.SetBool(jumpUpHash, false);
        animator.SetBool(jumpDownHash, false);
        animator.SetBool(deathHash, false);
        animator.SetBool(idleHash, true);
        animator.SetBool(attackfireplyer, false);

    }
    private void AttackFirePlayer()
    {
        animator.SetBool(runPlayerHash, false);
        animator.SetBool(attack1Hash, false);
        animator.SetBool(jumpUpHash, false);
        animator.SetBool(jumpDownHash, false);
        animator.SetBool(deathHash, false);
        animator.SetBool(idleHash, false);
        animator.SetBool(attackfireplyer, true);
    }
    private bool jumpUp(float altura)
    {
        if (altura > valorUp)
        {
            valorUp = altura;
            return true;
        }
        else
        {
            valorUp = 0;
            return false;
        }
    }

    public float GetPowerPlayer()
    {
        return Updamage;
    }
    private int ChuvaFogo(int valor)
    {
        float val = 0.5f;
        int n = valor;
        for (int i = 0; i < n; i++)
        {
            val = val * -1;
            Vector3 pointVector = point.transform.position;
            Instantiate(AttackFire, new Vector3(pointVector.x + val, pointVector.y, pointVector.z), Quaternion.identity);

            if (val > 0)
            {
                val += 0.3f;
            }
            else
            {
                val -= 0.3f;
            }
        }
        return valor-1;
    }

    private void OnDrawGizmos()
    {
        if (espada)
        {
            Gizmos.color = Color.red;
            Vector3 attackPosition = Player.transform.position + transform.right * 0.5f;
            Gizmos.DrawWireSphere(attackPosition, radius);

        }
    }
    void movimentPlayer()
    {
        float moveDirection = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = 1;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            move = true;
            movePlayer();

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = -1;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            move = true;
            movePlayer();

        }
        else
        {
            move = false;
        }

        rb.velocity = new Vector2(moveDirection * vel, rb.velocity.y);
    }
    
    void comando()
    {
      if (Input.GetKeyDown(KeyCode.UpArrow) & (jump > 0 & jump <= 2))
        {
            jump--;
            bool_jump = true; 
            rb.AddForce(new Vector2(0, forca*Time.fixedDeltaTime), ForceMode2D.Impulse);
                
        }
        else
        {
            bool_jump = false;
            
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            espada = true;
            AttackPlayer();
            timeAttack -= Time.deltaTime;

            if (timeAttack < 0)
            {
                Vector3 attackPosition = Player.transform.position + transform.right * 0.5f; 
                Collider2D[] isAttack = Physics2D.OverlapCircleAll(attackPosition, radius, EnemyLayer);

                foreach (Collider2D col in isAttack)
                {
                    if (col.CompareTag("enemies"))
                    {
                        col.transform.GetComponent<NPCMovement>().tomarDano(5 + (Updamage / 4));
                    }
                }

                timeAttack = 0.2f;
            }
        }
        else
        {
            espada = false;
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (Updamage >=20 )
            {
                Updamage -= 19;
                vida -= 10;
                AttackFirePlayer();
                chuvafogo = true;
                
            }
        }
    }    
}
