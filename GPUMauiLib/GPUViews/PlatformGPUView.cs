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

using System;
using System.Linq.Expressions;
using GPUVulkan;
using GPUMauiLib.GPUDevices;
using VulkanPlatform;
#nullable disable

namespace GPUMauiLib.GPUViews
{ 
    public partial class PlatformGPUView : IPlatformGPUView
	{
        private GPUView _gpuView;

        public GPUView View { get { return _gpuView; } }

        private GPUSupport _support;

        public GPUSupport Support { get { return _support; } }

        

        public event Action ViewSizeChanged;
        public event Action ViewLoaded;
        public event Action ViewRemoved;

       


        public void Connect(IGPUView virtualView)
        {
            _gpuView = virtualView as GPUView;
           
        }

        public void Disconnect() => _gpuView = null;
#if WINDOWS || IOS || MACCATALYST || ANDROID

#else

        public PlatformGPUView(GPUSupport support)
        {
            _support = support;

        }


        public void Invalidate()
        {
            throw new NotImplementedException();
        }
#endif


        /*

                //this can be moved elsewhere
                public void LoadShaders()
                {

        #if DEBUG
                    VulkanFlowTracer.AddItem("PlatformGPUView.LoadShaders");
        #endif
                    try {

                        byte[] buffer = new byte[100000];
                        int index = 0;


                        using (Stream vertexStream = FileSystem.OpenAppPackageFileAsync("vert.spv").GetAwaiter().GetResult())
                        {

        #if WINDOWS                    
                            if (vertexStream.CanSeek && vertexStream.Length > 0)
                            {

                                VertexShaderSpriv = new byte[vertexStream.Length];
                                vertexStream.Seek(0, SeekOrigin.Begin);
                                vertexStream.Read(VertexShaderSpriv, 0,(int) vertexStream.Length);
                            }
                            else
        #endif
                            {
                                // maccatylst loaded one shader and not the other shader
                                // android has extremely limited stream support, can't seek or copy ranges
                                int lastByte = 0;

                                while (lastByte >= 0)
                                {
                                    lastByte = vertexStream.ReadByte();
                                    if (lastByte > -1)
                                    {
                                        buffer[index] = Convert.ToByte(lastByte);
                                        index += 1;
                                    }
                                }
                                VertexShaderSpriv = new byte[index];
                                for (int i = 0; i < index; i++)
                                {
                                    VertexShaderSpriv[i] = buffer[i];
                                }
                            }
                            vertexStream.Close();
                        }


                        index = 0;
                        using (Stream fragmentStream = FileSystem.OpenAppPackageFileAsync("frag.spv").GetAwaiter().GetResult())
                        {
        #if WINDOWS
                            if (fragmentStream.CanSeek)
                            {
                                FragmentShaderSpriV = new byte[fragmentStream.Length];
                                fragmentStream.Seek(0, SeekOrigin.Begin);
                                fragmentStream.Read(buffer, 0, (int)fragmentStream.Length);
                            }
                            else
        #endif
                            {
                                // android has extremely limited stream support, can't seek or copy ranges
                                int lastByte = 0;

                                while (lastByte >= 0)
                                {
                                    lastByte = fragmentStream.ReadByte();
                                    if (lastByte > -1)
                                    {
                                        buffer[index] = Convert.ToByte(lastByte);
                                        index += 1;
                                    }
                                }
                                FragmentShaderSpriV = new byte[index];
                                for (int i = 0; i < index; i++)
                                {
                                    FragmentShaderSpriV[i] = buffer[i];
                                }

                            }
                            fragmentStream.Close();
                        }

                    }
                    catch (Exception ex) 
                    {
                        VulkanFlowTracer.AddItem("PlatformGPUView.LoadShaders failed: "+ex.Message);
                    }

                }
        */
#if IOS || MACCATALYST
        public int BestWidth
        {
            get
            {
                // MacOS and IOs often scale the output for drawing with want to ignore scaling
                bool platformWidthDefined = this.DrawableSize.Width > 1 && this.DrawableSize.Width != double.NaN;
                bool viewWidthDefined = View.Width > 1 && View.Width != double.NaN;
                
                return Convert.ToInt32(platformWidthDefined ? this.DrawableSize.Width : (viewWidthDefined ? View.Width : View.WidthRequest));

            }
        }

        public int BestHeight
        {
            get
            {

                // MacOS and IOs often scale the output for drawing with want to ignore scaling
                bool platformHeightDefined = this.DrawableSize.Height > 1 && this.DrawableSize.Height != double.NaN;
                bool viewHeightDefined = View.Height > 1 && View.Height != double.NaN;

                return Convert.ToInt32(platformHeightDefined ? this.DrawableSize.Height : (viewHeightDefined ? View.Height : View.HeightRequest));

            }
        }
#endif

#if ANDROID
        public int BestWidth
        {
            get
            {

                bool platformWidthDefined = Width > 1 && Width != double.NaN;
                bool viewWidthDefined = View.Width > 1 && View.Width != double.NaN;

                return Convert.ToInt32(platformWidthDefined ? Width : (viewWidthDefined ? View.Width : View.WidthRequest));

            }
        }

        public int BestHeight
        {
            get
            {
                bool platformHeightDefined = Height > 1 && Height != double.NaN;
                bool viewHeightDefined = View.Height > 1 && View.Height != double.NaN;

                return Convert.ToInt32(platformHeightDefined ? Height : (viewHeightDefined ? View.Height : View.HeightRequest));

            }
        }

#endif

#if WINDOWS
        public int BestWidth
        {
            get
            {

                bool platformWidthDefined = RenderSize.Width > 1 && RenderSize.Width != double.NaN;
                bool viewWidthDefined = View.Width > 1 && View.Width != double.NaN;

                return Convert.ToInt32(platformWidthDefined ? RenderSize.Width : (viewWidthDefined ? View.Width : View.WidthRequest));

            }
        }

        public int BestHeight
        {
            get
            {
                bool platformHeightDefined = RenderSize.Height > 1 && RenderSize.Height != double.NaN;
                bool viewHeightDefined = View.Height > 1 && View.Height != double.NaN;

                return Convert.ToInt32(platformHeightDefined ? RenderSize.Height : (viewHeightDefined ? View.Height : View.HeightRequest));

            }
        }
#endif

    }
}

