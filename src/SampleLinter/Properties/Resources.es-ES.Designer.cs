﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleLinter.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources___Copy {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources___Copy() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SampleLinter.Properties.Resources - Copy", typeof(Resources___Copy).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If the placed dropdowns are needing to be inputs, remember to rename them and mark them as input. If you do not need these as inputs, consider an alternative node..
        /// </summary>
        internal static string DropdownCallToAction {
            get {
                return ResourceManager.GetString("DropdownCallToAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have dropdown nodes placed that are not inputs..
        /// </summary>
        internal static string DropdownDescription {
            get {
                return ResourceManager.GetString("DropdownDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have nodes that are not in groups. Grouping your nodes is a best-practice when authoring Dynamo graphs. {0} nodes are allowed outside of groups based on the Linter Settings..
        /// </summary>
        internal static string NodesNotInGroupsCallToAction {
            get {
                return ResourceManager.GetString("NodesNotInGroupsCallToAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are {0} nodes that are not in groups..
        /// </summary>
        internal static string NodesNotInGroupsDescription {
            get {
                return ResourceManager.GetString("NodesNotInGroupsDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If the placed number sliders are needing to be inputs, remember to rename them and mark them as input. If you do not need these as inputs, consider an alternative node..
        /// </summary>
        internal static string SlidersCallToAction {
            get {
                return ResourceManager.GetString("SlidersCallToAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have number sliders placed that are not inputs..
        /// </summary>
        internal static string SlidersDescription {
            get {
                return ResourceManager.GetString("SlidersDescription", resourceCulture);
            }
        }
    }
}
