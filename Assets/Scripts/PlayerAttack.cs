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

    private void PerformAttack()
    {
        Debug.Log("Attack triggered!");

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Attack");
        }

        canAttack = false;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        // Tunggu selama cooldown, sesuaikan dengan durasi animasi Attack (0.5 detik atau sesuai kebutuhan)
        yield return new WaitForSeconds(attackCooldown);

        // Pastikan transisi kembali ke animasi Idle/Walk setelah cooldown selesai
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger("Attack");  // Reset trigger Attack setelah cooldown selesai
        }

        // Mengatur flag untuk bisa menyerang lagi
        canAttack = true;
    }
}
