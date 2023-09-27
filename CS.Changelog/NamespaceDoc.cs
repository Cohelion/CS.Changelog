using System.Runtime.CompilerServices;

namespace CS.Changelog
{

#pragma warning disable CA1812 // Avoid uninstantiated internal classes

    /// <summary>
    /// Main engine for changelog generation. Generally a changelog is created by:
    /// <list type="number">  
    ///    <item>  
    ///        <term>Reading commits</term>  
    ///        <description><see cref="GitExtensions.GetHistory">Obtain commit history, based on git commits</see></description>  
    ///    </item>  
    ///    <item>  
    ///        <term>Interpreting the history</term>  
    ///        <description><see cref="Parsing.Parse">Interpret commits</see> based on <see cref="ParseOptions"/>.</description>  
    ///    </item>  
    ///    <item>  
    ///        <term>Writing or appending the changelog</term>  
    ///        <description><see cref="Exporters.IChangelogExporter.Export">Export the changelog</see> to the requested <see cref="OutputFormat"/>.</description>  
    ///    </item>
    ///</list>
    ///</summary>
    ///<seealso cref="GitExtensions"/>
    ///<seealso cref="Exporters"/>
    ///<seealso cref="Parsing"/>
    [CompilerGenerated]
	sealed class NamespaceDoc
	{
		//Empty non-public class with name 'NamespaceDoc', with CompilerGenerated attribute can be used to document the namespace it is in.		
	}

	/// <summary>
	/// See <see cref="CS.Changelog"/>
	///</summary>
	[CompilerGenerated]
	sealed class NamespaceGroupDoc
	{
		//Empty non-public class with name 'NamespaceDoc', with CompilerGenerated attribute can be used to document the namespace it is in.		
	}
}
