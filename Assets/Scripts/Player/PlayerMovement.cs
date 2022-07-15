using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float playerMoveSpeed;
    private float horizontalInput;
    private float verticalInput;

    public float GetPlayerMoveSpeed()
    {
        return playerMoveSpeed;
    }
    public void SetPlayerMoveSpeed(float desiredSpeed)
    {
        playerMoveSpeed = desiredSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputValue();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void GetInputValue()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        Vector3 movementDirection = new Vector3(horizontalInput,verticalInput);
        transform.Translate(movementDirection.normalized * playerMoveSpeed * Time.fixedDeltaTime);
    }
}
