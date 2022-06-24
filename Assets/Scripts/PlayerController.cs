using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public RawImage hpBar;
    public AudioSource audioSource;
    public AudioClip gotHit;
    public AudioClip deathSound;
    public AudioClip[] footsteps;
    public float footstepOffset = 0.5f;
    private float footstepTimer = 0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        currentHealth = maxHealth;
        hpBar.rectTransform.sizeDelta = new Vector2(currentHealth, 10);
    }

    private void Update()
    {
        HandleFootsteps();
    }

    public void Heal()
    {
        if(currentHealth < maxHealth)
            currentHealth++;
        hpBar.rectTransform.sizeDelta = new Vector2(currentHealth, 10);
    }

    public void RecieveDamage(int damage)
    {
        audioSource.PlayOneShot(gotHit);
        currentHealth -= damage;
        hpBar.rectTransform.sizeDelta = new Vector2(currentHealth, 10);
        
        if (currentHealth <= 0)
            StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        audioSource.PlayOneShot(deathSound);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }

    private void HandleFootsteps()
    {
        if (rb.velocity.x > 0 || rb.velocity.y > 0)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
                footstepTimer = footstepOffset;
            }
        }
    }
}
