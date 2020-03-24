using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
	Piece selectedPiece = null;
	// Update is called once per frame
	void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
			if (hit.collider != null)
			{
				if (hit.collider.tag == "Triangle")
					selectedPiece = hit.collider.GetComponent<Piece>();
				if (selectedPiece != null) selectedPiece.onSelect();
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if(selectedPiece!=null)
				selectedPiece.onDeselect();
			selectedPiece = null;
		}
	}
}
