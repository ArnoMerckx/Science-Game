using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject destinationPortal;
    void Awake()
    {
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
        foreach (GameObject p in portals) 
        {
            if (p.name != gameObject.name)
            {
                destinationPortal = p;
            }
        }
    }

    public void TelePortToDestination()
    {
        if (destinationPortal != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = destinationPortal.transform.position;
            player.GetComponent<PlayerMovement>().currentPortal = destinationPortal;

        }
    }
}
