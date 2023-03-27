using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.PixelCrew.Utils
{
    public class PipeLineUtil : MonoBehaviour
    {
        [SerializeField] private RenderPipelineAsset _defaultPipelineAsset;
        [SerializeField] private RenderPipelineAsset _waterPipelineAsset;
        [SerializeField] private Pipeline _pipeline;

        private void Start()
        {
            string settingsPipelineName = GraphicsSettings.defaultRenderPipeline.name;
            bool currentPipelineIsDefault = settingsPipelineName == _defaultPipelineAsset.name;
            if (_pipeline == Pipeline.Default && !currentPipelineIsDefault)
            {
                GraphicsSettings.defaultRenderPipeline = _defaultPipelineAsset;
                QualitySettings.renderPipeline = _defaultPipelineAsset;
            }
            else if (_pipeline == Pipeline.WaterNoLight && currentPipelineIsDefault)
            {
                GraphicsSettings.defaultRenderPipeline = _waterPipelineAsset;
                QualitySettings.renderPipeline = _waterPipelineAsset;
            }
        }


    }

    [Serializable]
    public enum Pipeline { Default, WaterNoLight }
}