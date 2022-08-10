

// MOVE ARQUIVO
public void Main()
{
	DirectoryInfo diretorio = new DirectoryInfo(Dts.Variables["User::nome_variavel"].Value.ToString()); //variavel deve conter o diretorio origem do arquivo
	string diretorioDestino = Dts.Variables["User::nome_variavel"].Value.ToString(); //variavel deve conter o diretorio de destingo

	foreach(FileInfo arquivo in diretorio.GetFiles("*nome_arquivo*.nome_extensao")) //definir nome dos arquivos se necessário e/ou extensao. Exemplo: "*vendas*.xlsx"
	{
		if(arquivo.Name.Contains("")) //palavra contida no nome do arquivo a ser movido
		{
			File.Move(arquivo.FullName, diretorioDestino + @"\" + arquivo.Name);
		}
	}

	Dts.TaskResult = (int)ScriptResults.Success;
}


// VERIFICA SE EXISTE ARQUIVO EM UMA PASTA
public void Main()
{
	DirectoryInfo diretorio = new DirectoryInfo(Dts.Variables["User::nome_variavel"].Value.ToString()); //variavel deve conter o diretorio origem do arquivo 

	foreach(FileInfo arquivo in diretorio.GetFiles("*nome_arquivo*.nome_extensao")) //definir nome dos arquivos se necessário e/ou extensao. Exemplo: "*vendas*.xlsx"
	{
		if(arquivo.Name.Contains("")) //palavra contida no nome do arquivo a ser movido
		{
			Dts.Variables["User::nome_variavel"].Value = true; //set variável booleana para indicar que existe arquivo
		}
	}

	Dts.TaskResult = (int)ScriptResults.Success;
}


//RENOMEIA ARQUIVO
public void Main()
{
	DirectoryInfo diretorio = new DirectoryInfo(Dts.Variables["User::nome_variavel"].Value.ToString()); //variavel deve conter o diretorio origem do arquivo 

	foreach(FileInfo arquivo in diretorio.GetFiles("*nome_arquivo*.nome_extensao")) //definir nome dos arquivos se necessário e/ou extensao. Exemplo: "*vendas*.xlsx"
	{
		string dataCompleta = File.GetLastWriteTime(arquivo.Fullname).ToString("yyyyMMdd") + "_"; //retorna ultima data de mofidicacao
							  DateTime.Now.ToString("yyyyMMdd") + "_"; //retorna data atual

		File.Move(arquivo.FullName, diretorio.Fullname + @"\" + dataCompleto + "Novo Nome do Arquivo.nome_extensao");
	}

	Dts.TaskResult = (int)ScriptResults.Success;

}


//ARGUMENTO ZIP PARA COMPACTACAO DE ARQUIVOS
public void Main()
{
	string diretorioCarregados = Dts.Variables["User:nome_variavel"].Value.ToString() + @"\nome_diretorio"; //variavel deve conter o diretorio origem do arquivo
	int contador = 1; 

	foreach(FileInfo arquivo in new DirectoryInfo(diretorioCarregados).GetFiles("*" + DateTime.Now.ToString("yyyyMMdd") + "*.zip"))
	{
		contador += 1;
	}

	string argumentoZip = "a -tzip " + diretorioCarregados + @"\" + DateTime.Now.ToString("yyyyMMdd") + "_" + contador + "nome_arquivo.zip" + diretorioCarregados + @"\*.nome_extensao";

	Dts.Variables["User::nome_variavel_argumento_zip"].Value = argumentoZip;

	Dts.TaskResult = (int)ScriptResults.Success;
}


//CRIAR ARQUIVO A PARTIR DE UM TEMPLATE
public void Main()
{
	try
	{
		string diretorio_raiz = Dts.Variables["User::nome_variavel"].Value.ToString() + @"\nome_diretorio"; //variavel deve conter o diretorio origem do arquivo
		string nome_parcial_arquivo = DateTime.Now.ToString("yyyyMMdd") + "nome_do_arquivo_X.nome_extensao"; 
		string arquivo_copiar = Dts.Variables["User::nome_variavel_template"].Value.ToString(); //variavel deve conter diretorio do arquivo template
		int contador = 0;

		DirectoryInfo directoryInfo = new DirectoryInfo(diretorio_raiz);
		foreach(FileInfo fileInfo in directoryInfo.GetFiles("*Carregado_" + DateTime.Now.ToString("yyyyMMdd") + "*"))
		{
			contador += 1;
		}

		nome_parcial_arquivo = nome_parcial_arquivo.Replace("X", contador.ToString());
		File.Copy(arquivo_copiar, diretorio_raiz + @"\" + nome_parcial_arquivo);
	}
	catch(Exception ex)
	{
		MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
		throw ex;
	}

	Dts.TaskResult = (int)ScriptResults.Success;
}

