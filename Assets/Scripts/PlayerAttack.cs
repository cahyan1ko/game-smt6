using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour, PlayerController.IAttackActions
{
    private PlayerController m_PlayerController;
    private InputAction m_AttackAction;
    public Animator playerAnimator;
    public float attackCooldown = 0.767f;
    private bool canAttack = true;

    private void Awake()
    {
        m_PlayerController = new PlayerController();
        m_AttackAction = m_PlayerController.Attack.Attack;
        m_PlayerController.Attack.SetCallbacks(this);
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable() => m_PlayerController.Enable();
    private void OnDisable() => m_PlayerController.Disable();

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
        {
            Debug.Log("Attack input detected");
            PerformAttack();
        }
    }

    public void MobileAttack()
    {
        if (canAttack)
        {
            PerformAttack();
        }
    }


    private void PerformAttack()
    {
        Debug.Log("Attack triggered!");

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("AttackBool", true); // ✅ Mengatur parameter bool menjadi true
        }

        canAttack = false;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("AttackBool", false); // ✅ Reset menjadi false setelah cooldown
        }

        canAttack = true;
    }
}
