using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using NUnit.Framework;


[SetUpFixture]
public class Setup
{
    private string moduleRootFolder;
    List<string> resolutionPaths;

    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        var thisDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var configPath = Path.Combine(thisDir, "TestServices.dll.config");

        // Adjust the config file map because the config file
        // might not always be in the same directory as the dll.
        var map = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
        var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

        var element = config.AppSettings.Settings["DynamoBasePath"];
        moduleRootFolder = element?.Value ?? string.Empty;
 
        if (string.IsNullOrEmpty(moduleRootFolder))
        {
            throw new Exception("Missing DynamoBasePath in TestServices.dll.config. Please set the DynamoBasePath to a valid Dynamo bin folder.");
        }
        else if (!File.Exists(Path.Combine(moduleRootFolder, "DynamoCore.dll")))
        {
            throw new Exception("Invalid DynamoBasePath in TestServices.dll.config. Please set the DynamoBasePath to a valid Dynamo bin folder.");
        }

        resolutionPaths = new List<string>
        {
            // Search for culture specific resources
            Path.Combine(moduleRootFolder, CultureInfo.CurrentUICulture.Name),
            // Search for nodes
            Path.Combine(moduleRootFolder, "nodes"),
            // Search for culture specific node resources
            Path.Combine(moduleRootFolder, "nodes", CultureInfo.CurrentUICulture.Name)
        };
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve; ;
    }

    private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        try
        {
            var targetAssemblyName = new AssemblyName(args.Name).Name + ".dll";

            // First check the core path
            string assemblyPath = Path.Combine(moduleRootFolder, targetAssemblyName);
            if (File.Exists(assemblyPath))
            {
                return Assembly.LoadFrom(assemblyPath);
            }

            // Then check all additional resolution paths
            foreach (var resolutionPath in resolutionPaths)
            {
                assemblyPath = Path.Combine(resolutionPath, targetAssemblyName);
                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFrom(assemblyPath);
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("There location of the assembly, " +
                "{0} could not be resolved for loading.", args.Name), ex);
        }
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {
        AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
    }
}
