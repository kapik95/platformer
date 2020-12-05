using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardController : MonoBehaviour, IBeginDragHandler
{

    public PlayerControll PlayerControll;

    private void Start()
    {

        PlayerControll = PlayerControll == null ? GetComponent<PlayerControll>() : PlayerControll;
        if (PlayerControll == null)
        {
            Debug.LogError("Player not set to controller");
        }
    }

    private void FixedUpdate()
    {

        if (PlayerControll != null)
        {

            if (Input.GetKey(KeyCode.D) || Input.GetMouseButtonDown(0))
            {
                PlayerControll.MoveRight();
            }
            if (Input.GetKey(KeyCode.A))
            {
                PlayerControll.MoveLeft();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                PlayerControll.Jump();
            }

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        
        

    }
}