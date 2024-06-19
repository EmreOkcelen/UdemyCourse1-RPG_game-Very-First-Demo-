using UnityEngine;

public class Entity : MonoBehaviour      //inheritance --> protected
{
    protected Rigidbody2D rb;
    protected Animator anim;

    protected int facingDir = 1;
    protected bool facingRight = true;

    [Header("Collision info")]  //it helps to classify the parameters in unity (Clean Code factor)
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround; //for chosing layer called "Ground" in unity to detect floor

    protected bool isGrounded;
    protected bool isWallDetected;


    protected virtual void Start() //"protected virtual" --> inheritable!
    {
        rb = GetComponent<Rigidbody2D>(); //using the same game object's rigidbody component
        anim = GetComponentInChildren<Animator>(); //using  the animator of the child object of the player!!

        if (wallCheck == null)
            wallCheck = transform;
    }
    protected virtual void Update()
    {
        CollisionChecks();
    }
    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround); //for checking the ground
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facingDir, whatIsGround);
    }
    protected virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    protected virtual void OnDrawGizmos()  //**Unique method (creates line for detecting the ground in order to jump)
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));//vectoral check
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));//horizontal check
    }
}
