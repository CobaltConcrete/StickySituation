using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<HumanPlayerController>() != null ||
            other.GetComponentInParent<SlimePlayerController>() != null)
        {
            GameManager.Instance.EndRun();
        }
    }
}