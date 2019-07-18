using CS.Changelog.Utils;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
		(?<merge>
			Merge
			\s
			branch
			\s
			'(?<frombranch>[^'\n\s]+)
			'
			(?:
				\s
				of
				\s
				(?<remote>[^\n\s]+)
			)?
			(?:
				\s
				into
				\s
				(?<tobranch>[^\n\s]+)
			)?
		)?
		(?<remainder>(?:.(?!\n[\w\d]{40}))*)
	)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        private static readonly Regex commitMessageRegex = new Regex(@"
#the message category is anything (but a newline) in brackets
\[
	(?<category>[^\]\n]+)
\]

#the message can contain anything but the start of a new message, or a comment
(?<message>
(?:
	(?!
		\n
		(?:
			#indicates the start of a new message
			\[[^\]\n]+\]

			|

			#message comments (like merge conflict resolutions are to be ignored)
			\#
		)
	)
.
)+
)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        private static readonly Regex branchNameRegex = new Regex(@"((?<prefix>.*)\/)?(?<fullname>(?<issuenumber>[a-z0-9]+-\d+)?(?<name>.*))", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.ExplicitCapture);


        /// <summary>Parses the specified log.</summary>
        /// <param name="log">The log to parse</param>
        /// <param name="options">The options for parsing the log file.</param>
        /// <returns>A changeset</returns>
        /// <remarks>
        /// See internals on how changelog is to be obtained (<seealso cref="GitExtensions.GetHistory" />).
        /// Log format is:
        /// <code>
        /// HASH DATE MULTILINEMESSAGE
        /// </code>
        /// </remarks>
        /// <seealso cref="GitExtensions.GetHistory" />
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Contains global parsing catch")]
        public static ChangeSet Parse(string log, ParseOptions options = null)
        {
            //If no options are specified, use default options
            if (options == null)
                options = new ParseOptions();

            var matches = r.Matches(log);

            var result = new ChangeSet();

            foreach (Match match in matches)
            {
                var hash = match.Groups["hash"].Value.Trim();
                var merge = match.Groups["merge"].Value.Trim();
                var fromBranch = match.Groups["frombranch"].Value.Trim();
                var toBranch = match.Groups["tobranch"].Value.Trim();
                var remote = match.Groups["remote"].Value.Trim();

                //Derive logic
                var isMerge = !string.IsNullOrWhiteSpace(merge);
                var isMergeUponPull = isMerge && fromBranch.Equals(toBranch, c) && !string.IsNullOrWhiteSpace(remote);

                //Parse message to see if it was useful
                var message = match.Groups["message"].Value.Trim();
                var messagematches = commitMessageRegex.Matches(message);

                try
                {
                    bool ignored = false;

                    if (!isMerge)
                        if (messagematches.Count == 0)
                            LogIgnoredCommit($"Regular commit without explicit changelog messages omitted", match, ref ignored);
                        else
                        {
                            //Not an ignored commit nor a merge, see commit message handling later on
                        }
                    else if (isMergeUponPull)
                        if (messagematches.Count == 0)
                            LogIgnoredCommit($"Merge when pulling without explicit changelog messages omitted", match, ref ignored);
                        else
                        {
                            //Not an ignored commit nor a merge, see commit message handling later on
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
                            var fromBranchPrefix = fromBranchMatch.Groups["prefix"].Value;//.ToLower(CultureInfo.InvariantCulture);
                            var fromBranchFullName = fromBranchMatch.Groups["fullname"].Value;//.ToLower(CultureInfo.InvariantCulture);

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
                                    result.Add(hash: hash, category: "Unknown", message: $"Merge from '{fromBranchFullName}'");
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
                                    result.Add(hash, category: "Unknown", message: $"Merge from '{fromBranchFullName}'");
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
                            else if (toBranch.Equals(options.branch_preview, c))
                            {
                                if (fromBranchPrefix.Equals(options.prefix_feature, c))
                                {
                                    LogCommit($"Feature {fromBranchFullName} is selected for or updated on preview", match, ConsoleColor.Green, level: LogLevel.Info);
                                    result.Add(hash, options.category_feature, fromBranchFullName);
                                }
                                else if (fromBranch.Equals(options.branch_development, c)
                                     || (fromBranchPrefix.Equals(options.prefix_release, c)
                                     || (fromBranchPrefix.Equals(options.prefix_hotfix, c)))
                                     )
                                {
                                    LogIgnoredCommit($"Merging {fromBranch} into {options.branch_preview} is ignored : {match.Groups["message"].Value}", match, ref ignored);
                                }
                                else
                                {
                                    LogCommit($"Branch {fromBranch} is merged directly into preview", match, level: LogLevel.Warning);
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
                    if (!ignored && messagematches.Count > 0)
                    {
                        foreach (Match messagematch in messagematches)
                        {
                            LogCommit($"Commit with changelog category added : {message}", match, level: LogLevel.Info);
                            result.Add(hash, messagematch.Groups["category"].Value, messagematch.Groups["message"].Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    $@"Error while analyzing {match.Value} : {ex}".Dump(loglevel: LogLevel.Error);
                }
            }
            return result;
        }

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Loglevel is pending implementation")]
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Loglevel is pending implementation")]
        static void LogCommit(string message, Match match, System.ConsoleColor? color = null, LogLevel level = LogLevel.Info)
        {
            $@"{match.Groups["hash"].Value.Substring(0, 10)}... {DateTime.Parse(match.Groups["date"].Value, CultureInfo.CurrentCulture)} {message.Trim()}".Dump(color: color);

            LogCommitMessage(match.Groups["remainder"].Value, color);
        }

        static void LogCommitMessage(string message, ConsoleColor? color = null, LogLevel level = LogLevel.Info)
        {
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