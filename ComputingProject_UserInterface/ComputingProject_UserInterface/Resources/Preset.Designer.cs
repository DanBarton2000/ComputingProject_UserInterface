﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComputingProject_UserInterface.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Preset {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Preset() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ComputingProject_UserInterface.Resources.Preset", typeof(Preset).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;ArrayOfCelestialObject xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;
        ///  &lt;CelestialObject xsi:type=&quot;Star&quot;&gt;
        ///    &lt;Mass&gt;2E+30&lt;/Mass&gt;
        ///    &lt;TotalVelocity&gt;0&lt;/TotalVelocity&gt;
        ///    &lt;velocity&gt;
        ///      &lt;x&gt;14.117805166526075&lt;/x&gt;
        ///      &lt;y&gt;10.417108292279874&lt;/y&gt;
        ///    &lt;/velocity&gt;
        ///    &lt;Bearing&gt;0&lt;/Bearing&gt;
        ///    &lt;position&gt;
        ///      &lt;x&gt;307572036405.61365&lt;/x&gt;
        ///      &lt;y&gt;150726413546.9451&lt;/y&gt;
        ///    &lt;/position&gt;
        ///    &lt;screenPosition&gt;
        ///      &lt;x&gt;190.9135497 [rest of string was truncated]&quot;;.
        /// </summary>
        public static string SolarSystem {
            get {
                return ResourceManager.GetString("SolarSystem", resourceCulture);
            }
        }
    }
}
