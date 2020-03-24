using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Piece : MonoBehaviour
{
	public Sprite sprite_1, sprite_2, sprite_3;
	public SpriteRenderer sprite_selected, currentSprite, whiteLine;
	public int strength = 1;
	private bool isSelected = false, isConnected = false, isMoving = false;
	public bool isLost = false;
	public int col = 0, row = 0;
	private Vector2 target = new Vector2(0, 0);
    // Start is called before the first frame update
    void Start()
	{
		row = Mathf.RoundToInt(3 - transform.position.y);
		col = Mathf.RoundToInt(3 + transform.position.x);
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
		sprite_selected.enabled = isSelected||isConnected||isMoving;
		if (isSelected)
		{
			Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 distance = targetPos - transform.position; distance.z = 0;

			if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y)) distance -= new Vector3(0, distance.y, 0);
			else distance -= new Vector3(distance.x, 0, 0);
			whiteLine.transform.position = transform.position + distance / 2;
			float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
			whiteLine.transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
			whiteLine.transform.localScale = new Vector3(distance.magnitude, 1, 1);
		}
		if (isMoving)
		{
			Vector3 startPoint = transform.position;
			Vector3 endPoint = new Vector3(target.x - 3, 3 - target.y, 0);
			transform.position = Vector3.Lerp(startPoint, endPoint,  Time.deltaTime*10.0f);
			if (Mathf.Abs(transform.position.x - endPoint.x)<0.01f&& Mathf.Abs(transform.position.y - endPoint.y)<0.01f)
			{
				isMoving = false;
				row = Mathf.RoundToInt(3 - transform.position.y);
				col = Mathf.RoundToInt(3 + transform.position.x);
				Debug.Log((3 - transform.position.y) + " " + (3 + transform.position.x));
				if(isLost)Destroy(gameObject);
				isConnected = false;
			}
		}
	}
	public void onSelect()
	{
		isSelected = true;
	}
	public void onDeselect()
	{
		isSelected = false;
		whiteLine.transform.position = transform.position + new Vector3(0,0,1);
		whiteLine.transform.localScale = new Vector3(0, 1, 1);
	}
	public void onConnect(Piece otherPiece,int tmpStrength)
	{
		isConnected = true;
		if (otherPiece.tag == tag) strength += tmpStrength;
		else strength -= tmpStrength;
		if (strength < 0) strength = 0;
	}
	public void moveTo(float x, float y )
	{
		isMoving = true;
		target = new Vector2(x, y);
	}
	public void moveTo(Piece otherPiece )
	{
		moveTo(otherPiece.col, otherPiece.row);
	}
	public void moveTo( Vector2 newPoint)
	{
		isMoving = true;
		target = newPoint;
	}
}
