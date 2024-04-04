using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class PerkData_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Excel/PerkData.xlsx";
    private static readonly string[] sheetNames = { "PerkData", };
    
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/Resources_moved/Excel/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Entity_Perk)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_Perk));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_Perk>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

					// check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                	// add infomation
                    for (int i=1; i<= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;
                        
                        var p = new Entity_Perk.Param();
			
					cell = row.GetCell(0); p.index = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.code = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.perkType = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.perkRarity = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.isReplicatable = (cell == null ? false : cell.BooleanCellValue);
					cell = row.GetCell(6); p.taskAmount = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.taskType = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.taskCondition = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.taskReturnAmount = (cell == null ? "" : cell.StringCellValue);

                        data.param.Add(p);
                    }
                    
                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }
}
