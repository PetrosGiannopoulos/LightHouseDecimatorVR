using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyHealthBarScript : MonoBehaviour
{

    Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = transform.parent.transform.parent.GetComponent<FriendlyStatsScript>().healthBarAmount();

        //if (healthBar.fillAmount == 0) transform.parent.transform.parent.GetComponent<FriendlyStatsScript>().Destroy();
    }
}
