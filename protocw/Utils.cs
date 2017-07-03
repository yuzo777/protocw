using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace protocw
{
    /// <summary>
    /// Class holding Executable file paths
    /// </summary>
    public static class ExePath
    {
        public static readonly string ProcessDir = System.AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string Protoc = ProcessDir + "protoc.exe";
        public static readonly string Cpp = ProcessDir + "grpc_cpp_plugin.exe";
        public static readonly string CSharp = ProcessDir + "grpc_csharp_plugin.exe";
        public static readonly string Go = ProcessDir + "protoc-gen-go.exe";
        public static readonly string GRPCGateway = ProcessDir + "protoc-gen-grpc-gateway.exe";
        public static readonly string Java = ProcessDir + "protoc-gen-grpc-java.exe";
        public static readonly string Node = ProcessDir + "grpc_node_plugin.exe";
        public static readonly string ObjectiveC = ProcessDir + "grpc_objective_c_plugin.exe";
        public static readonly string Php = ProcessDir + "grpc_php_plugin.exe";
        public static readonly string Python = ProcessDir + "grpc_python_plugin.exe";
        public static readonly string Ruby = ProcessDir + "grpc_ruby_plugin.exe";
        public static readonly string Swagger = ProcessDir + "protoc-gen-swagger.exe";
    }

    /// <summary>
    /// Class holding protoc arguments corresponding to languages
    /// </summary>
    public class Arguments
    {
        public Arguments(Options options, string allProtoPath)
        {
            var outputDir = Path.GetFullPath(options.OutputDir);

            string commonGRPCOption;
            string goOption;
            string javaGRPCOption;

            if (options.GRPC)
            {
                commonGRPCOption = " --grpc_out=" + outputDir;
                goOption = " --go_out=plugins=grpc:" + outputDir;

                if (options.JavaLite)
                {
                    javaGRPCOption = " --grpc-java_out=lite:" + outputDir;
                }
                else if (options.JavaNano)
                {
                    javaGRPCOption = " --grpc-java_out=nano:" + outputDir;
                }
                else
                {
                    javaGRPCOption = " --grpc-java_out=" + outputDir;
                }
            }
            else
            {
                commonGRPCOption = "";
                goOption = " --go_out=" + outputDir;
                javaGRPCOption = "";
            }

            Cpp = allProtoPath + " --plugin=protoc-gen-grpc=" + ExePath.Cpp
                + " --cpp_out=" + outputDir + commonGRPCOption;

            CSharp = allProtoPath + " --plugin=protoc-gen-grpc=" + ExePath.CSharp
                + " --csharp_out=" + outputDir + commonGRPCOption;

            Go = allProtoPath + goOption;

            GrpcGateway = allProtoPath + " --grpc-gateway_out=logtostderr=true:" + outputDir;

            Java = allProtoPath + " --plugin=protoc-gen-grpc-java=" + ExePath.Java
                + " --java_out=" + outputDir + javaGRPCOption;

            Node = allProtoPath + " --plugin=protoc-gen-grpc=" + ExePath.Node
                + " --js_out=" + outputDir + commonGRPCOption;

            ObjectiveC = allProtoPath + " --plugin=protoc-gen-grpc=" + ExePath.ObjectiveC
                + " --objc_out=" + outputDir + commonGRPCOption;

            Php = allProtoPath + " --plugin=protoc-gen-grpc=" + ExePath.Php
                + " --php_out=" + outputDir + commonGRPCOption;

            Python = allProtoPath + " --plugin=protoc-gen-grpc=" + ExePath.Python
                + " --python_out=" + outputDir + commonGRPCOption;

            Ruby = allProtoPath + " --plugin=protoc-gen-grpc=" + ExePath.Ruby
                + " --ruby_out=" + outputDir + commonGRPCOption;

            Swagger = allProtoPath + " --swagger_out=logtostderr=true:" + outputDir;
        }

        public string Cpp { get; }
        public string CSharp { get; }
        public string Go { get; }
        public string GrpcGateway { get; }
        public string Java { get; }
        public string Node { get; }
        public string ObjectiveC { get; }
        public string Php { get; }
        public string Python { get; }
        public string Ruby { get; }
        public string Swagger { get; }
    }
}
