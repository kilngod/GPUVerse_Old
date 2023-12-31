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
using System.IO;

namespace VulkanPlatform
{
	public static class VulkanIO
	{
		
        public static byte[] LoadRawResource(string ResourceFilePath)
        {
            byte[] byteResult;
      
            using (FileStream rawStream = new FileStream(ResourceFilePath, FileMode.Open))
            {
                
                // MemoryStream appearently corrects corruption issue with using Seek on MacCatalyst or Android
                MemoryStream stream = new MemoryStream();
                rawStream.CopyTo(stream);
                byteResult = stream.ToArray();

              

                rawStream.Close();
            }

            return byteResult;

        }

    }
}

