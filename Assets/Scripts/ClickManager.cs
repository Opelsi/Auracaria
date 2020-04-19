using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
	const float MAXSWIPEDIST = 1.0f;
	Piece selectedPiece = null;
	public SpriteRenderer whiteLine;
	bool isSwiping = false;
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
				{
					selectedPiece.onSelect();
					isSwiping = true;
				}
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			isSwiping = false;
			if (selectedPiece != null)
			{
				selectedPiece.onDeselect();
				selectedPiece = null;
				whiteLine.transform.position = transform.position + new Vector3(0, 0, -1);
				whiteLine.transform.localScale = new Vector3(0, 1, 1);
			}
		}
		if (isSwiping)
		{
			if (selectedPiece != null)
			{
				////----TOUCHSCREEN-------
				//getting touch position
				Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 distance = targetPos - selectedPiece.transform.position; distance.z = 0;
				//straightening the whiteline
				if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y)) distance -= new Vector3(0, distance.y, 0);
				else distance -= new Vector3(distance.x, 0, 0);
				whiteLine.transform.position = selectedPiece.transform.position + distance / 2 + new Vector3(0, 0, -1);
				float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
				whiteLine.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				whiteLine.transform.localScale = new Vector3(distance.magnitude, 1, 1);
				//checking length
				if (distance.magnitude > MAXSWIPEDIST)
				{
					selectedPiece.movePiece(distance);
					selectedPiece = null;
					whiteLine.transform.position = transform.position + new Vector3(0, 0, -1);
					whiteLine.transform.localScale = new Vector3(0, 1, 1);
				}
			}
		}
	}
}
