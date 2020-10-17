using UnityEngine;

public class Enemy : MonoBehaviour
{
    //компоненты объекта "враг"
    private Transform _transform;
    private Animator _animator;
    private Rigidbody _rigidbody;

    public Transform[] points; // массив точек в которые ходит враг
    private float _speed = 1.5f; //скорость патрулирования
    private int _indexOfThePoint = 0; //переключатель 
    public float _patrolTime; //время патрулирования

    private MoveStateEnemy _moveStateEnemy = MoveStateEnemy.Patrol; //начальное состояние

    private Transform _player;
    private float _distanceToThePlayer;
    private bool _shase = true;
    private bool _isLeft;

    void Awake()
    {
        _player = FindObjectOfType<PlayerControll>().transform;
        _transform = GetComponent<Transform>();// запись компонента 
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _distanceToThePlayer = _player.position.x - _transform.position.x;//дистанция до игрока
        if (_distanceToThePlayer < 0) _isLeft = true;
        else _isLeft = false;
        if(_moveStateEnemy == MoveStateEnemy.Shase)
        {
            if (_shase == true)
            {
                Vector3 _position = _transform.position;
                _position.x += _distanceToThePlayer * _speed * Time.deltaTime;
                _transform.position = _position;
                //_rigidbody.AddForce(new Vector3((_position.x + _distanceToThePlayer), _position.y, _position.z) * 30 * Time.deltaTime);
            }
            if (Vector2.Distance(_transform.position, points[0].position) < 1f || Vector2.Distance(_transform.position, points[1].position) < 1f) //если враг подошел к краю платформы
            {
                _shase = false; // отключаем переключатель, благодаря которому он может идти
                if (_isLeft == true)
                {
                    if (_isLeft == false)
                    {
                        _shase = true;
                    }
                }
                else
                {
                    if (_isLeft == true)
                    {
                        _shase = true;
                    }
                }
            }
            else _shase = true;
        }

        if (_moveStateEnemy == MoveStateEnemy.Patrol)// если патрулируем
        {
            _transform.position = Vector2.MoveTowards(_transform.position, points[_indexOfThePoint].position, _speed * Time.deltaTime);//перемещаем врага от одной точки к другой
            if (Vector2.Distance(_transform.position, points[_indexOfThePoint].position) < 1f)//если враг подошел к нужной точке
            {
                if (_patrolTime <= 0) //если время патрулирования точки равно 0
                {
                    if (_indexOfThePoint == 0) _indexOfThePoint = 1;//примитивный переключатель для следующией точки
                    else _indexOfThePoint = 0;
                    _patrolTime = 5f;
                }
                else _patrolTime -= Time.deltaTime; // уменьшаем время
            }
            if (Mathf.Abs(_distanceToThePlayer) < 10)
            {
                _moveStateEnemy = MoveStateEnemy.Shase;
            }
        }
        if (Mathf.Abs(_distanceToThePlayer) > 10)
        {
            _moveStateEnemy = MoveStateEnemy.Patrol;
        }
    }

    enum MoveStateEnemy //состояния 
    {
        Patrol, // патрулирование
        Shase //погоня
    }
}
