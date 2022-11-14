using UnityEngine;

public static class Helpers 
{
    public static bool SetActive_Toggle(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
        return go.activeInHierarchy;
    }

    public static GameObject SetEnemyMoveDirection(this GameObject go, float newMovementSpeed)
    {
        var enemyControl = go.GetComponent<BaseEnemyControl>();
        enemyControl.overrideMovementSpeed = true;
        enemyControl.newMovementSpeed = newMovementSpeed;

        return go;
    }

    public static GameObject SetEnemyFacingDirection(this GameObject go, float direction)
    {
        go.transform.localScale = new Vector3(go.transform.localScale.x * Mathf.Sign(direction), 
            go.transform.localScale.y, go.transform.localScale.z);

        return go;
    }

    public static GameObject SetEnemyRigidbodyToDynamic(this GameObject go)
    {
        go.GetComponent<Rigidbody2D>().isKinematic = false;

        return go;
    }
}
