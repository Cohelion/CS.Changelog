using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CS.Changelog")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("CS.Changelog")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ff3e8efa-7842-4399-b920-6ffe03ca7ad4")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(CS.Changelog.Constants.Version)]
[assembly: AssemblyFileVersion(CS.Changelog.Constants.Version)]

namespace CS.Changelog {

	/// <summary>
	/// Reusable constant values
	/// </summary>
	public class Constants
	{
		/// <summary>
		/// The version number of CS.Changelog
		/// </summary>
		/// <remarks>When changing, also change the version number in the installer.</remarks>
		public const string Version = "1.1.6.0";
	}
}