using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using VRTK;

public class eli_LabyrinthController : MonoBehaviour {

	//public jared_wallController WallController;
	private string Binary = "";
	private string ThingToSave1 = "";
	public LabData Lab;
	
	public List<GameObject> WallObjects;
	public List<Collider> QuadrantColliders;
	
	// Use this for initialization 
	void Awake () {
		if(Binary == ""){
			Lab = new LabData();
			Lab.saved = ThingToSave1;
			
			GenerateMaze();
			
			Save();
		} else {
			Load();
		}
	}
/*
Circle
AAEAAAD/////AQAAAAAAAAAMAgAAAA9Bc3NlbWJseS1DU2hhcnAFAQAAAAdMYWJEYXRhBAAAAAtjdXJyZW50Q2VsbAxjdXJyZW50V2FsbHMFc2F2ZWQFQ2VsbHMEBwEDBENlbGwCAAAACHFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW0NlbGwsIEFzc2VtYmx5LUNTaGFycCwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQIAAAAJAwAAAAkEAAAABgUAAAAGQ2lyY2xlCQYAAAAFAwAAAARDZWxsBQAAAARzZWxmBHR5cGUJcXVhZHJhbnRzBXdhbGxzCW5laWdoYm9ycwAAAwcDCAN+U3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tTeXN0ZW0uSW50MzIsIG1zY29ybGliLCBWZXJzaW9uPTIuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dCH5TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249Mi4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0CAAAAAAAAADYJBwAAAAkIAAAACQkAAAAPBAAAABAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQGAAAAcVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbQ2VsbCwgQXNzZW1ibHktQ1NoYXJwLCBWZXJzaW9uPTAuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24EAAAGQ2VsbFtdAgAAAAgICQoAAAAIAAAACAAAAAQHAAAAflN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLkludDMyLCBtc2NvcmxpYiwgVmVyc2lvbj0yLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQMAAAAGX2l0ZW1zBV9zaXplCF92ZXJzaW9uBwAACAgICQsAAAABAAAAAQAAAA8IAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAA/v///wAAAAAAAAAAAAAAAAEAAAAAAAAAAQkAAAAHAAAACQwAAAAQAAAAEAAAAAcKAAAAAAEAAAAIAAAABARDZWxsAgAAAAkDAAAACQ0AAAAJDgAAAAkPAAAACRAAAAAJEQAAAAkSAAAACRMAAAAPCwAAAAQAAAAIBgAAAAAAAAAAAAAAAAAAAA8MAAAAEAAAAAj///////////////////////////////////////////////8BAAAA/////////////////////wcAAAD/////AQ0AAAADAAAAAQAAADMJFAAAAAkVAAAACRYAAAABDgAAAAMAAAACAAAAMAkXAAAACRgAAAAJGQAAAAEPAAAAAwAAAAMAAAAxCRoAAAAJGwAAAAkcAAAAARAAAAADAAAABAAAADIJHQAAAAkeAAAACR8AAAABEQAAAAMAAAAFAAAANQkgAAAACSEAAAAJIgAAAAESAAAAAwAAAAYAAAA4CSMAAAAJJAAAAAklAAAAARMAAAADAAAABwAAADcJJgAAAAknAAAACSgAAAABFAAAAAcAAAAJKQAAAAEAAAABAAAADxUAAAAQAAAACAAAAAAAAAAAAQAAAP7///8AAAAAAAAAAAAAAAD/////AAAAAAEAAAD+////AAAAAAAAAAAAAAAAAAAAAAAAAAABFgAAAAcAAAAJKgAAABAAAAAQAAAAARcAAAAHAAAACSsAAAABAAAAAQAAAA8YAAAAEAAAAAgBAAAAAAAAAAEAAAD+////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAARkAAAAHAAAACSwAAAAQAAAAEAAAAAEaAAAABwAAAAktAAAAAQAAAAEAAAAPGwAAABAAAAAIAQAAAAEAAAAAAAAA/v/////////+////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEcAAAABwAAAAkuAAAAEAAAABAAAAABHQAAAAcAAAAJLwAAAAEAAAABAAAADx4AAAAQAAAACAAAAAABAAAAAAAAAAAAAAAAAAAA/v///wEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABHwAAAAcAAAAJMAAAABAAAAAQAAAAASAAAAAHAAAACTEAAAABAAAAAQAAAA8hAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAP7///8BAAAAAAAAAP////8AAAAAAAAAAAAAAAD+////AQAAAAAAAAAAAAAAASIAAAAHAAAACTIAAAAQAAAAEAAAAAEjAAAABwAAAAkzAAAAAQAAAAEAAAAPJAAAABAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/v///wEAAAAAAAAAAQAAAAElAAAABwAAAAk0AAAAEAAAABAAAAABJgAAAAcAAAAJNQAAAAEAAAABAAAADycAAAAQAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+//////////7///8AAAAAAQAAAAEAAAABKAAAAAcAAAAJNgAAABAAAAAQAAAADykAAAAEAAAACAMAAAAAAAAAAAAAAAAAAAAPKgAAABAAAAAI//////////8CAAAA////////////////////////////////AAAAAP///////////////////////////////w8rAAAABAAAAAgAAAAAAAAAAAAAAAAAAAAADywAAAAQAAAACAMAAAD/////AQAAAP////////////////////////////////////////////////////////////////////8PLQAAAAQAAAAIAQAAAAAAAAAAAAAAAAAAAA8uAAAAEAAAAAgCAAAABAAAAP//////////////////////////////////////////////////////////////////////////Dy8AAAAEAAAACAIAAAAAAAAAAAAAAAAAAAAPMAAAABAAAAAI/////wMAAAD/////////////////////BQAAAP///////////////////////////////////////////////w8xAAAABAAAAAgFAAAAAAAAAAAAAAAAAAAADzIAAAAQAAAACP///////////////////////////////wQAAAD///////////////////////////////8GAAAA//////////8PMwAAAAQAAAAICAAAAAAAAAAAAAAAAAAAAA80AAAAEAAAAAj/////////////////////////////////////////////////////////////////////BQAAAP////8HAAAADzUAAAAEAAAACAcAAAAAAAAAAAAAAAAAAAAPNgAAABAAAAAI//////////////////////////////////////////////////////////////////////////8AAAAABgAAAAs=

Cross
AAEAAAD/////AQAAAAAAAAAMAgAAAA9Bc3NlbWJseS1DU2hhcnAFAQAAAAdMYWJEYXRhBAAAAAtjdXJyZW50Q2VsbAxjdXJyZW50V2FsbHMFc2F2ZWQFQ2VsbHMEBwEDBENlbGwCAAAACHFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW0NlbGwsIEFzc2VtYmx5LUNTaGFycCwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQIAAAAJAwAAAAkEAAAABgUAAAAFQ3Jvc3MJBgAAAAUDAAAABENlbGwFAAAABHNlbGYEdHlwZQlxdWFkcmFudHMFd2FsbHMJbmVpZ2hib3JzAAADBwMIA35TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249Mi4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0IflN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLkludDMyLCBtc2NvcmxpYiwgVmVyc2lvbj0yLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQIAAAAAAAAANgkHAAAACQgAAAAJCQAAAA8EAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAYAAABxU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tDZWxsLCBBc3NlbWJseS1DU2hhcnAsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAAAZDZWxsW10CAAAACAgJCgAAAAcAAAAHAAAABAcAAAB+U3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tTeXN0ZW0uSW50MzIsIG1zY29ybGliLCBWZXJzaW9uPTIuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24HAAAICAgJCwAAAAEAAAABAAAADwgAAAAQAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAD+////AAAAAAAAAAAAAAAAAQAAAAAAAAABCQAAAAcAAAAJDAAAABAAAAAQAAAABwoAAAAAAQAAAAgAAAAEBENlbGwCAAAACQMAAAAJDQAAAAkOAAAACQ8AAAAJEAAAAAkRAAAACRIAAAAKDwsAAAAEAAAACAYAAAAAAAAAAAAAAAAAAAAPDAAAABAAAAAI////////////////////////////////////////////////AQAAAP////////////////////8EAAAA/////wENAAAAAwAAAAEAAAAzCRMAAAAJFAAAAAkVAAAAAQ4AAAADAAAAAgAAADQJFgAAAAkXAAAACRgAAAABDwAAAAMAAAADAAAANQkZAAAACRoAAAAJGwAAAAEQAAAAAwAAAAQAAAA3CRwAAAAJHQAAAAkeAAAAAREAAAADAAAABQAAADQJHwAAAAkgAAAACSEAAAABEgAAAAMAAAAGAAAAMQkiAAAACSMAAAAJJAAAAAETAAAABwAAAAklAAAAAQAAAAEAAAAPFAAAABAAAAAIAAAAAAAAAAD//////v///wAAAAAAAAAAAAAAAAEAAAAAAAAAAQAAAP7///8AAAAAAAAAAAAAAAAAAAAAAAAAAAEVAAAABwAAAAkmAAAAEAAAABAAAAABFgAAAAcAAAAJJwAAAAEAAAABAAAADxcAAAAQAAAACAAAAAAAAAAAAAAAAP7//////////v///wAAAAABAAAAAQAAAAAAAAD+//////////7///8AAAAAAAAAAAAAAAABGAAAAAcAAAAJKAAAABAAAAAQAAAAARkAAAAHAAAACSkAAAABAAAAAQAAAA8aAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAP7/////////AAAAAAEAAAAAAAAAAAAAAAAAAAD+/////////wAAAAAAAAAAARsAAAAHAAAACSoAAAAQAAAAEAAAAAEcAAAABwAAAAkrAAAAAQAAAAEAAAAPHQAAABAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7///8BAAAA/v///wAAAAABAAAA/////wEeAAAABwAAAAksAAAAEAAAABAAAAABHwAAAAcAAAAJLQAAAAEAAAABAAAADyAAAAAQAAAACAAAAAAAAAAAAAAAAP7///8BAAAA/v///wAAAAD//////////wAAAAD+////AQAAAP7///8AAAAAAAAAAAAAAAABIQAAAAcAAAAJLgAAABAAAAAQAAAAASIAAAAHAAAACS8AAAABAAAAAQAAAA8jAAAAEAAAAAj//////////wAAAAD+////AQAAAP7///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAASQAAAAHAAAACTAAAAAQAAAAEAAAAA8lAAAABAAAAAgDAAAAAAAAAAAAAAAAAAAADyYAAAAQAAAACP////////////////////////////////////8CAAAA/////wAAAAD///////////////////////////////8PJwAAAAQAAAAIBAAAAAAAAAAAAAAAAAAAAA8oAAAAEAAAAAj/////////////////////////////////////AQAAAAMAAAD/////////////////////////////////////DykAAAAEAAAACAUAAAAAAAAAAAAAAAAAAAAPKgAAABAAAAAI//////////////////////////////////////////8CAAAA/////////////////////////////////////w8rAAAABAAAAAgHAAAAAAAAAAAAAAAAAAAADywAAAAQAAAACP//////////////////////////////////////////////////////////BQAAAP//////////AAAAAP////8PLQAAAAQAAAAIBAAAAAAAAAAAAAAAAAAAAA8uAAAAEAAAAAj/////////////////////BgAAAP///////////////////////////////wQAAAD/////////////////////Dy8AAAAEAAAACAEAAAAAAAAAAAAAAAAAAAAPMAAAABAAAAAI/////////////////////wUAAAD//////////////////////////////////////////////////////////ws=

MediumRooms
AAEAAAD/////AQAAAAAAAAAMAgAAAA9Bc3NlbWJseS1DU2hhcnAFAQAAAAdMYWJEYXRhBAAAAAtjdXJyZW50Q2VsbAxjdXJyZW50V2FsbHMFc2F2ZWQFQ2VsbHMEBwEDBENlbGwCAAAACHFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW0NlbGwsIEFzc2VtYmx5LUNTaGFycCwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQIAAAAJAwAAAAkEAAAABgUAAAALTWVkaXVtUm9vbXMJBgAAAAUDAAAABENlbGwFAAAABHNlbGYEdHlwZQlxdWFkcmFudHMFd2FsbHMJbmVpZ2hib3JzAAADBwMIA35TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249Mi4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0IflN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLkludDMyLCBtc2NvcmxpYiwgVmVyc2lvbj0yLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQIAAAAAAAAANgkHAAAACQgAAAAJCQAAAA8EAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAYAAABxU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tDZWxsLCBBc3NlbWJseS1DU2hhcnAsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAAAZDZWxsW10CAAAACAgJCgAAAAoAAAAKAAAABAcAAAB+U3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tTeXN0ZW0uSW50MzIsIG1zY29ybGliLCBWZXJzaW9uPTIuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24HAAAICAgJCwAAAAEAAAABAAAADwgAAAAQAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAD+////AAAAAAAAAAAAAAAA/////wAAAAABCQAAAAcAAAAJDAAAABAAAAAQAAAABwoAAAAAAQAAABAAAAAEBENlbGwCAAAACQMAAAAJDQAAAAkOAAAACQ8AAAAJEAAAAAkRAAAACRIAAAAJEwAAAAkUAAAACRUAAAANBg8LAAAABAAAAAgGAAAAAAAAAAAAAAAAAAAADwwAAAAQAAAACP///////////////////////////////////////////////wEAAAD///////////////////////////////8BDQAAAAMAAAABAAAAQQkWAAAACRcAAAAJGAAAAAEOAAAAAwAAAAIAAAAyCRkAAAAJGgAAAAkbAAAAAQ8AAAADAAAAAwAAAEQJHAAAAAkdAAAACR4AAAABEAAAAAMAAAAEAAAANgkfAAAACSAAAAAJIQAAAAERAAAAAwAAAAUAAAAzCSIAAAAJIwAAAAkkAAAAARIAAAADAAAABgAAADAJJQAAAAkmAAAACScAAAABEwAAAAMAAAAHAAAAQgkoAAAACSkAAAAJKgAAAAEUAAAAAwAAAAgAAAA4CSsAAAAJLAAAAAktAAAAARUAAAADAAAACQAAAEMJLgAAAAkvAAAACTAAAAABFgAAAAcAAAAJMQAAAAQAAAAEAAAADxcAAAAQAAAACAEAAAABAAAAAQAAAAIAAAABAAAA/v///wAAAAABAAAA/////wEAAAD+//////////7///8AAAAAAAAAAAAAAAABGAAAAAcAAAAJMgAAABAAAAAQAAAAARkAAAAHAAAACTMAAAABAAAAAQAAAA8aAAAAEAAAAAgAAAAAAQAAAAAAAAAAAAAAAAAAAP7///8BAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAARsAAAAHAAAACTQAAAAQAAAAEAAAAAEcAAAABwAAAAk1AAAABAAAAAQAAAAPHQAAABAAAAAIAAAAAAAAAAAAAAAA/v/////////+////AQAAAP////8BAAAAAAAAAP7///8BAAAAAgAAAAEAAAABAAAAAQAAAAEeAAAABwAAAAk2AAAAEAAAABAAAAABHwAAAAcAAAAJNwAAAAEAAAABAAAADyAAAAAQAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAD+////AAAAAAAAAAAAAAAAAQAAAAAAAAABIQAAAAcAAAAJOAAAABAAAAAQAAAAASIAAAAHAAAACTkAAAABAAAAAQAAAA8jAAAAEAAAAAgAAAAAAAAAAAEAAAD+////AAAAAAAAAAAAAAAA/////wAAAAABAAAA/v///wAAAAAAAAAAAAAAAAAAAAAAAAAAASQAAAAHAAAACToAAAAQAAAAEAAAAAElAAAABwAAAAk7AAAAAQAAAAEAAAAPJgAAABAAAAAIAQAAAAAAAAABAAAA/v///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEnAAAABwAAAAk8AAAAEAAAABAAAAABKAAAAAcAAAAJPQAAAAQAAAAEAAAADykAAAAQAAAACAEAAAABAAAAAAAAAP7///8BAAAAAgAAAAEAAAD/////AQAAAAAAAAD+//////////7///8BAAAAAAAAAAAAAAABKgAAAAcAAAAJPgAAABAAAAAQAAAAASsAAAAHAAAACT8AAAABAAAAAQAAAA8sAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+////AQAAAAAAAAABAAAAAS0AAAAHAAAACUAAAAAQAAAAEAAAAAEuAAAABwAAAAlBAAAABAAAAAQAAAAPLwAAABAAAAAIAAAAAAAAAAD//////v/////////+////AAAAAAEAAAD/////AQAAAAIAAAABAAAA/v///wAAAAABAAAAAQAAAAEwAAAABwAAAAlCAAAAEAAAABAAAAAPMQAAAAQAAAAIAAAAAAEAAAADAAAABAAAAA8yAAAAEAAAAAj/////AgAAAP////////////////////////////////////8AAAAA////////////////////////////////DzMAAAAEAAAACAIAAAAAAAAAAAAAAAAAAAAPNAAAABAAAAAI/////wEAAAD/////////////////////AwAAAP///////////////////////////////////////////////w81AAAABAAAAAgEAAAABQAAAAcAAAAIAAAADzYAAAAQAAAACP///////////////////////////////wIAAAD/////////////////////////////////////BAAAAP////8PNwAAAAQAAAAIBgAAAAAAAAAAAAAAAAAAAA84AAAAEAAAAAj///////////////////////////////////////////////8FAAAA/////////////////////wMAAAD/////DzkAAAAEAAAACAMAAAAAAAAAAAAAAAAAAAAPOgAAABAAAAAI//////////8GAAAA////////////////////////////////BAAAAP///////////////////////////////w87AAAABAAAAAgAAAAAAAAAAAAAAAAAAAAADzwAAAAQAAAACAcAAAD/////BQAAAP////////////////////////////////////////////////////////////////////8PPQAAAAQAAAAIAQAAAAIAAAAEAAAABQAAAA8+AAAAEAAAAAgGAAAA////////////////////////////////////////////////////////////////CAAAAP//////////Dz8AAAAEAAAACAgAAAAAAAAAAAAAAAAAAAAPQAAAABAAAAAI/////////////////////////////////////////////////////////////////////wcAAAD/////CQAAAA9BAAAABAAAAAgDAAAABAAAAAYAAAAHAAAAD0IAAAAQAAAACP///////////////////////////////////////////////////////////////////////////////wgAAAAL

NE-Trap
AAEAAAD/////AQAAAAAAAAAMAgAAAA9Bc3NlbWJseS1DU2hhcnAFAQAAAAdMYWJEYXRhBAAAAAtjdXJyZW50Q2VsbAxjdXJyZW50V2FsbHMFc2F2ZWQFQ2VsbHMEBwEDBENlbGwCAAAACHFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW0NlbGwsIEFzc2VtYmx5LUNTaGFycCwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQIAAAAJAwAAAAkEAAAABgUAAAAHTkUtVHJhcAkGAAAABQMAAAAEQ2VsbAUAAAAEc2VsZgR0eXBlCXF1YWRyYW50cwV3YWxscwluZWlnaGJvcnMAAAMHAwgDflN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLkludDMyLCBtc2NvcmxpYiwgVmVyc2lvbj0yLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQh+U3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tTeXN0ZW0uSW50MzIsIG1zY29ybGliLCBWZXJzaW9uPTIuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAgAAAAAAAAA2CQcAAAAJCAAAAAkJAAAADwQAAAAQAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBgAAAHFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW0NlbGwsIEFzc2VtYmx5LUNTaGFycCwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQMAAAAGX2l0ZW1zBV9zaXplCF92ZXJzaW9uBAAABkNlbGxbXQIAAAAICAkKAAAACQAAAAkAAAAEBwAAAH5TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249Mi4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgcAAAgICAkLAAAAAQAAAAEAAAAPCAAAABAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAP7///8AAAAAAAAAAAAAAAD/////AAAAAAEJAAAABwAAAAkMAAAAEAAAABAAAAAHCgAAAAABAAAAEAAAAAQEQ2VsbAIAAAAJAwAAAAkNAAAACQ4AAAAJDwAAAAkQAAAACREAAAAJEgAAAAkTAAAACRQAAAANBw8LAAAABAAAAAgGAAAAAAAAAAAAAAAAAAAADwwAAAAQAAAACP///////////////////////////////////////////////wEAAAD///////////////////////////////8BDQAAAAMAAAABAAAAMwkVAAAACRYAAAAJFwAAAAEOAAAAAwAAAAIAAAAwCRgAAAAJGQAAAAkaAAAAAQ8AAAADAAAAAwAAADEJGwAAAAkcAAAACR0AAAABEAAAAAMAAAAEAAAAMgkeAAAACR8AAAAJIAAAAAERAAAAAwAAAAUAAAA1CSEAAAAJIgAAAAkjAAAAARIAAAADAAAABgAAADQJJAAAAAklAAAACSYAAAABEwAAAAMAAAAHAAAAMQknAAAACSgAAAAJKQAAAAEUAAAAAwAAAAgAAAAyCSoAAAAJKwAAAAksAAAAARUAAAAHAAAACS0AAAABAAAAAQAAAA8WAAAAEAAAAAgAAAAAAAAAAAEAAAD+////AAAAAAAAAAAAAAAA/////wAAAAABAAAA/v///wAAAAAAAAAAAAAAAAAAAAAAAAAAARcAAAAHAAAACS4AAAAQAAAAEAAAAAEYAAAABwAAAAkvAAAAAQAAAAEAAAAPGQAAABAAAAAIAQAAAAAAAAABAAAA/v///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEaAAAABwAAAAkwAAAAEAAAABAAAAABGwAAAAcAAAAJMQAAAAEAAAABAAAADxwAAAAQAAAACAEAAAABAAAAAAAAAP7//////////v///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABHQAAAAcAAAAJMgAAABAAAAAQAAAAAR4AAAAHAAAACTMAAAABAAAAAQAAAA8fAAAAEAAAAAgAAAAAAQAAAAAAAAAAAAAAAAAAAP7///8BAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAASAAAAAHAAAACTQAAAAQAAAAEAAAAAEhAAAABwAAAAk1AAAAAQAAAAEAAAAPIgAAABAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAD+////AQAAAAAAAAABAAAAAAAAAAAAAAAAAAAA/v////////8AAAAAAAAAAAEjAAAABwAAAAk2AAAAEAAAABAAAAABJAAAAAcAAAAJNwAAAAEAAAABAAAADyUAAAAQAAAACAAAAAAAAAAAAAAAAP7///8BAAAA/v///wAAAAD/////AQAAAAAAAAD+//////////7///8AAAAAAAAAAAAAAAABJgAAAAcAAAAJOAAAABAAAAAQAAAAAScAAAAHAAAACTkAAAABAAAAAQAAAA8oAAAAEAAAAAj/////AQAAAAAAAAD+////AQAAAP7///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAASkAAAAHAAAACToAAAAQAAAAEAAAAAEqAAAABwAAAAk7AAAAAQAAAAEAAAAPKwAAABAAAAAIAAAAAAEAAAAAAAAAAAAAAAAAAAD+////AQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEsAAAABwAAAAk8AAAAEAAAABAAAAAPLQAAAAQAAAAIAwAAAAAAAAAAAAAAAAAAAA8uAAAAEAAAAAj//////////wIAAAD///////////////////////////////8AAAAA////////////////////////////////Dy8AAAAEAAAACAAAAAAAAAAAAAAAAAAAAAAPMAAAABAAAAAIAwAAAP////8BAAAA/////////////////////////////////////////////////////////////////////w8xAAAABAAAAAgBAAAAAAAAAAAAAAAAAAAADzIAAAAQAAAACAIAAAAEAAAA//////////////////////////////////////////////////////////////////////////8PMwAAAAQAAAAIAgAAAAAAAAAAAAAAAAAAAA80AAAAEAAAAAj/////AwAAAP////////////////////8FAAAA////////////////////////////////////////////////DzUAAAAEAAAACAUAAAAAAAAAAAAAAAAAAAAPNgAAABAAAAAI////////////////////////////////BAAAAP////8GAAAA/////////////////////////////////////w83AAAABAAAAAgEAAAAAAAAAAAAAAAAAAAADzgAAAAQAAAACP////////////////////8HAAAA////////////////BQAAAP////////////////////////////////////8POQAAAAQAAAAIAQAAAAAAAAAAAAAAAAAAAA86AAAAEAAAAAj/////CAAAAP//////////BgAAAP//////////////////////////////////////////////////////////DzsAAAAEAAAACAIAAAAAAAAAAAAAAAAAAAAPPAAAABAAAAAI/////wcAAAD/////////////////////BQAAAP///////////////////////////////////////////////ws=

MAAAZZEE
AAEAAAD/////AQAAAAAAAAAMAgAAAA9Bc3NlbWJseS1DU2hhcnAFAQAAAAdMYWJEYXRhBAAAAAtjdXJyZW50Q2VsbAxjdXJyZW50V2FsbHMFc2F2ZWQFQ2VsbHMEBwEDBENlbGwCAAAACHFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW0NlbGwsIEFzc2VtYmx5LUNTaGFycCwgVmVyc2lvbj0wLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQIAAAAJAwAAAAkEAAAABgUAAAAITUFBQVpaRUUJBgAAAAUDAAAABENlbGwFAAAABHNlbGYEdHlwZQlxdWFkcmFudHMFd2FsbHMJbmVpZ2hib3JzAAADBwMIA35TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249Mi4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0IflN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLkludDMyLCBtc2NvcmxpYiwgVmVyc2lvbj0yLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQIAAAAAAAAANgkHAAAACQgAAAAJCQAAAA8EAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAYAAABxU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tDZWxsLCBBc3NlbWJseS1DU2hhcnAsIFZlcnNpb249MC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAAAZDZWxsW10CAAAACAgJCgAAABIAAAASAAAABAcAAAB+U3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tTeXN0ZW0uSW50MzIsIG1zY29ybGliLCBWZXJzaW9uPTIuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24HAAAICAgJCwAAAAEAAAABAAAADwgAAAAQAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAD+////AAAAAAAAAAAAAAAA/////wAAAAABCQAAAAcAAAAJDAAAABAAAAAQAAAABwoAAAAAAQAAACAAAAAEBENlbGwCAAAACQMAAAAJDQAAAAkOAAAACQ8AAAAJEAAAAAkRAAAACRIAAAAJEwAAAAkUAAAACRUAAAAJFgAAAAkXAAAACRgAAAAJGQAAAAkaAAAACRsAAAAJHAAAAAkdAAAADQ4PCwAAAAQAAAAIBgAAAAAAAAAAAAAAAAAAAA8MAAAAEAAAAAj///////////////////////////////////////////////8BAAAA////////////////////////////////AQ0AAAADAAAAAQAAADMJHgAAAAkfAAAACSAAAAABDgAAAAMAAAACAAAAMAkhAAAACSIAAAAJIwAAAAEPAAAAAwAAAAMAAAAxCSQAAAAJJQAAAAkmAAAAARAAAAADAAAABAAAADQJJwAAAAkoAAAACSkAAAABEQAAAAMAAAAFAAAANwkqAAAACSsAAAAJLAAAAAESAAAAAwAAAAYAAAA4CS0AAAAJLgAAAAkvAAAAARMAAAADAAAABwAAADUJMAAAAAkxAAAACTIAAAABFAAAAAMAAAAIAAAANAkzAAAACTQAAAAJNQAAAAEVAAAAAwAAAAkAAAAzCTYAAAAJNwAAAAk4AAAAARYAAAADAAAACgAAADYJOQAAAAk6AAAACTsAAAABFwAAAAMAAAALAAAASgk8AAAACT0AAAAJPgAAAAEYAAAAAwAAAAwAAAAyCT8AAAAJQAAAAAlBAAAAARkAAAADAAAADQAAADUJQgAAAAlDAAAACUQAAAABGgAAAAMAAAAOAAAAOAlFAAAACUYAAAAJRwAAAAEbAAAAAwAAAA8AAAA3CUgAAAAJSQAAAAlKAAAAARwAAAADAAAAEAAAADAJSwAAAAlMAAAACU0AAAABHQAAAAMAAAARAAAAQglOAAAACU8AAAAJUAAAAAEeAAAABwAAAAlRAAAAAQAAAAEAAAAPHwAAABAAAAAIAAAAAAAAAAABAAAA/v///wAAAAAAAAAAAAAAAP////8AAAAAAQAAAP7///8AAAAAAAAAAAAAAAAAAAAAAAAAAAEgAAAABwAAAAlSAAAAEAAAABAAAAABIQAAAAcAAAAJUwAAAAEAAAABAAAADyIAAAAQAAAACAEAAAAAAAAAAQAAAP7///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABIwAAAAcAAAAJVAAAABAAAAAQAAAAASQAAAAHAAAACVUAAAABAAAAAQAAAA8lAAAAEAAAAAgBAAAAAQAAAAAAAAD+////AQAAAP7///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAASYAAAAHAAAACVYAAAAQAAAAEAAAAAEnAAAABwAAAAlXAAAAAQAAAAEAAAAPKAAAABAAAAAIAAAAAAAAAAAAAAAA/v///wEAAAD+////AAAAAP//////////AAAAAP7///8BAAAA/v///wAAAAAAAAAAAAAAAAEpAAAABwAAAAlYAAAAEAAAABAAAAABKgAAAAcAAAAJWQAAAAEAAAABAAAADysAAAAQAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+////AQAAAP7///8AAAAA/////wEAAAABLAAAAAcAAAAJWgAAABAAAAAQAAAAAS0AAAAHAAAACVsAAAABAAAAAQAAAA8uAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+////AQAAAAAAAAABAAAAAS8AAAAHAAAACVwAAAAQAAAAEAAAAAEwAAAABwAAAAldAAAAAQAAAAEAAAAPMQAAABAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAD+/////////wAAAAABAAAAAAAAAAAAAAAAAAAA/v///wEAAAAAAAAAAAAAAAEyAAAABwAAAAleAAAAEAAAABAAAAABMwAAAAcAAAAJXwAAAAEAAAABAAAADzQAAAAQAAAACAAAAAAAAAAAAAAAAP7//////////v///wAAAAABAAAAAQAAAAAAAAD+//////////7///8AAAAAAAAAAAAAAAABNQAAAAcAAAAJYAAAABAAAAAQAAAAATYAAAAHAAAACWEAAAABAAAAAQAAAA83AAAAEAAAAAgAAAAAAAAAAAEAAAD+////AAAAAAAAAAAAAAAAAQAAAAAAAAABAAAA/v///wAAAAAAAAAAAAAAAAAAAAAAAAAAATgAAAAHAAAACWIAAAAQAAAAEAAAAAE5AAAABwAAAAljAAAAAQAAAAEAAAAPOgAAABAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAP7///8AAAAAAAAAAAAAAAABAAAAAAAAAAE7AAAABwAAAAlkAAAAEAAAABAAAAABPAAAAAcAAAAJZQAAAAgAAAAIAAAADz0AAAAQAAAACAEAAAABAAAAAQAAAAIAAAABAAAAAgAAAAEAAAABAAAAAQAAAP/////+////AQAAAAIAAAABAAAAAQAAAAEAAAABPgAAAAcAAAAJZgAAABAAAAAQAAAAAT8AAAAHAAAACWcAAAABAAAAAQAAAA9AAAAAEAAAAAgAAAAAAQAAAAAAAAAAAAAAAAAAAP7///8BAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAUEAAAAHAAAACWgAAAAQAAAAEAAAAAFCAAAABwAAAAlpAAAAAQAAAAEAAAAPQwAAABAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAD+////AQAAAAAAAAD/////AAAAAAAAAAAAAAAA/v///wEAAAAAAAAAAAAAAAFEAAAABwAAAAlqAAAAEAAAABAAAAABRQAAAAcAAAAJawAAAAEAAAABAAAAD0YAAAAQAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7///8BAAAAAAAAAAEAAAABRwAAAAcAAAAJbAAAABAAAAAQAAAAAUgAAAAHAAAACW0AAAABAAAAAQAAAA9JAAAAEAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/v///wEAAAD+////AAAAAP////8BAAAAAUoAAAAHAAAACW4AAAAQAAAAEAAAAAFLAAAABwAAAAlvAAAAAQAAAAEAAAAPTAAAABAAAAAIAQAAAAAAAAABAAAA/v///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFNAAAABwAAAAlwAAAAEAAAABAAAAABTgAAAAcAAAAJcQAAAAQAAAAEAAAAD08AAAAQAAAACAEAAAABAAAAAAAAAP7///8BAAAAAgAAAAEAAAD/////AQAAAAAAAAD+//////////7/////////AAAAAAAAAAABUAAAAAcAAAAJcgAAABAAAAAQAAAAD1EAAAAEAAAACAMAAAAAAAAAAAAAAAAAAAAPUgAAABAAAAAI//////////8CAAAA////////////////////////////////AAAAAP///////////////////////////////w9TAAAABAAAAAgAAAAAAAAAAAAAAAAAAAAAD1QAAAAQAAAACAMAAAD/////AQAAAP////////////////////////////////////////////////////////////////////8PVQAAAAQAAAAIAQAAAAAAAAAAAAAAAAAAAA9WAAAAEAAAAAgCAAAADAAAAP//////////BAAAAP//////////////////////////////////////////////////////////D1cAAAAEAAAACAQAAAAAAAAAAAAAAAAAAAAPWAAAABAAAAAI/////////////////////wMAAAD///////////////////////////////8FAAAA/////////////////////w9ZAAAABAAAAAgHAAAAAAAAAAAAAAAAAAAAD1oAAAAQAAAACP//////////////////////////////////////////////////////////BAAAAP///////////////wYAAAAPWwAAAAQAAAAICAAAAAAAAAAAAAAAAAAAAA9cAAAAEAAAAAj/////////////////////////////////////////////////////////////////////BwAAAP////8FAAAAD10AAAAEAAAACAUAAAAAAAAAAAAAAAAAAAAPXgAAABAAAAAI//////////////////////////////////////////8IAAAA/////////////////////wYAAAD//////////w9fAAAABAAAAAgEAAAAAAAAAAAAAAAAAAAAD2AAAAAQAAAACP////////////////////////////////////8JAAAABwAAAP////////////////////////////////////8PYQAAAAQAAAAIAwAAAAAAAAAAAAAAAAAAAA9iAAAAEAAAAAj//////////xAAAAD/////////////////////CAAAAP////8KAAAA////////////////////////////////D2MAAAAEAAAACAYAAAAAAAAAAAAAAAAAAAAPZAAAABAAAAAI////////////////////////////////////////////////CQAAAP////////////////////8LAAAA/////w9lAAAACAAAAAgAAAAAAQAAAAIAAAADAAAABAAAAAUAAAAHAAAACAAAAA9mAAAAEAAAAAj//////////////////////////////////////////////////////////////////////////woAAAD/////D2cAAAAEAAAACAIAAAAAAAAAAAAAAAAAAAAPaAAAABAAAAAI/////wMAAAD/////////////////////DQAAAP///////////////////////////////////////////////w9pAAAABAAAAAgFAAAAAAAAAAAAAAAAAAAAD2oAAAAQAAAACP///////////////////////////////wwAAAD///////////////////////////////8OAAAA//////////8PawAAAAQAAAAICAAAAAAAAAAAAAAAAAAAAA9sAAAAEAAAAAj/////////////////////////////////////////////////////////////////////DQAAAP////8PAAAAD20AAAAEAAAACAcAAAAAAAAAAAAAAAAAAAAPbgAAABAAAAAI//////////////////////////////////////////////////////////8EAAAA////////////////DgAAAA9vAAAABAAAAAgAAAAAAAAAAAAAAAAAAAAAD3AAAAAQAAAACBEAAAD/////CQAAAP////////////////////////////////////////////////////////////////////8PcQAAAAQAAAAIAQAAAAIAAAAEAAAABQAAAA9yAAAAEAAAAAgQAAAA////////////////////////////////////////////////////////////////////////////////Cw==

*/

	private void GenerateMaze(){
		Room('H');
		Room('1');
		Room('2');
		Room('5');
		Room('8');
		Room('7');
		Room('6');
		Room('3');
		Room('0');
		Room('B');
		

		ConnectRooms(0,1);
		
		ConnectRooms(1,2);
		ConnectRooms(2,3);
		ConnectRooms(3,4);
		ConnectRooms(4,5);
		ConnectRooms(5,6);
		ConnectRooms(6,7);
		ConnectRooms(7,8);
		ConnectRooms(8,1);

		ConnectRooms(7,9);
		
		
		Lab.currentCell = Lab.Cells[0];
		Move(7);
	}

	public void HeadsetEnteredQuadrant(object sender, HeadsetCollisionEventArgs e){
		for(int i = 0; i < 9; i++){
			if(e.collider == QuadrantColliders [i]){
				Move(i);
				return;
			}
		}
	}

	private int lastMove = -1;
	public void Move(int quadrant){
		if (quadrant != lastMove) {
			Debug.Log("Move() called: "+ quadrant);
			Cell n = getCellFromQuadrant(quadrant);
			Debug.Log("n = " + n);
			if(n != null){
				Lab.currentCell = n;
				Rebuild(quadrant);
			}
			lastMove = quadrant;
		}
	}
	
	private void Rebuild(int quadrant){
		//init
		float[] newWalls = new float[16]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		int[] CellWallsToActivate = new int[16]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
		
		List<Cell> toCells = new List<Cell>();
		List<Cell> fromCell = new List<Cell>();
		List<int> addWalls = new List<int>();
		List<float> fromScores = new List<float>();
		for(int w = 0; w < 16; w++){
			if(Lab.currentCell.walls[w] != 0){
				Cell c = null;
				if (Lab.currentCell.neighbors[w] >= 0){
					c = Lab.Cells[Lab.currentCell.neighbors[w]];
				}
				
				toCells.Add(c);
				fromCell.Add(Lab.currentCell);
				addWalls.Add(w);
				fromScores.Add(ManDist(quadrant, w) * Lab.currentCell.walls[w]); // get dist to each wall for big rooms? (1 for small?) (include +/-)
			}
		}
		
		
		bool changed = true;
		while(changed == true){
			changed = false;
			List<Cell> nextCells = new List<Cell>();
			List<Cell> nextFromCell = new List<Cell>();
			List<int> nextWalls = new List<int>();
			List<float> nextScores = new List<float>();
			
			for(int i = 0; i < addWalls.Count; i++){
				//add walls
				float dist = fromScores[i];
				if(newWalls[addWalls[i]] == 0 || Math.Abs(dist) < Math.Abs(newWalls[addWalls[i]])){
					newWalls[addWalls[i]] = dist;
					changed = true;
					//activate wall items
					CellWallsToActivate[addWalls[i]] = fromCell[i].self;
				}
				//add neighbors
				if(toCells[i] != null){
					for(int w = 0; w < 16; w++){
						if(toCells[i].walls[w] != 0){
							Cell c = null;
							if (toCells[i].neighbors[w] >= 0){
								c = Lab.Cells[toCells[i].neighbors[w]];
							}
							
							nextCells.Add(c);
							nextFromCell.Add(toCells[i]);
							nextWalls.Add(w);
							nextScores.Add((Math.Abs(fromScores[i])+WalkDist(addWalls[i], w)) * toCells[i].walls[w]); //(include +/-)
						}
					}
					
				}
			}
			
			//replace
			toCells = nextCells;
			fromCell = nextFromCell;
			addWalls = nextWalls;
			fromScores = nextScores;
		}
		
		//set walls
		List<bool> r = new List<bool>();
		for(int i = 0; i < 16; i++){
			if(newWalls[i] > 0){
				r.Add(false);
			} else {
				r.Add(true);
			}
		}
		sendDebug(quadrant, r);
		setWalls(r);
		
	}

	private void setWalls(List<bool> activeWalls){
		for(int i = 0; i < 16; i++){
			WallObjects[i].SetActive(activeWalls[i]);
		}
	}
	
	private void sendDebug(int q, List<bool> r){
		List<string> p = new List<string>();
		p.Add("  ");
		p.Add("  ");
		p.Add("  ");
		p.Add("  ");
		p.Add("  ");
		p.Add("  ");
		p.Add("  ");
		p.Add("  ");
		p.Add("  ");
		p[q] = "x";
		
		string debug = "----------  Cell-"+Lab.currentCell.self+"\r\n";
		if(r[0]){debug += "|"+p[0]+" |";} else {debug += "|"+p[0]+"  ";}
		if(r[1]){debug += ""+p[1]+" |";} else {debug += ""+p[1]+"  ";}
		debug += ""+p[2]+" |\r\n";
		if(r[2]){debug += "|--";} else {debug += "|  ";}
		if(r[3]){debug += "+";} else {debug += "  ";}
		if(r[4]){debug += "--";} else {debug += "  ";}
		if(r[5]){debug += "+";} else {debug += "  ";}
		if(r[6]){debug += "--";} else {debug += "  ";}
		debug += "|\r\n";
		if(r[7]){debug += "|"+p[3]+" |";} else {debug += "|"+p[3]+"  ";}
		if(r[8]){debug += ""+p[4]+" |";} else {debug += ""+p[4]+"  ";}
		debug += ""+p[5]+" |\r\n";
		if(r[9]){debug += "|--";} else {debug += "|  ";}
		if(r[10]){debug += "+";} else {debug += " ";}
		if(r[11]){debug += "--";} else {debug += "  ";}
		if(r[12]){debug += "+";} else {debug += "  ";}
		if(r[13]){debug += "--";} else {debug += "  ";}
		debug += "|\r\n";
		if(r[14]){debug += "|"+p[6]+" |";} else {debug += "|"+p[6]+"  ";}
		if(r[15]){debug += ""+p[7]+" |";} else {debug += ""+p[7]+"  ";}
		debug += ""+p[8]+" |\r\n";
		debug += "----------";
		Debug.Log(debug);
	}
	
	private Cell getCellFromQuadrant(int quad){
		foreach(Cell n in getNeighbors(Lab.currentCell)) {
			foreach(int q in n.quadrants){
				if(q == quad){
					return n;
				}
			}
		}
		for(int w = 0; w < 16; w++){
			if(Lab.currentCell.neighbors[w] >= 0){
				foreach(int q in Lab.Cells[Lab.currentCell.neighbors[w]].quadrants){
					//if(q == quad && Lab.Cells[Lab.currentCell.neighbors[w]]){
						//return n;
					//}
				}
			}
		}
		foreach(int q in Lab.currentCell.quadrants){
			if(q == quad){
				return Lab.currentCell;
			}
		}
		return null;
	}
	
	private float ManDist(int quad, int wall){
		int x1 = -999;
		int y1 = -999;
		int x2 = -999;
		int y2 = -999;
		
		x1 = (quad % 3)*2;
		y1 = Mathf.RoundToInt(Mathf.Floor(quad/3.0f))*2;
		
		switch (wall) {
			case 2:
			case 9:
				x2 = 0;
				break;
			case 0:
			case 3:
			case 7:
			case 10:
			case 14:
				x2 = 1;
				break;
			case 4:
			case 11:
				x2 = 2;
				break;
			case 1:
			case 5:
			case 8:
			case 12:
			case 15:
				x2 = 3;
				break;
			case 6:
			case 13:
				x2 = 4;
				break;
			default:
				break;
		}
		if(wall >=0 && wall <=1)
			y2 = 0;
		else if(wall >=2 && wall <=6)
			y2 = 1;
		else if(wall >=7 && wall <=8)
			y2 = 2;
		else if(wall >=9 && wall <=13)
			y2 = 3;
		else if(wall >=14 && wall <=15)
			y2 = 4;
		
		return Math.Abs(x1-x2)+Math.Abs(y1-y2);
	}
	
	private float WalkDist(int w1, int w2){
		int x1 = -999;
		int y1 = -999;
		int x2 = -999;
		int y2 = -999;
		
		switch (w1) {
			case 2:
			case 9:
				x1 = 0;
				break;
			case 0:
			case 3:
			case 7:
			case 10:
			case 14:
				x1 = 1;
				break;
			case 4:
			case 11:
				x1 = 2;
				break;
			case 1:
			case 5:
			case 8:
			case 12:
			case 15:
				x1 = 3;
				break;
			case 6:
			case 13:
				x1 = 4;
				break;
			default:
				break;
		}
		if(w1 >=0 && w1 <=1)
			y1 = 0;
		else if(w1 >=2 && w1 <=6)
			y1 = 1;
		else if(w1 >=7 && w1 <=8)
			y1 = 2;
		else if(w1 >=9 && w1 <=13)
			y1 = 3;
		else if(w1 >=14 && w1 <=15)
			y1 = 4;
		
		switch (w2) {
			case 2:
			case 9:
				x2 = 0;
				break;
			case 0:
			case 3:
			case 7:
			case 10:
			case 14:
				x2 = 1;
				break;
			case 4:
			case 11:
				x2 = 2;
				break;
			case 1:
			case 5:
			case 8:
			case 12:
			case 15:
				x2 = 3;
				break;
			case 6:
			case 13:
				x2 = 4;
				break;
			default:
				break;
		}
		if(w2 >=0 && w2 <=1)
			y2 = 0;
		else if(w2 >=2 && w2 <=6)
			y2 = 1;
		else if(w2 >=7 && w2 <=8)
			y2 = 2;
		else if(w2 >=9 && w2 <=13)
			y2 = 3;
		else if(w2 >=14 && w2 <=15)
			y2 = 4;
		
		float a = x1-x2;
		float b = y1-y2;
		return Mathf.Sqrt((a*a)+(b*b));
	}
	
	private void Save() {
		MemoryStream stream = new MemoryStream();
		BinaryFormatter bf = new BinaryFormatter();
		
		bf.Serialize(stream, Lab);
		Binary = Convert.ToBase64String(stream.ToArray());
	}
	
	private void Load() {
		MemoryStream stream = new MemoryStream(Convert.FromBase64String(Binary));
		BinaryFormatter bf = new BinaryFormatter();
		LabData data = (LabData)bf.Deserialize(stream);
		Lab = data;
		ThingToSave1 = data.saved;
	}
	
	private List<Cell> getNeighbors(Cell c){
		List<Cell> r = new List<Cell>();
		foreach(int n in c.neighbors){
			if(n >= 0){
				r.Add(Lab.Cells[n]);
			}
		}
		return r;
	}
	
	
	
	private void ConnectRooms(int c1, int c2){
		int conn = -1;
		for(int i = 0; i < 16; i++){
			if(Lab.Cells[c1].walls[i] == -1 && Lab.Cells[c2].walls[i] == -1){
				conn = i;
			}
		}
		Lab.Cells[c1].neighbors[conn] = c2;
		Lab.Cells[c2].neighbors[conn] = c1;
		Lab.Cells[c1].walls[conn] = 1;
		Lab.Cells[c2].walls[conn] = 1;
	}
	
	private int Room(char type){
		int cellNum = Lab.Cells.Count;
		Cell c = new Cell(cellNum,type);
		
		switch (type) {
			case '0':
				c.quadrants.Add(0);
				c.walls[3] = -2;
				
				c.walls[0] = -1;
				c.walls[2] = -1;
				break;
			case '1':
				c.quadrants.Add(1);
				c.walls[3] = -2;
				c.walls[5] = -2;
				
				c.walls[0] = -1;
				c.walls[1] = -1;
				c.walls[4] = -1;
				break;
			case '2':
				c.quadrants.Add(2);
				c.walls[5] = -2;
				
				c.walls[1] = -1;
				c.walls[6] = -1;
				break;
			case '3':
				c.quadrants.Add(3);
				c.walls[3] = -2;
				c.walls[10] = -2;
				
				c.walls[2] = -1;
				c.walls[7] = -1;
				c.walls[9] = -1;
				break;
			case '4':
				c.quadrants.Add(4);
				c.walls[3] = -2;
				c.walls[5] = -2;
				c.walls[10] = -2;
				c.walls[12] = -2;
				
				c.walls[4] = -1;
				c.walls[7] = -1;
				c.walls[8] = -1;
				c.walls[11] = -1;
				break;
			case '5':
				c.quadrants.Add(5);
				c.walls[5] = -2;
				c.walls[12] = -2;
				
				c.walls[6] = -1;
				c.walls[8] = -1;
				c.walls[13] = -1;
				break;
			case '6':
				c.quadrants.Add(6);
				c.walls[10] = -2;
				
				c.walls[9] = -1;
				c.walls[14] = -1;
				break;
			case '7':
				c.quadrants.Add(7);
				c.walls[10] = -2;
				c.walls[12] = -2;
				
				c.walls[11] = -1;
				c.walls[14] = -1;
				c.walls[15] = -1;
				break;
			case '8':
				c.quadrants.Add(8);
				c.walls[12] = -2;
				
				c.walls[13] = -1;
				c.walls[15] = -1;
				break;
			default:
				return BigRoom(type);
		}
		
		Lab.Cells.Add(c);
		return cellNum;
	}
	
	private int BigRoom(char type){
		int cellNum = Lab.Cells.Count;
		Cell c = new Cell(cellNum,type);
		
		switch (type) {
			case 'A': //2x2 top left
				c.quadrants.Add(0);
				c.quadrants.Add(1);
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				
				c.walls[3] = 2;
				c.walls[5] = -2;
				c.walls[10] = -2;
				c.walls[12] = -2;
				
				c.walls[0] = 1;
				c.walls[2] = 1;
				c.walls[4] = 1;
				c.walls[7] = 1;
				
				c.walls[1] = -1;
				c.walls[8] = -1;
				c.walls[9] = -1;
				c.walls[11] = -1;
				break;
			case 'B': //2x2 top right
				c.quadrants.Add(1);
				c.quadrants.Add(2);
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				
				c.walls[3] = -2;
				c.walls[5] = 2;
				c.walls[10] = -2;
				c.walls[12] = -2;
				
				c.walls[1] = 1;
				c.walls[4] = 1;
				c.walls[6] = 1;
				c.walls[8] = 1;
				
				c.walls[0] = -1;
				c.walls[7] = -1;
				c.walls[11] = -1;
				c.walls[13] = -1;
				break;
			case 'C': //2x2 bottom left
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				c.quadrants.Add(6);
				c.quadrants.Add(7);
				
				c.walls[3] = -2;
				c.walls[5] = -2;
				c.walls[10] = 2;
				c.walls[12] = -2;
				
				c.walls[7] = 1;
				c.walls[9] = 1;
				c.walls[11] = 1;
				c.walls[14] = 1;
				
				c.walls[2] = -1;
				c.walls[4] = -1;
				c.walls[8] = -1;
				c.walls[15] = -1;
				break;
			case 'D': //2x2 bottom right
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				c.quadrants.Add(7);
				c.quadrants.Add(8);
				
				c.walls[3] = -2;
				c.walls[5] = -2;
				c.walls[10] = -2;
				c.walls[12] = 2;
				
				c.walls[8] = 1;
				c.walls[11] = 1;
				c.walls[13] = 1;
				c.walls[15] = 1;
				
				c.walls[4] = -1;
				c.walls[6] = -1;
				c.walls[7] = -1;
				c.walls[14] = -1;
				break;
				
			case 'E'://left 6
				c.quadrants.Add(0);
				c.quadrants.Add(1);
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				c.quadrants.Add(6);
				c.quadrants.Add(7);
				
				c.walls[3] = 2;
				c.walls[5] = -2;
				c.walls[10] = 2;
				c.walls[12] = -2;
				
				c.walls[0] = 1;
				c.walls[2] = 1;
				c.walls[4] = 1;
				c.walls[7] = 1;
				c.walls[9] = 1;
				c.walls[11] = 1;
				c.walls[14] = 1;
				
				c.walls[1] = -1;
				c.walls[8] = -1;
				c.walls[15] = -1;
				break;
			case 'F'://right 6
				c.quadrants.Add(1);
				c.quadrants.Add(2);
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				c.quadrants.Add(7);
				c.quadrants.Add(8);
				
				c.walls[3] = -2;
				c.walls[5] = 2;
				c.walls[10] = -2;
				c.walls[12] = 2;
				
				c.walls[1] = 1;
				c.walls[4] = 1;
				c.walls[6] = 1;
				c.walls[8] = 1;
				c.walls[11] = 1;
				c.walls[13] = 1;
				c.walls[15] = 1;
				
				c.walls[0] = -1;
				c.walls[7] = -1;
				c.walls[14] = -1;
				break;
			case 'G'://top 5
				c.quadrants.Add(0);
				c.quadrants.Add(1);
				c.quadrants.Add(2);
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				
				c.walls[3] = 2;
				c.walls[5] = 2;
				c.walls[10] = -2;
				c.walls[12] = -2;
				
				c.walls[0] = 1;
				c.walls[1] = 1;
				c.walls[2] = 1;
				c.walls[4] = 1;
				c.walls[6] = 1;
				c.walls[7] = 1;
				c.walls[8] = 1;
				
				c.walls[9] = -1;
				c.walls[11] = -1;
				c.walls[13] = -1;
				break;
			case 'H'://bottom 6
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				c.quadrants.Add(6);
				c.quadrants.Add(7);
				c.quadrants.Add(8);
				
				c.walls[3] = -2;
				c.walls[5] = -2;
				c.walls[10] = 2;
				c.walls[12] = 2;
				
				c.walls[7] = 1;
				c.walls[8] = 1;
				c.walls[9] = 1;
				c.walls[11] = 1;
				c.walls[13] = 1;
				c.walls[14] = 1;
				c.walls[15] = 1;
				
				c.walls[2] = -1;
				c.walls[4] = -1;
				c.walls[6] = -1;
				break;
				
			case 'I'://entrance bottom right
				c.quadrants.Add(0);
				c.quadrants.Add(1);
				c.quadrants.Add(2);
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				c.quadrants.Add(6);
				c.quadrants.Add(7);
				
				c.walls[3] = 2;
				c.walls[5] = 2;
				c.walls[10] = 2;
				c.walls[12] = -2;
				
				c.walls[0] = 1;
				c.walls[1] = 1;
				c.walls[2] = 1;
				c.walls[4] = 1;
				c.walls[6] = 1;
				c.walls[7] = 1;
				c.walls[8] = 1;
				c.walls[9] = 1;
				c.walls[11] = 1;
				c.walls[14] = 1;
				
				c.walls[13] = -1;
				c.walls[15] = -1;
				break;
			case 'J': //entrance bottom left
				c.quadrants.Add(0);
				c.quadrants.Add(1);
				c.quadrants.Add(2);
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				c.quadrants.Add(7);
				c.quadrants.Add(8);
				
				c.walls[3] = 2;
				c.walls[5] = 2;
				c.walls[10] = -2;
				c.walls[12] = 2;
				
				c.walls[0] = 1;
				c.walls[1] = 1;
				c.walls[2] = 1;
				c.walls[4] = 1;
				c.walls[6] = 1;
				c.walls[7] = 1;
				c.walls[8] = 1;
				c.walls[11] = 1;
				c.walls[13] = 1;
				c.walls[15] = 1;
				
				c.walls[9] = -1;
				c.walls[14] = -1;
				break;
			case 'K'://entrance top right
				c.quadrants.Add(0);
				c.quadrants.Add(1);
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				c.quadrants.Add(6);
				c.quadrants.Add(7);
				c.quadrants.Add(8);
				
				c.walls[3] = 2;
				c.walls[5] = -2;
				c.walls[10] = 2;
				c.walls[12] = 2;
				
				c.walls[0] = 1;
				c.walls[2] = 1;
				c.walls[4] = 1;
				c.walls[7] = 1;
				c.walls[8] = 1;
				c.walls[9] = 1;
				c.walls[11] = 1;
				c.walls[13] = 1;
				c.walls[14] = 1;
				c.walls[15] = 1;
				
				c.walls[1] = -1;
				c.walls[6] = -1;
				break;
			case 'L'://entrance top left
				c.quadrants.Add(1);
				c.quadrants.Add(2);
				c.quadrants.Add(3);
				c.quadrants.Add(4);
				c.quadrants.Add(5);
				c.quadrants.Add(6);
				c.quadrants.Add(7);
				c.quadrants.Add(8);
				
				c.walls[3] = -2;
				c.walls[5] = 2;
				c.walls[10] = 2;
				c.walls[12] = 2;
				
				c.walls[1] = 1;
				c.walls[4] = 1;
				c.walls[6] = 1;
				c.walls[7] = 1;
				c.walls[8] = 1;
				c.walls[9] = 1;
				c.walls[11] = 1;
				c.walls[13] = 1;
				c.walls[14] = 1;
				c.walls[15] = 1;
				
				c.walls[0] = -1;
				c.walls[2] = -1;
				break;
			
			default:
				return -1;
		}
		
		Lab.Cells.Add(c);
		return cellNum;
	}
	
	
	
	private LabData RoomTypes = null;
	private char getConnectedRoomType(char type, int wall){
		if(RoomTypes == null){
			LabData temp = Lab;
			Lab = new LabData();
			foreach(char ch in new char[]{'0','1','2','3','4','5','6','7','8'}){
				Room(ch);
			}
			RoomTypes = Lab;
			Lab = temp;
		}
		
		List<char> validTypes = new List<char>();
		List<int> quads = null;
		foreach(Cell c in RoomTypes.Cells){
			if(c.type == type){
				quads = c.quadrants;
				break;
			}
		}
		foreach(Cell c in RoomTypes.Cells){
			if(c.walls[wall] == -1){
				if(!quads.Intersect(c.quadrants).Any()){
					validTypes.Add(c.type);
				}
			}
		}
		return validTypes[0];
	}
}

[Serializable]
public class LabData {
	public Cell currentCell;
	public int[] currentWalls = new int[16]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
	public string saved;
	
	public List<Cell> Cells = new List<Cell>();
}

[Serializable]
public class Cell {
	public int self;
	public char type;
	public List<int> quadrants = new List<int>();
	public int[] walls = new int[16]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
	public List<int> neighbors = new List<int>();
	
	public Cell(int s, char t) {
		self = s;
		type = t;
		for(int i = 0; i < 16; i++){
			neighbors.Add(-1);
		}
	}
}