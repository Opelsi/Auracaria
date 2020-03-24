using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Piece : MonoBehaviour
{
	public Sprite sprite_1;
	public Sprite sprite_2;
	public Sprite sprite_3;
	public SpriteRenderer sprite_selected;
	public SpriteRenderer currentSprite;
	public int strength = 1;
	private bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		switch (strength)
		{
			case 1:
				currentSprite.sprite = sprite_1;
				break;
			case 2:
				currentSprite.sprite = sprite_2;
				break;
			case 3:
				currentSprite.sprite = sprite_3;
				break;
		}
		sprite_selected.enabled = isSelected;
	}
	public void onSelect()
	{
		Debug.Log("onSelect");
		isSelected = true;
	}
	public void onDeselect()
	{
		Debug.Log("onDeselect");
		isSelected = false;
	}
}
