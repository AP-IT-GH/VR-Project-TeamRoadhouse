using UnityEngine;

public class Player : MonoBehaviour
{
    private Environment environment;

    private void Start()
    {
        environment = GetComponentInParent<Environment>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collidable"))
        {
            //print("Player in wall");
        }
    }
}
