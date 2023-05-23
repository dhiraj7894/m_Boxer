using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using RehtseStudio.FreeLookCamera3rdPersonCharacterController.Scripts;

[RequireComponent(typeof(CharacterController),typeof(PlayerInput))]
public class P_Character : Singleton<P_Character>
{
    private BaseState CurrentState;


    public PlayerInput PlayerInput;
    public CharacterController Controller;
    public Rigidbody rb;
    public Animator Animator;
    public Transform CameraTransform;
    public GameObject HUI;
    //public GameObject FreeLookCamera;
    public PlayerControllerWithFreeLookCamera PCWFC;

    public float PlayerSpeed = 5;
    public float GravityMultiplier = 3;

    [Range(0,1)]
    public float TurnSmoothDamp = 0.1f;

    [Range(0, 1)]
    public float PlayerSmoothDamp = 0.1f;

    [Range(0, 1)]
    public float PlayerAttackSpeedModifier = 0;

    public IdleState IDLE;
    public AttackState ATTACKING;
    public HeavyAttack HEAVYATTACKING;

    public PhotonView PV;
   

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        PV = GetComponent<PhotonView>();

        IDLE = new IdleState(this);
        ATTACKING = new AttackState(this);
        HEAVYATTACKING = new HeavyAttack(this);
        CurrentState = IDLE;
        CurrentState.EnterState();

        if (!PV.IsMine)
        {
            Destroy(CameraTransform.gameObject);
            Destroy(HUI);
            Destroy(rb);
            //Destroy(FreeLookCamera);
            Destroy(PCWFC);
            Destroy(GetComponentInChildren<Cinemachine.CinemachineFreeLook>().gameObject);
        }

    }
    private void Update()
    {
        if (PV.IsMine)
        {            
            CurrentState.InputManager();
            CurrentState.LogicManager();
        }
        else { return; }
        
    }

    public void ChangeState(BaseState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
