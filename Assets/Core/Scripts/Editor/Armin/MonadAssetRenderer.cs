using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

namespace Nocci
{
    public class MonadAssetRenderer : MonoBehaviour
    {
        [FoldoutGroup("Camera Presets", expanded: true)]
        [Button("Front View", ButtonSizes.Large, ButtonStyle.Box)]
        [GUIColor("#fc7430")]
        private void CameraPresetFront()
        {
            DisableAllCameras();
            EnableObject(cameraPresetFront, true);
        }
        [FoldoutGroup("Camera Presets", expanded: true)]
        [Button("Front Top View", ButtonSizes.Large, ButtonStyle.Box)]
        [GUIColor("#fc4b30")]
        private void CameraPresetFrontTop()
        {
            DisableAllCameras();
            EnableObject(cameraPresetFrontTop, true);
        }
        [FoldoutGroup("Camera Presets", expanded: true)]
        [Button("Turntable View", ButtonSizes.Large, ButtonStyle.Box)]
        [GUIColor("#fc30ae")]
        private void CameraPresetTurnTable()
        {
            DisableAllCameras();
            EnableObject(cameraPresetTurntable, true);
        }
        [FoldoutGroup("Camera Presets", expanded: true)]
        [Button("Closeup View", ButtonSizes.Large, ButtonStyle.Box)]
        [GUIColor("#30fc52")]
        private void CameraPresetCloseUp()
        {
            DisableAllCameras();
            EnableObject(cameraPresetCloseUp, true);
        }
        [FoldoutGroup("Camera Presets", expanded: true)]
        [Button("Bottom Top View", ButtonSizes.Large, ButtonStyle.Box)]
        [GUIColor("#d330fc")]
        private void CameraPresetBottomTop()
        {
            DisableAllCameras();
            EnableObject(cameraPresetBottomTop, true);
        }


        [FoldoutGroup("Lighting Presets", expanded: false)]
        [Button("Next Light Preset", ButtonSizes.Large, ButtonStyle.Box)]
        private void NextLightPreset()
        {
            curentLightPreset = (curentLightPreset + 1) % lightPresets.Length;
            SwitchLightPreset();
        }
        [FoldoutGroup("Lighting Presets", expanded: false)]
        [Button("Previous Light Preset", ButtonSizes.Large, ButtonStyle.Box)]
        private void PreviousLightPreset()
        {
            curentLightPreset = (curentLightPreset - 1 + lightPresets.Length) % lightPresets.Length;
            SwitchLightPreset();
        }

        [FoldoutGroup("Lighting Config", expanded: false)]
        [FoldoutGroup("HDRI Presets", expanded: false)]




        [Button("Render Asset", ButtonSizes.Large, ButtonStyle.Box)]
        private void RenderAsset()
        {

        }

        [FoldoutGroup("Settings", expanded: false)]
        [SerializeField] private GameObject cameraPresetFront;
        [FoldoutGroup("Settings", expanded: false)]
        [SerializeField] private GameObject cameraPresetFrontTop;
        [FoldoutGroup("Settings", expanded: false)]
        [SerializeField] private GameObject cameraPresetTurntable;
        [FoldoutGroup("Settings", expanded: false)]
        [SerializeField] private GameObject cameraPresetCloseUp;
        [FoldoutGroup("Settings", expanded: false)]
        [SerializeField] private GameObject cameraPresetBottomTop;
        [FoldoutGroup("Settings", expanded: false)]
        public GameObject[] lightPresets;
        [FoldoutGroup("Settings", expanded: false)]
        private int curentLightPreset = 0;

        private void EnableObject(GameObject objectName = null, bool state = false)
        {
            objectName.SetActive(state);
        }

        private void DisableAllCameras()
        {
            cameraPresetFront?.SetActive(false);
            cameraPresetFrontTop?.SetActive(false);
            cameraPresetTurntable?.SetActive(false);
            cameraPresetCloseUp?.SetActive(false);
            cameraPresetBottomTop?.SetActive(false);
        }

        private void SwitchLightPreset()
        {
            foreach (GameObject obj in lightPresets)
            {
                obj.SetActive(false);
            }

            lightPresets[curentLightPreset].SetActive(true);
        }
    }
}
