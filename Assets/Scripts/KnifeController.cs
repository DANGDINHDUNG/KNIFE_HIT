using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [SerializeField] private Vector2 throwForce;

    private bool isActive = true;
    private Rigidbody2D rb;
    private ParticleSystem knifeHitEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knifeHitEffect = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0))
        {
            rb.AddForce(throwForce, ForceMode2D.Impulse);
            rb.gravityScale = 1;

            GameController.Instance.GameUI.DecrementDisplayKnifeCount();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;

        isActive = false;

        if (collision.collider.CompareTag("Knife"))
        {
            rb.velocity = new Vector2(rb.velocity.x, -4f);
            AudioManager.audioInstance.Play("KnifeHitKnife");
            GameController.Instance.StartGameOverSequence(false);
        }
        else if (collision.collider.CompareTag("Wood") || collision.collider.CompareTag("Boss"))
        {
            knifeHitEffect.Play();
            AudioManager.audioInstance.Play("KnifeHitWood");
            rb.velocity = new Vector2(0, 0);
            rb.bodyType = RigidbodyType2D.Kinematic;
            //rb.simulated = false;

            this.transform.SetParent(collision.collider.transform);

            GameController.Instance.OnSuccessfulKnifeHit();
        }
    }
}

