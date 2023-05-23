using UnityEngine;
using UnityEngine.InputSystem;
public abstract class BaseState
{
    public P_Character Character;
    protected Vector2 _input;

    protected InputAction _movement;
    protected InputAction _lightAttack;
    protected InputAction _heavyAttack;
    protected InputAction _blockAttack;

    protected bool isIdle = false;    
    protected bool isBlocked = false;

    protected float _playerSpeed = 5;

    private float _turnSmoothVelocity;
    private float _gravity = -9.8f;
    private float _gravityMultiplier = 3.0f;

    private Vector3 _velocity;

    public BaseState(P_Character character)
    {
        Character = character;
    }

    public virtual void EnterState()
    {
        Debug.Log($"Current State : " + this.ToString());
        _movement = Character.PlayerInput.actions["Move"];
        _lightAttack = Character.PlayerInput.actions["LightAttack"];
        _heavyAttack = Character.PlayerInput.actions["HeavyAttack"];
        _blockAttack = Character.PlayerInput.actions["BlockAttack"];

        _playerSpeed = Character.PlayerSpeed;
        _gravityMultiplier = Character.GravityMultiplier;
    }

    public virtual void InputManager() {
    }
    public virtual void LogicManager() { }
    public virtual void ExitState() { }

    //float targetAngle;
    protected void MovementUpdate(float speed)
    {
        float targetAngle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg + Character.CameraTransform.eulerAngles.y;
        //targetAngle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(Character.transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, Character.TurnSmoothDamp);
        Character.transform.rotation = Quaternion.Euler(0, angle, 0);
        Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        //Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * new Vector3(1,0,1);
        Character.Controller.Move(moveDir.normalized * speed * Time.deltaTime);
        
        addGeavity();
    }

    protected void addGeavity()
    {
        if (Character.Controller.isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }
        else
        {
            _velocity.y += _gravity * _gravityMultiplier * Time.deltaTime;
        }

        Character.Controller.Move(_velocity * Time.deltaTime);
    }

}
