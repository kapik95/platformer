
using UnityEngine;

public class CameraMoveToPlayer : MonoBehaviour
{
    public float _dumping = 1.5f; //сглаживание
    public Vector2 _offset = new Vector2(6f, 2.5f); //вектор смещения
    public bool isLeft;
    private Transform _player;
    private int lastX;

    void Start()
    {
        _offset = new Vector2(Mathf.Abs(_offset.x), _offset.y);
        FindPlayer(isLeft);
    }

    public void FindPlayer(bool playerIsLeft)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        lastX = Mathf.RoundToInt(_player.position.x);
        if (playerIsLeft)
        {
            transform.position = new Vector3(_player.position.x - _offset.x, _player.position.y - _offset.y, transform.position.z);
        }
        else transform.position = new Vector3(_player.position.x + _offset.x, _player.position.y + _offset.y, transform.position.z);
    }
    void FixedUpdate()
    {
        if (_player)
        {
            int currentX = Mathf.RoundToInt(_player.position.x);
            if (currentX > lastX) isLeft = false;
            else if (currentX < lastX) isLeft = true;
            lastX = Mathf.RoundToInt(_player.position.x);

            Vector3 target;
            if (isLeft)
            {
                target = new Vector3(_player.position.x - _offset.x, _player.position.y + _offset.y, transform.position.z);
            }
            else target = new Vector3(_player.position.x + _offset.x, _player.position.y + _offset.y, transform.position.z);

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, _dumping * Time.deltaTime);
            transform.position = currentPosition;
        }
    }
}
