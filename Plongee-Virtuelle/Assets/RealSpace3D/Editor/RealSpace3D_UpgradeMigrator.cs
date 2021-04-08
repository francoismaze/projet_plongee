// *******************************************************************************
// * Copyright (c) 2012,2013,2014 VisiSonics, Inc.
// * This software is the proprietary information of VisiSonics, Inc.
// * All Rights Reserved.
// *
// * © VisiSonics Corporation, 2013-2014
// * VisiSonics Confidential Information
// * Source code provided under the terms of the Software License Agreement 
// * between VisiSonics Corporation and Oculus VR, LLC dated 09/10/2014
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
using RealSpace3DXMLDrone;

public class RealSpace3D_UpgradeMigrator : MonoBehaviour
{
	[MenuItem("Help/RealSpace3D/Upgrade Migrator", false, 906)]

	/// <summary>
	/// Init this instance. Display the upgrade migration popup
	/// </summary>
	private static void Init()
	{        
		bool bNotify = false;
        string sNotice = "This action will move the necessary files from your project /Assets/StreamingAssets/RealSpace3D/DontTouch folder to its new location (located in /RealSpace3D/Resources/DontTouch/DataFiles)." +
                   " This folder is a RS3D internal data folder that you should not have placed any of your own files. If you have, please remove them for safe keeping. Once you have completed the upgrade migration, please run your project " +
            "and verify that all works. The /Assets/StreamingAssets/RealSpace3D/DontTouch folder will no longer be referenced. You may delete it if you wish. Select 'Yes' when you are ready to begin migration.";

        if(EditorUtility.DisplayDialog("RealSpace3D Copyright 2011 - 2016", sNotice, "Yes", "No"))
        {
            bNotify = true;

            // do migration work here
            RealSpace3D_XMLDrone.Instance.DoUpgradeMigration();
        }

        else
            bNotify = false;
            
        if(bNotify)
            sNotice = "The upgrade migration has completed. If you should have any issues, please contact support@visisonics.com."; 

        else
            sNotice = "You have chosen not to migrate the data. The result will be any custom RS3D Virtual Room Presets or Materials you have created " +
            "will not be available until you have completed the migration.";

        EditorUtility.DisplayDialog("RealSpace3D Copyright 2011 - 2016", sNotice, "Ok");
	}
}


