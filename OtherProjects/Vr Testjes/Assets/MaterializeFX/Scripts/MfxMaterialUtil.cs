using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.MaterializeFX.Scripts
{
    internal static class MfxMaterialUtil
    {
        private const string MfxShaderName = "QFX/MFX/Uber/Standart";

        private const string Color2PropName = "_Color2";
        private const string MainTex2PropName = "_MainTex2";
        private const string BumpMap2PropName = "_BumpMap2";
        private const string EmissionColor2PropName = "_EmissionColor2";
        private const string EmissionMap2PropName = "_EmissionMap2";
        private const string EmissionMap2ScrollPropName = "_EmissionMap2_Scroll";
        private const string EmissionSize2PropName = "_EmissionSize2";
        private const string EdgeColorPropName = "_EdgeColor";
        private const string EdgeSizePropName = "_EdgeSize";
        private const string EdgeStrengthPropName = "_EdgeStrength";
        private const string EdgeRampMap1PropName = "_EdgeRampMap1";
        private const string EdgeRampMap1ScrollPropName = "_EdgeRampMap1_Scroll";
        private const string DissolveMap1PropName = "_DissolveMap1";
        private const string DissolveMap1ScrollPropName = "_DissolveMap1_Scroll";
        private const string MaskPropName = "_Mask";
        private const string CutoffAxisPropName = "_CutoffAxis";
        private const string MaskOffsetPropName = "_MaskOffset";
        private const string MaskPositionPropName = "_MaskPosition";
        private const string DissolveSizePropName = "_DissolveSize";
        private const string DissolveEdgeColorPropName = "_DissolveEdgeColor";
        private const string DissolveEdgeSizePropName = "_DissolveEdgeSize";

        public static void CopyMfxProperties(Material mfxTemplateMaterial, Material emptyMfxMaterial)
        {
            CopyPropertyToMaterial<Color>(mfxTemplateMaterial, emptyMfxMaterial, Color2PropName, Color2PropName);
            CopyPropertyToMaterial<Texture>(mfxTemplateMaterial, emptyMfxMaterial, MainTex2PropName, MainTex2PropName);
            CopyPropertyToMaterial<Texture>(mfxTemplateMaterial, emptyMfxMaterial, BumpMap2PropName, BumpMap2PropName);
            CopyPropertyToMaterial<Color>(mfxTemplateMaterial, emptyMfxMaterial, EmissionColor2PropName, EmissionColor2PropName);
            CopyPropertyToMaterial<Texture>(mfxTemplateMaterial, emptyMfxMaterial, EmissionMap2PropName, EmissionMap2PropName);
            CopyPropertyToMaterial<Vector4>(mfxTemplateMaterial, emptyMfxMaterial, EmissionMap2ScrollPropName, EmissionMap2ScrollPropName);
            CopyPropertyToMaterial<float>(mfxTemplateMaterial, emptyMfxMaterial, EmissionSize2PropName, EmissionSize2PropName);
            CopyPropertyToMaterial<Color>(mfxTemplateMaterial, emptyMfxMaterial, EdgeColorPropName, EdgeColorPropName);
            CopyPropertyToMaterial<float>(mfxTemplateMaterial, emptyMfxMaterial, EdgeSizePropName, EdgeSizePropName);
            CopyPropertyToMaterial<float>(mfxTemplateMaterial, emptyMfxMaterial, EdgeStrengthPropName, EdgeStrengthPropName);
            CopyPropertyToMaterial<Texture>(mfxTemplateMaterial, emptyMfxMaterial, EdgeRampMap1PropName, EdgeRampMap1PropName);
            CopyPropertyToMaterial<Vector4>(mfxTemplateMaterial, emptyMfxMaterial, EdgeRampMap1ScrollPropName, EdgeRampMap1ScrollPropName);
            CopyPropertyToMaterial<Texture>(mfxTemplateMaterial, emptyMfxMaterial, DissolveMap1PropName, DissolveMap1PropName);
            CopyPropertyToMaterial<Vector4>(mfxTemplateMaterial, emptyMfxMaterial, DissolveMap1ScrollPropName, DissolveMap1ScrollPropName);
            CopyPropertyToMaterial<int>(mfxTemplateMaterial, emptyMfxMaterial, MaskPropName, MaskPropName);
            CopyPropertyToMaterial<int>(mfxTemplateMaterial, emptyMfxMaterial, CutoffAxisPropName, CutoffAxisPropName);
            CopyPropertyToMaterial<float>(mfxTemplateMaterial, emptyMfxMaterial, MaskOffsetPropName, MaskOffsetPropName);
            CopyPropertyToMaterial<Vector4>(mfxTemplateMaterial, emptyMfxMaterial, MaskPositionPropName, MaskPositionPropName);
            CopyPropertyToMaterial<float>(mfxTemplateMaterial, emptyMfxMaterial, DissolveSizePropName, DissolveSizePropName);
            CopyPropertyToMaterial<float>(mfxTemplateMaterial, emptyMfxMaterial, DissolveSizePropName, DissolveSizePropName);
            CopyPropertyToMaterial<Color>(mfxTemplateMaterial, emptyMfxMaterial, DissolveEdgeColorPropName, DissolveEdgeColorPropName);
            CopyPropertyToMaterial<float>(mfxTemplateMaterial, emptyMfxMaterial, DissolveEdgeSizePropName, DissolveEdgeSizePropName);

            emptyMfxMaterial.renderQueue = mfxTemplateMaterial.renderQueue;
            emptyMfxMaterial.shaderKeywords = mfxTemplateMaterial.shaderKeywords;
            emptyMfxMaterial.SetOverrideTag("RenderType", mfxTemplateMaterial.GetTag("RenderType", false));
            emptyMfxMaterial.SetOverrideTag("Queue", mfxTemplateMaterial.GetTag("Queue", false));
            emptyMfxMaterial.SetOverrideTag("IsEmissive", mfxTemplateMaterial.GetTag("IsEmissive", false));
            emptyMfxMaterial.SetOverrideTag("PerformanceChecks", mfxTemplateMaterial.GetTag("PerformanceChecks", false));
            emptyMfxMaterial.SetOverrideTag("DisableBatching", mfxTemplateMaterial.GetTag("DisableBatching", false));
        }

        public static void ReplaceRenderersMaterials(Material mfxMaterial, GameObject targetObject, bool editorMode)
        {
            var renderers = targetObject.GetComponentsInChildren<Renderer>();

            foreach (var targetRenderer in renderers)
            {
                var targetRendererMaterials = targetRenderer.sharedMaterials;
                var newMaterials = ReplaceMaterialsToMfx(mfxMaterial, targetRendererMaterials, editorMode);
                targetRenderer.sharedMaterials = newMaterials.ToArray();
            }
        }

        public static List<Material> ReplaceMaterialsToMfx(Material mfxMaterial, Material[] targetRendererMaterials, bool editorMode)
        {
            var newMaterials = new List<Material>();

            foreach (var targetRendererMaterial in targetRendererMaterials)
            {
                if (targetRendererMaterial == null)
                    continue;

                string newAssetPath = string.Empty;

                if (editorMode)
                {
                    var materialPath = AssetDatabase.GetAssetPath(targetRendererMaterial);

                    var extensionIdx = materialPath.LastIndexOf("/", StringComparison.Ordinal);
                    if (extensionIdx <= 0)
                    {
                        Debug.LogError("the path is incorrect");
                        continue;
                    }

                    var pathWithoutExtension = materialPath.Substring(0, extensionIdx);

                    newAssetPath = pathWithoutExtension + "/" + targetRendererMaterial.name + "_MFX.mat";

                    var assetType = AssetDatabase.GetMainAssetTypeAtPath(newAssetPath);
                    if (assetType != null)
                    {
                        var existingMaterial = AssetDatabase.LoadAssetAtPath<Material>(newAssetPath);
                        newMaterials.Add(existingMaterial);
                        continue;
                    }
                }

                var newMaterial = new Material(targetRendererMaterial)
                {
                    shader = Shader.Find(MfxShaderName)
                };

                CopyMfxProperties(mfxMaterial, newMaterial);

                newMaterials.Add(newMaterial);

                if (editorMode)
                {
                    AssetDatabase.CreateAsset(newMaterial, newAssetPath);
                }
            }

            return newMaterials;
        }

        private static void CopyPropertyToMaterial<T>(Material fromMaterial, Material toMaterial, string fromProperty, string toProperty)
        {
            if (!fromMaterial.HasProperty(fromProperty) || !toMaterial.HasProperty(toProperty))
                return;

            var tType = typeof(T);

            if (tType == typeof(Texture))
            {
                var tex = fromMaterial.GetTexture(fromProperty);
                toMaterial.SetTexture(toProperty, tex);

                var texScale = fromMaterial.GetTextureScale(fromProperty);
                toMaterial.SetTextureScale(toProperty, texScale);
            }
            else if (tType == typeof(Color))
            {
                var col = fromMaterial.GetColor(fromProperty);
                toMaterial.SetColor(toProperty, col);
            }
            else if (tType == typeof(float))
            {
                var f = fromMaterial.GetFloat(fromProperty);
                toMaterial.SetFloat(toProperty, f);
            }
            else if (tType == typeof(int))
            {
                var f = fromMaterial.GetInt(fromProperty);
                toMaterial.SetInt(toProperty, f);
            }
            else if (tType == typeof(Vector4))
            {
                var f = fromMaterial.GetVector(fromProperty);
                toMaterial.SetVector(toProperty, f);
            }
        }
    }
}
