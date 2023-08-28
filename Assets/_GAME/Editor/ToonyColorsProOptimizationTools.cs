using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace EasyClap
{

	public static class ToonyColorsProOptimizationTools
	{
		private const string Menu = "EASY CLAP/Materials/";

		[MenuItem(Menu + "Convert Hybrid Shaders to Custom Toony", priority = 5001)]
		public static void ConvertHybridShadersToCustomToony()
		{
			var hybridShader = FindShader("Toony Colors Pro 2/Hybrid Shader");
			var hybridOutlineShader = FindShader("Toony Colors Pro 2/Hybrid Shader Outline");
			var customShader = FindShader("Toony Colors Pro 2/User/Custom Toony Shader");
			var customOutlineShader = FindShader("Toony Colors Pro 2/User/Custom Toony Shader Outline");

			var allMaterialGUIDs = AssetDatabase.FindAssets("t:Material");
			for (int i = 0; i < allMaterialGUIDs.Length; i++)
			{
				var path = AssetDatabase.GUIDToAssetPath(allMaterialGUIDs[i]);
				var material = AssetDatabase.LoadAssetAtPath<Material>(path);
				if (material.shader == hybridShader)
				{
					material.shader = customShader;
					Debug.Log($"Converted material shader to Custom Toony for: <b>{material}</b>", material);
				}
				else if (material.shader == hybridOutlineShader)
				{
					material.shader = customOutlineShader;
					Debug.Log($"Converted material shader to Custom Toony Outline for: <b>{material}</b>", material);
				}
			}

			AssetDatabase.SaveAssets();
		}

		[MenuItem(Menu + "List Unconverted Toony Materials", priority = 5002)]
		public static void ListUnconvertedToonyMaterials()
		{
			var convertedShaderNames = new string[]
			{
				"Toony Colors Pro 2/User/Custom Toony Shader",
				"Toony Colors Pro 2/User/Custom Toony Shader Outline",
			};
			var unconvertedShaderNameStartsWith = "Toony Colors Pro 2/";

			var count = 0;
			var guids = AssetDatabase.FindAssets("t:Material");
			for (int i = 0; i < guids.Length; i++)
			{
				var path = AssetDatabase.GUIDToAssetPath(guids[i]);
				var material = AssetDatabase.LoadAssetAtPath<Material>(path);
				var shaderName = material.shader.name;
				if (!convertedShaderNames.Contains(shaderName) &&
				    shaderName.StartsWith(unconvertedShaderNameStartsWith))
				{
					count++;
					Debug.LogError($"Shader '<b>{shaderName}</b>' detected in material '<b>{material.name}</b>' which is heavy to render and increases build times drastically.", material);
				}
			}
			if (count > 0)
			{
				throw new Exception($"Found '{count}' materials that should be looked into. Click on errors above to go to these materials. Use bulk converter to use optimized shaders.");
			}
			else
			{
				Debug.Log("OK! There are no Toony Colors Pro materials left with unoptimized shaders.");
			}
		}

		// [MenuItem(Menu + "Set Mobile Mode For Toony", priority = 5021)]
		// public static void SetMobileMod()
		// {
		// 	var hybridShader = FindShader("Toony Colors Pro 2/Hybrid Shader");
		//
		// 	var allMaterialGUIDs = AssetDatabase.FindAssets("t:Material");
		// 	for (int i = 0; i < allMaterialGUIDs.Length; i++)
		// 	{
		// 		var path = AssetDatabase.GUIDToAssetPath(allMaterialGUIDs[i]);
		// 		var material = AssetDatabase.LoadAssetAtPath<Material>(path);
		// 		if (material.shader == hybridShader)
		// 		{
		// 			if (material.GetFloat("_UseMobileMode") != 1.0f)
		// 			{
		// 				material.SetFloat("_UseMobileMode", 1.0f);
		// 				Debug.Log("Enabled mobile mode for material: " + material, material);
		// 			}
		// 		}
		// 	}
		//
		// 	AssetDatabase.SaveAssets();
		// }

		private static Shader FindShader(string shaderName)
		{
			var shader = Shader.Find(shaderName);
			if (!shader)
			{
				throw new Exception($"Failed to find '{shaderName}'");
			}
			return shader;
		}
	}

}
