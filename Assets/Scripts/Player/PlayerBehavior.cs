using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D myRb;
    private float moveInputDirection;
    private float dashTimeLeft;
    private float lastTimeSlideWall = 0;
    private float lastDash = -100;
    private float ManaSkill = 100;
    private float maxManaSkill = 100;
    private float ManaEachSecond;
    private float HPEachSecond;
    private float PercentSpeed;

    private int amountOfJumpLeft;
    private int facingDirection;
    private int idSkillUntil;


    private bool isFacingRight;
    private bool isTouchingWall;
    private bool isGrounded;
    private bool isFlying;
    private bool isWallSliding;
    private bool isDashing;
    private bool isSkilling;
    private bool canJump;
    private bool canSlidings;
    private bool canMove;
    private bool canFlip;
    private bool canJumpStomp;
    private bool canCheckHitBoxJumpSomp;
    private float DistanceDownAnimDust;

    //public int amountOfJump = 1;

    [SerializeField]
    private PlayerData playerData;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform hitBoxJumpForce;
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;

    [SerializeField]
    private PlayerAnimation animCtrl;
    [SerializeField]
    private PlayerEffect animEffect;

    private PlayerLadder playerLadder;

    void Start()
    {
        InvokeRepeating("IncreaseMana", 1f,1f);
        myRb = GetComponent<Rigidbody2D>();
        canJump = true;
        isFacingRight = true;
        amountOfJumpLeft = playerData.amountOfJump;
        lastTimeSlideWall = 0;
        canSlidings = true;
        canMove = true;
        facingDirection = 1;
        canFlip = true;
        DistanceDownAnimDust = -0.5f;
        ManaEachSecond = 3;
        HPEachSecond = 0;
        PercentSpeed = 1;

        playerLadder = GetComponent<PlayerLadder>();
    }

    void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        CheckInput();
        CheckMoveDirection();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckDash();
        if (Input.GetKeyDown(KeyCode.C))
        {
            SkillSentoryu();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SkillHoaDon();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SkillDivineDepature();
        }
    }

    private void FixedUpdate()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        ApplyMove();
        CheckSurroundings();
    }

    private void IncreaseMana()
    {
        if(ManaSkill < maxManaSkill)
        {
            ManaSkill += ManaEachSecond;
            if(ManaSkill > maxManaSkill)
            {
                ManaSkill = maxManaSkill;
            }
            UIManager.Instance.SetManaUi(ManaSkill,maxManaSkill);
        }
    }

    private void CheckMoveDirection()
    {
        if (isFacingRight && moveInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && moveInputDirection > 0)
        {
            Flip();
        }
    }

    private void CheckSurroundings()
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, playerData.wallCheckDistance, whatIsGround);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, whatIsGround);

        // Jump animation
        if (!isGrounded)
        {
            animCtrl.StartJump();
        }
        else
        {
            animCtrl.FinishJump();
        }

        //Kiểm tra khi chạm dất bằng dậm nhảy
        if(isGrounded && canCheckHitBoxJumpSomp) 
        {
             CheckAttackHitBoxJumpForce();
        }
        //audio landing
        if (!isGrounded)
            isFlying = true;
        //Kiểm tra vừa rơi xuống đất
        if(isFlying && isGrounded)
        {
            AudioManager.Instance.PlaySound("Landing1");
            isFlying = false;
            //Animation Effect Dust
            GameObject dust = EffectManager.Instance.Take();
            dust.transform.position = new Vector2(transform.position.x, transform.position.y + DistanceDownAnimDust);
            Dust d = dust.GetComponent<Dust>();
            d.StartAnimDustLand();
        }
    }
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && myRb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else if (isTouchingWall && !isGrounded && myRb.velocity.y > 0)
        {
            lastTimeSlideWall = 0;
            canSlidings = true;
            isWallSliding = false;
        }
        else
        {
            isWallSliding = false;

            // Animation Wall Slide
            animCtrl.FinishWallSlide();
        }
    }
    private void CheckDash()
    {
        if(isDashing && !isSkilling)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canJump = false;
                canFlip = false;
                myRb.velocity = new Vector2(playerData.dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.deltaTime;

                // Animation Dash
                animCtrl.StartDash();
            }
            if(dashTimeLeft < 0)
            {
                isDashing = false;
                canMove=true;
                canJump = true;
                canFlip = true;

                // Animation Dash
                animCtrl.FinishDash();
            }
        }
    }

    private void CheckInput()
    {
        // Run animation
        if (Input.GetAxisRaw("Horizontal") != 0 && isGrounded)
        {
            //animCtrl.StartRun();
        }
        else
        {
            //animCtrl.FinishRun();
        }
        //moveInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.S))
            {
                canJumpStomp = true;
                JumpStomp();
            }
            else
            {
                Jump();
            }
            
        }

        if(Input.GetKeyUp(KeyCode.S))
        {
            canJumpStomp= false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= lastDash + playerData.dashCoolDown)
            {
                AttemptToDash();
            }
        }
    }
    private void AttemptToDash()
    {
        AudioManager.Instance.PlaySound("Jump");
        isDashing = true;
        dashTimeLeft = playerData.dashTime;
        lastDash = Time.time;
        //Animation Dash
        GameObject dust = EffectManager.Instance.Take();
        dust.transform.position = new Vector2(transform.position.x, transform.position.y + DistanceDownAnimDust);
        Dust d = dust.GetComponent<Dust>();
        d.StartAnimDash();
    }
    private void ApplyMove()
    {
        if (canMove && !isSkilling)
        {
            //animCtrl.StartRun();
            myRb.velocity = new Vector2(playerData.moveSpeed * moveInputDirection*PercentSpeed, myRb.velocity.y);
        }

        if (isWallSliding && canSlidings)
        {
            // Animation Wall Sliding
            animCtrl.StartWallSlide();
            
            myRb.velocity = new Vector2(myRb.velocity.x, -playerData.wallSlidingSpeed);
            lastTimeSlideWall += Time.deltaTime;
            if(lastTimeSlideWall > 0.7f)
            {
                canSlidings = false;

                // Animation Wall Sliding
                animCtrl.FinishWallSlide();
            }
        }
    }

    private void Flip()
    {
        if(canFlip && !isSkilling)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
            if (isFacingRight)
            {
                facingDirection = 1;
            }
            else
            {
                facingDirection = -1;
            }
        }
    }
    
    private void Jump()
    {
        if (canJump && !canJumpStomp && !isSkilling)
        {
            if (isGrounded)
            {
                // Ignore the ladder
                if (playerLadder != null)
                {
                    StartCoroutine(playerLadder.DisableCollision());
                }

                GameObject dust = EffectManager.Instance.Take();
                dust.transform.position = new Vector2(transform.position.x, transform.position.y + DistanceDownAnimDust);
                Dust d = dust.GetComponent<Dust>();
                d.StartAnimJump();
            }

            AudioManager.Instance.PlaySound("Jump");
            myRb.velocity = new Vector2(myRb.velocity.x, playerData.jumpForce);
            amountOfJumpLeft--;
        }

    }
    
    private void JumpStomp()
    {
        if (canJumpStomp && isFlying && !isSkilling)
        {
            myRb.velocity = new Vector2(0, -playerData.jumpStompForce);
            Debug.Log("JumpStomp");
            canCheckHitBoxJumpSomp = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);

        Gizmos.DrawWireSphere(hitBoxJumpForce.position, playerData.HitBoxJumpForce);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + playerData.wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
    private void CheckIfCanJump()
    {
        if(isGrounded && myRb.velocity.y <= 0.1)
        {
            canJump = true;
            amountOfJumpLeft = playerData.amountOfJump;
        }

        if (isTouchingWall)
        {
            canJump = true;
            amountOfJumpLeft = playerData.amountOfJump;
        }

        if (amountOfJumpLeft < 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void CheckAttackHitBoxJumpForce()
    {
        Collider2D[] detectObject = Physics2D.OverlapCircleAll(hitBoxJumpForce.position, playerData.HitBoxJumpForce, whatIsEnemy);
        if(detectObject == null)
        {
            return;
        }
        foreach(Collider2D collider in detectObject)
        {
            collider.transform.SendMessage("IsDamaged", 10f);

            if (collider.gameObject.CompareTag("Interactable Object"))
            {

                collider.gameObject.GetComponent<IInteractable>().InteractOn();
            }
        }
        canCheckHitBoxJumpSomp = false;
    }

    private void SkillSentoryu()
    {
        if (isSkilling)
        {
            return;
        }
        else if (ManaSkill <= 80)
        {
            UIManager.Instance.NotEnoughMana();
            return;
        }
        ManaSkill -= 80;
        UIManager.Instance.SetManaUi(ManaSkill,maxManaSkill);
        myRb.gravityScale = 0f;
        isSkilling = true;
        StartCoroutine(SpawnSentoryu());
    }
    private IEnumerator SpawnSentoryu()
    {
        myRb.gravityScale = 0f;
        for (int i = 1; i <= 3; i++)
        {
            myRb.velocity = Vector3.zero;

            if (i == 1)
            {
                GameObject str1 = BulletManager.Instance.TakeSentoryu1();
                str1.transform.position = this.transform.position;
                Sentoryu s = str1.GetComponent<Sentoryu>();
                s.SetUp(facingDirection, 1);
            }

            if (i == 2)
            {
                GameObject str2 = BulletManager.Instance.TakeSentoryu2();
                str2.transform.position = this.transform.position;
                Sentoryu s = str2.GetComponent<Sentoryu>();
                s.SetUp(facingDirection, 2);
            }

            if (i == 3)
            {
                GameObject str3 = BulletManager.Instance.TakeSentoryu3();
                str3.transform.position = this.transform.position;
                Sentoryu s = str3.GetComponent<Sentoryu>();
                s.SetUp(facingDirection, 3);
            }

            if (i < 2)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else if (i == 2)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        isSkilling = false;
        myRb.gravityScale = 4f;
    }

    private void SkillHoaDon()
    {
        if (isSkilling)
        {
            return;
        }
        else if (ManaSkill <= 80)
        {
            UIManager.Instance.NotEnoughMana();
            return;
        }
        ManaSkill -= 80;
        UIManager.Instance.SetManaUi(ManaSkill, maxManaSkill);
        myRb.gravityScale = 0f;
        isSkilling = true;
        StartCoroutine(SpawnHoaCau());
    }

    private IEnumerator SpawnHoaCau()
    {
        myRb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        GameObject hoadon = BulletManager.Instance.TakeHoaDon();
        hoadon.transform.position = this.transform.position;
        HoaDon hd = hoadon.GetComponent<HoaDon>();
        hd.SetUp(facingDirection);
        yield return new WaitForSeconds(0.2f);
        myRb.gravityScale = 4f;
        isSkilling = false;
    }
    private void SkillDivineDepature()
    {
        if (isSkilling)
        {
            return;
        }
        else if (ManaSkill <= 80)
        {
            UIManager.Instance.NotEnoughMana();
            return;
        }
        ManaSkill -= 80;
        UIManager.Instance.SetManaUi(ManaSkill, maxManaSkill);
        isSkilling = true;
        StartCoroutine(SpawnDivine());
    }
    private IEnumerator SpawnDivine()
    {
        animEffect.StartQ3();
        myRb.velocity = new Vector2(0, 18f);
        yield return new WaitForSeconds(0.5f);
        myRb.velocity = new Vector2(0, -60f);
        yield return new WaitForSeconds(0.55f);
        isSkilling = false;
    }

    #region UI Button Move
    public void ButtonMoveLeftEnter()
    {
        moveInputDirection = -1;
        animCtrl.StartRun();
    }
    public void ButtonMoveRightEnter()
    {
        moveInputDirection = 1;
        animCtrl.StartRun();
    }
    public void ButtonMoveUp() {  
        moveInputDirection = 0; 
        canJumpStomp = false;
        animCtrl.FinishRun();
    }
    public void ButtonDash()
    {
        if (Time.time >= lastDash + playerData.dashCoolDown)
        {
            AttemptToDash();
        }
    }
    public void ButtonJump()
    {
        Jump();
    }
    public void ButtonJumpStomp()
    {
        JumpStomp();
    }
    public void ButtonMoveDownEnter()
    {
        canJumpStomp = true;
    }
    public void ButonFireBall()
    {
        switch (idSkillUntil)
        {
            case 1:
                SkillHoaDon();
                break;
            case 2:
                SkillSentoryu();
                break;
            case 3:
                SkillDivineDepature();
                break;
        }
    }
    #endregion
    //Skill tree
    public void UpgradeSkill(int idSkill)
    {
        switch(idSkill)
        {
            case 1: Skill1(); break;
            case 2: Skill2(); break;
            case 3: Skill3(); break;
            case 4: Skill4(); break;
            case 5: Skill5(); break;
            case 6: Skill6(); break;
            case 7: Skill7(); break;
            case 8: Skill8(); break;
            case 9: Skill9(); break;
        }
    }
    private void Skill1()
    {
        HPEachSecond = 2;
        this.gameObject.GetComponent<PlayerCombatController>().HoiHP(HPEachSecond);
    }
    private void Skill2()
    {
        this.gameObject.GetComponent<PlayerCombatController>().TangHP(150);
    }
    private void Skill3()
    {
        idSkillUntil = 1;
    }
    private void Skill4()
    {
        ManaEachSecond = 5;
    }
    private void Skill5()
    {
        ManaSkill += 50;
        maxManaSkill = 150;
        if (ManaSkill > 150)
        {
            ManaSkill = 150;
        }
        UIManager.Instance.SetManaUi(ManaSkill,maxManaSkill);
    }
    private void Skill6()
    {
        idSkillUntil = 2;
    }
    private void Skill7()
    {
        PercentSpeed = 1.25f;
    }
    private void Skill8()
    {
        this.gameObject.GetComponent<PlayerCombatController>().PercentDamage = 1.25f;
    }
    private void Skill9()
    {
        idSkillUntil = 3;
    }
    //
}
