using UnityEngine;

public class Player : Entity   //inherited from Entity
{

    [Header("Move info")]
    //Clean Code tip #1 -> SerializeField
    [SerializeField] private float moveSpeed;   //for manually giving the input when the parameter is private
    [SerializeField] private float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;

    [Header("Attack info")]
    [SerializeField] private float comboTime = 0.3f;
    private float comboTimeWindow;
    private bool isAttacking;
    private int comboCounter;

    private float xInput;

    protected override void Start()
    {
        base.Start();  //inherit
    }
    protected override void Update()
    {
        base.Update();

        Movement();
        CheckInput();

        dashTime -= Time.deltaTime; //for counting 
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;


        FlipController();
        AnimatorControllers();
    }

    //Clean Code tip 2# -> Functions
    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;

        if (comboCounter > 2)
            comboCounter = 0;
    }
    private void CheckInput()//while manifacturing the clean code principals dont forget to put every input taking action in this method!
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    private void StartAttackEvent()
    {
        if (!isGrounded)
            return;
        if (comboTimeWindow < 0)
            comboCounter = 0;

        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;  //until the value is negative(because of the counting) dash is in cool down! 
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if (isAttacking)
            rb.velocity = new Vector2(0, 0);
        else if (dashTime > 0)
            rb.velocity = new Vector2(facingDir * dashSpeed, 0f); //facingDir used for to solve the idle->dash action! 
        else
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }
    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y); //checking if the character is jumping(positive value) or falling(negative value) to play the accurate anim [used with the blendtree]

        anim.SetBool("isMoving", isMoving); //For anim to start
        anim.SetBool("isGrounded", isGrounded); //in order to make this work inactivate the "can transition to" radio button in animator!
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }
    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }
}
