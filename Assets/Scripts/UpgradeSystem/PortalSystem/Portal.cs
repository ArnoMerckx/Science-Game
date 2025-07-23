using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject destinationPortal;
    public PortalColor portalColor;
    void Awake()
    {
        FindOtherPortal();
    }

    public void TelePortToDestination()
    {
        if (destinationPortal != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = destinationPortal.transform.position;
            player.GetComponent<PlayerManager>().currentPortal = destinationPortal;

        }
    }

    public void ResetDestinationPortal()
    {
        FindOtherPortal();
    }

    private void FindOtherPortal()
    {
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
        foreach (GameObject p in portals)
        {
            if (p.name != gameObject.name)
            {
                destinationPortal = p;
                break;
            }
        }
    }
}

public enum PortalColor
{
    Blue,
    Orange
}
