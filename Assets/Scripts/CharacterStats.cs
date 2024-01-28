using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    public float power = 10;
    [SerializeField] Slider healthSlider;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _vfxHit;
    [SerializeField] GameObject _vfxDead;
    [SerializeField] GameObject _vfxUltimate;
    [SerializeField] Slider _ultSlider;
    public float fillSpeed = 0.05f;

    [SerializeField] private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (_ultSlider != null)
        {
            if (_ultSlider.value == 1)
            {
                _ultSlider.value = 0;
            }
            else
            {
                FillUltOverTime();
            }
        }
    }
    public void ChangeHealth(float value)
    {
        currentHealth += value;
        if (healthSlider != null)
        {
            float fillAmount = currentHealth / maxHealth;
            healthSlider.value = fillAmount;
        }
        if (transform.CompareTag("Player"))
        {
            if (_animator != null)
                _animator.SetTrigger("isHit");
        }
        else if (transform.CompareTag("Enemy"))
        {
            //AnimationsController animationsController = transform.GetComponent<AnimationsController>();
            /*if (animationsController != null)
            {
                animationsController.Hit();
            }*/
            if (_vfxHit!= null)
            Instantiate(_vfxHit,transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (transform.CompareTag("Enemy"))
        {
            /*AnimationsController animationsController = transform.GetComponent<AnimationsController>();
            if (animationsController != null)
            {
                animationsController.SetDead();
            }*/
        }
        if (gameObject.layer == LayerMask.NameToLayer("Mineral"))
        {
            gameObject.SetActive(false);
            if (_vfxDead != null)
            Instantiate(_vfxDead, transform.position, Quaternion.identity);
        }
    }
    private void FillUltOverTime()
    {
        // Increment the slider value based on time and fill speed
        _ultSlider.value += fillSpeed * Time.deltaTime;

        // Ensure that the value stays between 0 and 1
        _ultSlider.value = Mathf.Clamp01(_ultSlider.value);
    }
    public void OnDead()
    {
        Destroy(gameObject,0.1f);
    }

}
