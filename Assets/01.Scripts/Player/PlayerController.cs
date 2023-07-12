using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    float _moveSpeed = 8f;
    Vector2 _maxPos = new Vector2(18f, 15f);
    Vector2 _minPos = new Vector2(-17f, -18f);

    Animator _animator;
    Rigidbody2D _rigidBody;
    public BoxCollider2D boxCollider;
    SpriteRenderer _spriteRenderer;
    

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (UIManager.Instance == null) return;
        if(UIManager.Instance.hpValue <= 0)
        {
            UIManager.Instance.GameOver();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rigidBody.velocity = Vector2.zero;
        if (!GameManager.Instance.isStarting) return;
        if (!GameManager.Instance.isAdding) return;
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rigidBody.velocity = movement * _moveSpeed;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _minPos.x, _maxPos.x), Mathf.Clamp(transform.position.y, _minPos.y, _maxPos.y));
        if (_rigidBody.velocity == Vector2.zero)
        {
            _animator.SetBool("Running", false);
        }
        else
        {
            _animator.SetBool("Running", true);
        }
        if(_rigidBody.velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if(_rigidBody.velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    public void BeAttack(float damage)
    { 
        UIManager.Instance.HpDown(damage);
    }

    public void HpHeal(float damage)
    {
        UIManager.Instance.HpUp(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireOrbItem"))
        {
            UIManager.Instance.FireOrbUp();
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("LightningOrbItem"))
        {
            UIManager.Instance.LightningOrbUp();
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("Door"))
        {
            SceneManager.LoadScene(3);
        }
    }
}
