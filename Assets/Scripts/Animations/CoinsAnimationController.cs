using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsAnimationController : MonoBehaviour
{
    [SerializeField]
    private List<Animator> coinAnimators = new(6);

    private void Start()
    {
        if (coinAnimators.Count != 0)
        {
            var ind = 1;
            foreach (var coin in coinAnimators)
            {
                coin.Play($"Flip{ind}");
                ind++;
            }
        }
    }
}
