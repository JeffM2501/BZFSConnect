﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BZFlag.Game.Host {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BZFlag.Game.Host.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Rejected.
        /// </summary>
        internal static string APIRejectMessage {
            get {
                return ResourceManager.GetString("APIRejectMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication Failed.
        /// </summary>
        internal static string BadAuthMessage {
            get {
                return ResourceManager.GetString("BadAuthMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid callsign.
        /// </summary>
        internal static string BadCallsignMessage {
            get {
                return ResourceManager.GetString("BadCallsignMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Callsign is registered.
        /// </summary>
        internal static string NameTakenMessagae {
            get {
                return ResourceManager.GetString("NameTakenMessagae", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This server does not support client side robot players.
        /// </summary>
        internal static string NoRobotsMessage {
            get {
                return ResourceManager.GetString("NoRobotsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registered Users Only.
        /// </summary>
        internal static string NoUnregMessage {
            get {
                return ResourceManager.GetString("NoUnregMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are in observer mode.
        /// </summary>
        internal static string ObserverModeNotificatioMessage {
            get {
                return ResourceManager.GetString("ObserverModeNotificatioMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The team %T is full.
        /// </summary>
        internal static string TeamFullMessage {
            get {
                return ResourceManager.GetString("TeamFullMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to usage: BZFSPro.exe [options] config_file
        ///  options:
        ///    -save_config, saves a config template to the
        ///		  specificed config file location
        ///
        ///  config files:
        ///    config files can be YAML, XML, or JSON
        ///    format is specified by extension.
        /// </summary>
        internal static string UseageText {
            get {
                return ResourceManager.GetString("UseageText", resourceCulture);
            }
        }
    }
}
