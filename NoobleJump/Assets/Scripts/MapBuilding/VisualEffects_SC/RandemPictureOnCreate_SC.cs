using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandemPictureOnCreate_SC : MonoBehaviour
{
    public Sprite[] pictures;

	void Start ()
	{
        // Если есть SpriteRenderer,
        // то меняем стандартную картинку на случайную из массива

	    SpriteRenderer sr = GetComponent<SpriteRenderer>();
	    if (sr)
	        sr.sprite = pictures[Random.Range(0, pictures.Length)];
	}
}
