﻿using NUnit.Framework;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SampleLibraryTests")]
[assembly: Guid("c3d8c6fc-62ad-4272-91a9-492be76577e5")]

// The RequiresThread(ApartmentState.STA) attribute is required by
// NUNit to run all tests, that use the UI, on the same thread.
[assembly: RequiresThread(ApartmentState.STA)]