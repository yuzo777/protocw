using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace protocw
{
    /// <summary>
    /// Class holding application options and flags
    /// </summary>
    public class Options
    {
        [Option('i', "input_dir", Required = false, DefaultValue =".", HelpText = "Root directory where the proto files are stored")]
        public string InputDir { get; set; }

        [Option('o', "output_dir", Required = false, DefaultValue = ".", HelpText = "Destination directory for the generated source files")]
        public string OutputDir { get; set; }

        [OptionList('p', "proto_path", Separator =';', HelpText = "Include proto_path (separated by ; when there are multiple)")]
        public IList<string> ProtoPath { get; set; } 
        
        [Option("cpp", DefaultValue = false, HelpText = "Genarate C++ source files")]
        public bool Cpp { get; set; }

        [Option("java", DefaultValue = false, HelpText = "Genarate Java source files (can not be specified simultaneously with other Java options)")]
        public bool Java { get; set; }

        [Option("java_lite", DefaultValue = false, HelpText = "Genarate Java-Lite source files (can not be specified simultaneously with other Java options)")]
        public bool JavaLite { get; set; }

        [Option("java_nano", DefaultValue = false, HelpText = "Genarate Java-Nano source files (can not be specified simultaneously with other Java options)")]
        public bool JavaNano { get; set; }

        [Option("python", DefaultValue = false, HelpText = "Genarate Python source files")]
        public bool Python { get; set; }

        [Option("go", DefaultValue = false, HelpText = "Genarate Go source files")]
        public bool Go { get; set; }

        [Option("ruby", DefaultValue = false, HelpText = "Genarate Ruby source files")]
        public bool Ruby { get; set; }

        [Option("csharp", DefaultValue = false, HelpText = "Genarate C# source files")]
        public bool CSharp { get; set; }

        [Option("node", DefaultValue = false, HelpText = "Genarate Node source files")]
        public bool Node { get; set; }

        [Option("objc", DefaultValue = false, HelpText = "Genarate ObjectiveC source files")]
        public bool ObjectiveC { get; set; }

        [Option("php", DefaultValue = false, HelpText = "Genarate PHP source files")]
        public bool Php { get; set; }

        [Option("grpc_gateway", DefaultValue = false, HelpText = "Genarate grpc-gateway source files")]
        public bool GrpcGateway { get; set; }

        [Option("swagger", DefaultValue = false, HelpText = "Genarate swagger.json files")]
        public bool Swagger { get; set; }

        [Option("grpc", DefaultValue = false, HelpText = "Generate gRPC source file of the language specified by the other option")]
        public bool GRPC { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
