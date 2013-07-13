using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.IO;
using Recurity.Swf;

namespace Recurity.Blitzableiter
{
    /// <summary>
    /// The Blitzableiter command line tool
    /// </summary>
    public class Program
    {
        private static List<string> Files;
        private static List<string> Directories;
        private static bool Recursive = false;
        private static bool Single = true;
        private static bool Overwrite = false;
        private static int DebugMessages = 0;
        private static int Warnings = 0;
        private static int Errors = 0;
        private static bool Verbose = false;
        private static bool VeryVerbose = false;

        private static string Header = Environment.NewLine + " Recurity Labs Blitzableiter " + Environment.NewLine +
                " Flash File Format Normalizer Version 1.0" + Environment.NewLine +
                " Copyright © 2011  Recurity Labs GmbH. All rights reserved" + Environment.NewLine + Environment.NewLine;

        /// <summary>
        /// The program entry point
        /// </summary>
        /// <param name="args">The arguments</param>
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("");
                Console.WriteLine(" Too few arguments");
                PrintHelp();
                return;
            }

            Files = new List<string>();
            Directories = new List<string>();

            Recurity.Swf.Log.LogsDebugEvent += new LogEventHandler(PrintLogDebug);
            Recurity.Swf.Log.LogsWarningEvent += new LogEventHandler(PrintLogWarning);
            Recurity.Swf.Log.LogsErrorEvent += new LogEventHandler(PrintLogError);

            Console.Write(Header);
            ParseCMDArgs(args);
            Console.WriteLine(" {0} Errors, {1} Warnings, {2} Debug Messages", Errors, Warnings, DebugMessages);
            if (Warnings > 0)
            {
                Console.WriteLine(" Use the option -v to view Warnings");
            }
            if (DebugMessages > 0)
            {
                Console.WriteLine(" Use the option -vv to view Warnings and Debug Messages");
            }
            //Console.WriteLine(" Press any key to continue");
            //Console.ReadLine();
        }

        private static void PrintHelp()
        {
            Console.Write(Header);
            ConsoleColor normal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Name" + Environment.NewLine);
            Console.ForegroundColor = normal;
            Console.WriteLine("\t Blitzableiter" + Environment.NewLine + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Synopsis" + Environment.NewLine);
            Console.ForegroundColor = normal;
            Console.WriteLine("\t bb [-option(s)] input output" + Environment.NewLine +
                              "\t bb -o [-option(s)] input" + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Examples" + Environment.NewLine);
            Console.ForegroundColor = normal;
            Console.WriteLine("\t bb c:/myflashfiles/myinputfile.swf c:/myflashfiles/myoutputfile.swf" + Environment.NewLine + Environment.NewLine +
                              "\t \t Scans c:/myflashfiles/myinputfile.swf and emmits a scanned copy to c:/myflashfiles/myoutputfile.swf" + Environment.NewLine + Environment.NewLine +
                              "\t bb -o c:/myflashfiles/myinputfile.swf" + Environment.NewLine + Environment.NewLine +
                              "\t \t Scans the file c:/myflashfiles/myinputfile.swf and replaces it with the scanned copy" + Environment.NewLine + Environment.NewLine +
                              "\t bb -d c:/myflashfiles/ c:/myscannedflashfiles/" + Environment.NewLine + Environment.NewLine +
                              "\t \t Scans the directory c:/myflashfiles/ and writes the scanned files into c:/myscannedflashfiles/" + Environment.NewLine + Environment.NewLine +
                              "\t bb -d -r c:/myflashfiles/ c:/myscannedflashfiles/" + Environment.NewLine + Environment.NewLine +
                              "\t \t Scans the directory c:/myflashfiles/ and all sub directories" + Environment.NewLine + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Description" + Environment.NewLine);
            Console.ForegroundColor = normal;
            Console.WriteLine("\t A format nomalizer for the Adobe SWF file fromat " + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Options" + Environment.NewLine);
            Console.ForegroundColor = normal;
            Console.WriteLine("\t -d  : Scans a directory" + Environment.NewLine +
                              "\t -o  : Overwrites the original file(s)" + Environment.NewLine +
                              "\t -r  : Scans recursively when scanning directories" + Environment.NewLine +
                              "\t -v  : Prints errors and warnings " + Environment.NewLine +
                              "\t -vv : Prints errors, warnings and debug messages " + Environment.NewLine + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Copyright" + Environment.NewLine);
            Console.ForegroundColor = normal;

            Console.WriteLine("\t Blitzableiter is copyright © 2009 by Recurity Labs GmbH.");
            Console.WriteLine("\t Blitzableiter is made available as software library source code for");
            Console.WriteLine("\t ISO/IEC 23271:2006 and ISO/IEC 23270:2006 compatible runtime environments ");
            Console.WriteLine("\t (e.g .NET) and licensed exclusively for use in free software under the ");
            Console.WriteLine("\t GNU General Public License, Version 3.");
            Console.WriteLine("\t ");
            Console.WriteLine("\t A license for use of the Blitzableiter library for commercial or ");
            Console.WriteLine("\t proprietary applications must be obtained separately from Recurity Labs GmbH.");
            Console.WriteLine("\t ");
            Console.WriteLine("\t Blitzableiter is free software: you can redistribute it and/or modify it under ");
            Console.WriteLine("\t the terms of the GNU General Public License as published by the Free Software ");
            Console.WriteLine("\t Foundation, either version 3 of the License, or (at your option) any later version.");
            Console.WriteLine("\t ");
            Console.WriteLine("\t Blitzableiter is distributed in the hope that it will be useful, but WITHOUT ANY ");
            Console.WriteLine("\t WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A ");
            Console.WriteLine("\t PARTICULAR PURPOSE. See the GNU General Public License for more details.");
            Console.WriteLine("\t ");
            Console.WriteLine("\t You should have received a copy of the GNU General Public License along with ");
            Console.WriteLine("\t Blitzableiter. If not, see http://www.gnu.org/licenses/.");
        }

        /// <summary>
        /// Parses command line arguments
        /// </summary>
        /// <param name="args">The arguments as string array</param>
        private static void ParseCMDArgs(string[] args)
        {
            Console.Write(" Arguments : ");
            for (int i = 0; i < args.Length; i++)
            {

                Console.Write(" [{0}] {1}", i + 1, args[i]);
            }

            Console.WriteLine(Environment.NewLine);

            int lastSwitch = 0;
            foreach (string s in args)
            {
                if (s.StartsWith("-")) // cmd switch
                {
                    switch (s)
                    {
                        case "-o": // Overwrite
                            Overwrite = true;
                            lastSwitch++;
                            break;

                        case "-r": // Recursive
                            Recursive = true;
                            lastSwitch++;
                            break;

                        case "-d": // Directories
                            Single = false;
                            lastSwitch++;
                            break;

                        case "-v": // Verbose
                            Verbose = true;
                            lastSwitch++;
                            break;

                        case "-vv": // Very verbose
                            Verbose = true;
                            VeryVerbose = true;
                            lastSwitch++;
                            break;
                        default:
                            Console.WriteLine(" ");
                            Console.WriteLine(" Invalid argument \"" + s + "\"");
                            return;


                    }
                }
                else // File, directory or invalid argumrent
                {
                    if (args.Length - lastSwitch > 2)
                    {
                        Console.WriteLine(" Too many arguments" + Environment.NewLine);
                        return;
                    }
                    else if (args.Length - lastSwitch < 1)
                    {
                        Console.WriteLine(" Too few arguments");
                        return;
                    }
                    else if (args.Length - lastSwitch == 2)
                    {
                        string name = args[lastSwitch];

                        if (File.Exists(name))
                        {
                            Files.Add(args[lastSwitch]);
                            Files.Add(args[lastSwitch + 1]);
                            break;
                        }
                        else if (Directory.Exists(name))
                        {
                            Directories.Add(args[lastSwitch]);
                            Directories.Add(args[lastSwitch + 1]);
                            break;
                        }
                        else
                        {
                            Console.WriteLine(" Invalid file or directory argument. The input file or directory must exist.");
                            return;
                        }

                    }
                    else if (args.Length - lastSwitch == 1)
                    {
                        if (File.Exists(args[args.Length - 1]))
                        {
                            Files.Add(args[args.Length - 1]);
                            break;
                        }
                        else if (Directory.Exists(args[args.Length - 1]))
                        {
                            Directories.Add(args[args.Length - 1]);
                            break;
                        }
                        else
                        {
                            Console.WriteLine(" Invalid file or directory argument. The input file or directory must exist.");
                            return;
                        }
                    }
                }
            }

            if (Verbose && VeryVerbose)
            {
                Verbose = true;
            }

            if (Single) // Single File
            {
                if (Overwrite)
                {
                    if (Files.Count != 1)
                    {
                        Console.WriteLine(" Invalid command line arguments. Try : bb -o /yourinputdirectory/yourinputfile.swf");
                        return;
                    }
                    else
                    {
                        try
                        {
                            ScanSingle(Files[0]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(" " + e.Message);
                        }
                    }
                }
                else
                {
                    if (Files.Count != 2)
                    {
                        Console.WriteLine(" Invalid command line arguments. Try : bb /yourinputdirectory/yourinputfile.swf  /youroutputdirectory/youroutputfile.swf");
                        return;
                    }
                    else
                    {
                        try
                        {
                            ScanSingle(Files[0], Files[1]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(" " + e.Message);
                        }
                    }
                }
            }
            else// Directories
            {
                if (Recursive)
                {
                    if (Overwrite)
                    {
                        if (Directories.Count != 1)
                        {
                            Console.WriteLine(" Invalid command line arguments. Try : bb -d -r -o /yourinputdirectory");
                            return;
                        }
                        else
                        {
                            try
                            {

                                ScanDir(Directories[0], true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" " + e.Message);
                            }
                        }

                    }
                    else// Overwrite
                    {
                        if (Directories.Count != 2)
                        {
                            Console.WriteLine(" Invalid command line arguments. Try : bb -d -r /yourinputdirectory /youroutputdirectory");
                            return;
                        }
                        else
                        {
                            try
                            {
                                ScanDir(Directories[0], Directories[1], true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" " + e.Message);
                            }
                        }
                    }
                }
                else// Top dir only
                {
                    if (Overwrite)
                    {
                        if (Directories.Count != 1)
                        {
                            Console.WriteLine(" Invalid command line arguments. Try : bb -d -o /yourinputdirectory");
                            return;
                        }
                        else
                        {
                            try
                            {
                                ScanDir(Directories[0], true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" " + e.Message);
                            }
                        }

                    }
                    else// Overwrite
                    {
                        if (Directories.Count != 2)
                        {
                            Console.WriteLine(" Invalid command line arguments. Try : bb -d /yourinputdirectory /youroutputdirectory");
                            return;
                        }
                        else
                        {
                            ScanDir(Directories[0], Directories[1], true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Scans a directory with swf files and replaces them with scanned files
        /// </summary>
        /// <param name="s">The input directory</param>
        /// <param name="b">If a recursive scan is desired</param>
        private static void ScanDir(string s, bool b)
        {
            String[] flashFiles = Directory.GetFiles(s, "*.swf", b ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach (string flashFile in flashFiles)
            {
                ScanSingle(flashFile);
            }
        }

        /// <summary>
        /// Scans a directory with swf files and writes them into an output directory
        /// </summary>
        /// <param name="s1">The input directory</param>
        /// <param name="s2">The output directory</param>
        /// <param name="b">If a recursive scan is desired</param>
        private static void ScanDir(string s1, string s2, bool b)
        {
            String[] flashFiles = Directory.GetFiles(s1, "*.swf", b ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach (string flashFile in flashFiles)
            {
                FileInfo outputFile = new FileInfo(flashFile);
                string outputFileName = Path.Combine(s2, outputFile.Name);
                ScanSingle(flashFile, outputFileName);
            }
        }

        /// <summary>
        /// Overwrites a single file with a scanned copy
        /// </summary>
        /// <param name="s">The input file</param>
        private static void ScanSingle(string s)
        {
            bool read = false;
            bool verified = false;

            SwfFile file = new SwfFile();

            try
            {
                using (FileStream fs = File.OpenRead(s))
                {
                    file.Read(fs);
                    Console.WriteLine(" Reading successfull");
                    read = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(" Read error : " + e.Message + Environment.NewLine);
            }

            if (read)
            {
                Console.WriteLine(" Verifiying: " + s);

                try
                {
                    file.Verify();
                    Console.WriteLine(" Verifiying successfull");
                    verified = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(" Verifiy error : " + e.Message + Environment.NewLine);

                }
            }


            if (read && verified)
            {
                Console.WriteLine(" Writing: " + s);

                try
                {
                    using (FileStream fs = File.OpenWrite(s))
                    {
                        file.Write(fs);
                        Console.WriteLine(" Writing successfull" + Environment.NewLine);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" Write error : " + e.Message + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Scans a single file
        /// </summary>
        /// <param name="s1">The input file name</param>
        /// <param name="s2">The output file name</param>
        private static void ScanSingle(string s1, string s2)
        {
            bool read = false;
            bool verified = false;

            try
            {
                FileInfo f = new FileInfo(s2);

                if (!Directory.Exists(f.DirectoryName))
                {
                    Directory.CreateDirectory(f.DirectoryName);
                }
            }
            catch (Exception e)
            {
                throw new DirectoryNotFoundException(e.Message);
            }

            SwfFile file = new SwfFile();

            try
            {
                using (FileStream fs = File.OpenRead(s1))
                {
                    Console.WriteLine(" Reading: " + s1);
                    file.Read(fs);
                    Console.WriteLine(" Reading successfull");
                    read = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(" Read error : " + e.Message + Environment.NewLine);
            }

            if (read)
            {
                Console.WriteLine(" Verifiying: " + s1);

                try
                {
                    file.Verify();
                    Console.WriteLine(" Verifiying successfull");
                    verified = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(" Verifiy error : " + e.Message + Environment.NewLine);

                }
            }


            if (read && verified)
            {
                Console.WriteLine(" Writing: " + s2);

                try
                {
                    using (FileStream fs = File.OpenWrite(s2))
                    {
                        file.Write(fs);
                        Console.WriteLine(" Writing successfull" + Environment.NewLine);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" Write error : " + e.Message + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Event handler for Debug events
        /// </summary>
        /// <param name="o">The sending object</param>
        /// <param name="e">The event arguments as LogEventArgs</param>
        private static void PrintLogDebug(object o, LogEventArgs e)
        {
            DebugMessages += 1;
            if (VeryVerbose)
            {
                Console.WriteLine(" [Debug #{0:d4}] {1}", DebugMessages, e.Message);
            }
        }

        /// <summary>
        /// Event handler for Warning events
        /// </summary>
        /// <param name="o">The sending object</param>
        /// <param name="e">The event arguments as LogEventArgs</param>
        private static void PrintLogWarning(object o, LogEventArgs e)
        {
            Warnings += 1;
            if (Verbose)
            {
                Console.WriteLine(" [Warning #{0:d4}] Location : {1} Reason : {2}", Warnings, o.ToString(), e.Message);
            }
        }

        /// <summary>
        /// Event handler for Error events
        /// </summary>
        /// <param name="o">The sending object</param>
        /// <param name="e">The event arguments as LogEventArgs</param>
        private static void PrintLogError(object o, LogEventArgs e)
        {
            Errors += 1;
            Console.WriteLine(" [Error #{0:d4}] Location : {1} Reason : {2}", Errors, o.ToString(), e.Message);
        }
    }

}