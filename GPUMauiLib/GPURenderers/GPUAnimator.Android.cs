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
using System.Threading;
using System.Threading.Tasks;

namespace GPUMauiLib.GPURenderers
{
    public partial class GPUAnimator
    {
      
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
    }
}

