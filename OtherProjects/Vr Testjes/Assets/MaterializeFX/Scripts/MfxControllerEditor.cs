using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Assets.MaterializeFX.Scripts
{
    [CustomEditor(typeof(MfxController))]
    internal sealed class MfxControllerEditor : Editor
    {
        private GameObject _targetObject;
        private bool _modifyChildren;

        private AnimationCurve _maskOffsetCurve;
        private string _scaleTimeFactor;
        private string _scalePositionFactor;

        private bool _byDistance;
        private GameObject _distanceBasedObject;

        private bool _replaceMaterial;
        private bool _replaceMaterialMode;
        private Material _mfxMaterial;

        public override void OnInspectorGUI()
        {
            var mfxController = (MfxController)target;

            _targetObject = mfxController.TargetObject;
            _modifyChildren = mfxController.ModifyChildren;

            _maskOffsetCurve = mfxController.MaskOffsetCurve;
            _scaleTimeFactor = mfxController.ScaleTimeFactor.ToString(CultureInfo.InvariantCulture);
            _scalePositionFactor = mfxController.ScaleOffsetFactor.ToString(CultureInfo.InvariantCulture);

            _byDistance = mfxController.ByDistance;
            _distanceBasedObject = mfxController.DistanceBasedObject;

            _replaceMaterial = mfxController.ReplaceMaterial;
            _replaceMaterialMode = mfxController.ReplaceMaterialMode;
            _mfxMaterial = mfxController.MfxMaterial;

            EditorGUILayout.Separator();

            // Modify children
            EditorGUILayout.Separator();
            _modifyChildren = EditorGUILayout.Toggle(MfxEditorLocalization.ModifyChildrenLabel, _modifyChildren);
            mfxController.ModifyChildren = _modifyChildren;

            // Target object
            _targetObject = (GameObject)EditorGUILayout.ObjectField(MfxEditorLocalization.TargetObjectLabel, _targetObject, typeof(GameObject), true);
            mfxController.TargetObject = _targetObject;

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField(MfxEditorLocalization.DistanceParamsLabel, EditorStyles.boldLabel);
          
            // ReplaceMaterials depending on the distance
            _byDistance = EditorGUILayout.Toggle(MfxEditorLocalization.ByDistanceLabel, _byDistance);
            mfxController.ByDistance = _byDistance;

            // Object To Calculate Distance
            if (_byDistance)
            {
                _distanceBasedObject = (GameObject)EditorGUILayout.ObjectField(MfxEditorLocalization.DistanceBasedObjectLabel, _distanceBasedObject, typeof(GameObject), true);
                mfxController.DistanceBasedObject = _distanceBasedObject;
            }

            if (!_byDistance)
            {
                // Direction type
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(MfxEditorLocalization.MfxParamsLabel, EditorStyles.boldLabel);

                _maskOffsetCurve = EditorGUILayout.CurveField(MfxEditorLocalization.MaskOffsetCurve, _maskOffsetCurve);
                mfxController.MaskOffsetCurve = _maskOffsetCurve;
               
                _scaleTimeFactor = EditorGUILayout.TextField(MfxEditorLocalization.ScaleTimeLabel, _scaleTimeFactor);
                _scalePositionFactor = EditorGUILayout.TextField(MfxEditorLocalization.ScalePositionLabel, _scalePositionFactor);
                mfxController.ScaleTimeFactor = float.Parse(_scaleTimeFactor);
                mfxController.ScaleOffsetFactor = float.Parse(_scalePositionFactor);
            }

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField(MfxEditorLocalization.ReplaceMaterialParamsLabel, EditorStyles.boldLabel);

            _replaceMaterial = EditorGUILayout.Toggle(MfxEditorLocalization.ReplaceMaterialLabel, _replaceMaterial);

            mfxController.ReplaceMaterial = _replaceMaterial;

            if (_replaceMaterial)
            {
                _mfxMaterial = (Material)EditorGUILayout.ObjectField(MfxEditorLocalization.MaterialLabel, _mfxMaterial, typeof(Material), true);
                mfxController.MfxMaterial = _mfxMaterial;
            }

            if (_replaceMaterial)
            {
                _replaceMaterialMode = EditorGUILayout.Toggle(MfxEditorLocalization.ReplaceMaterialInEditorLabel, _replaceMaterialMode);
                mfxController.ReplaceMaterialMode = _replaceMaterialMode;

                if (_replaceMaterial && _replaceMaterialMode)
                {
                    if (GUILayout.Button(MfxEditorLocalization.ReplaceMaterialButton))
                    {
                        if (_mfxMaterial == null)
                            Debug.LogWarning("template mfx materials is not selected");
                        else
                        {
                            var targetObject = mfxController.Target;

                            MfxMaterialUtil.ReplaceRenderersMaterials(_mfxMaterial, targetObject, true);
                        }
                    }
                }
            }

            EditorUtility.SetDirty(target);
        }

        private static class MfxEditorLocalization
        {
            public const string TargetObjectLabel = "Target Object";
            public const string ModifyChildrenLabel = "Modify Children";
            public const string DistanceParamsLabel = "Distance Params";
            public const string ByDistanceLabel = "Depending on the distance";
            public const string DistanceBasedObjectLabel = "Object to calcualte distance";

            public const string MfxParamsLabel = "Mfx Params";
            public const string MaskOffsetCurve = "Mask Offset Curve";
            public const string ScaleTimeLabel = "Scale Time Factor";
            public const string ScalePositionLabel = "Scale Offset Factor";

            public const string ReplaceMaterialParamsLabel = "Replace Material Params";
            public const string ReplaceMaterialLabel = "Replace Material";
            public const string ReplaceMaterialInEditorLabel = "Replace in Editor";
            public const string ReplaceMaterialButton = "Copy & Replace";
            public const string MaterialLabel = "Mfx Material Template";
        }
    }
}