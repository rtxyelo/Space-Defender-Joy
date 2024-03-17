using UnityEngine;
using DG.Tweening;

public class PlanetsAnimationController : MonoBehaviour
{
    private enum Rotatindirection
    {
        Left = -1, 
        Right = 1
    }

    [SerializeField]
    private Rotatindirection rotatinDirection;

    [SerializeField]
    private float rotationSpeed = 10f;

    private Vector3 rotationAmount = new(0, 0, 360);

    void Start()
    {
        DOTween.Init();

        rotationAmount *= (int)rotatinDirection;

        transform.DORotate(rotationAmount, rotationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear)
            .SetSpeedBased(true);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform, true);
        DOTween.Clear();
    }
}
