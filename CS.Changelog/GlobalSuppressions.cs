// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:CS.Changelog.ChangelogService.GetChangelogs(System.Collections.Generic.IReadOnlyDictionary{System.IO.FileInfo,CS.Changelog.Exporters.IChangelogDeserializer},System.Boolean,System.Boolean,System.Collections.Generic.List{System.String})~CS.Changelog.ChangelogReadResult")]
[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>", Scope = "member", Target = "~M:CS.Changelog.GitExtensions.GetHistory(System.String,System.String,System.Boolean,System.String)~System.String")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:CS.Changelog.ChangelogService.GetChangelogs(System.Collections.Generic.IReadOnlyDictionary{System.IO.FileInfo,CS.Changelog.Exporters.IChangelogDeserializer},System.Boolean,System.Boolean,System.Collections.Generic.IEnumerable{System.String})~CS.Changelog.ChangelogReadResult")]
[assembly: SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "<Pending>", Scope = "member", Target = "~M:CS.Changelog.Utils.ConsoleExtensions.Dump(System.Object,CS.Changelog.LogLevel,System.Nullable{System.ConsoleColor})")]
[assembly: SuppressMessage("Performance", "CA1852:Seal internal types"            , Justification = "This is namespace documentation", Scope = "type", Target = "~T:CS.Changelog.Utils.NamespaceDoc")]
