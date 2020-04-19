using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Piece : MonoBehaviour
{

	const float MAXMOVEDIST = 10.0f;

	public Sprite sprite_1, sprite_2, sprite_3;
	public SpriteRenderer sprite_selected, currentSprite;
	public LayerMask moveObstacles; 

	public int strength = 1;
	private bool isSelected = false, isConnected = false, isMoving = false;
	public bool isLost = false;

	public int col = 0, row = 0;
	private Vector2 target = new Vector2(0, 0);
    // Start is called before the first frame update
    void Start()
	{
		moveObstacles = ~(LayerMask.GetMask("Player"));
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
	}
	private void FixedUpdate()
	{
		sprite_selected.enabled = isSelected || isConnected || isMoving;
		if (isSelected)
		{
			//if is selected
		}
		if (isMoving)
		{
			Vector3 startPoint = transform.position;
			Vector3 endPoint = new Vector3(target.x, target.y, startPoint.z);
			transform.position = Vector3.Lerp(startPoint, endPoint, Time.deltaTime * 10.0f);
			if (Mathf.Abs(transform.position.x - endPoint.x) < 0.1f && Mathf.Abs(transform.position.y - endPoint.y) < 0.1f)
			{
				isMoving = false;
				transform.position = endPoint;
				row = Mathf.RoundToInt(3 - transform.position.y);
				col = Mathf.RoundToInt(3 + transform.position.x);
				if (isLost) Destroy(gameObject);
				isConnected = false;
			}
		}
	}
	public void onSelect()
	{
		if (!isMoving)isSelected = true;
	}
	public void onDeselect()
	{
		isSelected = false;
	}
	public void movePiece(Vector2 newDirection)
	{
		onDeselect();
		Vector3 origin = transform.position;
		Vector3 direction = (new Vector3(newDirection.x, newDirection.y, transform.position.z)).normalized;
		RaycastHit2D hit = Physics2D.Raycast(origin, direction, MAXMOVEDIST, moveObstacles.value);
		if (hit.collider!=null)
		{
			if (hit.collider.gameObject != gameObject)
			{
				if(hit.collider.tag == "Square")
				{
					isMoving = true;
					Piece otherPiece = hit.collider.GetComponent<Piece>();
					//Add logic to placing of winning and losing piece
					target = hit.collider.transform.position;
				}
				if (hit.collider.tag == "Triangle")
				{
					isMoving = true;
					target = hit.collider.transform.position;
				}
				if (hit.collider.tag == "Wall")
				{
					isMoving = true;
					target = new Vector3(hit.point.x,hit.point.y,transform.position.z) - direction*0.5f;
				}
			}
		}
		
	}
	public void onConnect( Piece otherPiece, int tmpStrength )
	{
		isConnected = true;
		if (otherPiece.tag == tag) strength += tmpStrength;
		else strength -= tmpStrength;
		if (strength < 0) strength = 0;
	}
	//old script
	public void moveTo(float x, float y )
	{
		isMoving = true;
		target = new Vector2(x, y);
	}
	public void moveTo( Piece otherPiece )
	{
		moveTo(otherPiece.col, otherPiece.row);
	}
	public void moveTo( Vector2 newPoint )
	{
		isMoving = true;
		target = newPoint;
	}
}
