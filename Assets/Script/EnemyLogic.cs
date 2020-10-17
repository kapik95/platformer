using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    // Данные объекта "враг"
    private Transform _transformEnemy;
    private Animator _animatorEnemy;
    private Rigidbody _rigidbodyEnemy;
    private MoveEnemy _moveEnemy = MoveEnemy.Patrol;
    private DirectionState _directionState = DirectionState.Right;

    // Данные объекта "игрок"
    private Transform _playerTransform;

    private float _speedEnemy; //скорость врага
    private float _distanceToThePlayer; //дистанция до игрока
    private bool _isLeft; //переключатель в зависимости от положения игрока

    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerControll>().transform;
        _transformEnemy = GetComponent<Transform>();
        _rigidbodyEnemy = GetComponent<Rigidbody>();
        //_animatorEnemy = GetComponent<Animator>(); инициализировать по готовности врага
        _directionState = transform.localScale.x > 0 ? DirectionState.Right : DirectionState.Left;
        Patrol();
    }

    private void FixedUpdate()
    {
        //Получаем текущую дистанцию до игрока (видит ли его враг)
        _distanceToThePlayer = _playerTransform.position.x - _transformEnemy.position.x;
        //Проверяем с какой стороны игрок
        if (_distanceToThePlayer < 0) _isLeft = true;
        else _isLeft = false; //делать проверку в погоне, а не всегда

        if (_moveEnemy == MoveEnemy.Patrol) //если враг сейчас патрулирует
        {
            //если враг смотрит вправо, то применяем вектор вправо, иначе влево 
            _rigidbodyEnemy.velocity = ((_directionState == DirectionState.Right ? Vector2.right : Vector2.left)
                                   * _speedEnemy * Time.deltaTime); //добавляем скорость враг(выбираем вектор и умножаем его на скорость)
        }
    }
    private void OnTriggerEnter(Collider other)//когда враг доходит до контрольных точек срабатывает метод
    {
        if (other.tag == "point" )
        {
            if (_directionState == DirectionState.Right)
            {
                _transformEnemy.localScale = new Vector3(-_transformEnemy.localScale.x, _transformEnemy.localScale.y, _transformEnemy.localScale.z);
                _directionState = DirectionState.Left;
            }
            else
            {
                _transformEnemy.localScale = new Vector3(-_transformEnemy.localScale.x, _transformEnemy.localScale.y, _transformEnemy.localScale.z);
                _directionState = DirectionState.Right;
            }
        }
    }
    private void Patrol()
    {
        _speedEnemy = 50f;
    }

    enum DirectionState
    {
        Right,
        Left
    }

    enum MoveEnemy //Состояния врага (добавить атаку)
    {
        Patrol, // патрулирование
        Shase //погоня
    }
}
