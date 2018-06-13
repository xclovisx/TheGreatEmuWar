using UnityEngine;

namespace Assets.MaterializeFX.Scripts
{
    internal sealed class MfxController : MonoBehaviour
    {
        private const string MfxMaskOffsetProperty = "_MaskOffset";
        private const string MfxMaskPositionProperty = "_MaskWorldPosition";

        public AnimationCurve MaskOffsetCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public float ScaleTimeFactor = 1;
        public float ScaleOffsetFactor = 1;
        public bool ModifyChildren = true;
        public GameObject TargetObject;

        public bool ByDistance;
        public GameObject DistanceBasedObject;

        public bool ReplaceMaterial;
        public bool ReplaceMaterialMode; //Runtime, Editor
        public Material MfxMaterial;

        private float _startTime;
        private bool _isEnabled;
        private MfxObjectMaterialUpdater _mfxObjectMaterialUpdater;
        private Transform _targetTransform;

        public GameObject Target
        {
            get
            {
                return TargetObject != null ? TargetObject : gameObject;
            }
        }

        public void ReplaceMaterials()
        {
            _mfxObjectMaterialUpdater.Replace(MfxMaterial);
        }

        public void RevertMaterials()
        {
            _mfxObjectMaterialUpdater.Revert();
        }

        private void Start()
        {
            _mfxObjectMaterialUpdater = new MfxObjectMaterialUpdater(Target, ModifyChildren, ReplaceMaterial, MfxMaterial);

            _targetTransform = Target.transform;
            _startTime = Time.time;
        }

        private void Update()
        {
            if (!_isEnabled || _targetTransform == null)
                return;

            if (ByDistance)
            {
                if (DistanceBasedObject == null)
                {
                    Debug.LogError("By distance property was set, but object was not set");
                    return;
                }

                _mfxObjectMaterialUpdater.SetVector(MfxMaskPositionProperty, DistanceBasedObject.transform.position);

                return;
            }

            var time = Time.time - _startTime;
            var maskOffset = MaskOffsetCurve.Evaluate(time / ScaleTimeFactor) * ScaleOffsetFactor;
            _mfxObjectMaterialUpdater.SetFloat(MfxMaskOffsetProperty, maskOffset);
        }

        private void OnEnable()
        {
            _isEnabled = true;
        }

        private void OnDisable()
        {
            _isEnabled = false;
        }
    }
}