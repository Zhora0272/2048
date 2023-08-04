#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using static VTabs.Libs.VUtils;


namespace VTabs
{
    public static class VTabsMenuItems
    {
        public static bool addTabEnabled { get => EditorPrefs.GetBool("vTabs-addTabsEnabled", true); set => EditorPrefs.SetBool("vTabs-addTabsEnabled", value); }
        public static bool closeTabEnabled { get => EditorPrefs.GetBool("vTabs-closeTabsEnabled", true); set => EditorPrefs.SetBool("vTabs-closeTabsEnabled", value); }
        public static bool reopenTabEnabled { get => EditorPrefs.GetBool("vTabs-reopenTabsEnabled", true); set => EditorPrefs.SetBool("vTabs-reopenTabsEnabled", value); }
        public static bool dragndropEnabled { get => EditorPrefs.GetBool("vTabs-dragndropEnabled", true); set => EditorPrefs.SetBool("vTabs-dragndropEnabled", value); }

        public static bool shiftscrollSwitchTabEnabled { get => EditorPrefs.GetBool("vTabs-shiftscrollSwitchTabEnabled", true); set => EditorPrefs.SetBool("vTabs-shiftscrollSwitchTabEnabled", value); }
        public static bool shiftscrollMoveTabEnabled { get => EditorPrefs.GetBool("vTabs-shiftscrollMoveTabEnabled", true); set => EditorPrefs.SetBool("vTabs-shiftscrollMoveTabEnabled", value); }

        public static bool sidescrollSwitchTabEnabled { get => EditorPrefs.GetBool("vTabs-sidescrollSwitchTabEnabled", Application.platform == RuntimePlatform.OSXEditor); set => EditorPrefs.SetBool("vTabs-sidescrollSwitchTabEnabled", value); }
        public static bool sidescrollMoveTabEnabled { get => EditorPrefs.GetBool("vTabs-sidescrollMoveTabEnabled", Application.platform == RuntimePlatform.OSXEditor); set => EditorPrefs.SetBool("vTabs-sidescrollMoveTabEnabled", value); }
        public static float sidescrollSensitivity { get => EditorPrefs.GetFloat("vTabs-sidescrollSensitivity", 1); set => EditorPrefs.SetFloat("vTabs-sidescrollSensitivity", value); }

        public static bool fixPhantomScrollingEnabled { get => EditorPrefs.GetBool("vTabs-fixPhantomScrollingEnabled", Application.platform == RuntimePlatform.OSXEditor); set => EditorPrefs.SetBool("vTabs-fixPhantomScrollingEnabled", value); }





        const string menuDir = "Tools/vTabs/";

#if UNITY_EDITOR_OSX
        const string cmd = "Cmd";
#else
        const string cmd = "Ctrl";
#endif

        const string addTab = menuDir + "" + cmd + "-T to add tab";
        const string closeTab = menuDir + "" + cmd + "-W to close tab";
        const string reopenTab = menuDir + "" + cmd + "-Shift-T to reopen closed tab";
        const string dragndrop = menuDir + "Drag-and-drop objects or folders to create tabs";


        const string shiftscrollSwitchTab = menuDir + "Shift-Scroll to switch tabs";
        const string shiftscrollMoveTab = menuDir + "" + cmd + "-Shift-Scroll to move active tab";
        const string fixPhantomScrolling = menuDir + "Fix phantom scrolling";

        const string sidescrollSwitchTab = menuDir + "Sidescroll to switch tabs";
        const string sidescrollMoveTab = menuDir + "" + cmd + "-Sidescroll to move active tab";
        const string incSidescrollSens = menuDir + "Increase sensitivity";
        const string decSidescrollSens = menuDir + "Decrease sensitivity";


        const string disable = menuDir + "Disable vTabs";




        [MenuItem(menuDir + "Shortcuts", false, 1)] static void dadsas() { }
        [MenuItem(menuDir + "Shortcuts", true, 1)] static bool dadsas123() => false;

        [MenuItem(addTab, false, 1)] static void dadsadadsas() => addTabEnabled = !addTabEnabled;
        [MenuItem(addTab, true, 1)] static bool dadsaddasadsas() { Menu.SetChecked(addTab, addTabEnabled); return true; }

        [MenuItem(closeTab, false, 1)] static void dadsadasdadsas() => closeTabEnabled = !closeTabEnabled;
        [MenuItem(closeTab, true, 1)] static bool dadsadsaddasadsas() { Menu.SetChecked(closeTab, closeTabEnabled); return true; }

        [MenuItem(reopenTab, false, 1)] static void dadsadsadasdadsas() => reopenTabEnabled = !reopenTabEnabled;
        [MenuItem(reopenTab, true, 1)] static bool dadsaddsasaddasadsas() { Menu.SetChecked(reopenTab, reopenTabEnabled); return true; }

        [MenuItem(dragndrop, false, 1)] static void dadsadsadasdsadadsas() => dragndropEnabled = !dragndropEnabled;
        [MenuItem(dragndrop, true, 1)] static bool dadsaddsasadadsdasadsas() { Menu.SetChecked(dragndrop, dragndropEnabled); return true; }



        [MenuItem(menuDir + "Shift-Scroll", false, 12)] static void daadsdsas() { }
        [MenuItem(menuDir + "Shift-Scroll", true, 12)] static bool dadsasads() => false;

        [MenuItem(shiftscrollSwitchTab, false, 12)] static void dadsadsadsadsadasdsadadsas() => shiftscrollSwitchTabEnabled = !shiftscrollSwitchTabEnabled;
        [MenuItem(shiftscrollSwitchTab, true, 12)] static bool dadsadasdasddsasadadsdasadsas() { Menu.SetChecked(shiftscrollSwitchTab, shiftscrollSwitchTabEnabled); return true; }

        [MenuItem(shiftscrollMoveTab, false, 12)] static void dadsadsadsadasdsadadsas() => shiftscrollMoveTabEnabled = !shiftscrollMoveTabEnabled;
        [MenuItem(shiftscrollMoveTab, true, 12)] static bool dadsadasddsasadadsdasadsas() { Menu.SetChecked(shiftscrollMoveTab, shiftscrollMoveTabEnabled); return true; }



        [MenuItem(menuDir + "Sidescroll", false, 123)] static void daadsdsadsas() { }
        [MenuItem(menuDir + "Sidescroll", true, 123)] static bool dadsasasdads() => false;

        [MenuItem(sidescrollSwitchTab, false, 123)] static void dadsadsadsadsadasdadssadadsas() => sidescrollSwitchTabEnabled = !sidescrollSwitchTabEnabled;
        [MenuItem(sidescrollSwitchTab, true, 123)] static bool dadsadasdasddsadassadadsdasadsas() { Menu.SetChecked(sidescrollSwitchTab, sidescrollSwitchTabEnabled); return true; }

        [MenuItem(sidescrollMoveTab, false, 123)] static void dadsadsadsaddasasdsadadsas() => sidescrollMoveTabEnabled = !sidescrollMoveTabEnabled;
        [MenuItem(sidescrollMoveTab, true, 123)] static bool dadsadasddsaasdsadadsdasadsas() { Menu.SetChecked(sidescrollMoveTab, sidescrollMoveTabEnabled); return true; }

        [MenuItem(incSidescrollSens, false, 123)] static void qdadadsssa() { sidescrollSensitivity += .2f; Debug.Log("vTabs: sidescroll sensitivity increased to " + sidescrollSensitivity); }

        [MenuItem(decSidescrollSens, false, 123)] static void qdasadsssa() { sidescrollSensitivity -= .2f; Debug.Log("vTabs: sidescroll sensitivity decreased to " + sidescrollSensitivity); }



        [MenuItem(menuDir + "Troubleshooting", false, 1232)] static void daadsadsdsadsas() { }
        [MenuItem(menuDir + "Troubleshooting", true, 1232)] static bool dadsaadssasdads() => false;

        [MenuItem(fixPhantomScrolling, false, 1232)] static void dadsadsaadsdsadasdsadadsas() => fixPhantomScrollingEnabled = !fixPhantomScrollingEnabled;
        [MenuItem(fixPhantomScrolling, true, 1232)] static bool dadsadasdasdadsdsasadadsdasadsas() { Menu.SetChecked(fixPhantomScrolling, fixPhantomScrollingEnabled); return true; }

        [MenuItem(disable, false, 1232)] static void asdsasda() => ToggleDefineDisabledInScript(typeof(VTabs));
        [MenuItem(disable, true, 1232)] static bool adsassad() { Menu.SetChecked(disable, ScriptHasDefineDisabled(typeof(VTabs))); return true; }


    }
}
#endif