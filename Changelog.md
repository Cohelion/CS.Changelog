# (13/09/2021)  #

## Feature ##
- Now underscores are replaced with spaces in the message when suspected usage of feature branch name as message. 

- Now issue number will me moved to the end of the message when available at the beginning. ([e916b596](https://github.com/cswebworks/CS.Changelog/commit/e916b596ab3f954528b2245cd539969f8a66b0af))

  

# v1.3.0.0 (08/09/2021)  #

## Feature ##
- Added multi-file parsing and filtering functionality ([297de634](https://github.com/cswebworks/CS.Changelog/commit/297de634313cc5ef9c1115db68372715ac1c6884))

## Rebranding ##
- Converted CS /webworks to Cohelion
- Added application and package icon ([2e334593](https://github.com/cswebworks/CS.Changelog/commit/2e334593618579abb4ff7a6b53464036f344bb51))

# v1.2.0.6 #
- Used only one tab for indentation

# v1.2.0.4 (22/03/2021)  #
- Set default json output indentation character to tabs (no config option yet)

# v1.2.0.3  #
1.2.0.3: Fixed ignoring of startTag argument.

# v1.2.0.2  #
1.2.0.2: Migrated console to .Net core

# v1.2.0.1 (26/07/2019)  #

## Fix ##
- Made some changelog properties settable
  - RepositoryUrl
  - IssueNumberRegex
  - IssueTrackerUrl
  In some cases these properties may need to be adjusted. In our case a link to the issue tracker was conditional. ([6af8997b](https://github.com/cswebworks/CS.Changelog/commit/6af8997b331877ec9ca83ebbfe410b828c51b721))
# v1.2.0 (21/07/2019) - Initial public release  #

## Fix ##
- Changed `ShouldSerializeIgnored` to `ShouldSerializeIgnore`, indicating that the property `Ignore` should only be serialized when true. This should prevent `ignore=false` to appear in json-serialized change logs. ([a7d15791](https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM/commit/a7d1579123c09db9e47a6aa8ef7f132cb70bea4c))
- *breaking change* Changed `ignored` attribute to `ignore`
This is an instruction to the parser, and now is in line with the change log of this app :) ([3fe489c9](https://github.com/cswebworks/CS.Changelog/commit/3fe489c93273177836550b09ad909fb0ee071fde))
- Refreshed files information after actual file creation
Bumped version, created new installer ([f6c1cf85](https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM/commit/f6c1cf85be21779b276f169419f208042ffccc9a))
- Corrected console switches
Default values are false, set to true when used
Added switch for creating full changelog instead of incremental
Removed invalid default value for repository location
Made opening the output file optional in interactive mode ([5a79d73e](https://github.com/cswebworks/CS.Changelog/commit/5a79d73e0f4b51b5635e9e0ff6dab7a5e0eee48c))

## Feature ##
- Created NuGet package, exposed changelog deserializers ([2006558b](https://github.com/cswebworks/CS.Changelog/commit/2006558b91474fc7e0e791442e5f9d3c317234ad))
- Added option for manually overriding the start tag
Allowing generating a changelog incremental from a manually specified tag ([ee95d3a4](https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM/commit/ee95d3a46832db050fac7530659f3625284593c6))
- Added `ignore` property to ChangeLogMessage
This allows hiding commits that would otherwise reappear in the change log when regenerating it. ([09f0b686](https://github.com/cswebworks/CS.Changelog/commit/09f0b686e64d1f4d57e5cc3730cbc69c1f586673))
- Added a proper installer ([336eb3d8](https://github.com/cswebworks/CS.Changelog/commit/336eb3d83c6efb39b8e6d406eb6ec71051f2e35f))
- Allowed multiple release notes per commit message (see this commit) ([43bebfb1](https://github.com/cswebworks/CS.Changelog/commit/43bebfb16657093a09bc6c716ccafdb8a205d8fc))
- Prevented comments in commit message from appearing in the release notes (like merge conflict details) ([43bebfb1](https://github.com/cswebworks/CS.Changelog/commit/43bebfb16657093a09bc6c716ccafdb8a205d8fc))
- Implemented Html exporter ([189aafa2](https://github.com/cswebworks/CS.Changelog/commit/189aafa293b8218f48c912a2cfabd3b614ead9db))
- Allowed specification of release name, allowed passing output format as argument ([73212b25](https://github.com/cswebworks/CS.Changelog/commit/73212b257dc7221f224428f5b9062a00d0eb0b95))

## Bugfix ##
- Made default IssueNumberRegex case insensitive ([0e9729db](https://github.com/cswebworks/CS.Changelog/commit/0e9729dbcdc70e6d545721d503de39e9b31c2853))
- [CS-419](https://project.cs.nl/issue/CS-419) Escaped possible empty/missing hashes in existing change log
When appending an existing change log, it is now permitted to have empty hashes. ([6fd6854f](https://github.com/cswebworks/CS.Changelog/commit/6fd6854f88c9127d3fb173d9a06f5da878f0988f))
- Corrected Markdown output, made grouping for messages and categories case insensitive ([c8d1b918](https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM/commit/c8d1b9182be679ef520838967c887b349c7c840d))
- Corrected serialization and deserialization of changelog and changesets.
Prevented commits from being logged multiple times.
Added indicators about serialization and writing to file for exporters ([552cbee8](https://github.com/cswebworks/CS.Changelog/commit/552cbee8e81f12a57ad1f97d5de04bc518051cdb))

## Documentation ##
- documented code, documented usage ([0c74a4ec](https://github.com/cswebworks/CS.Changelog/commit/0c74a4ec9169068cec57ebce1b05735e9171328e))

## Tests ##
- Added tests for changelog exporters, implemented proper JSON serialization, added draft of Xml exporter ([f09bdb29](https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM/commit/f09bdb29e7cd60594e7c68d6c71efe87d47d8d5f))
