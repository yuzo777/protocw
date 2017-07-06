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
                    if (!IsFlagsFine(options)) return;

                    if (!IsExecutablesExist(options)) return;

                    if (!IsDirsFine(options)) return;

                    var arguments = new Arguments(options, GetProtoPath(options));

                    foreach (var directory in Directory.EnumerateDirectories(Path.GetFullPath(options.InputDir), "*", SearchOption.AllDirectories).ToList())
                    {
                        if (directory.Contains(" "))
                        {
                            Console.Error.WriteLine("Directory contains spaces : " + directory);
                            return;
                        }

                        var files = Directory.EnumerateFiles(directory, "*.proto").ToList();

                        if (files.Count > 0)
                        {
                            var allFiles = CombineWithSpace(files);

                            if (options.Cpp) Execute(arguments.Cpp + allFiles);

                            if (options.CSharp) Execute(arguments.CSharp + allFiles);

                            if (options.Go) Execute(arguments.Go + allFiles);

                            if (options.GRPCGateway) Execute(arguments.GrpcGateway + allFiles);

                            if (options.Java || options.JavaLite || options.JavaNano) Execute(arguments.Java + allFiles);

                            if (options.Node) Execute(arguments.Node + allFiles);

                            if (options.ObjectiveC) Execute(arguments.ObjectiveC + allFiles);

                            if (options.PHP) Execute(arguments.Php + allFiles);

                            if (options.Python) Execute(arguments.Python + allFiles);

                            if (options.Ruby) Execute(arguments.Ruby + allFiles);

                            if (options.Swagger) Execute(arguments.Swagger + allFiles);
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
        private static string CombineWithSpace(List<string> files)
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
        private static bool IsFlagsFine(Options options)
        {
            if(!(options.Cpp || options.CSharp || options.Go || options.GRPCGateway
                || options.Java || options.JavaLite || options.JavaNano || options.Node
                || options.ObjectiveC || options.PHP || options.Python || options.Ruby || options.Swagger))
            {
                Console.Error.WriteLine("No language option is specified");
                return false;
            }

            if(!(options.Proto || options.GRPC || options.Swagger))
            {

                Console.Error.WriteLine("No category option is specified (--proto or --grpc or --swagger)");
                return false;
            }

            if(options.Java ? (options.JavaLite || options.JavaNano) : (options.JavaLite && options.JavaNano))
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
        private static bool IsExecutablesExist(Options options)
        {
            Func<bool, string, bool> check = (bool isOptionEnable, string path) =>
            {
                if(isOptionEnable)
                {
                    if (!File.Exists(path))
                    {
                        Console.Error.WriteLine("Executable file does not exist : " + path);
                        return false;
                    }
                }
                return true;
            };

            if (!check(true, ExePath.Protoc)) return false;

            if (!check(options.Cpp, ExePath.Cpp)) return false;

            if (!check(options.CSharp, ExePath.CSharp)) return false;

            if (!check(options.Go, ExePath.Go)) return false;

            if (!check(options.GRPCGateway, ExePath.GRPCGateway)) return false;

            if (!check(options.Java || options.JavaLite || options.JavaNano, ExePath.Java)) return false;

            if (!check(options.Node, ExePath.Node)) return false;

            if (!check(options.ObjectiveC, ExePath.ObjectiveC)) return false;

            if (!check(options.PHP, ExePath.PHP)) return false;

            if (!check(options.Python, ExePath.Python)) return false;

            if (!check(options.Ruby, ExePath.Ruby)) return false;

            if (!check(options.Swagger, ExePath.Swagger)) return false;

            if (!check(options.Cpp, ExePath.Cpp)) return false;

            if (!check(options.Cpp, ExePath.Cpp)) return false;

            return true;
        }

        /// <summary>
        /// Confirm existence of directory designated as argument.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        static private bool IsDirsFine(Options options)
        {
            if (ExePath.ProcessDir.Contains(" "))
            {
                Console.Error.WriteLine("protocw path contains spaces : " + ExePath.ProcessDir);
                return false;
            }

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
                }
            }

            return true;
        }

        static private string GetProtoPath(Options options)
        {
            var builder = new StringBuilder();

            builder.Append("-I" + Path.GetFullPath(options.InputDir));

            if (options.ProtoPath != null)
            {
                foreach (var path in options.ProtoPath)
                {
                    builder.Append(" -I" + Path.GetFullPath(path));
                }
            }

            return builder.ToString();
        }
    }
}
