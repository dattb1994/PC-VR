using System.Collections;
using UnityEngine;

public class ShakingChecker : MonoBehaviour
{
    public bool shook = false;
    [SerializeField] float threshold;
    [SerializeField] float thresholdNum = 10;
    [SerializeField] float delay = .05f;
    public int numShark = 0;
    [SerializeField] Transform root;

    Vector3 prePos;
    private IEnumerator Start()
    {
        if (root == null)
            root = transform;
        while (true)
        {
            prePos = root.position;
            yield return new WaitForSeconds(delay);
            float distance = Vector3.Distance(prePos, root.position);
            if (distance >= threshold)
            {
                numShark++;
                if (!shook && numShark >= thresholdNum)
                {
                    shook = true;
                }
            }
            else
            {
                if (numShark > 0)
                {
                    numShark--;
                }
            }
        }
    }

    public void StartChecking()
    {
        shook = false;
        numShark = 0;
    }
}
