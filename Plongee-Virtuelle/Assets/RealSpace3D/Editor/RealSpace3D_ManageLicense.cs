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
// Comments:    Screwing w/ this code in anyway will result in your license key not being activated and thus disabling RS3D Audio. So, gonna talk right 
//              down to earth in a language that everybody here can easily understand...DON'T F#%K WITH THIS CODE! ;)
// 

using UnityEngine;
using UnityEditor;
using System;
using RealSpace3D;
using RealSpace3DPlatformSwitcher;
using RealSpace3D_DNA_Status_Codes;

public class RealSpace3D_ManageLicense : EditorWindow
{
    bool bButtonActivate =      false;
    bool bButtonDeactivate =    false;
    bool bButtonReactivate =    false;

    bool bDoOnce =              true;
    bool bLicenseActivated =    false;
    bool bLicenseSetup =        false;

    string sActivationCode = "Please enter key here";
    string sNewPassword;
    string sPassword;
    string sPlatform;

    private ErrorMsg    theErrorMsg =                               ErrorMsg.Instance;
    private static      RealSpace3D_PlatformSwitcher _theSwitcher = RealSpace3D_PlatformSwitcher.Instance;
    private string      sDataPath =                                 string.Empty;

    [MenuItem("Help/RealSpace3D/Manage License", false, 905)]
    static void Init()
    {
        RealSpace3D_ManageLicense window =  ScriptableObject.CreateInstance<RealSpace3D_ManageLicense>();
        window.position =                   new Rect(Screen.width/2, Screen.height/2, 360, 200);

        window.ShowPopup();
    }
        
    void OnGUI()
    {     
        if(bDoOnce)
        {
            // do once when the window becomes active...otherwise OnGUI will call this repeatedly

            sDataPath =             Application.persistentDataPath;

            bLicenseSetup =         _theSwitcher.GetSetupActivated(sDataPath);

            int nResult =           _theSwitcher.GetLicenseActivated(sDataPath, vsMODE.EDITOR, vsPLATFORMS.NONE);

            if(nResult ==           (int) vsSTATUS_DNA.kSUCCESS)
                bLicenseActivated = true;
            else
                bLicenseActivated = false;
            
            bButtonActivate =       _theSwitcher.GetButtonActivated(sDataPath);
            bButtonDeactivate =     _theSwitcher.GetButtonDeactivated(sDataPath);
            bButtonReactivate =     _theSwitcher.GetButtonReactivated(sDataPath);
          
            bDoOnce = false;
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        for(int i = 0; i < 2; i++)
            EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("RealSpace3D License Management");

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();

        for(int i = 0; i < 5; i++)
            EditorGUILayout.Space();

        if(!bButtonActivate && !bButtonReactivate)
            GUI.enabled = false;
        
        sActivationCode = EditorGUILayout.TextField(new GUIContent("Enter Activation Code: ", "Enter your RealSpace3D Activation Code"), sActivationCode);

        GUI.enabled = true;

        EditorGUILayout.EndVertical();

        GUILayout.Space(70);

        EditorGUILayout.BeginHorizontal();

        GUI.enabled = bButtonActivate;

        if(GUILayout.Button("Activate Key", GUILayout.Width(100)))
        {
            if(!bLicenseActivated)
            {
                if(!bLicenseSetup)
                {
                    // get the platform that is being used
                    if (Application.platform == RuntimePlatform.WindowsEditor)
                        sPlatform = "WIN";

                    if (Application.platform == RuntimePlatform.OSXEditor)
                        sPlatform = "MAC";                    

                    int nOSBitSize = _theSwitcher.GetProcessorBitSize();

                    _theSwitcher.DoSetup(sPlatform, sDataPath, nOSBitSize);
                }

                if(VerifyLicenseKeyEntry() == false)
                    return;

                // this password is an internal password for RS3D..screwing w/ this 
                // well it's your money lost and project not generating 3D Auido
                // you've been warned 
                sPassword = ReverseString(sActivationCode);

                sNewPassword = Guid.NewGuid().ToString().Substring(0, 10);

                int nResult = _theSwitcher.SetActivationCode(sDataPath, sActivationCode, sPassword, sNewPassword);

                if(nResult != 0)
                {
                    string sErrorMsg = theErrorMsg.GetErrorMsg((vsSTATUS_DNA)nResult);
                    Debug.LogError(sErrorMsg); 
                    DisplayResult(sErrorMsg);

                    if(nResult == (int)vsSTATUS_DNA.kSTATUS_REACTIVATION_EXPECTED || nResult == (int)vsSTATUS_DNA.kSTATUS_REACTIVATION_EXPECTED_MU)
                        bButtonReactivate = true;
                }

                else
                {
                    Debug.Log("Your RS3D Audio Authoring license has been activated along with your initial free commercial platform release."); 
                    Debug.LogWarning("Reminder: For additional commercial plaform releases, a run-time library license must be purchased. Visit www.realspace3daudio.com for details.");
                    this.Close();
                }
            }

            else
            {
                Debug.Log("Your RS3D Audio License key is already activated.");
            }
        }
            
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUI.enabled = bButtonDeactivate;
            
        if(GUILayout.Button("De-Activate Key", GUILayout.Width(100)))
        {
            int nResult = _theSwitcher.DoDeactivation(sDataPath);

            if(nResult != 0)
            {
                string sErrorMsg = theErrorMsg.GetErrorMsg((vsSTATUS_DNA)nResult);
                Debug.LogError(sErrorMsg); 
                DisplayResult(sErrorMsg);
            }

            else
            {
                Debug.Log("Success: Your RS3D Audio License key is now deactivated.");

                this.Close();
            }        
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUI.enabled = bButtonReactivate;

        if(GUILayout.Button("Re-Activate Key", GUILayout.Width(100)))
        {
            if(VerifyLicenseKeyEntry() == false)
                return;

            int nResult = _theSwitcher.DoReactivation(sDataPath, sActivationCode);

            if(nResult != 0)
            {
                string sErrorMsg = theErrorMsg.GetErrorMsg((vsSTATUS_DNA)nResult);
                Debug.LogError(sErrorMsg); 
                DisplayResult(sErrorMsg);
            }

            else
            {
                Debug.Log("Success: Your RS3D Audio license key has been re-activated.");

                this.Close();
            }        
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        GUI.enabled = true;

        if(GUILayout.Button("Cancel", GUILayout.Width(100)))
        {
            this.Close();
        }

        EditorGUILayout.EndHorizontal();
    }

    void DisplayResult(string sMessage)
    {
        EditorUtility.DisplayDialog("RealSpace3D Copyright 2011 - 2016", sMessage, "Ok");
    }

    string ReverseString(string s)
    {
        char[] arr = s.ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }

    bool VerifyLicenseKeyEntry()
    {
        if(sActivationCode == "Please enter code here" || sActivationCode == "")
        {
            string sErrorMsg = theErrorMsg.GetErrorMsg(vsSTATUS_DNA.kSTATUS_INVALID_ACTIVATION_CODE);
            Debug.LogError(sErrorMsg); 
            DisplayResult(sErrorMsg);
            return false;
        }

        return true;
    }
}