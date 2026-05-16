using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public static class OrganPartSetup
{
    private const string ColliderChildName = "Collider";
    private const string PartDataFolder = "Assets/ScriptableObjects/PartData/Organs";
    private const string EventChannelPath = "Assets/EventChannels/PartFocusEvent.asset";

    [MenuItem("Tools/Vet XR/4. Setup Organ Part", false, 10)]
    private static void SetupHoverPart()
    {
        var selection = Selection.gameObjects;

        if (selection.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Nothing selected",
                "Select one or more mesh GameObjects in the Hierarchy first.",
                "OK");
            return;
        }

        var eventChannel = AssetDatabase.LoadAssetAtPath<PartFocusEnteredEventChannelSO>(EventChannelPath);

        if (eventChannel == null)
        {
            EditorUtility.DisplayDialog(
                "Event channel not found",
                $"Could not find PartFocusEnteredEventChannelSO at:\n{EventChannelPath}",
                "OK");
            return;
        }

        EnsureFolderExists(PartDataFolder);

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Setup Hover Parts");

        int success = 0;

        foreach (var go in selection)
        {
            if (go == null) continue;

            if (ConvertToHoverPart(go, eventChannel))
                success++;
        }

        Undo.CollapseUndoOperations(undoGroup);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[OrganPartSetup] Set up {success}/{selection.Length} hover part(s).");
    }

    [MenuItem("Tools/Vet XR/1. Setup Hover Part", true)]
    private static bool SetupHoverPartValidate()
    {
        return Selection.gameObjects.Length > 0;
    }

    private static bool ConvertToHoverPart(
        GameObject mesh,
        PartFocusEnteredEventChannelSO eventChannel)
    {
        if (mesh.name == ColliderChildName)
        {
            Debug.LogWarning($"[OrganPartSetup] '{mesh.name}' looks like a Collider duplicate. Skipping.");
            return false;
        }

        if (mesh.GetComponent<ModelPart>() != null)
        {
            Debug.LogWarning($"[OrganPartSetup] '{mesh.name}' already has a ModelPart component. Skipping.");
            return false;
        }

        string boneName = mesh.name;
        string originalId = mesh.name;

        // =====================================================
        // 1. Create Parent
        // =====================================================

        var parent = new GameObject(boneName);
        Undo.RegisterCreatedObjectUndo(parent, "Create Part Parent");

        Undo.SetTransformParent(parent.transform, mesh.transform.parent, "Reparent Parent");

        parent.transform.SetSiblingIndex(mesh.transform.GetSiblingIndex());

        parent.transform.position = mesh.transform.position;
        parent.transform.rotation = mesh.transform.rotation;
        parent.transform.localScale = mesh.transform.localScale;

        // =====================================================
        // 2. Move Mesh Under Parent
        // =====================================================

        Undo.SetTransformParent(mesh.transform, parent.transform, "Reparent Mesh");

        mesh.transform.localPosition = Vector3.zero;
        mesh.transform.localRotation = Quaternion.identity;
        mesh.transform.localScale = Vector3.one;

        // =====================================================
        // 3. Create Collider Duplicate
        // =====================================================

        var collider = Object.Instantiate(mesh, parent.transform);
        Undo.RegisterCreatedObjectUndo(collider, "Create Collider Duplicate");

        collider.name = ColliderChildName;

        collider.transform.localPosition = Vector3.zero;
        collider.transform.localRotation = Quaternion.identity;
        collider.transform.localScale = Vector3.one;

        var colliderMC = Undo.AddComponent<MeshCollider>(collider);
        colliderMC.convex = true;

        var colliderFilter = collider.GetComponent<MeshFilter>();

        if (colliderFilter != null && colliderFilter.sharedMesh != null)
        {
            colliderMC.sharedMesh = colliderFilter.sharedMesh;
        }

        // =====================================================
        // 4. Add XRSimpleInteractable
        // =====================================================

        var interactable = Undo.AddComponent<XRSimpleInteractable>(parent);

        // =====================================================
        // 5. Add HoverTint
        // =====================================================

        var hoverTint = Undo.AddComponent<HoverTint>(parent);

        MeshRenderer targetRenderer = mesh.GetComponent<MeshRenderer>();

        if (targetRenderer != null)
        {
            Undo.RecordObject(hoverTint, "Assign HoverTint Renderer");
            hoverTint.targetRenderer = targetRenderer;
            EditorUtility.SetDirty(hoverTint);
        }
        else
        {
            Debug.LogWarning($"[OrganPartSetup] No MeshRenderer found on '{mesh.name}'. HoverTint targetRenderer not assigned.");
        }

        // =====================================================
        // 6. Create / Fetch ModelPartDataSO
        // =====================================================

        string assetPath = $"{PartDataFolder}/{boneName}.asset";

        var partData = AssetDatabase.LoadAssetAtPath<ModelPartDataSO>(assetPath);

        if (partData == null)
        {
            partData = ScriptableObject.CreateInstance<ModelPartDataSO>();

            partData.id = originalId;
            partData.partName = boneName;
            partData.descriptionChunks = new string[0];

            AssetDatabase.CreateAsset(partData, assetPath);
        }
        else
        {
            Undo.RecordObject(partData, "Update existing ModelPartDataSO");

            partData.id = originalId;
            partData.partName = boneName;

            EditorUtility.SetDirty(partData);
        }

        // =====================================================
        // 7. Add ModelPart
        // =====================================================

        var modelPart = Undo.AddComponent<ModelPart>(parent);

        Undo.RecordObject(modelPart, "Wire ModelPart");

        modelPart.partData = partData;
        modelPart.focusEventChannel = eventChannel;
        modelPart.interactable = interactable;

        EditorUtility.SetDirty(modelPart);

        return true;
    }

    // =========================================================
    // Utility
    // =========================================================

    private static void EnsureFolderExists(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
            return;

        string[] split = folderPath.Split('/');

        string current = split[0];

        for (int i = 1; i < split.Length; i++)
        {
            string next = current + "/" + split[i];

            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, split[i]);
            }

            current = next;
        }
    }
}