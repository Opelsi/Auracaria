using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
	GameObject[] triangles;
	Piece selectedPiece;
	bool isRotating = false;
	bool isMoving = false;
	private void rotateTriangles(int n)
	{
		if (triangles != null)
		{
			n += triangles.Length;
			n = n % triangles.Length;
			for (int i = 0; i < n; i++) shiftTriangles();
			isRotating = false;
		}
	}
	private void shiftTriangles()
	{
		if (triangles != null)
		{
			GameObject tmp = triangles[triangles.Length - 1];
			for (int i = triangles.Length - 1; i > 0; i--)
			{
				triangles[i] = triangles[i - 1];
			}
			triangles[0] = tmp;
			tmp = null;
		}
	}
	// Start is called before the first frame update
	void Start()
    {
		triangles = GameObject.FindGameObjectsWithTag("Triangle");
		selectedPiece = triangles[0].GetComponent<Piece>();
	}

	// Update is called once per frame
	void Update()
	{
		string currentSceneName = SceneManager.GetActiveScene().name;
		if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(currentSceneName);
		if (triangles != null)
		{
			if (selectedPiece != null && !selectedPiece.isMoving && isMoving)
			{
				isMoving = false;
				selectedPiece.onSelect();
			}
			if (selectedPiece != null)
			{
				selectedPiece.onSelect();
				if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)))
				{
					if (!isRotating)
					{
						selectedPiece.onDeselect();
						isRotating = true;
						if (Input.GetKeyDown(KeyCode.Q)) rotateTriangles(-1);
						else if (Input.GetKeyDown(KeyCode.E)) rotateTriangles(1);
						if (triangles[0] != null)
						{
							selectedPiece = triangles[0].GetComponent<Piece>();
							selectedPiece.onSelect();
						}
					}
				}
				else
				{
					if (Input.GetAxis("Horizontal") != 0.0 || Input.GetAxis("Vertical") != 0.0)
					{
						if (!isMoving)
						{
							isMoving = true;
							Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
							if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) direction -= new Vector3(0, direction.y, 0);
							else direction -= new Vector3(direction.x, 0, 0);
							selectedPiece.movePiece(direction);
						}
					}
				}
			}
		}
    }
	private void FixedUpdate()
	{
		if (triangles != null)
		{
			if (selectedPiece == null)
			{
				if (triangles != null)
				{
					triangles = GameObject.FindGameObjectsWithTag("Triangle");
					if (triangles != null && triangles.Length > 0)
					{
						selectedPiece = triangles[0].GetComponent<Piece>();
						selectedPiece.onSelect();
					}
				}
			}
		}
	}
}
