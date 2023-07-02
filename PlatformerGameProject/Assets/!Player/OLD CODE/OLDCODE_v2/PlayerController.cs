using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Playerr Player;
    private float horizontalMove;

    private void Start()
    {

        Player = Player == null ? GetComponent<Playerr>() : Player;
        if (Player == null)
        {
            Debug.LogError("Player not set to controller");
        }
    }

    private void Update()
    {
        if (Player != null)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A)))
            {      
                Player.Move(horizontalMove);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Player.Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Player.Dash();
            }
        }
    }
}
