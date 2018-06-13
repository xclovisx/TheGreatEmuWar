using System;
using UnityEngine;

namespace MaterializationFX.Scripts
{
    internal sealed class MfxController : MonoBehaviour
    {
        private const string PositionPropertyName = "_Position";
        private const string DirectionPropertyName = "_Direction";
        private const string PositionTypePropertyName = "_PositionType";

        public MfxType MfxShaderType;
        public MfxDirection MfxDirection;
        public PositionType PositionType;
        public AnimationCurve Position;
        public AnimationCurve PositionX;
        public AnimationCurve PositionY;
        public AnimationCurve PositionZ;
        public float ScaleTime = 1;
        public float ScalePosition = 1;
        public bool MofidyChildren;

        public GameObject TargetObject;
        public bool ByDistance;
        public Vector3 WorldPositionOffset;

        private float _startTime;
        private bool _isEnabled;
        private string _shaderName;
        private ShaderParameterSetter _shaderParameterSetter;

        private void Start()
        {
            var shaderName = MfxShaderType.GetFullShaderName();

            GameObject go;

            if (TargetObject != null && ByDistance)
                go = gameObject;
            else if (TargetObject != null)
                go = TargetObject;
            else
                go = gameObject;
            
            _shaderParameterSetter = new ShaderParameterSetter();
            _shaderParameterSetter.Init(go, shaderName, modifyChildren: MofidyChildren);

            var dissolveDirection = MfxDirection.ToVector3();
            _shaderParameterSetter.SetVector(DirectionPropertyName, dissolveDirection);
            _shaderParameterSetter.SetInt(PositionTypePropertyName, (int) PositionType);

            _startTime = Time.time;
        }

        private void Update()
        {
            if (!_isEnabled)
                return;

            if (TargetObject != null)
            {
                Vector3 worldPos;

                if (ByDistance)
                    worldPos = TargetObject.transform.position - transform.position + WorldPositionOffset;
                else
                    worldPos = transform.position + WorldPositionOffset;

                _shaderParameterSetter.SetVector(DirectionPropertyName, worldPos);

                return;
            }

            var time = Time.time - _startTime;
            
            switch (PositionType)
            {
                case PositionType.Local:
                    var position = Position.Evaluate(time / ScaleTime) * ScalePosition;
                    _shaderParameterSetter.SetFloat(PositionPropertyName, position);
                    break;
                case PositionType.World:
                    var posX = transform.position.x + PositionX.Evaluate(time / ScaleTime) * ScalePosition;
                    var posY = transform.position.y + PositionY.Evaluate(time / ScaleTime) * ScalePosition;
                    var posZ = transform.position.z + PositionZ.Evaluate(time / ScaleTime) * ScalePosition;
                    var vector3 = new Vector3(posX, posY, posZ);
                    _shaderParameterSetter.SetVector(DirectionPropertyName, vector3 + WorldPositionOffset);
                    break;
            }
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

    internal enum MfxType
    {
        SingleAlbedo,
        TwoAlbedo,
    }

    internal enum MfxDirection
    {
        None,
        Normal,
        XAxys,
        YAxis,
        ZAxis
    }

    internal enum PositionType
    {
        Local,
        World
    }

    internal static class MfxControllerExtensions
    {
        public static string GetFullShaderName(this MfxType mfxType)
        {
            switch (mfxType)
            {
                case MfxType.SingleAlbedo:
                    return "QFX/MFX/MfxSingleAlbedo";
                case MfxType.TwoAlbedo:
                    return "QFX/MFX/MfxTwoAlbedo";
                default:
                    throw new ArgumentOutOfRangeException("mfxType", mfxType, null);
            }
        }

        public static Vector3 ToVector3(this MfxDirection mfxDirection)
        {
            switch (mfxDirection)
            {
                case MfxDirection.None:
                case MfxDirection.Normal:
                    return new Vector3(0, 0, 0);
                case MfxDirection.XAxys:
                    return new Vector3(1, 0, 0);
                case MfxDirection.YAxis:
                    return new Vector3(0, 1, 0);
                case MfxDirection.ZAxis:
                    return new Vector3(0, 0, 1);
                default:
                    return Vector3.zero;
            }
        }
    }
}