using System.IO.Filesystem.Ntfs;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Security.Principal;

public class Program
{
    public static void Main()
    {
        Console.Title = "AllNtfsEntries | Made by https://github.com/GabryB03/";

        if (!(new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
        {
            Console.WriteLine("Please, run the program with Administrator privileges.");
            Console.WriteLine("Press the ENTER key in order to exit from the program.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Succesfully fetched the main drive letter: {Environment.SystemDirectory[0]}:\\");
        Console.WriteLine("Press the ENTER key in order to start fetching your MFT.");
        Console.ReadLine();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        DriveInfo driveToAnalyze = new DriveInfo(Environment.SystemDirectory[0].ToString());
        NtfsReader ntfsReader = new NtfsReader(driveToAnalyze, RetrieveMode.Minimal);
        IEnumerable<INode> nodes = ntfsReader.GetNodes(driveToAnalyze.Name);

        stopwatch.Stop();
        Console.WriteLine("Time elapsed to fetch all NTFS entries from the MFT: " + stopwatch.ElapsedMilliseconds.ToString() + "ms.");
        Console.WriteLine("Press ENTER to display all *.txt files in your system (path & size).");
        Console.ReadLine();

        foreach (INode node in nodes)
        {
            if (node.FullName.EndsWith(".txt"))
            {
                Console.WriteLine("{\"path\": \"" + node.FullName + "\", \"size\": \"" + node.Size + "\"}");
            }
        }

        Console.WriteLine("Displaying finished.");
        Console.WriteLine("Press the ENTER key in order to exit from the program.");
        Console.ReadLine();
    }
}