using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class BendingManager : MonoBehaviour
{
    #region Constants

    private const string BENDING_FEATURE = "ENABLE_BENDING";

    private static readonly int BENDING_AMOUNT =
      Shader.PropertyToID("_BendingAmount");

    #endregion


    #region Inspector
    [SerializeField]
    private bool bendOnEdit = true;
    [SerializeField]
    [Range(0f, 0.02f)]
    private float bendingAmount = 0.015f;

    #endregion


    #region Fields

    private float _prevAmount;

    #endregion


    #region MonoBehaviour

    private void Awake()
    {
        if (Application.isPlaying || bendOnEdit)
            Shader.EnableKeyword(BENDING_FEATURE);
        else
            Shader.DisableKeyword(BENDING_FEATURE);

        UpdateBendingAmount();
    }

    private void OnEnable()
    {
        if (!Application.isPlaying)
            return;

        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void Update()
    {
        if (Math.Abs(_prevAmount - bendingAmount) > Mathf.Epsilon)
            UpdateBendingAmount();
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    #endregion


    #region Methods

    private void UpdateBendingAmount()
    {
        _prevAmount = bendingAmount;
        Shader.SetGlobalFloat(BENDING_AMOUNT, bendingAmount);
    }

    private static void OnBeginCameraRendering(ScriptableRenderContext ctx,
                                                Camera cam)
    {
        cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99) *
                            cam.worldToCameraMatrix;
    }

    private static void OnEndCameraRendering(ScriptableRenderContext ctx,
                                              Camera cam)
    {
        cam.ResetCullingMatrix();
    }

    #endregion
}