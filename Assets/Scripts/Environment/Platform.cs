using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private GameObject player;
    private bool platformFlipped;

    void Awake()
    {
        player = GameObject.Find("Player");
        platformFlipped = false;
    }
    void Update()
    {
        
        if (player.GetComponent<PlayerStatsManager>().playerStats.IsGravityFlipped)
        {
            if (!platformFlipped)
            {
                GetComponent<PlatformEffector2D>().rotationalOffset = 180f;
                platformFlipped = true;
            }
            
        }
        else
        {
            if (platformFlipped)
            {
                GetComponent<PlatformEffector2D>().rotationalOffset = 0;
                platformFlipped = false;
            }
            
        }
    }
}
