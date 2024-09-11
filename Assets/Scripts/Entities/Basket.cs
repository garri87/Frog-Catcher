using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    private BoxCollider _boxCollider;
    public ParticleSystem particle;
    private GameManager gameManager;
    private PlayerController playerController;
    private void Awake()
    {
        
        _boxCollider = GetComponent<BoxCollider>(); 

        gameManager = GameManager.Instance;

        playerController = GetComponentInParent<PlayerController>();
    }


    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Frog") )
        {
            FrogController frogController = other.GetComponent<FrogController>();
            if(!frogController.IsGrounded && !playerController.IsInvulnerable)
            {
                other.gameObject.SetActive(false);
                particle.Play();
                gameManager.catchedFrogs += 1;
            }
            
        }
    }


}
