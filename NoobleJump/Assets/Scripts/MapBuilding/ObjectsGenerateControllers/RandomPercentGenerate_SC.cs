using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPercentGenerate_SC : MonoBehaviour
{
    [Range(0, 1)]
    public float generateChance = 0.1f;

    private void Awake()
    {
        float random = Random.Range(0f, 1f);
        if (random >= generateChance)
        {
            Destroy(gameObject);
        }
    }


    void Start ()
	{
		
	}

}
