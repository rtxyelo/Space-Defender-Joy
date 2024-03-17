using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace StarsAnimation
{
    [RequireComponent(typeof(Animator))]
    public class StarsAnimationController : MonoBehaviour
    {
        [HideInInspector]
        public bool isRandomAnimSpeed;

        [HideInInspector]
        public float animationSpeed = 1f;

        private void Start()
        {
            if (isRandomAnimSpeed)
            {
                animationSpeed = Random.Range(0.3f, 2f);
                //Debug.Log("Animation speed " + animationSpeed);
            }

            var animator = GetComponent<Animator>();
            animator.speed = animationSpeed;

            if (gameObject.CompareTag("StarUI"))
                animator.Play("BrillianceUI");
            else
                animator.Play("Brilliance");
        }
    }

    [CustomEditor(typeof(StarsAnimationController))]
    public class StarsAnimationControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (StarsAnimationController)target;

            script.isRandomAnimSpeed = EditorGUILayout.Toggle("Random Animation Speed", script.isRandomAnimSpeed);

            if (script.isRandomAnimSpeed == true)
                return;

            script.animationSpeed = EditorGUILayout.FloatField("Animation Speed", script.animationSpeed);
        }
    }
}
