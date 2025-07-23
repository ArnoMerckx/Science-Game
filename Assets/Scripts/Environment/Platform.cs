using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private GameObject player;
    public bool goingThroughPlatform;
    void Awake()
    {
        goingThroughPlatform = false;
        player = GameObject.Find("Player");
    }
    void Update()
    {
        if (goingThroughPlatform) { return; }
        if (player.transform.position.y < transform.position.y + 1)
        {
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<Collider2D>(),false);
            Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(), GetComponent<Collider2D>(), false);
        }
    }

    public IEnumerator PlatformCheckCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        goingThroughPlatform = false;
    }
}
