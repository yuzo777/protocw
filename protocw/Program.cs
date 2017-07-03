using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace protocw
{
    /// <summary>
    /// Main class of application
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main Method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try {

                var options = new Options();
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    string include;

                    if (!isFlagsFine(options)) return;

                    if (!isExecutablesFine(options)) return;

                    if (!isDirsFine(options, out include)) return;

                    var arguments = new Arguments(options, include);

                    foreach (var directory in Directory.EnumerateDirectories(options.InputDir, "*", SearchOption.AllDirectories).ToList())
                    {
                        if (directory.Contains(" "))
                        {
                            Console.Error.WriteLine("Directory contains spaces : " + directory);
                            return;
                        }

                        var files = Directory.EnumerateFiles(directory, "*.proto").ToList();

                        if (files.Count > 0)
                        {
                            var protos = SpacedFullPath(files);

                            if (options.Cpp) Execute(arguments.Cpp + protos);

                            if (options.CSharp) Execute(arguments.CSharp + protos);

                            if (options.Go) Execute(arguments.Go + protos);

                            if (options.GrpcGateway) Execute(arguments.GrpcGateway + protos);

                            if (options.Java || options.JavaLite || options.JavaNano) Execute(arguments.Java + protos);

                            if (options.Node) Execute(arguments.Node + protos);

                            if (options.ObjectiveC) Execute(arguments.ObjectiveC + protos);

                            if (options.Php) Execute(arguments.Php + protos);

                            if (options.Python) Execute(arguments.Python + protos);

                            if (options.Ruby) Execute(arguments.Ruby + protos);

                            if (options.Swagger) Execute(arguments.Swagger + protos);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("Exception raised : " + e.ToString());
            }
        }

        /// <summary>
        /// Combine the given string list with a space delimiter and return it.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private static string SpacedFullPath(List<string> files)
        {
            var builder = new StringBuilder();

            foreach (var file in files)
            {
                builder.Append(" ");
                builder.Append(file);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Execute protoc with given arguments and print errors if it out.
        /// </summary>
        /// <param name="arguments"></param>
        private static void Execute(string arguments)
        {
            using (var p = Process.Start(
                new ProcessStartInfo
                {
                    FileName = ExePath.Protoc,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true
                }))
            {
                p.WaitForExit();
                var err = p.StandardError.ReadToEnd();

                if (err.Length > 0)
                {
                    Console.Error.WriteLine(err);
                }
            }
        }

        /// <summary>
        /// Confirm whether there is no inconsistency in the flag.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static bool isFlagsFine(Options options)
        {
            if(!(options.Cpp || options.CSharp || options.Go || options.GrpcGateway
                || options.Java || options.JavaLite || options.JavaNano || options.Node
                || options.ObjectiveC || options.Php || options.Python || options.Ruby || options.Swagger))
            {
                Console.Error.WriteLine("No language option is specified");
                return false;
            }

            int Java = 0;
            if (options.Java)
            {
                Java++;
            }
            if (options.JavaLite)
            {
                Java++;
            }
            if (options.JavaNano)
            {
                Java++;
            }
            if (Java > 1)
            {
                Console.Error.WriteLine("More than one Java option is specified");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Confirm existence of necessary executable file.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static bool isExecutablesFine(Options options)
        {
            if(ExePath.ProcessDir.Contains(" "))
            {
                Console.Error.WriteLine("protocw path contains spaces : " + ExePath.ProcessDir);
                return false;
            }

            const string errorMessagePrefix = "Executable file does not exist : ";

            if (!File.Exists(ExePath.Protoc))
            {
                Console.Error.WriteLine(errorMessagePrefix + ExePath.Cpp);
                return false;
            }
            
            if (options.Cpp)
            {
                if (!File.Exists(ExePath.Cpp))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.Cpp);
                    return false;
                }
            }

            if(options.CSharp)
            {
                if (!File.Exists(ExePath.CSharp))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.CSharp);
                    return false;
                }
            }

            if (options.Go)
            {
                if (!File.Exists(ExePath.Go))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.Go);
                    return false;
                }
            }

            if (options.GrpcGateway)
            {
                if (!File.Exists(ExePath.GRPCGateway))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.GRPCGateway);
                    return false;
                }
            }

            if (options.Java || options.JavaLite || options.JavaNano)
            {
                if (!File.Exists(ExePath.Java))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.Java);
                    return false;
                }
            }

            if (options.Node)
            {
                if (!File.Exists(ExePath.Node))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.Node);
                    return false;
                }
            }

            if (options.ObjectiveC)
            {
                if (!File.Exists(ExePath.ObjectiveC))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.ObjectiveC);
                    return false;
                }
            }

            if (options.Php)
            {
                if (!File.Exists(ExePath.Php))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.Php);
                    return false;
                }
            }

            if (options.Python)
            {
                if (!File.Exists(ExePath.Python))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.Python);
                    return false;
                }
            }

            if (options.Ruby)
            {
                if (!File.Exists(ExePath.Ruby))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.Ruby);
                    return false;
                }
            }

            if(options.Swagger)
            {
                if (!File.Exists(ExePath.Swagger))
                {
                    Console.Error.WriteLine(errorMessagePrefix + ExePath.Swagger);
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// Confirm existence of directory designated as argument.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        static private bool isDirsFine(Options options, out string include)
        {
            include = "";

            if (!Directory.Exists(options.InputDir))
            {
                Console.Error.WriteLine("input_dir not exist : " + Path.GetFullPath(options.InputDir));
                return false;
            }

            if(Path.GetFullPath(options.InputDir).Contains(" "))
            {
                Console.Error.WriteLine("input_dir contains spaces : " + Path.GetFullPath(options.InputDir));
                return false;
            }

            if (!Directory.Exists(options.OutputDir))
            {
                Console.Error.WriteLine("output_dir not exist : " + Path.GetFullPath(options.OutputDir));
                return false;
            }

            if (Path.GetFullPath(options.OutputDir).Contains(" "))
            {
                Console.Error.WriteLine("output_dir contains spaces : " + Path.GetFullPath(options.OutputDir));
                return false;
            }

            var builder = new StringBuilder();
            builder.Append("-I" + Path.GetFullPath(options.InputDir));

            if (options.ProtoPath != null)
            {
                foreach (var path in options.ProtoPath)
                {
                    if (!Directory.Exists(path))
                    {
                        Console.Error.WriteLine("proto_path does not exist : " + Path.GetFullPath(path));
                        return false;
                    }

                    if(Path.GetFullPath(path).Contains(" "))
                    {
                        Console.Error.WriteLine("proto_path contains spaces : " + Path.GetFullPath(path));
                        return false;
                    }

                    builder.Append(" -I" + Path.GetFullPath(path));
                }
            }

            include = builder.ToString();
            return true;
        }
    }
}
