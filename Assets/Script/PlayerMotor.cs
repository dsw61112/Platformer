using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class PlayerMotor : MonoBehaviour
{
    Vector2 direction;
    public float acceleration = 10;
    public float stoppingForce = 10;
    public float maxSpeedX = 10;
    public float stoppingPoint = 0.1f;
    public float enemyHitForce = 50;
    public float jumpForce = 10;
    public float dashForce = 10;
    private Rigidbody2D _rigidbody2D;
    private bool _canJump = true;
    private bool _canDash = true;
    public int maxJump = 2;
    private int currentJumps;
    private Animator animator;
    private float initialScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialScale = transform.localScale.x;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {

        animator.SetFloat("SpeedY", _rigidbody2D.linearVelocityY);
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(initialScale, transform.localScale.y, transform.localScale.x);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-initialScale, transform.localScale.y, transform.localScale.x);
        }
        MovePlayer();
        LimitMaxSpeed();
    }

    private void Update()
    {
        Debug.Log(direction);   
    }
    private void LimitMaxSpeed()
    {
        //Limit max speed
        if (!_canDash)
        {
            return;
        }
        if (_rigidbody2D.linearVelocityX >= maxSpeedX)
        {
            _rigidbody2D.linearVelocityX = maxSpeedX;
        }
        else if (_rigidbody2D.linearVelocityX <= -maxSpeedX)
        {
            _rigidbody2D.linearVelocityX = -maxSpeedX;
        }
    }

    private void MovePlayer()
    {
        //accelerate if pressing button
        if (direction.x != 0)
        {
            _rigidbody2D.AddForce(new Vector2(direction.x * acceleration, 0));
            animator.SetBool("IsMoving", true);
            if (direction.x > 0)
            {
                gameObject.transform.localScale = new Vector3(initialScale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-initialScale, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
        }
        // if not accelerating start slowing down
        else if (_rigidbody2D.linearVelocityX != 0)
        {
            //if almost stopped, force stop
            if (_rigidbody2D.linearVelocityX < stoppingPoint && _rigidbody2D.linearVelocityX > -stoppingPoint)
            {
                _rigidbody2D.linearVelocity = new Vector2(0.0f, _rigidbody2D.linearVelocityY);
            }
            //add stopping force
            else
            {
                _rigidbody2D.AddForce(new Vector2(-_rigidbody2D.linearVelocityX * stoppingForce, 0));
            }
            animator.SetBool("IsMoving", false );   
        }
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    //private void On+NazwaAkcji()
    private void OnDash()
    {
        //Debug.Log("Dashing");
        if (_canDash)
        {

            _rigidbody2D.AddForce(new Vector2(direction.x * dashForce, 0), ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody2D.AddForce(new Vector2(dashForce, 0), ForceMode2D.Impulse);
        }
        _canDash = false;
        StartCoroutine(ResetDash(1));
       
    }

    IEnumerator ResetDash(float cooldown)
    {   
        yield return new WaitForSeconds(cooldown);
        _canDash = true;
    }

    private void OnJump()
    {
        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        currentJumps++;
        if (currentJumps >= maxJump)
        {
            _canJump = false;
        }
    }

    void OnCollisionEnder2D(Collision2D collision)
    {
        _canJump = true;
        currentJumps = 0;
    }

    private void OnHealthChanged(int oldHealth, int amountChanged, Vector2 origin)
    {
        _rigidbody2D.AddForce(new Vector3(origin.x - transform.position.x, 0, 0) * enemyHitForce, ForceMode2D.Impulse);
    }
}