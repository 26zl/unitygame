using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    // How many seconds before the bullet is destroyed
    public float lifeTime = 5f;

    void Start()
    {
        // Destroy the bullet GameObject after 'lifeTime' seconds
        Destroy(gameObject, lifeTime);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
