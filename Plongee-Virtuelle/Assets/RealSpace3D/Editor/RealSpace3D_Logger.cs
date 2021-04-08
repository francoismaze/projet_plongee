// *******************************************************************************
// * Copyright (c) 2012,2013,2014,2015,2016,2017,2018 VisiSonics, Inc.
// * This software is the proprietary information of VisiSonics, Inc.
// * All Rights Reserved.
// *
// * © VisiSonics Corporation, 2013-2018
// ********************************************************************************
// 
// Original Author: R E Haxton
// $Author$
// $Date$
// $LastChangedDate$
// $Revision$
//
// Purpose:
//
// Comments: 
// 

using UnityEngine;
using UnityEditor;
using RealSpace3D;

public class RealSpace3D_Logger : MonoBehaviour
{
	[MenuItem("Help/RealSpace3D/Development Log", false, 903)]

	/// <summary>
	/// Init this instance. Display the logging dialog and handle on/off.
	/// </summary>
	private static void Init()
	{
		bool bNotify = 				false;
		string sNotice = 			"Turn on RealSpace3D internal logging?";

        if(EditorUtility.DisplayDialog("RealSpace3D Copyright 2011 - 2019", sNotice, "Yes", "No"))
        {
            bNotify = true;

            PlayerPrefs.SetInt("RS3D_LogOn", 1);
            PlayerPrefs.Save();
        }

        else
        {
            PlayerPrefs.SetInt("RS3D_LogOn", 0);
            PlayerPrefs.Save();
        }

		if(bNotify) 
		{
			sNotice = "The logfile can be located at: "; 

#if UNITY_IPHONE

			sNotice += "/RealSpace3d/DontTouch/rs.log";

#elif UNITY_ANDROID
			sNotice += "/RealSpace3d/DontTouch/rs.log";

#else
			sNotice += Application.persistentDataPath + "/RealSpace3d/DontTouch/rs.log";
#endif
			EditorUtility.DisplayDialog("RealSpace3D Copyright 2011 - 2019", sNotice, "Ok");
		}
	}
}

