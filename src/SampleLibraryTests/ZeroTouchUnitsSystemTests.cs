using System.Collections.Generic;
using SystemTestServices;

using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes.ZeroTouch;
using Dynamo.Tests;

using NUnit.Framework;

namespace SampleLibraryTests
{
  /// <summary>
  /// ZeroTouchUnitsSystemTests is a test fixture that contains
  /// system tests for Dynamo using Units APIs. System tests test the entire 
  /// Dynamo system including the UI. They do this by starting
  /// a session of Dynamo, then opening .dyn files, executing them
  /// and comparing the values returned from Dynamo with those
  /// stored on our test class.
  /// 
  /// IMPORTANT! 
  /// System tests have dependencies on Dynamo core dlls. In 
  /// order for these tests to work, your test dll needs to be 
  /// located in the Dynamo core directory. The project currently assumes
  /// that Dynamo is built in a directory adjacent to the DynamoSamples
  /// repository, so a relative path is set to the debug build folder for Dynamo.
  /// If your setup is different, then you will need to explicitly set the output path
  /// to your Dynamo installation directory.
  /// </summary>
  [TestFixture]
    public class ZeroTouchUnitsSystemTests : SystemTestBase
    {
        protected override void GetLibrariesToPreload(List<string> libraries)
        {
          libraries.Add("FunctionObject.ds");
          libraries.Add("BuiltIn.ds");
          libraries.Add("DSCoreNodes.dll");
          libraries.Add("VMDataBridge.dll");
          libraries.Add("DynamoConversions.dll");
          libraries.Add("DynamoUnits.dll");
          libraries.Add("DesignScriptBuiltin.dll");
          libraries.Add("..\\..\\..\\..\\dynamo_package\\Dynamo Samples\\bin\\SampleZeroTouchUnits.dll");
          base.GetLibrariesToPreload(libraries);
        }

        // The RequiresSTA attribute is required by
        // NUNit to run tests that use the UI.
        [Test, Apartment(System.Threading.ApartmentState.STA)]
        public void HelloRectangleUnitsTest()
        {
            // RectangleUnitsExample.dyn is a test .dyn file which
            // should be copied to the output directory, so it's available
            // for testing. You can also change this path to anywhere you
            // would like to get your test file from, but it has to be
            // a relative path from the dynamo core directory (the working directory).

            OpenAndRunDynamoDefinition(@".\RectangleUnitsExample.dyn");

            // Ensure that the graph opened without any "dummy nodes".
            // Dummy nodes would appear if your graph had a node that
            // could not be found in the library.

            AssertNoDummyNodes();

            // Get the nodes from the workspace.
            var out1 = Model.CurrentWorkspace.NodeFromWorkspace("{15b2e81d-84df-4a17-ae34-8ee982ab87d9}") as CoreNodeModels.Watch;
            var out2 = Model.CurrentWorkspace.NodeFromWorkspace("{77e2a5bb-3d46-4c56-94f4-44ea1e064deb}") as CoreNodeModels.Watch;
            var out3 = Model.CurrentWorkspace.NodeFromWorkspace("{bbb9d9f6-be8e-498e-8b9c-b8cae214c921}") as CoreNodeModels.Watch;
            Assert.NotNull(out1);
            Assert.NotNull(out2);
            Assert.NotNull(out3);

            // Ensure that the value of that node after evaluation is
            // the value that we are looking for.
            Assert.AreEqual(out1.CachedValue, 500);
            Assert.AreEqual(out2.CachedValue, 5);
            Assert.AreEqual(out3.CachedValue, "538.195520835486ft^2");
    }
    }
}
