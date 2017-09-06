using CS.Changelog.Utils;
using System;
using System.Text.RegularExpressions;

namespace CS.Changelog
{
	/// <summary>
	/// Class for parsing the commit message history.
	/// </summary>
	public static class Parsing
	{
		private const StringComparison c = StringComparison.InvariantCultureIgnoreCase;

		private static readonly Regex r = new Regex(@"
	^(?<hash>[\w\d]{40})
	\s*
	'(?<date>[T\d-\s\:\+]+)'
	\s*
	(?<message>
		(?<merge>Merge\sbranch
			\s
			'(?<frombranch>[^'\n\s]+)
			'\s
			(?:
				into
				\s
				(?<tobranch>[^\n\s]+)
			)?
		)?
		(?<remainder>(?:.(?!\n[\w\d]{40}))*)
	)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline);

		private static readonly Regex commitMessageRegex = new Regex(@"\[(?<category>[^\]]+)\](?<message>.+)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline);

		private static readonly Regex branchNameRegex = new Regex(@"((?<prefix>.*)\/)?(?<fullname>(?<issuenumber>[a-z0-9]+-\d+)?(?<name>.*))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>Parses the specified log.</summary>
		/// <param name="log">The log to parse</param>
		/// <param name="options">The options for parsing the log file.</param>
		/// <returns>A changeset</returns>
		/// <remarks>
		/// See internals on how changelog is to be obtained (<seealso cref="GitExtensions.GetHistory"/>).
		/// Log format is: 
		/// <code>
		/// HASH DATE MULTILINEMESSAGE
		/// </code>
		/// </remarks>
		/// <seealso cref="GitExtensions.GetHistory"/>
		public static ChangeSet Parse(string log, ParseOptions options)
		{
			var matches = r.Matches(log);

			var result = new ChangeSet();

			foreach (Match match in matches)
			{
				var hash = match.Groups["hash"].Value.Trim();
				var merge = match.Groups["merge"].Value.Trim();
				var isMerge = !string.IsNullOrWhiteSpace(merge);
				var fromBranch = match.Groups["frombranch"].Value.Trim();
				var toBranch = match.Groups["tobranch"].Value.Trim();
				
				//Parse message to see if it was useful
				var message = match.Groups["message"].Value.Trim();
				var messagematch = commitMessageRegex.Match(message);

				try
				{
					bool ignored = false;

					if (!isMerge)
						if (!messagematch.Success)
							LogIgnoredCommit($"Regular commit with changelog category omitted", match, ref ignored);
						else
						{
							//Not an ignored commit nor a merge, see commit message hadling later on
						}
					else
					{
						if (fromBranch.Equals(options.branch_development, c))

							//Ignore merging the development branch into anything					
							LogIgnoredCommit("Ignoring catchup merge", match, ref ignored);

						else if (fromBranch.Equals(options.branch_master, c))

							//Ignore merging the master branch into anything					
							LogIgnoredCommit("Ignoring merging master back into anything", match, ref ignored);

						else
						{
							var fromBranchMatch = branchNameRegex.Match(fromBranch);
							var fromBranchPrefix = fromBranchMatch.Groups["prefix"].Value.ToLower();
							var fromBranchFullName = fromBranchMatch.Groups["fullname"].Value.ToLower();

							if (string.IsNullOrWhiteSpace(toBranch))
							{
								if (fromBranchPrefix.Equals(options.prefix_feature, c))
								{
									LogCommit($"Feature {fromBranch} is merged directly into master!", match, level: LogLevel.Warning);
									result.Add(hash, options.category_feature, fromBranchFullName);
								}
								else if (fromBranchPrefix.Equals(options.prefix_release, c))

									//ignore release into dev
									LogIgnoredCommit($"Merging {fromBranchPrefix} into {toBranch} is ignored", match, ref ignored);

								else if (fromBranchPrefix.Equals(options.prefix_hotfix, c))
								{
									LogCommit($"{fromBranchPrefix} {fromBranchFullName} is merged", match, ConsoleColor.Green, level: LogLevel.Info);
									result.Add(hash: hash, category: options.category_hotfix, message: fromBranchFullName);
								}
								else
								{
									LogCommit($"Non-gitflow Branch {fromBranch} is merged directly into master!", match, level: LogLevel.Warning);
									result.Add(hash: hash, category: "Change", message: fromBranchFullName);
								}
							}
							else if (toBranch.Equals(options.branch_development, c))
							{
								if (fromBranchPrefix.Equals(options.prefix_feature, c))
								{
									LogCommit($"Feature {fromBranchFullName} is completed", match, ConsoleColor.Green, level: LogLevel.Info);
									result.Add(hash, options.category_feature, fromBranchFullName);
								}
								else if (fromBranchPrefix.Equals(options.prefix_release, c)
									   || fromBranchPrefix.Equals(options.prefix_hotfix, c))
								{
									LogIgnoredCommit($"Merging {fromBranchPrefix} into {toBranch} is ignored", match, ref ignored);
								}
								else
								{
									LogCommit($"Branch {fromBranchPrefix} {fromBranchFullName} is completed", match, ConsoleColor.Green);
									result.Add(hash, "Unknown", fromBranchFullName);
								}
							}
							else if (toBranch.Equals(options.branch_master, c))
							{
								if (fromBranchPrefix.Equals(options.prefix_feature, c))
								{
									LogIgnoredCommit($"Merging {fromBranchPrefix} into {options.branch_master} is ignored : {match.Groups["message"].Value}", match, ref ignored);
								}
								else if (fromBranchPrefix.Equals(options.prefix_release, c)
									|| (fromBranchPrefix.Equals(options.prefix_hotfix, c)))
								{
									LogIgnoredCommit($"Merging {fromBranchPrefix} into {options.branch_master} is ignored : {match.Groups["message"].Value}", match, ref ignored);
								}
								else
								{
									LogCommit($"Branch {fromBranchPrefix} {fromBranchFullName} is merged directly into master", match, level: LogLevel.Warning);
								}
							}
							else
							{
								LogCommit($@"{match.Groups["frombranch"].Value} => {match.Groups["tobranch"].Value} {match.Groups["remainder"].Value}", match, level: LogLevel.Warning);
								//if (fromBranchPrefix.Equals(o.prefix_feature, c)) { }
								//else if (fromBranchPrefix.Equals(o.prefix_release, c)) { }
								//else if (fromBranchPrefix.Equals(o.prefix_hotfix, c)) { }
								//else { }
							}
						}
					}

					//If entire commit is not ignore, append any categorized release message
					if (!ignored && messagematch.Success)
					{
						LogCommit($"Commit with changelog category added : {message}", match, level: LogLevel.Info);
						result.Add(hash, messagematch.Groups["category"].Value, messagematch.Groups["message"].Value);
					}
				}
				catch (Exception ex)
				{
					$@"Error while analyzing {match.Value} : {ex}".Dump(loglevel: LogLevel.Error);
				}
			}
			return result;
		}

		static void LogCommit(string message, Match match, System.ConsoleColor? color = null, LogLevel level = LogLevel.Info)
		{
			$@"{match.Groups["hash"].Value.Substring(0, 10)}... {DateTime.Parse(match.Groups["date"].Value)} {message.Trim()}".Dump(color: color);

			LogCommitMessage(match.Groups["remainder"].Value, color);
		}

		static void LogCommitMessage(string message, System.ConsoleColor? color = null, LogLevel level = LogLevel.Info)
		{
			var commitMessageRegex = new Regex(".+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			var i = 0;
			foreach (Match match in commitMessageRegex.Matches(message))
			{
				if (string.IsNullOrWhiteSpace(match.Value))
					continue;

				i++;

				$@"{i})		{match.Value.Trim()}".Dump(color: color.GetValueOrDefault(level.ToConsoleColor()));
			}
		}
		static void LogIgnoredCommit(string reason, Match match, ref bool ignored)
		{
			LogCommit($"{reason} : {match.Groups["message"].Value}", match, level: LogLevel.Debug);
			ignored = true;
		}
	}
}