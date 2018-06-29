using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	Vector3 moveJump = Vector2.zero;
	float horMove, vertMove;
	void Start(){
		SheetAssigner SA = FindObjectOfType<SheetAssigner>();
		Vector2 tempJump = SA.roomDimensions + SA.gutterSize;
		moveJump = new Vector3(tempJump.x, tempJump.y, 0); //distance b/w rooms: to be used for movement
	}
	void Update()
	{
		if (Input.GetKeyDown("w") || Input.GetKeyDown("s") || 
			Input.GetKeyDown("a") || Input.GetKeyDown("d")) //if any 'wasd' key is pressed
		{
			horMove = System.Math.Sign(Input.GetAxisRaw("Horizontal"));//capture input
			vertMove = System.Math.Sign(Input.GetAxisRaw("Vertical"));
			Vector3 tempPos = transform.position;
			tempPos += Vector3.right * horMove * moveJump.x; //jump bnetween rooms based opn input
			tempPos += Vector3.up * vertMove * moveJump.y;
            //transform.position = tempPos;
            //print(horMove + " " + vertMove);
		}
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.name == "DoorTriggerCubeDown(Clone)") {
            transform.position += Vector3.down * moveJump.y;
            transform.position += new Vector3(0, 0, 100);
            print("Down");
        }
        if (col.name == "DoorTriggerCubeUp(Clone)")
        {
            transform.position += Vector3.up * moveJump.y;
            transform.position += new Vector3(0, 0, -100);
            print("Up");
        }
        if (col.name == "DoorTriggerCubeLeft(Clone)")
        {
            transform.position += Vector3.left * moveJump.x / 2;
            print("Left");
        }
        if (col.name == "DoorTriggerCubeRight(Clone)")
        {
            transform.position += Vector3.right * moveJump.x / 2;
            print("Right");
        }
    }
}
