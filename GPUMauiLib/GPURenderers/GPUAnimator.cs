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
using System.Diagnostics;
#nullable disable
namespace GPUMauiLib.GPURenderers
{
	public partial class GPUAnimator
	{
        Action AnimateMethod { get; set; }

        public bool isRunning = false;
        public void set(Action animateMethod)
        {
            AnimateMethod = animateMethod;
        }

        private void update()
        {
            if (isRunning)
            {
                AnimateMethod?.Invoke();
            }
        }

#if ANDROID || IOS || MACCATALYST || WINDOWS

#else
        public void start()
        {
            isRunning = true;
            Task.Factory.StartNew(() => Loop(), TaskCreationOptions.LongRunning);
        }

        int frameTime = 1000 / 60;//每秒60帧
        private void Loop()
        {
            while (isRunning)
            {
                try
                {
                    Thread.Sleep(frameTime);

                    update();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Encountered an error while rendering: " + e);
                    //throw;
                }
            }
        }


        public void cancel()
        {
            isRunning = false;
        }

#endif

    }
}

