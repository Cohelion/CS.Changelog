using CS.Changelog;
using CS.Changelog.Exporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CS.ChangelogConsole.Tests
{
	/// <summary>
	/// Tests <see cref="Parsing"/>
	/// </summary>
	[TestClass()]
	public class ParsingTests
	{
		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a feature completion.</summary>
		[TestMethod()]
		public void ParseFeatureCompletion()
		{
			//Arrange
			var logs = new []{
				@"f4451af20c0f4023d7d785a6e7f647e43bc64fa2 '2017-06-27T12:28:01+02:00'  Merge branch 'feature/updatecubes' into develop"
			};

			ParseOptions options = new ParseOptions();

			//Act & assert
			foreach (var log in logs)
				AssertChange(log, options.category_feature);
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a hotfix merge back into the development branch.</summary>
		[TestMethod()]
		public void ParseCatchup()
		{
			//Arrange
			var logs = new[]{
				@"4890379be68202cacdb4377fbf35f91d77de04f5 '2017-06-08T08:13:37+02:00'  Merge branch 'develop' into feature/ cDCM - 769_stationstatuslog"
			};

			ParseOptions options = new ParseOptions();

			//Act & assert
			foreach (var log in logs)
				AssertIgnoredCommit(log);
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a hotfix merge back into the development branch.</summary>
		[TestMethod()]
		public void ParseHotfixCatchup()
		{
			//Arrange
			var logs = new[]{
				@"bdf971617e7e576f7770401cefdb3090f89d3fcc '2017-06-23T17:21:32+02:00'  Merge branch 'hotfix/cDCM-661_Reimport_file_with_existing_file' into develop"
			};

			ParseOptions options = new ParseOptions();

			//Act & assert
			foreach (var log in logs)
				AssertIgnoredCommit(log);
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a merge from master back into the development branch (or anything else).</summary>
		[TestMethod()]
		public void ParseMasterSync()
		{
			//Arrange
			var logs = new[]{
				@"bdf971617e7e576f7770401cefdb3090f89d3fcc '2017-06-23T17:21:32+02:00' Merge branch 'master' into development"
			};

			ParseOptions options = new ParseOptions();

			//Act & assert
			foreach (var log in logs)
				AssertIgnoredCommit(log);
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a hotfix completion.</summary>
		[TestMethod()]
		public void ParseHotfixCompletion()
		{
			//Arrange
			var logs = new []{
				@"5bc8f9d0723bdb699580cabe4fbaf55bd7960545 '2017-06-23T17:21:31+02:00'  Merge branch 'hotfix/cDCM-661_Reimport_file_with_existing_file'"
			};

			ParseOptions options = new ParseOptions();

			//Act & assert
			foreach (var log in logs)
				AssertChange(log, options.category_hotfix);
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a commit message without a change log message.</summary>
		[TestMethod()]
		public void ParseIgnoredCommit()
		{
			//Arrange
			var logs = new[]{
				@"e226d3ad1c5f49caaaddf3e6e43135f092cc3c5e '2017-06-26T15:09:24+02:00' cDCM-704 - Enabled InActive selection in reporting",
				@"d0f0c6e80284de11a6b067fa0bd0e14f3a7a5f3a '2017-05-30T08:38:16+02:00'  Merge branch 'develop' into feature/ cDCM - 773_list - ignored - data - imports
# Conflicts:
#	www/_Api/Custom/Facts/Import/Post.cfm",
				@"e58b2f1b80dff8569601a2077c7a7ca9f4e4188d '2017-05-30T15:53:35+02:00'  Revert ""cDCM-712 - Remove non-required columns from excel export""
This reverts commit 3c14f373aff2dc280907c4ee3878107f0a05b527."
			};

			foreach (var log in logs)
				AssertIgnoredCommit(log);
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a commit message without a change log message.</summary>
		[TestMethod()]
		public void ParseRegularCommitWithReleaseMessage()
		{
			var expectedMessage = @"cDCM-704 - Enabled InActive selection in reporting
Some more lines containing release information regarding this commit";
			//Arrange
			var cases = new[]{
				new {log = $@"e226d3ad1c5f49caaaddf3e6e43135f092cc3c5e '2017-06-26T15:09:24+02:00' [category]{expectedMessage}"}
			};

			foreach (var @case in cases)
			{
				var entry = AssertChange(@case.log, "category");
				Assert.AreEqual(expectedMessage, entry.Message.Trim());
			}
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a commit message without a change log message.</summary>
		[TestMethod()]
		public void ParseCommitWithChangelogMessage()
		{
			//Arrange
			const string category = "GUI";
			var log = $@"e226d3ad1c5f49caaaddf3e6e43135f092cc3c5e '2017-06-26T15:09:24+02:00' [{category}] cDCM-704 - Enabled InActive selection in reporting";

			//Act & assert
			AssertChange(log, category);
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/> by parsing a merge upon pull.</summary>
		[TestMethod()]
		public void ParsePull() {
			//Arrange
			var logs = new[]{
				@"356b6f76da7cf718ecb9b3a9eddc0d8de4a8bf38 '2017-08-25T17:05:36+02:00' Merge branch 'preview' of https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM into preview"
			};

			ParseOptions options = new ParseOptions();

			//Act & assert
			foreach (var log in logs)
				AssertIgnoredCommit(log);
		}

		private static void AssertIgnoredCommit(string log)
		{
			ParseOptions options = new ParseOptions();

			//Act
			var changeset = Parsing.Parse(log, options);

			//Assert
			Assert.IsFalse(changeset.Any(), $"Log entry should lead to ignored commit. Entry: {log} caused change log message :  {(changeset.Any()?changeset.First().Message:null)} ({(changeset.Any() ? changeset.First().Hash : null)})");
		}

		private static ChangeLogMessage AssertChange(string log, string category)
		{
			//Arrange
			ParseOptions options = new ParseOptions();

			//Act
			var changeset = Parsing.Parse(log, options);

			//Assert
			Assert.AreEqual(1, changeset.Count());
			var change = changeset.Single();

			Assert.AreEqual(change.Category, category);

			return change;
		}

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/>.</summary>
		[TestMethod()]
		public void ParseTest1()
		{
			//Just a log from Swissport Cargo DCM
			const string log = @"f4451af20c0f4023d7d785a6e7f647e43bc64fa2 '2017-06-27T12:28:01+02:00'  Merge branch 'feature/updatecubes' into develop
e226d3ad1c5f49caaaddf3e6e43135f092cc3c5e '2017-06-26T15:09:24+02:00'  cDCM-704 - Enabled InActive selection in reporting
324e6f2fcdca54f24a07864a8ccf7a10717345ce '2017-06-26T11:58:57+02:00'  Merge branch 'feature/cDCM-785_pivotedWoReview_remove_cache_option' into develop
1d10dd04dfeb44f009480d704a7a64f97182b2ac '2017-06-26T09:17:48+02:00'  Merge branch 'master' into develop
e86ff8b48e7010ec8a9672cb698b3c8f3dd5ecc0 '2017-06-26T09:17:06+02:00'  updatecube from _dev
e05267ed40b2d3d4628ee9f59d5af94356e04309 '2017-06-26T09:15:51+02:00'  Merge branch 'hotfix/update_cube_from__dev' into develop
a79570b7fa39425d4baf7f2008c31569d5b610ac '2017-06-26T09:14:53+02:00'  updatecube from _dev
329094ed25877e3b1baf944473f64b36342eaadd '2017-06-25T23:15:09+02:00'  updatecube from _dev
e4e6eee36d6d54191606ab18bcd43a1d531357a8 '2017-06-23T17:46:21+02:00'  cDCM-785: pivoted WO review; replaced ""removed cached file"" with ""refresh All regions export""; new xlsx is created and copied to T-drive on prod
bdf971617e7e576f7770401cefdb3090f89d3fcc '2017-06-23T17:21:32+02:00'  Merge branch 'hotfix/cDCM-661_Reimport_file_with_existing_file' into develop
5bc8f9d0723bdb699580cabe4fbaf55bd7960545 '2017-06-23T17:21:31+02:00'  Merge branch 'hotfix/cDCM-661_Reimport_file_with_existing_file'
159504d23968c7a3c261d73bf19f7b62b11c46e8 '2017-06-23T17:20:31+02:00'  cDCM - 661 Reimport file with existing file
342a3904ad0f874e4b65a58bb4831f1d48db2e53 '2017-06-23T15:08:01+02:00'  Merge branch 'master' into develop
2ed15e0c4dd655ef00c3675b01265abba4af79c0 '2017-06-23T11:37:33+02:00'  Merge branch 'feature/cDCM-774_HitCounter' into develop
# Conflicts:
#	www/_Sys/Classes/ReportPortal/TrendReport/Report.cfc
a4d613683fe2b29b8e1245d02801ce6dea3b3696 '2017-06-23T11:28:34+02:00'  Merge branch 'feature/cDCM-773_list-ignored-data-imports' into develop
d68c6f2793469690fdd8870dac39e303df8a2ddb '2017-06-23T11:27:53+02:00'  Merge branch 'feature/cDCM-772_ParentCompany_CommercialReport' into develop
94aca3cc0b24f9c1b691d320d07f42181a19cc28 '2017-06-23T11:27:45+02:00'  Merge branch 'feature/cDCM-769_stationstatuslog' into develop
64a77a846cedbe3f3f36d3ebe06d7d1b1245661c '2017-06-23T09:44:07+02:00'  cDCM - 704 - Hide ""InActive"" from reporting section
49c0d99a023a3190b1f81160a31a155258078962 '2017-06-23T09:31:10+02:00'  Merge branch 'develop' into feature/ cDCM - 704_Disregard_station_IsActive
0ebc6abcb29682a60fb5100fabf93c4029f1c84e '2017-06-22T10:42:01+02:00'  Merge branch 'hotfix/Feed_import_-_Mapping_-_Setting_IsUsed_Fix' into develop
299cc158f23208ca3b75b26ecafde07a867b0170 '2017-06-22T10:42:00+02:00'  Merge branch 'hotfix/Feed_import_-_Mapping_-_Setting_IsUsed_Fix'
dec41f9305ec80ab54612464d061d9ca9cc00556 '2017-06-22T10:41:49+02:00'  Feed import: Mapping: Fix for setting IsUsed -limit to one systemlevel
8db5f093112f6d85f44e4fad47b32d2ca9cef50d '2017-06-21T14:37:19+02:00'  cDCM - 785: Pivoted WO Review, add option ""remove cached file""
0dcf4bd64655aca41e58e9949dd2e1dffe86519a '2017-06-21T14:28:45+02:00'  Merge branch 'hotfix/cDCM-787_Feed_import_tweak' into develop
7b1fd1d640835e70d5e160f32829b75742afd50a '2017-06-21T14:28:45+02:00'  Merge branch 'hotfix/cDCM-787_Feed_import_tweak'
2ea0be71dd52d02c41a2669146a68c89ac385732 '2017-06-21T14:27:13+02:00'  cDCM - 787 Feed import tweak
f717c6d7ab27101174c28fb754ed1ab64f3201e1 '2017-06-20T16:06:53+02:00'  Merge branch 'hotfix/cDCM-787_Inactive_state_for_mapping' into develop
ff43cd916b297e6141e973785c24b6acff4ec930 '2017-06-20T16:06:52+02:00'  Merge branch 'hotfix/cDCM-787_Inactive_state_for_mapping'
bdc3fa18bfd0f4b547819ef72c40db36033f78a9 '2017-06-20T16:06:39+02:00'  cDCM - 787 Stricter member notification
71d555a97ec48b92e28a36db78413066591669c9 '2017-06-20T15:14:34+02:00'  Merge branch 'hotfix/cDCM-787_Inactive_state_for_mapping' into develop
4b42ce6b6105bcd3cb0b4e70b8c6aa88869b0c97 '2017-06-20T15:13:13+02:00'  cDCM - 787 Feed import: Improve messages, use inactive state
e21ec21581d8492502b5e3cb9b01afb619c0800e '2017-06-20T15:12:56+02:00'  cDCM - 787 Add inactive state to mapping cache
f2b1b55f0e4967d226e21039f6d4892a8b77498c '2017-06-19T15:16:06+02:00'  Re - added development build profile
fa04848c14b284e36bd5ac3cf83541b7c7c5ee23 '2017-06-19T15:10:49+02:00'  Added NPM install steps for Angular2 Prod and Preview build configs
a2ab0d30c06667ee27bd091a9c1782fd4410ce2b '2017-06-19T13:51:47+02:00'  cDCM - 773: enabled wordwrap in modal window
c74c2ece01fec026e6942b0ed710e68944f49b4a '2017-06-14T11:16:17+02:00'  Merge branch 'hotfix/trendreport_legenda_aanpassing' into develop
2405206dd12b210d321b8eb5820be72ead529fa1 '2017-06-14T11:16:16+02:00'  Merge branch 'hotfix/trendreport_legenda_aanpassing'
c4c56838fc7751a978829d06f739cae8ced588be '2017-06-14T11:11:09+02:00'  trendreport legenda aanpassing
775eb7340040667121c6405a3163c517d7064b20 '2017-06-13T16:54:50+02:00'  Merge branch 'master' into develop
51bd2950f7bc1f22911dd87c6494fd283048f990 '2017-06-13T15:38:45+02:00'  Merge branch 'hotfix/cDCM-763'
3dbd81c6b8274e110f77a1bf930325620b02a0df '2017-06-13T15:38:45+02:00'  Merge branch 'hotfix/cDCM-763' into develop
0ff19d89a53f6cf1bad99a95fa79c3478882d831 '2017-06-13T15:37:50+02:00'  cDCM - 763: Support using actuals in warehouse statistics over other designations
3040f858ef95cf1006c4363bad71965ff276ac40 '2017-06-13T15:25:28+02:00'  cDCM - 763: Support using actuals in warehouse statistics over other designations
cb0a3caca91c14ae71fbf7ffc70d626515b333e9 '2017-06-13T14:02:59+02:00'  Merge branch 'hotfix/budget_disable_edit_before_roundstart' into develop
36c4053c1c4cc5a934a6766f78d3a7867a4c66ef '2017-06-13T14:02:58+02:00'  Merge branch 'hotfix/budget_disable_edit_before_roundstart'
bb4867dd75967bab520f86f933476aa59b22ea75 '2017-06-13T13:59:38+02:00'  fix: budgets not editable before start of round 1
d3acfd030ef5e3abcb918019c0627ebd9b25ae1e '2017-06-13T13:50:29+02:00'  Merge branch 'hotfix/cDCM-783_Keep_cursor_after_savin_hours_in_actuals_data_entry' into develop
a92880f418f3ceb3d8d0763e4b5b0a4f176c33e7 '2017-06-13T13:50:26+02:00'  Merge branch 'hotfix/cDCM-783_Keep_cursor_after_savin_hours_in_actuals_data_entry'
7fa29b538d0487092b6022e06de09e7e25edd068 '2017-06-13T13:44:07+02:00'  Fix losing focus after refresh of data
e9e4eaf9c4666d78723eb5331fdf7bdebdfd049b '2017-06-12T18:10:51+02:00'  Fix data entrygrid
4fcd22503e43ae5119d192cf1d99ee24a7e81d84 '2017-06-09T11:16:42+02:00'  Merge branch 'hotfix/feed_AE_disable_MDMfallback' into develop
f41648471107a0bd02f823686e1bb29cadc5b6a3 '2017-06-09T11:16:41+02:00'  Merge branch 'hotfix/feed_AE_disable_MDMfallback'
d365fe2b5104d638115bf700801f0eda79c951d6 '2017-06-09T11:16:32+02:00'  Removed MDM fall back from feed AE
ef3ba409e0a165bacb77f8628dfa5a0fc2ad845d '2017-06-08T16:02:45+02:00'  Merge branch 'hotfix/dataentry_timetillupdate' into develop
8daed19a43334f59be30b3cf8f81349350fa118f '2017-06-08T16:02:45+02:00'  Merge branch 'hotfix/dataentry_timetillupdate'
bbe1420b285e0bf775c674eb4d449fe7eb2e10f1 '2017-06-08T16:02:36+02:00'  re - enabled data entry for current month (disabled for previous months)
300b8a6315a47dbaea513cde93b876b9b69d147b '2017-06-08T12:43:29+02:00'  cDCM - 773: show XLS import warnings in Modal window
e35a49683eddbe16abc1d06a1e759319f68b6ca4 '2017-06-08T12:34:04+02:00'  Merge branch 'hotfix/feed_ae_exceptionduringimport' into develop
a7eece905ac6cae5ac1c73e7b95f58dea7ad2053 '2017-06-08T12:34:03+02:00'  Merge branch 'hotfix/feed_ae_exceptionduringimport'
1be1c5209daf91555feb46154392bd94b94ca52b '2017-06-08T12:33:18+02:00'  AE transform: fix for proper try-catch handling
201df71218163a9e2377c4df5235807e428b785c '2017-06-08T09:07:54+02:00'  cDCM - 769: update station status also after approve changes
4890379be68202cacdb4377fbf35f91d77de04f5 '2017-06-08T08:13:37+02:00'  Merge branch 'develop' into feature/ cDCM - 769_stationstatuslog
f7601d8ea4cf181df3185cb7d2eeda2bf4fdf843 '2017-06-07T10:42:34+02:00'  Merge branch 'hotfix/feed_headerissues' into develop
2892ceae1aea1d59296815faa5df32966dca7ba5 '2017-06-07T10:42:34+02:00'  Merge branch 'hotfix/feed_headerissues'
913ae89bb2e68ef197b756fd72b118fd50c00af1 '2017-06-07T10:41:59+02:00'  for feedimport fixed reference to first data row
2bf3b7f583e33561074ed578cbcb6d236de54b8a '2017-06-02T11:11:55+02:00'  Merge branch 'hotfix/deeplinking_not_working_on_actuals_state_grid' into develop
0127a60366ae4003d2c5965fc6125f81ee670af8 '2017-06-02T11:11:54+02:00'  Merge branch 'hotfix/deeplinking_not_working_on_actuals_state_grid'
040c0c17d61cfdd3b673df715531edd1c4fac62e '2017-06-02T11:11:21+02:00'  Added parameter IdStation for angular deeplinking purposes
f4790c8250f563802c722c75dfa4e3c0952ebb63 '2017-06-02T11:06:55+02:00'  Merge branch 'hotfix/emailheaderimg_codecleanup' into develop
a1bad3ead24b3f2f92c72d40488810307480fe75 '2017-06-02T11:06:55+02:00'  Merge branch 'hotfix/emailheaderimg_codecleanup'
224c485763edf3f35dc8176ea6cd90f9b2f56f63 '2017-06-02T11:06:29+02:00'  moved header image url to initapplication.cfc
e8f91e3741dc70902625622e0662320c69437198 '2017-06-01T11:36:30+02:00'  Merge branch 'hotfix/undo_testje' into develop
ccbcfa2ea1549122635726e66df344df3c6889fd '2017-06-01T11:36:30+02:00'  Merge branch 'hotfix/undo_testje'
5ac731e3f836c4181aaa67921743604df2e55a75 '2017-06-01T11:35:57+02:00'  undo test
7d6eb20e9b96bee9a33631f669788e44b0e99409 '2017-06-01T11:13:44+02:00'  Merge branch 'hotfix/Uitlijning' into develop
604e8cee2aed2db9dbc10e881e2ce58fd848404d '2017-06-01T11:13:44+02:00'  Merge branch 'hotfix/Uitlijning'
0468578fc722b91a1bca8fb86a573f3d34e96e1b '2017-06-01T11:12:11+02:00'  hoi
55b1bc58d01e7a618036217bf77528347effaa46 '2017-06-01T11:05:39+02:00'  Merge branch 'feature/updatecubes' into develop
c048352001470379a6b6b1b90541aeabcf628966 '2017-06-01T11:02:02+02:00'  express wijzigingen gemaakt
52656a1c052beff9658cf4bc6d1c46cb2db0b5a9 '2017-06-01T10:54:00+02:00'  refreshed met nieuwe code
10868b31a20bb7d6ef623f98698351409ba2ec64 '2017-05-31T11:13:45+02:00'  Merge branch 'hotfix/cDCM-777_Initially_selected_station_must_be_active'
006438774f6e07dae36c780ada6cadf6ba35c47b '2017-05-31T11:13:45+02:00'  Merge branch 'hotfix/cDCM-777_Initially_selected_station_must_be_active' into develop
8dadfe9215fdbf129bd06dde7f14dc2234c485de '2017-05-31T11:10:31+02:00'  cDCM - 777 Initially selected station must be active
f5f142ce107a9f45f4f8f7d6930fda4911b38245 '2017-05-31T08:26:00+02:00'  set default Parent Company to Carrier label
bc38cd09d81430c8652975ecd4e3f858f1757501 '2017-05-30T16:18:34+02:00'  cDCM - 774 - Hit counter fix for all reports; ignore hit count for scheduled jobs; code refactoring
df190d9395b3c7188dc23f185b6faf2d31ff1542 '2017-05-30T16:02:22+02:00'  Merge branch 'hotfix/rk_cube_improvements' into develop
606790b1101313f0be363deb60fdea3d854b3b7c '2017-05-30T16:02:21+02:00'  Merge branch 'hotfix/rk_cube_improvements'
3a68e259439bfa2b1caf4cddc1d5e1a7b98a7b6d '2017-05-30T16:00:40+02:00'  cube improvement(request Roeland)
028295170cb98d782bcd090db1bebb0d4cce4375 '2017-05-30T15:54:10+02:00'  Merge branch 'hotfix/cDCM-703_undoEarlyMerge' into develop
f5b992878f902a2600c7563610cc96d7066789d8 '2017-05-30T15:54:09+02:00'  Merge branch 'hotfix/cDCM-703_undoEarlyMerge'
e58b2f1b80dff8569601a2077c7a7ca9f4e4188d '2017-05-30T15:53:35+02:00'  Revert ""cDCM-712 - Remove non-required columns from excel export""
This reverts commit 3c14f373aff2dc280907c4ee3878107f0a05b527.
aea43723bd88d0813c56b08a5fedc8b6c12dda34 '2017-05-30T15:48:27+02:00'  cDCM - 703 - Undo changes from Dev, QA
6108150637d945da32664f79fd395cf713fc036d '2017-05-30T14:06:20+02:00'  Merge branch 'hotfix/cDCM-773_trendreport_onepage' into develop
1d446c5f7dd9351801c9e3f5bc2445ade4575f11 '2017-05-30T14:06:19+02:00'  Merge branch 'hotfix/cDCM-773_trendreport_onepage'
2f874d132aaccb684ef8a8467685d0bde7418c43 '2017-05-30T14:06:05+02:00'  cDCM - 773: fixed column widths +forced PDF on one page
2774e57c02160b26616c9d8a9f87fffb26a758bd '2017-05-30T12:27:56+02:00'  cDCM - 773: Changed remote location of Test DB to db3
a4764ce78eca79f79df580d16512b7ca9fbaeb43 '2017-05-30T11:13:19+02:00'  cDCM - 772: fixed query in ParentCompanyProvider
535206167efc8e4e5647015d0ac59a67a8dfbc13 '2017-05-30T09:29:11+02:00'  Merge branch 'hotfix/MDM_sync_fix' into develop
7681ebc4f648d4bf492c5d4bca21d7c862af1980 '2017-05-30T09:29:11+02:00'  Merge branch 'hotfix/MDM_sync_fix'
c7cc5a08d7c99a800efe0c318a1fce8edcbd9507 '2017-05-30T09:28:19+02:00'  MDM_sync_fix: removed non-existing function
d0f0c6e80284de11a6b067fa0bd0e14f3a7a5f3a '2017-05-30T08:38:16+02:00'  Merge branch 'develop' into feature/ cDCM - 773_list - ignored - data - imports
# Conflicts:
#	www/_Api/Custom/Facts/Import/Post.cfm
4a93ef688de3edf2878f285983a0d936224159e4 '2017-05-29T17:21:53+02:00'  DCM merge
242cac4a6528d932b56fb6db644a5e5d30c58b66 '2017-05-26T17:52:35+02:00'  cDCM - 773: added NoAccess, DesignationAccess, LevelRelationConflict &SnapshotOverride to Feed Import event log details
5f6ee7bd6265624c2d871d2ea191985d6dad31c3 '2017-05-26T15:24:34+02:00'  cDCM-773: added visual warning for ignored data during actuals xls import";

			var changeset = Parsing.Parse(log, new ParseOptions());

			Assert.IsTrue(changeset.Any());

			IChangelogExporter e = new TraceChangelogExporter();
			e.Export(changeset,null);
		}

		//Just a log from Swissport Cargo DCM
		internal const string logParseTest2 = @"7ba108c90a435353c909ad71b9d125de7971c251 '2017-09-07T14:55:52+02:00' Merge branch 'feature/cDCM-811_BudgetPrep_Finance_HQ' into preview
65f5d69e6802403bd77c56b8b6d0ba2f5779d1ae '2017-09-07T14:51:45+02:00' cDCM-811 - Budget prep report - Ramp/Overhead tabs renamed
d6c103879bd984e56de4c8a689da3a8cd153e6a5 '2017-09-07T11:06:24+02:00' Merge branch 'feature/cDCM-811_BudgetPrep_Finance_HQ' into preview
55de56a96971754e7edf0332e7c89f922360ce05 '2017-09-06T18:14:26+02:00' cDCM-811 - Ramp & HQ hours values as real numbers instead of percentages
f6f47287d5412fd6ca0c2065779bfd734f27acc5 '2017-09-01T17:59:25+02:00' Merge branch 'feature/CSPM_New_Interface' into preview
533cc239ac00b0de1bec2f3d46c8ad38a7e68a92 '2017-09-01T17:58:58+02:00' Merge branch 'feature/CSPM_New_Interface' of https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM into feature/CSPM_New_Interface
3fffb5f2bdaacc589572025422ec9c443a256337 '2017-09-01T17:58:52+02:00' added rules
c97ab21f8772a933927e3df04a8cd9f039f6515d '2017-09-01T17:28:02+02:00' Merge branch 'feature/CSPM_New_Interface' of https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM into feature/CSPM_New_Interface
6731b28d5cb980dfb85f29818d6a34bcff990b83 '2017-09-01T17:27:35+02:00' Dataevents
f68cf16cba2a573cb70f77e0d7a4d1528b2f592b '2017-09-01T16:51:43+02:00' Updated description
045c26ac5adf596ea6d2518e443c862c9140dd21 '2017-09-01T15:42:22+02:00' added celltype to the rule
ea6a3759c1d6176b07e7faee7f3e984c3006d33e '2017-09-01T15:26:38+02:00' Merge branch 'feature/cDCM-811_BudgetPrep_Finance_HQ' into preview
de1d2e7d22097eb37a649fc10c9b558714d48451 '2017-09-01T15:20:14+02:00' cDCM-811 - Budget Preparation & Finance report adjustments
2e570c495a71855921b7e4c6b0dcf80a66cf1fac '2017-09-01T12:24:13+02:00' Rest Api: made error message more descriptive
952789d40086ed6bc08ab265ff09192e750457f5 '2017-09-01T12:03:07+02:00' Fix HoursMonth rules
84d63cf644e4566ca18ae78667c64c18970a2851 '2017-09-01T11:30:14+02:00' added entry rules for hours
0621ee23a8cd98e3c10fc7b9060b9ed811410bf1 '2017-09-01T11:29:58+02:00' Remove de canedit style rule
88b9a173eb6bf72cc5e05b937bdc327ac7be1763 '2017-09-01T11:29:23+02:00' Rest Api: add application name in swagger
146f80c0c3a54c3b5655e940085267259395328b '2017-08-31T11:43:22+02:00' Rest Api Reporting: added reporting tabs names
6f437da1fe86ed5f24347841685d04281c04829e '2017-08-31T09:50:33+02:00' added didplay label for the columns
df1e12f9d29dd7411d5238d621ae1578a3d34da6 '2017-08-30T17:40:47+02:00' Merge branch 'feature/CSPM_New_Interface' into preview
757ed5fd48965ecdc3f26938230c6d0f4bdf4fe1 '2017-08-30T17:24:42+02:00' added comparisons to Cargo
fc1e32e4d262a8ba8018af7389622203fc5ccbf4 '2017-08-30T16:41:36+02:00' Merge branch 'feature/CSPM_New_Interface' into preview
7c7f25bd4e9b3e3ca402f75a8114e027db1a53c0 '2017-08-30T16:40:34+02:00' Swagger generator: copy from GH
be305d0337bb97e24be538bc478fb8a90132f918 '2017-08-30T16:40:11+02:00' Rest Api: Sync changes from GH. Removed typed arrays that contain objects which contain objects
398cd11df36e3f458c56282b5bcae7bd340da2a7 '2017-08-30T14:27:41+02:00' Merge branch 'feature/DCM-1421_Datepicker_fix' into preview
b740f569d2d96ec17a5d2773c65342d32aab86b7 '2017-08-30T14:25:44+02:00' DCM-1421 - Datepicker fix for new & old interface
1e6548f9e864b424a89ff0b266c7cfe1203b4eac '2017-08-30T12:30:33+02:00' Messaging data entry grids (old and new UI)
ff8f3f289f7fb530536f8848ee6e359c48a45ede '2017-08-30T09:58:24+02:00' disable buttons when actuals
d5ff50f2e0a437a1df5055f06a9aad00b2faf711 '2017-08-30T09:58:05+02:00' remove 2017 header and disable buttons when actuals
ad30fcc098e6a38eec132faceb8851859e9d8c05 '2017-08-29T11:06:35+02:00' Enable outlooks for editing
c21b709edab19923c933b58356097a7ef693ce17 '2017-08-29T08:59:44+02:00' Merge branch 'feature/CSPM_New_Interface' into preview
0af1519041726f345f6578029db6970574ae40f0 '2017-08-29T08:59:17+02:00' removing group
08a9eca5a7040ab2ccb52653b74b5c49b6ca1b88 '2017-08-28T17:04:02+02:00' Merge branch 'feature/CSPM_New_Interface' into preview
9303c5bfe1f8d16ba77e393453f94637c585d048 '2017-08-28T16:03:17+02:00' refactor client config
cd0c6c991bbbe854d6394171d71fc4c19e1f5c2f '2017-08-28T16:02:49+02:00' added popover edit on awb
fefab192d952883974194ca25a0b9dd694617759 '2017-08-28T16:02:32+02:00' added Edit on hours month
99065b57bb597a93075bc4ccfcb304fcf9e4618c '2017-08-25T17:50:59+02:00' Merge branch 'feature/CSPM_New_Interface' of https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM into feature/CSPM_New_Interface
af1fb74b915429235b00b0c72093d0a4ec5c7f54 '2017-08-25T17:50:52+02:00' Rest Api: sync with GH
80e49928b409cbcbff22bc6d82c9e79dec1f8ef2 '2017-08-25T17:09:17+02:00' Merge branch 'feature/CSPM_New_Interface' into preview
356b6f76da7cf718ecb9b3a9eddc0d8de4a8bf38 '2017-08-25T17:05:36+02:00' Merge branch 'preview' of https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM into preview
57e91d7d4929ac5c687a9e55432e83d15feb556d '2017-08-25T16:53:41+02:00' Calendar debug
b3a4fcf0f9a4afd0e8812ff8df7c3f6306f8c48f '2017-08-25T16:07:52+02:00' Rest Api: fix excel export returntype
7c440ef3f89369a97146f0d6a9bcd5cf635aabb0 '2017-08-25T15:13:40+02:00' Merge branch 'hotfix/cDCM-810_Pivoted_CargoWO_Report_on_Server' into develop
53d03a27a868f50f661a94719fd3292f862d17f6 '2017-08-25T14:44:31+02:00' add legacyKeys when List for month
8f25f7f6fa616443028b443a344ea3682adeda6b '2017-08-25T12:10:46+02:00' added rules for edit styles
5672a5bb9e226c42c1b48951fd9217edf38a5ca3 '2017-08-23T09:01:18+02:00' Removed deletion of rest service in onResetApplication
6627b82c068efbe70befcb7d39ee6b58ae6a8293 '2017-08-23T08:59:34+02:00' Added http statuscodes to restapiutil/reset.cfm; fix init of rest service in onStartApplication
b0e61e22bb22e1403ec61e92c7fc82fe3d9b19b5 '2017-08-22T15:28:22+02:00' Added RestApiUtils to _Dev
2da2da6fb46d2aa5ac337a0719c1ac63eed7e891 '2017-08-22T15:23:44+02:00' CSPM-82: import fixes
21ab6fd779e8b0b6fdf31fa3be3b17312d0b033e '2017-08-22T14:57:57+02:00' CSPM-82: used import for Model references in services
ba70db231b342d40a12c17800aae290b5dbd7560 '2017-08-21T14:56:23+02:00' Merge branch 'develop' of https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM into develop
8882b534495966c62f60893350bf1a8277e27549 '2017-08-21T10:49:05+02:00' Merge branch 'hotfix/DCM-1413_Budget_figures_not_showing_in_CBT' into develop
391ecb8a2404d974abf89519b80b9b76201595ca '2017-08-21T08:21:44+02:00' Check-in _DataEntryState changes (Roeland)
6046b583b705757c9857aa45e26092159097c523 '2017-08-18T16:30:30+02:00' Merge branch 'hotfix/DCMDataExportReport_-_fixes' into develop
c468afd16bf4a0e3b76bd1ec45144090e2dba44a '2017-08-18T16:21:54+02:00' Merge branch 'develop' of https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM into develop
d89436db046b92fe4cb697eab7878b22e5208b1f '2017-08-18T16:21:10+02:00' Merge branch 'hotfix/fix_scheduledpdf' into develop
339cc40ba28893f21257c9588319888c2c082e16 '2017-08-18T15:56:33+02:00' Merge branch 'hotfix/BudgetPrepReport_-_Remove_paid_hour_columns' into develop
bd35e9d6d04f2390d1e39c2822cc9db2963d0d19 '2017-08-18T13:22:51+02:00' Merge branch 'hotfix/cDCM-809_Productivity_Target_report' into develop
b4c806a11c38ff4d9dc1902c414f06aaadb7ac15 '2017-08-18T12:39:48+02:00' Merge branch 'develop' of https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM into develop
c06fefbb7e6572d10c20553fae839b8224ec1c1f '2017-08-18T12:36:09+02:00' added compare rules to base config
42c6533861c4124bbee3da64169fe612ceea53e5 '2017-08-18T12:35:26+02:00' Merge branch 'hotfix/Datepicker_Fix' into develop
dca7204e1990b7a57b4c57fc88006ae91aec832a '2017-08-18T09:02:49+02:00' Merge branch 'master' into develop
a133564341e047536e2c0b341a730cd07a07f68f '2017-08-17T18:52:57+02:00' Merge branch 'hotfix/DCM-1410_Budget_DateRange_2018' into develop
0ce6776f9fa66e46babcc66f576c37c0ba89094b '2017-08-17T13:47:44+02:00' Merge branch 'release/v20170804_Operation_High_Ground' into develop
b865471bb4ef22965b53e1348f155be50196138d '2017-08-17T12:44:35+02:00' Merge branch 'hotfix/fix_hours_indicators_on_landingpage' into develop
634a095cbb209a7175ed41215350bd0ca536b32a '2017-08-17T12:00:41+02:00' added injected column for add carrier
2e8a61306a6c34f88ff78005f6ff90d3eed8eb87 '2017-08-17T11:21:22+02:00' Merge branch 'hotfix/fix_data_entry_state_hours_for_2018' into develop
e4bc117a8753e29082540a3ceb2a7caea9c9282b '2017-07-27T12:37:30+02:00' Merge branch 'feature/CSPM-15_Swagger_API_Docs' into preview
943d173d74d932df4787750f16baeb5ff56df062 '2017-07-26T18:38:21+02:00' Merge branch 'feature/CSPM-15_Swagger_API_Docs' into preview
340f18881f7bc9f0d6f53c240869c09c74924dd1 '2017-07-26T09:51:18+02:00' Merge branch 'feature/CSPM-15_Swagger_API_Docs' into preview
4ee91a5c7eba89a4900e140d7d42f6188d80cb2d '2017-07-25T13:40:52+02:00' Merge branch 'feature/CSPM-15_Swagger_API_Docs' into preview
bf28f0f16efb71da135411a1d8557e9c64289055 '2017-07-25T13:05:59+02:00' Merge branch 'develop' into preview
# Conflicts:
#       www/App/Home/_DataEntryState.cfm
326144666df8822a79d6b9931fd53587e077ab43 '2017-07-21T11:06:49+02:00' Merge branch 'develop' into preview
dfe91a0ce978df37461de0430ed790d1f7073bc5 '2017-07-19T12:29:49+02:00' Merge branch 'develop' into preview
0100d294b1700842bd384f9eb5e767bb577f5a86 '2017-07-18T12:25:36+02:00' Merge branch 'feature/cDCM-798_Weekly_scheduled_email_PDF' into preview
5339153a2bbc41e19883f2c8ace3c2e5f6b9d09d '2017-07-16T12:58:45+02:00' Merge branch 'feature/CSPM-15_Swagger_API_Docs' into preview
f42120d9b82fd6bd33479aeffd5a252749ebfdef '2017-07-16T12:27:53+02:00' Merge branch 'feature/CSPM-15_Swagger_API_Docs' into preview
3762cc869010dc2dfbbbf5b3724ff164338de06d '2017-07-12T12:29:17+02:00' Merge branch 'feature/DCM-1367_Station_code_consistency' into preview
6f3cad976a39af2ed652f4957bf580376271d688 '2017-07-12T12:29:11+02:00' Merge branch 'develop' into preview
1d9b3be59d0df38bd1f237b69bf86bdae2be2ad5 '2017-07-12T09:13:52+02:00' Merge branch 'feature/CSPM-15_Swagger_API_Docs' into preview";

		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/>.</summary>
		[TestMethod()]
		public void ParseTest2()
		{	
			var changeset = Parsing.Parse(logParseTest2, new ParseOptions());

			Assert.IsTrue(changeset.Any());

			IChangelogExporter e = new TraceChangelogExporter();
			e.Export(changeset, null);
		}
	}
}