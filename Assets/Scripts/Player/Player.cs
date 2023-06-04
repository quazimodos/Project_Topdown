using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    //SerializeFields
    [Header("Options")]
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float rotationSpeed = .4f;

    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Target")]
    [SerializeField] AutoTargetManager autoTargetManager;

    //Components   
    private PlayerInput playerInput;
    private CharacterController controller;
    private GunController gunController;
    private Modifiers modifiers;
    //Vectors
    private Vector3 playerVelocity;
    private Vector2 aim;
    //InputActions
    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction shootAction;
    private InputAction reloadAction;
    Transform targetEnemy;

    //Booleans
    private bool groundedPlayer;

    private bool isGameOver;

    protected override void Start()
    {
        base.Start();

        gunController = GetComponent<GunController>();
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        modifiers = GetComponent<Modifiers>();

        moveAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
        shootAction = playerInput.actions["Shoot"];
        reloadAction = playerInput.actions["Reload"];

        GameManager.Instance.OnGameOverEvent += GameManager_OnGameOverEvent;
        GameManager.Instance.OnGameWonEvent += GameManager_OnGameWonEvent;
    }

    private void GameManager_OnGameWonEvent(object sender, GameManager.OnGameWonEventEventArgs e)
    {
        OnGameOver();
    }

    private void GameManager_OnGameOverEvent(object sender, System.EventArgs e)
    {
        OnGameOver();
    }

    void Update()
    {
        if (isGameOver) return;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        targetEnemy = autoTargetManager.TargetEnemy();

        //Movement Input
        Movement();

        //WeaponInput
        if (shootAction.ReadValue<float>() > 0.2f)
        {
            gunController.OnTriggerHold();
        }
        if (shootAction.ReadValue<float>() < 0.2f)
        {
            gunController.OnTriggerRelease();
        }
        if (reloadAction.triggered)
        {
            gunController.Reload();
        }
    }
    private void Movement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * Vector3.right + move.z * Vector3.forward;
        move.y = 0f;
        controller.Move((playerSpeed * modifiers.playerSpeed) * Time.deltaTime * move);


        if (targetEnemy)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetEnemy.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            if (targetAngle != 0)
            {
                Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }

        if (input != Vector2.zero)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    public void ReCalculateHealth()
    {
        var totalHealth = GetTotalHealth();
        var currentHealth = GetCurrentHealth();

        var percentage = (currentHealth / totalHealth);

        var increasedMaxHealth = totalHealth * modifiers.health;
        var increasedCurrentHealth = increasedMaxHealth * percentage;

        SetCurrentHealth(increasedCurrentHealth);
        SetTotalHealth(increasedMaxHealth);
    }


    private void OnGameOver()
    {
        isGameOver = true;
        animator.SetBool("Run", false);
    }
}
