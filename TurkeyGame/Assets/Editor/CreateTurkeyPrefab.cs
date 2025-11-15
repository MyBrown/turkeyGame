using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor utility to create a turkey prefab from a sprite asset named containing "turkey".
/// Use the menu: Tools -> Turkey -> Create Turkey Prefab
/// </summary>
public static class CreateTurkeyPrefab
{
    [MenuItem("Tools/Turkey/Create Turkey Prefab")]
    public static void CreatePrefabFromSprite()
    {
        // Search for a sprite that contains "turkey" in its name (case-insensitive)
        Sprite chosenSprite = null;
        string foundPath = null;
        string[] guids = AssetDatabase.FindAssets("t:Sprite");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var sp = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sp != null && sp.name.ToLower().Contains("turkey"))
            {
                chosenSprite = sp;
                foundPath = path;
                break;
            }
        }

        if (chosenSprite == null)
        {
            EditorUtility.DisplayDialog("Create Turkey Prefab",
                "No sprite with 'turkey' in the filename was found. Please ensure your turkey sprite asset name contains 'turkey' (case-insensitive).",
                "OK");
            Debug.LogError("CreateTurkeyPrefab: Could not find a turkey sprite in the project.");
            return;
        }

        // Ensure the Prefabs folder exists
        const string prefabFolder = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        string prefabPath = Path.Combine(prefabFolder, "Turkey.prefab");

        // Create a temporary GameObject and configure it
        var go = new GameObject("Turkey_Prefab");
        try
        {
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = chosenSprite;

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            var bc = go.AddComponent<BoxCollider2D>();
            // approximate collider size from sprite bounds
            try
            {
                bc.size = chosenSprite.bounds.size;
            }
            catch { }

            // Try to attach the project's `TurkeyMovement` component if the compiled type exists
            Type movementType = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var t in asm.GetTypes())
                    {
                        if (t.Name == "TurkeyMovement")
                        {
                            movementType = t;
                            break;
                        }
                    }
                }
                catch { }
                if (movementType != null) break;
            }

            if (movementType != null)
            {
                go.AddComponent(movementType);
                Debug.Log("CreateTurkeyPrefab: Attached existing TurkeyMovement to prefab.");
            }
            else
            {
                Debug.Log("CreateTurkeyPrefab: TurkeyMovement type not found in compiled assemblies. The prefab will be created without it. Add the script later or re-run after compiling.");
            }

            // Save prefab
            var prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            if (prefab != null)
            {
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Create Turkey Prefab", $"Prefab created at: {prefabPath}", "OK");
                Debug.Log($"CreateTurkeyPrefab: Created prefab at {prefabPath} using sprite {foundPath}.");

                // Attempt to assign the created prefab to any Spawner_Script instances in the open scene(s)
                try
                {
                    var spawners = UnityEngine.Object.FindObjectsByType<Spawner_Script>(FindObjectsSortMode.None);
                    if (spawners != null && spawners.Length > 0)
                    {
                        foreach (var sp in spawners)
                        {
                            var so = new SerializedObject(sp);
                            var prop = so.FindProperty("turkeyPrefab");
                            if (prop != null)
                            {
                                prop.objectReferenceValue = prefab;
                                so.ApplyModifiedProperties();
                                Debug.Log($"CreateTurkeyPrefab: Assigned prefab to Spawner_Script on '{sp.gameObject.name}'.");
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"CreateTurkeyPrefab: Could not auto-assign prefab to Spawner_Script(s): {e.Message}");
                }
            }
            else
            {
                Debug.LogError("CreateTurkeyPrefab: Failed to create prefab asset.");
            }
        }
        finally
        {
            // Clean up the temporary GameObject
            UnityEngine.Object.DestroyImmediate(go);
        }
    }
}
