using System;
namespace VulkanPlatform
{
    public static class VulkanIO
    {

        public static byte[] LoadRawResource(string ResourceFilePath)
        {
            byte[] byteResult;

            using (FileStream rawStream = new FileStream(ResourceFilePath, FileMode.Open))
            {

                // MemoryStream apparently corrects corruption issue with using Seek on MacCatalyst or Android
                MemoryStream stream = new MemoryStream();
                rawStream.CopyTo(stream);
                byteResult = stream.ToArray();



                rawStream.Close();
            }

            return byteResult;

        }

    }
}

