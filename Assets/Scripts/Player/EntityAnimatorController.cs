using UnityEngine;

public enum EntityAnimState
{
    None = 0,
    Jump,
    Fall
}
public class EntityAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator Animator_Entity;
    [SerializeField] private TierUP_3DPlayerController Controller_Player;

    private EntityAnimState _currentAnimState;

    private void Awake()
    {
        if (Controller_Player == null)
        {
            Controller_Player = GetComponent<TierUP_3DPlayerController>();
        }

        if (Controller_Player == null)
        {
            Debug.LogWarning($"Controller 컴포넌트를 찾을 수 없습니다 : {gameObject.name}");
            return;
        }

        Controller_Player.OnStateChanged += SetState;
        Controller_Player.OnMoveSpeedChanged += SetMoveSpeed;
    }

    private void OnDestroy()
    {
        if (Controller_Player == null)
        {
            return;
        }

        Controller_Player.OnStateChanged -= SetState;
        Controller_Player.OnMoveSpeedChanged -= SetMoveSpeed;
    }

    public void SetState(EntityAnimState newState)
    {
        if (newState == _currentAnimState)
        {
            return;
        }

        _currentAnimState = newState;
        ResetAllAnimParameters();

        switch (_currentAnimState)
        {
            case EntityAnimState.Jump:
                Animator_Entity.SetBool("IsJump", true);
                break;
            case EntityAnimState.Fall:
                Animator_Entity.SetBool("IsFall", true);
                break;
            default:
                break;
        }
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        Animator_Entity.SetFloat("MoveSpeed", moveSpeed);
    }

    private void ResetAllAnimParameters()
    {
        Animator_Entity.SetBool("IsJump", false);
        Animator_Entity.SetBool("IsFall", false);
    }
}