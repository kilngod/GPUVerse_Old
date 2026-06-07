// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2023 ILGPU Project
//                                    www.ilgpu.net
//
// File: .cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using GPUVulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanPlatform;

#nullable disable

namespace WinVulkanApp
{

    public partial class HelloTriangle
    {
        const uint WIDTH = 800;
        const uint HEIGHT = 600;

        private Form window;

        private VulkanSupport VSupport;

        private IVulkanRenderer renderer;
        private ComputeSample computeSample;

        public Form InitWindow()
        {
            window = new Form();
            window.Text = "Vulkan Triangle Rasterization";
            window.Size = new System.Drawing.Size((int)WIDTH, (int)HEIGHT);
            window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            window.Show();
            return window;
        }

        protected void DoCompute()
        {
            computeSample = new ComputeSample(VSupport);

            computeSample.SetupComputePipeline();
            computeSample.Compute();

        }

        public void InitVulkan()
        {
            VSupport = new VulkanSupport(DeliveryPlatform.Windows);

            renderer = new WinVulkanRenderer(window);
            (renderer as WinVulkanRenderer).VSupport = VSupport;

            (renderer as WinVulkanRenderer).SetupSurfaceAndDevice();


            DoCompute();

            (renderer as WinVulkanRenderer).SetupPipeline();


        }

        private void MainLoop()
        {
            bool isClosing = false;
            window.FormClosing += (s, e) =>
            {
                isClosing = true;
            };

            while (!isClosing)
            {
                Application.DoEvents();
                if (isClosing)
                {
                    break;
                }

                renderer.DrawFrame();
            }

            VulkanHelpers.CheckErrors(VulkanNative.vkDeviceWaitIdle(VSupport.Device));
        }

        public void CleanUp()
        {
            computeSample?.CleanUp();
            (renderer as WinVulkanRenderer).CleanUpPipeline();
            VSupport.CleanupVulkanSupport();
        }

        public void Run()
        {
            this.InitWindow();

            this.InitVulkan();

            this.MainLoop();

            this.CleanUp();
        }

    }
}
