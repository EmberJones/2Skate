using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RandomAIMove : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range = 10f; // radius of sphere

    public Transform centrePoint;

    [Header("Idle Settings")]
    public float minIdleTime = 1.5f;
    public float maxIdleTime = 4f;

    [Header("Ragdoll Integration")]
    public AIRagdollController ragdollController;

    private float idleTimer;
    private bool isWaiting;
    private bool pendingRetry;

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (ragdollController == null) ragdollController = GetComponent<AIRagdollController>();

        agent.avoidancePriority = Random.Range(0, 99);
        PickNewDestination();
    }

    private void Update()
    {
        // Ragdoll owns movement right now 
        if (ragdollController != null && ragdollController.IsRagdolled) 
        {
            //StartCoroutine(Wait());
            return;
        } 

        if (agent.pathPending) return;

        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
            idleTimer = Random.Range(minIdleTime, maxIdleTime);
        }

        if (isWaiting)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                isWaiting = false;
                PickNewDestination();
            }
        }
    }

    IEnumerator Wait()
    {

        yield return new WaitForSeconds(5f);
        isWaiting = true;
        //PickNewDestination();

    }

    private void PickNewDestination()
    {
        if (RandomPoint(centrePoint.position, range, out Vector3 point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            agent.SetDestination(point);
        }
        else if (!pendingRetry)
        {
            // Couldn't find a valid point this frame (e.g. sphere mostly off the mesh) -
            // try again shortly instead of getting stuck idle forever.
            pendingRetry = true;
            Invoke(nameof(RetryPickDestination), 0.2f);
        }
    }

    private void RetryPickDestination()
    {
        pendingRetry = false;
        PickNewDestination();
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        const int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            // Flatten to a horizontal disc rather than a full sphere - for a room/floor
            // NPC, points above/below the walkable surface just waste sampling attempts.
            Vector2 circle = Random.insideUnitCircle * range;
            Vector3 randomPoint = center + new Vector3(circle.x, 0f, circle.y);

            // maxDistance scales with range instead of a fixed 1f, so larger search
            // areas don't silently fail to find a nearby NavMesh point.
            float sampleDistance = Mathf.Max(2f, range * 0.25f);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, sampleDistance, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (centrePoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(centrePoint.position, range);
    }
}