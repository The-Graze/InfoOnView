using BepInEx;
using GorillaLocomotion;
using Photon.Pun;
using System;
using UnityEngine;
using TMPro;

namespace InfoOnView
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private TextMeshPro TextBox;
        private int frameCount;
        private float deltaTime;
        private float fps;
        private GameObject DebugCanvas;
        private Rigidbody rb;
        private DateTime now;

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
        }

        private void OnPlayerSpawned()
        {
            DebugCanvas = GorillaTagger.Instance.mainCamera.transform.FindChildRecursive("DebugCanvas").gameObject;
            DebugCanvas.SetActive(true);
            TextBox = DebugCanvas.transform.GetChild(0).GetComponent<TextMeshPro>();
            rb = Player.Instance.GetComponent<Rigidbody>();

            Destroy(DebugCanvas.GetComponent<DebugHudStats>());
            DebugCanvas.layer = LayerMask.NameToLayer("MetaReportScreen");
            TextBox.gameObject.layer = LayerMask.NameToLayer("MetaReportScreen");
            TextBox.renderer.material.shader = Shader.Find("GUI/Text Shader");
        }

        void Update()
        {
            frameCount++;
            deltaTime += Time.unscaledDeltaTime;

            if (deltaTime > 1.0f)
            {
                fps = frameCount / deltaTime;
                frameCount = 0;
                deltaTime = 0.0f;
            }

            now = DateTime.Now;
            TextBox.text = $"TIME: {now:hh:mmtt}\nFPS: {fps:F1}\nPING: {GetPing()}\nM/S: {rb.velocity.magnitude:F1}";
        }

        private string GetPing()
        {
            return PhotonNetwork.InRoom ? PhotonNetwork.GetPing().ToString() : "N/A";
        }
    }
}
