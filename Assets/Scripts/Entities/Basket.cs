using System.Collections;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public ParticleSystem particle;
    private GameManager gameManager;
    private PlayerController playerController;
    public float basketSize = 1;
    public float growSpeed = 1;
        
    private void Awake()
    {
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
                CatchEvent(other.gameObject);
            }    
        }

     
    }

    private void CatchEvent(GameObject target)
    {
        target.SetActive(false);
        particle.Play();
        gameManager.catchedFrogs += 1;
    }



}
