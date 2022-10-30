using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class CharacterController2D : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    public Camera mainCamera;
    public Transform groundCheckObj;
    [SerializeField] bool facingRight = true;
    [SerializeField] float moveDirection = 0;
    [SerializeField] bool isGrounded = false;
    [SerializeField] AudioClip salto_SFX;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        anim = GetComponent<Animator>();
        facingRight = t.localScale.x > 0;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if (Input.GetAxis("Horizontal") == -1 || Input.GetAxis("Horizontal") == 1)
        {
            moveDirection = Input.GetAxis("Horizontal");
        }
        else
        {
            if (isGrounded || r2d.velocity.magnitude < 0.01f)
            {
                moveDirection = 0;
            }
        }


        // Change facing direction
        if (moveDirection != 0)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
                if (isGrounded) anim.SetBool("caminando", true);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
                
                if(isGrounded) anim.SetBool("caminando", true);
            }
        }else
        {
            anim.SetBool("caminando", false);
        }


        // Jumping
        if(isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.W)  || Input.GetAxis("Vertical") > 0 || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                Saltar();
            }
        }
        

        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, t.position.y, cameraPos.z);
        }
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos;
        if (r2d.gravityScale > 0)
            groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        else
            groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f +.55f, 0);
        groundCheckObj.position = groundCheckPos;
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    anim.SetBool("isGrounded", isGrounded);
                    break;
                }
            }
        }

        // Apply movement velocity
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);

        // Simple debug
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }

    public void Saltar()
    {
        r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight * (r2d.gravityScale));
        AudioManager.instance.PlayOneShot(salto_SFX);
        anim.Play("Air");
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("caminando", false);

    }

    public void ForzarSalto(float poder = 1f)
    {
        r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight * poder* (r2d.gravityScale));
        anim.Play("Air");
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("caminando", false);

    }
}