using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class player : MonoBehaviour
{

    private int vel = 4;
    private Vector3 posicao;
    private Rigidbody2D rb;
    private int jump = 2;
    private int forca = 90;
    public bool move, espada,death = false;
    private bool bool_jump, bool_jumpUp = false;
    private Animator animator;
    private int runPlayerHash = Animator.StringToHash("runPlayer");
    private int attack1Hash = Animator.StringToHash("Attack1");
    private int deathHash = Animator.StringToHash("death");
    private int jumpUpHash = Animator.StringToHash("jumpUp");
    private int jumpDownHash = Animator.StringToHash("jumpDown");
    private int idleHash = Animator.StringToHash("idle");
    private int vida = 100;
    private float valorUp = 0;
    private bool damage = false;
    private GameObject Player;
    private GameObject Fire;
    private Collider2D isAttack;
    public float radius;
    public LayerMask EnemyLayer;
    private float timeAttack = 0.2f;
    private float timeAttackFire = 0f;
    private float altura;
    private GameObject AttackFire;
    private GameObject Enemy;
    private GameObject Enemy2;
    private GameObject point;
    private GameObject barraVida;
    private bool chuvafogo = false;
    int valor = 10;
    public float tempoVidaFire = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        animator = GetComponent<Animator>();
        Fire = Resources.Load("FireObjeto") as GameObject;
        Player = GameObject.FindWithTag("idle");
        GameObject attackObject = GameObject.FindWithTag("espada");
        isAttack=attackObject.GetComponent<Collider2D>();
        AttackFire = Resources.Load("Fogo") as GameObject;
        Enemy = GameObject.FindWithTag("enemies");
        Enemy2 = GameObject.FindWithTag("enemy2");
        point = GameObject.FindWithTag("pointAttackFire");
        barraVida = GameObject.FindWithTag("vidaIdle");
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
    }


    private void AttackPlayer()
    {
        animator.SetBool(runPlayerHash, false);
        animator.SetBool(attack1Hash, espada==true && bool_jump==false);
        animator.SetBool(jumpUpHash, false);
        animator.SetBool(jumpDownHash, false);
        animator.SetBool(deathHash, false);
        animator.SetBool(idleHash, false);
        
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

    private int ChuvaFogo(int valor)
    {
        float val = 0.5f;
        int n = valor;
        for (int i = 0; i < n; i++)
        {
            val = val * -1;
            Vector3 pointVector = point.transform.position;
            Instantiate(AttackFire, new Vector3(pointVector.x + val, pointVector.y, pointVector.z), Quaternion.identity);
            Collider2D[] isAttackFire = Physics2D.OverlapCircleAll(AttackFire.transform.position, radius, EnemyLayer);

            if (val > 0)
            {
                val += 0.3f;
            }
            else
            {
                val -= 0.3f;
            }

            foreach (Collider2D col in isAttackFire)
            {
                if (col.CompareTag("enemies"))
                {
                    col.transform.GetComponent<NPCMovement>().tomarDano(20);
                }
                if (col.CompareTag("enemy2"))
                {
                    col.transform.GetComponent<NPCMovement1>().tomarDano(20);
                }
            }
        }
        return valor-1;
    }
    void comando()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            transform.Translate(new Vector2(vel * Time.deltaTime, 0));
            move = true;
            movePlayer();
            
        } else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            transform.Translate(new Vector2(vel * Time.deltaTime, 0));
            move = true;
            movePlayer();
        }
        else
        {
            move = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) & (jump > 1 & jump <= 2))
            {
                jump--;
                bool_jump = true; 
                rb.AddForce(new Vector2(0, 2 * forca), ForceMode2D.Force);
                
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
                Collider2D[] isAttack = Physics2D.OverlapCircleAll(Player.transform.position, radius, EnemyLayer);
                foreach (Collider2D col in isAttack)
                {
                    if (col.CompareTag("enemies"))
                    {
                        col.transform.GetComponent<NPCMovement>().tomarDano(5);
                    }
                    else if (col.CompareTag("enemy2"))
                    {
                        col.transform.GetComponent<NPCMovement1>().tomarDano(5);
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
            chuvafogo = true;
        }
    }    
}
