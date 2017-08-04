using CS.Changelog;
using CS.Changelog.Console;
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
		/// <summary>Tests <see cref="Parsing.Parse(string, ParseOptions)"/>.</summary>
		[TestMethod()]
		public void ParseTest()
		{
			//Just a log from Swissport Cargo DCM
			const string log = @" f4451af20c0f4023d7d785a6e7f647e43bc64fa2 '2017-06-27T12:28:01+02:00'  Merge branch 'feature/updatecubes' into develop
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
		}
	}
}