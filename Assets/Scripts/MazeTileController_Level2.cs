using UnityEngine;

public class MazeTileController : MonoBehaviour
{
    public GameObject frontWall;
    public GameObject backWall;
    public GameObject leftWall;
    public GameObject rightWall;

    public void SetWalls(bool front, bool back, bool left, bool right)
    {
        if (frontWall != null) frontWall.SetActive(front);
        if (backWall != null) backWall.SetActive(back);
        if (leftWall != null) leftWall.SetActive(left);
        if (rightWall != null) rightWall.SetActive(right);
    }
}