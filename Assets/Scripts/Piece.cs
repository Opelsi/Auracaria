using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Piece : MonoBehaviour
{

	const float MAXMOVEDIST = 10.0f;
	public Sprite[] numbers;
	public SpriteRenderer selectedSprite, strengthSprite;
	public int strength = 1;

	public bool isMoving = false;
	public bool isLost = false;

	public int col = 0, row = 0;				//col and row coordinates
	private Vector2 target = new Vector2(0, 0); //uses col and row coordinates
	private LayerMask pieceObstacles;
	private bool isSelected = false;
	private bool isConnected = false;

	Piece connectedPiece = null;

	// Start is called before the first frame update
	void Start()
	{
		pieceObstacles = ~(1<<9|1<<11);//all layers except 9 and 11
		row = Mathf.RoundToInt(3 - transform.position.y);
		col = Mathf.RoundToInt(3 + transform.position.x);
	}

    // Update is called once per frame
    void Update()
    {
		if(numbers!=null && numbers.Length!=0)
		strengthSprite.sprite = numbers[strength];
	}
	private void FixedUpdate()
	{
		selectedSprite.enabled = isSelected;
		if (isMoving)
		{
			Vector3 startPoint = transform.position;
			Vector3 endPoint = new Vector3(target.x, target.y, startPoint.z);
			transform.position = Vector3.Lerp(startPoint, endPoint, Time.deltaTime * 10.0f);
			if (Mathf.Abs(transform.position.x - endPoint.x) < 0.1f && Mathf.Abs(transform.position.y - endPoint.y) < 0.1f)
			{
				isMoving = false;
				//Piece full stop
				transform.position = endPoint;
				row = Mathf.RoundToInt(3 - transform.position.y);
				col = Mathf.RoundToInt(3 + transform.position.x);
			}
		}
		if (tag != "Circle")
		{
			//Piece connection
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.zero, 0.0f, pieceObstacles.value);
			if (hit.collider != null && hit.collider.gameObject != gameObject)
			{
				if (!isConnected && !isMoving)
				{
					connectedPiece = hit.collider.GetComponent<Piece>();
					int tmp = strength;
					isConnected = true;
					onConnect(connectedPiece, connectedPiece.strength);
					connectedPiece.onConnect(this, tmp);
				}
			}
			isConnected = false;
		}
		if (isLost) Destroy(gameObject);
	}
	public void onSelect()
	{
		if (!isMoving)isSelected = true;
		gameObject.layer = LayerMask.NameToLayer("Player");
	}
	public void onDeselect()
	{
		isSelected = false;
		gameObject.layer = LayerMask.NameToLayer("Piece");
	}
	public void movePiece(Vector2 newDirection)
	{
		Vector3 origin = transform.position;
		Vector3 direction = (new Vector3(newDirection.x, newDirection.y, transform.position.z)).normalized;
		RaycastHit2D hit = Physics2D.Raycast(origin, direction, MAXMOVEDIST, pieceObstacles.value);
		if (hit.collider!=null)
		{
			if (hit.collider.gameObject != gameObject)
			{
				if (connectedPiece != null)
				{
					if (connectedPiece.tag == "Circle") { connectedPiece.gameObject.layer = LayerMask.NameToLayer("Piece"); connectedPiece = null; }
				}
				if (hit.collider.tag == "Circle")
				{
					isMoving = true;
					target = hit.collider.transform.position;
				}
				if (hit.collider.tag == "Square")
				{
					isMoving = true;
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
		if (tag != "Circle")
		{
			if (otherPiece.tag == tag) { strength += tmpStrength; otherPiece.isLost = true; isLost = false; }
			else if (otherPiece.tag != "Circle") strength -= tmpStrength;
		}
		else { strength--; gameObject.layer = LayerMask.NameToLayer("InactiveCircles"); }
		if (strength < 0) strength = 0;
		if (strength == 0) isLost = true;
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
