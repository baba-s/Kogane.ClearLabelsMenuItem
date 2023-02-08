using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    internal static class ClearLabelsMenuItem
    {
        private const string MENU_ITEM_NAME = @"Assets/Kogane/Clear Labels";

        [MenuItem( MENU_ITEM_NAME, true )]
        private static bool CanCopy()
        {
            return Selection.assetGUIDs is { Length: > 0 };
        }

        [MenuItem( MENU_ITEM_NAME, false, 1142154112 )]
        private static void Copy()
        {
            var assetGUIDs = Selection.assetGUIDs;

            if ( assetGUIDs is not { Length: > 0 } ) return;

            try
            {
                AssetDatabase.StartAssetEditing();

                var assets = assetGUIDs
                        .Select( x => AssetDatabase.GUIDToAssetPath( x ) )
                        .Select( x => AssetDatabase.LoadAssetAtPath<Object>( x ) )
                        .ToArray()
                    ;

                foreach ( var asset in assets )
                {
                    AssetDatabase.ClearLabels( asset );
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}