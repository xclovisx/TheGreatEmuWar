using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.MaterializeFX.Scripts
{
    internal sealed class MfxObjectMaterialUpdater
    {
        private readonly Renderer[] _renderers;
        private readonly Dictionary<Renderer, Material[]> _rendererToOriginalMaterialsMap = new Dictionary<Renderer, Material[]>();
        private readonly List<Material> _mfxMaterials = new List<Material>();

        public MfxObjectMaterialUpdater(GameObject targetObject, bool modifyChildren, bool replaceMaterials, Material mfxMaterialTemplate)
        {
            _renderers = modifyChildren ? targetObject.GetComponentsInChildren<Renderer>() : targetObject.GetComponents<Renderer>();

            if (!replaceMaterials)
            {
                foreach (var renderer in _renderers)
                    foreach (var material in renderer.sharedMaterials)
                        _mfxMaterials.Add(material);
                return;
            }

            Replace(mfxMaterialTemplate);
        }

        public void SetFloat(string propertyName, float value)
        {
            foreach (var mfxMaterial in _mfxMaterials)
                mfxMaterial.SetFloat(propertyName, value);
        }

        public void SetInt(string propertyName, int value)
        {
            foreach (var mfxMaterial in _mfxMaterials)
                mfxMaterial.SetInt(propertyName, value);
        }

        public void SetVector(string propertyName, Vector3 value)
        {
            foreach (var mfxMaterial in _mfxMaterials)
                mfxMaterial.SetVector(propertyName, value);
        }

        public void Replace(Material mfxMaterialTemplate)
        {
            _rendererToOriginalMaterialsMap.Clear();
            _mfxMaterials.Clear();

            foreach (var renderer in _renderers)
            {
                var rendererSharedMaterials = renderer.sharedMaterials;

                _rendererToOriginalMaterialsMap[renderer] = rendererSharedMaterials;
                var newMaterials = MfxMaterialUtil.ReplaceMaterialsToMfx(mfxMaterialTemplate, rendererSharedMaterials, false);
                renderer.sharedMaterials = newMaterials.ToArray();
                _mfxMaterials.AddRange(newMaterials);
            }
        }

        public void Revert()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_rendererToOriginalMaterialsMap.ContainsKey(_renderers[i]))
                    _renderers[i].materials = _rendererToOriginalMaterialsMap[_renderers[i]];
            }

            _rendererToOriginalMaterialsMap.Clear();

            foreach (var mfxMaterial in _mfxMaterials)
                Object.DestroyImmediate(mfxMaterial);

            _mfxMaterials.Clear();
        }
    }
}