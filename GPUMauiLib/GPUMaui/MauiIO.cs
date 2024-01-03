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
using System.Reflection;

namespace GPUMauiLib.GPUMaui
{
	public static class MauiIO
	{
        public static byte[] LoadRawResource(string MauiResourceFilePath)
        {
            byte[] byteResult;
            using (Stream rawStream = FileSystem.OpenAppPackageFileAsync(MauiResourceFilePath).GetAwaiter().GetResult())
            {

#if WINDOWS
                if (rawStream.CanSeek && rawStream.Length > 0)
                {

                    byteResult = new byte[rawStream.Length];
                    rawStream.Seek(0, SeekOrigin.Begin);
                    rawStream.Read(byteResult, 0, (int)rawStream.Length);
                    
                }
                else
#endif

                {
                    // MemoryStream appearently corrects corruption issue with using Seek on MacCatalyst or Android
                    MemoryStream stream = new MemoryStream();
                    rawStream.CopyTo(stream);
                    byteResult = stream.ToArray();

                    /*
                    // maccatylst loaded one shader and not the other shader
                    // android has extremely limited stream support, can't seek or copy ranges
                    int lastByte = 0;

                    while (lastByte >= 0)
                    {
                        lastByte = rawStream.ReadByte();
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
                    */
                }
                rawStream.Close();
            }

            return byteResult;

        }

        public static async Task<byte[]> LoadRawResourceAsync(string MauiResourceFilePath)
        {
            byte[] byteResult;
            using (Stream rawStream = await FileSystem.OpenAppPackageFileAsync(MauiResourceFilePath))
            {

#if WINDOWS
                if (rawStream.CanSeek && rawStream.Length > 0)
                {

                    byteResult = new byte[rawStream.Length];
                    rawStream.Seek(0, SeekOrigin.Begin);
                    rawStream.Read(byteResult, 0, (int)rawStream.Length);
                    
                }
                else
#endif
                {
                    MemoryStream stream = new MemoryStream();
                    rawStream.CopyTo(stream);
                    byteResult = stream.ToArray();

                    /*
                    // maccatylst loaded one shader and not the other shader
                    // android has extremely limited stream support, can't seek or copy ranges
                    int lastByte = 0;

                    while (lastByte >= 0)
                    {
                        lastByte = rawStream.ReadByte();
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
                    */
                }
                rawStream.Close();
            }

            return byteResult;

        }
    }
}

