using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakyBlock : MonoBehaviour
{
    private int durability = 2;
    [SerializeField] private Sprite crackedSprite;
    [SerializeField] private AudioClip DamageSFX;
    [SerializeField] private AudioClip BreakSFX;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject explosionObject;



    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Test Charge"))
        {
            
            durability--;
            if (durability == 1)
            {
                audioSource.PlayOneShot(DamageSFX, 0.5f);
                this.gameObject.GetComponent<SpriteRenderer>().sprite = crackedSprite;
            }
            if (durability <= 0)
            {
                AudioSource.PlayClipAtPoint(BreakSFX, Vector2.zero);
                GameObject explosion = Instantiate(explosionObject, transform.position, Quaternion.identity);
                explosion.GetComponent<RockExplosion>().RockLength = GetComponent<SpriteRenderer>().size.y;
                Destroy(gameObject);
            }
        }
    }
}
