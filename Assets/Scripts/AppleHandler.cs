using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Knife"))
        {
            Debug.Log("Apple Hit");
            GameController.Instance.appleScore += 10;
            Destroy(gameObject);
        }
    }
}
