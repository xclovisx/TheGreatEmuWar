using UnityEngine;

namespace MaterializationFX.Scripts
{
    internal sealed class ShaderParameterSetter
    {
        private Renderer[] _rends;

        public void Init(GameObject targetObject, string shaderName, bool modifyChildren)
        {
            _rends = !modifyChildren
                ? new[] {targetObject.GetComponent<Renderer>()}
                : targetObject.GetComponentsInChildren<Renderer>();

            foreach (var rend in _rends)
                rend.material.shader = Shader.Find(shaderName);
        }

        public void SetFloat(string propertyName, float value)
        {
            foreach (var rend in _rends)
                rend.material.SetFloat(propertyName, value);
        }

        public void SetInt(string propertyName, int value)
        {
            foreach (var rend in _rends)
                rend.material.SetInt(propertyName, value);
        }
        
        public void SetVector(string propertyName, Vector3 value)
        {
            foreach (var rend in _rends)
                rend.material.SetVector(propertyName, value);
        }
    }
}