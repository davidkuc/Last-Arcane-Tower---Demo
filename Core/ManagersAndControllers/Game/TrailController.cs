using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class TrailController : MonoBehaviour
{
    [SerializeField] private float trailLifeTime = 0.1f;

    private TrailRenderer trailRenderer;

    public bool IsTrailActive => trailRenderer.enabled;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        StartCoroutine(Trail());
        SetTrailLifeTime();
    }

    public void Clear() => trailRenderer.Clear();

    private void SetTrailLifeTime() => trailRenderer.time = trailLifeTime;

    public void ToggleTrail(bool active) => trailRenderer.enabled = active;

    private IEnumerator Trail()
    {
        while (true)
        {
            transform.position = InputManager.Instance.PrimaryPosition();
            yield return null;
        }
    }
}
