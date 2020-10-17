using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    //компоненты объекта "игрок"
    private Transform _transform;
    private Animator _animator;
    private Rigidbody _rigidbody;

    [Header("Speed")]
    public float _speed = 5; //скорость передвижения
    public float _walkTime; //время передвижения, служит для плавной остановки
    [Header("Jump Force")]
    public float jumpForce = 300f;// сила прыжка

    private MoveState _moveState = MoveState.Idle; // начальное состояние
    private DirectionState _directionState = DirectionState.Right; //начальное направление
    private bool directionBool = true; //переключатель для метода DirectionController()
    public bool jumpController = false; //переключатель в методах ходьбы, если пытаемся идти в воздухе


    void Start()
    {
        //записываем параметры компонентов в переменные
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_moveState == MoveState.Jump)//что делать если мы в прыжке
        {
            if (_rigidbody.velocity == Vector3.zero) //ждем когда вектор скорости станет 0
            {
                Idle();
                jumpController = false;
            }
            if (jumpController == true) //если во время прыжка пытаемся двигаться
            {
                _transform.position = new Vector3(_transform.position.x + (_directionState == DirectionState.Right ? _speed : -_speed) * Time.deltaTime, _transform.position.y, _transform.position.z);
            }
        }
        if (_moveState == MoveState.Walk )//если мы сейчасс идем
        {
            _transform.position = new Vector3(_transform.position.x + (_directionState == DirectionState.Right ? _speed : -_speed) * Time.deltaTime, _transform.position.y, _transform.position.z) ;
            _walkTime -= Time.deltaTime;
            if(_walkTime <= 0) Idle(); //уменьшаем переменную _walkTime и когда она равна или меньше 0 вызываем метод Idle()
        }
    }


    public void Idle() //метод покоя
    {
        _moveState = MoveState.Idle; //переключаем состояние на состояние покоя
        _animator.Play("Idle"); // включаем анимацию покоя
    }

    public void Jump() //метод прыжка
    {
        if (_moveState != MoveState.Jump) //проверяем, что мы не в прыжке
        {
            _moveState = MoveState.Jump; //меняем состояние на состояние прыжка
            _rigidbody.AddForce(Vector3.up * jumpForce); //прикладываем силу по вектору вверх с заданной силой
            _animator.Play("Jump");//запускаем анимацию прыжка
        }
    }

    public void MoveRight()
    {
        directionBool = true;
        if(_moveState != MoveState.Jump)//проверяем, что мы перемещаемся не в прыжке
        {
            _moveState = MoveState.Walk; //меняем состояние на ходьбу
            DirectionController(); //вызываем метод определяяющий направление игрока
            _animator.Play("Walk");
        }
        if(_moveState == MoveState.Jump)//если в прыжке
        {
            DirectionController();//вызываем метод определяяющий направление игрока
            jumpController = true;//для Update ставим галочку
        }
        _walkTime = 0.2f; //время ходьбы каждый кадр 0,2
    }

    public void MoveLeft()// аналогичен методу MoveRight()
    {
        directionBool = false;
        if (_moveState != MoveState.Jump)//проверяем, что мы перемещаемся не в прыжке
        {
            _moveState = MoveState.Walk; //меняем состояние на ходьбу
            DirectionController();
            _animator.Play("Walk");
        }
        if (_moveState == MoveState.Jump)
        {
            DirectionController();
            jumpController = true;
        }
        _walkTime = 0.2f;
    }

    public void DirectionController() // изменяет направление скина и состаяние лево/право
    {
        if (_directionState == DirectionState.Right && directionBool == false) //если смотрим вправо
        {
            _directionState = DirectionState.Left;//меняем состояние 
            _transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));//поворачиваем
        }
        
        if (_directionState == DirectionState.Left && directionBool == true) //если смотрим влево
        {
            _directionState = DirectionState.Right;
            _transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
    }

    enum DirectionState//направление игрока
    {
        Right,
        Left
    }

    enum MoveState //состояния (покой, ходьба, прыжок)
    {
        Idle,
        Walk,
        Jump
    }
}
