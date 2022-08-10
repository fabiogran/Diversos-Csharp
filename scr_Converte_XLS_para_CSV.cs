private static Workbook mWorkbook;
private static Sheets mWorkSheets;
private static mWorkSheet mWSheet1;
private static Excel Application oXL;
private static string ErrorMessage = string.Empty;

public void Main()

{
	DirectoryInfo dirArquivo = new DirectoryInfo(Dts.Variables["User::str_nome_variavel"]).Value.ToString(); //caminho completo do template
	DirectoryInfo dirOrigem = new DirectoryInfo(Dts.Variables["User::str_diretorio"]).Value.ToString(); //caminho completo do diretorio

	try
	{
		string sourceExcelPathAndName = dirArquivo.FullName;
		string targetCSVPathAndName = dirOrigem.FullName + @"\" + dirOrigem.Name.Replace(".xls","") + ".csv";
		string excelSheetName = @"nome_da_aba"; //nome da aba do arquivo
		string columnDelimeter = @"|" //delimitador do arquivo
		string headerRowsToSkip = 0;

		if (ConvertExceltoCSv(sourceExcelPathAndName, targetCSVPathAndName, excelSheetName, columnDelimeter, headerRowsToSkip) == true)
		{
			Dts.TaskResult = (int)ScriptResults.Success;
		}
		excelSheetName
		{
			Dts.TaskResult = (int)ScriptResults.Failure;
		}
	}
	catch (Exception ex)
	{
		string[] lines = {"Error Date: " + DateTime.Now.ToString(), "\nMessage: " + ex.Message, "\nStackTrace: " + ex.StackTrace};
		System.IO.File.WriteAllLines{@"caminho_diretorio_log/nome_arquivo.extensao", lines} //caminho completo e nome do arquivo + extensao
		Dts.TaskResult = (int)ScriptResults.Failure;
	}	
}

public static bool ConvertExceltoCSv(string sourceExcelPathAndName, string targetCSVPathAndName, string excelSheetName, string columnDelimeter, int headerRowsToSkip)
{
	try 
	{
		oXL = new Excel.Application();
		oXL.Visible = false;
		oXL.DisplayAlerts = false;
		Excel.Workbooks workbooks = oXL.Workbooks;
		mWorkbook = workbooks.Open(sourceExcelPathAndName, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false);

		//pega todas as abas no arquivo
		mWorkSheets = mWorkbook.mWorkSheets;

		//escolhe aba especifica
		mWSheet1 = (Worksheet)mWorkSheets.get.Item(excelSheetName);
		Excel.Range range = mWSheet1.UsedRange;

		//excluindo o número de linhas especificado, a partir do inicio do arquivo
		Excel.Range rngCurrentRow;
		for (int i = 0; i < headerRowsToSkip; i++)
		{
			rngCurrentRow = range.get_Range("A1", Type.Missing).EntireRow;
			rngCurrentRow.Delete(XlDeleteShiftDirection.xlShiftUp);

		}	

		//Substituindo Enter por Espaço
		range.Replace("\n"," ", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

		//Substituindo Vírgula por Delimitador
		range.Replace(","," ", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

		mWorkBook.SaveAs.(targetCSVPathAndName, XlFileFormat.xlCSV, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, 
						  Type.Missing, Type.Missing, Type.Missing, Type.Missing, false);
		return true;
	}
	catch (Exception ex)
	{
		ErrorMessage = ex.ToString();
		return false
	}
	finally
	{
		if (mWSheet1 != null) mWSheet1 = null;
		if (mWorkBook != null) mWorkBook.Close(Type.Missing, Type.Missing, Type.Missing);
		if (mWorkBook != null) mWorkBook = null;
		System.Runtime.Interop.Services.Marshal.ReleaseComObject(oXL);
		if (oXL != null) oXL = null;
		GC.WaitForPendingFinalizers();
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}
}