// See https://aka.ms/new-console-template for more information
// Gogisoripper v0.1 by Moburma
// A simple tool for ripping CDDA tracks to .wav files from iso images
using System;
using System.IO.MemoryMappedFiles;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: gogisoripper.exe <input file path> <Output path & Filename> <track start position> <track end position> ");
            return;
        }

        string filePath = args[0];
        string outputFile = args[1];
        string trackStartPos = args[2];
        string trackEndPos = args[3];
        long trackStart = long.Parse(trackStartPos);
        long trackEnd = long.Parse(trackEndPos);
        long length = trackEnd - trackStart;
        long startlength = length + 36;

        //get filename text
        char separator = '\\';
        int lastIndex = outputFile.LastIndexOf(separator);
        string trackFileName = outputFile.Substring(lastIndex + 1);


        Console.WriteLine($"Ripping {trackFileName} from start {trackStartPos} to {trackEndPos}");

        //Create Wav header

        byte[] trackheader1 = new byte[]
      {
            0x52, 0x49, 0x46, 0x46
      };

        byte[] trackheader3 = new byte[]
        {
            0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20, 0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00, 0x44, 0xAC, 0x00, 0x00, 0x10, 0xB1, 0x02, 0x00, 0x04, 0x00, 0x10, 0x00, 0x64, 0x61, 0x74, 0x61
        };

        int totalLength = trackheader1.Length + sizeof(long) + trackheader3.Length;
        byte[] combinedArray = new byte[totalLength];

        // Copy trackheader1 to combinedArray
        Buffer.BlockCopy(trackheader1, 0, combinedArray, 0, trackheader1.Length);

        // Copy startlength to combinedArray
        byte[] startLengthBytes = BitConverter.GetBytes(startlength);
        Array.Resize(ref startLengthBytes, 4);
        Buffer.BlockCopy(startLengthBytes, 0, combinedArray, trackheader1.Length, startLengthBytes.Length);

        // Copy trackheader3 to combinedArray
        Buffer.BlockCopy(trackheader3, 0, combinedArray, trackheader1.Length + startLengthBytes.Length, trackheader3.Length);

        // Copy length to combinedArray
        byte[] lengthBytes = BitConverter.GetBytes(length);
        Array.Resize(ref lengthBytes, 4);
        Buffer.BlockCopy(lengthBytes, 0, combinedArray, trackheader1.Length + startLengthBytes.Length + trackheader3.Length, lengthBytes.Length);

        // Read GOG iso and extract CDDA region
        using (MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(filePath))
        using (MemoryMappedViewAccessor memoryMappedView = memoryMappedFile.CreateViewAccessor(trackStart, length))
        {
            byte[] cdData = new byte[length];
            memoryMappedView.ReadArray(0, cdData, 0, (int)length);

            Console.WriteLine($"Writing to {outputFile}");
            //write ouputs
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                writer.Write(combinedArray);
                writer.Write(cdData);
            }
        }

    }
}
