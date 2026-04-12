using System.Collections;
using UnityEngine;

public class Trigger2WayTask : TriggerTask
{
    [Header("Trigger 2 Way Task")]
    [SerializeField] private PropType _badPropType = PropType.None;
    [SerializeField] private int _badScore = 100;

    [Header("Shrink Settings")]
    [SerializeField] private float _destroyAnimDuration = 0.5f;

    protected override void OnTriggerEnter(Collider other)
    {
        if (Done || !other.TryGetComponent(out Prop prop))
        {
            prop = other.GetComponentInParent<Prop>();
            if (prop == null) return;
        }

        if (IsTypeAccepted(prop.Type))
        {
            PlayerController playerController = prop.Controller as PlayerController;
            if (playerController == null && PlayerManager.Instance != null)
                playerController = PlayerManager.Instance.Players.Find(p => p.Id == prop.OwnerId);

            Succeed(playerController);
            GameManager.Instance.Scores.AddMissionScore(score, prop.OwnerId);
        }
        else if (prop.Type == _badPropType)
        {
            Failed();
            GameManager.Instance.Scores.SubMissionScore(_badScore, prop.OwnerId);
        }
        else return;

        if (isDestroyed) StartCoroutine(AnimateAndDestroy(prop.gameObject));
    }

    private IEnumerator AnimateAndDestroy(GameObject target)
    {
        if (target == null) yield break;

        Collider col = target.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Vector3 originalScale = target.transform.localScale;
        float timer = 0f;

        while (timer < _destroyAnimDuration)
        {
            if (target == null) yield break;
            timer += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, timer / _destroyAnimDuration);
            yield return null;
        }

        if (target != null) Destroy(target);
    }
}