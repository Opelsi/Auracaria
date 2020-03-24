using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
	Piece selectedPiece = null;
	Piece connectedPiece = null;
	
	int findWinnerPoint(int distance, int strengthA, int strengthB, int pointA, int pointB)
	{
		int result = 0;
		if (distance % 2 != 0)
		{
			if (strengthA > strengthB)
			{
				result = pointA + distance / 2;
			}
			else
			{
				result = pointB - distance/2;
			}
		}
		else
		{
			result = pointA + distance / 2;
		}
		return result;
	}
	// Update is called once per frame
	void Update()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
			if (hit.collider != null)
			{
				if (hit.collider.tag == "Triangle")
					selectedPiece = hit.collider.GetComponent<Piece>();
				if (selectedPiece != null)
					selectedPiece.onSelect();
			}
		}
		if (selectedPiece != null && Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
			if (hit.collider != null && hit.collider.gameObject!=selectedPiece.gameObject)
			{
				if ((hit.collider.tag == "Triangle" || hit.collider.tag == "Square")&&(Mathf.Abs(selectedPiece.transform.position.x-hit.collider.transform.position.x)<0.01f)|| (Mathf.Abs(selectedPiece.transform.position.y - hit.collider.transform.position.y)<0.01f))
					connectedPiece = hit.collider.GetComponent<Piece>();
				if (connectedPiece != null)
				{
					//connection process
					int strengthA = selectedPiece.strength;
					int strengthB = connectedPiece.strength;
					connectedPiece.onConnect(selectedPiece, strengthA);
					if (selectedPiece.tag!=connectedPiece.tag)
						selectedPiece.onConnect(connectedPiece, strengthB);
					//after connection
					if (selectedPiece.tag == connectedPiece.tag)
					{
						selectedPiece.isLost = true;
						selectedPiece.moveTo(connectedPiece);
						connectedPiece.moveTo(connectedPiece);
					}
					else
					{
						connectedPiece.isLost = strengthA >= strengthB;
						selectedPiece.isLost = strengthB >= strengthA;
						int distCol = connectedPiece.col - selectedPiece.col;
						int distRow = connectedPiece.row - selectedPiece.row;
						int winnerPoint = 0;
						Vector2 newPoint = new Vector2(0, 0);
						if (distCol != 0)
						{
							winnerPoint = findWinnerPoint(distCol, strengthA, strengthB, selectedPiece.col, connectedPiece.col);
							newPoint = new Vector2(winnerPoint, selectedPiece.row);
						}
						if (distRow != 0)
						{
							winnerPoint = findWinnerPoint(distRow, strengthA, strengthB, selectedPiece.row, connectedPiece.row);
							newPoint = new Vector2(selectedPiece.col,winnerPoint );
						}
						selectedPiece.moveTo(newPoint);
						connectedPiece.moveTo(newPoint);
					}
				}
				connectedPiece = null;
			}
			if (selectedPiece!=null && connectedPiece == null)
			{
				selectedPiece.onDeselect();
			}
			selectedPiece = null;
		}
	}
}
