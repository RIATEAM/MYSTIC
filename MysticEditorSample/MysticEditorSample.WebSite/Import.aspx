<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Import.aspx.cs" Inherits="MysticEditorSample.WebSite.Import" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DocToHelpNew</title>
    <link rel="stylesheet" href="stile.css" type="text/css">
    <style type="text/css">
        .txtantracite
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 10px;
            font-weight: normal;
            color: #858585;
            text-decoration: none;
        }
    </style>

    <script type="text/javascript">	

	function StartUpload() {

	    if (document.insdoc.tbxFileName.value == "") {
	       	        
	        alert("Inserisci il nome del file da convertire.");
	        return false;
	    }	   
		
		if (document.insdoc.TextBox1.value == "") {
	       	        
	        alert("Inserisci il titolo del file da convertire.");
	        return false;
	    }
		//Using try...catch to prevent java script error if ActiveX generates error
	    try {	        

	        //setto il paramtero server
	        ActiveXPowUpload.Server = top.location.host;
	        ActiveXPowUpload.Upload();

		}
		catch(err) {
		    return false;
		}

		document.getElementById("step2").style.display = 'none';
		document.getElementById("step3").style.display = 'none';
		document.getElementById("LbAttendi").innerHTML = "Compilazione in corso. Attendere prego....";
		return true;
	}
	
	function ShowServerResponse()
	{
		var responselable = document.getElementById("serverresponse");
		responselable.innerHTML = "Response code " + ActiveXPowUpload.GetServerStatusCode();
		responselable.innerHTML += " " + ActiveXPowUpload.GetServerStatusText();
		responselable.innerHTML += "<br>" + ActiveXPowUpload.GetServerReply(65001);		
	}
    </script>

    <script for="ActiveXPowUpload" event="OnUploadClicked()" language="JavaScript">		
		ActiveXPowUpload.RemoveAllFormItems();
		var includeSelectedPath = document.getElementById("UploadStructure").checked;
		var FileNo=0;
		for(i=0; i<ActiveXPowUpload.FileListItemCount; i++)
		{
			var file = ActiveXPowUpload.GetItem(i);
			if(!file.IsFile)
				continue;
			ActiveXPowUpload.AddFormItem("CreationTime_" + FileNo, file.CreationTime);
			ActiveXPowUpload.AddFormItem("LastAccessTime_" + FileNo, file.LastAccessTime);
			ActiveXPowUpload.AddFormItem("LastWriteTime_" + FileNo, file.LastWriteTime);
			
			if(includeSelectedPath)			    
				ActiveXPowUpload.AddFormItem("SelectedPath_" + FileNo, file.SelectedPath);
			++FileNo;
		}
		if(includeSelectedPath)
			ActiveXPowUpload.AddFormItem("UploadStructure", "yes");
    </script>

    <script for="ActiveXPowUpload" event="OnUploadCompletedSuccessfully()" language="JavaScript">
	ShowServerResponse();
	
    </script>

    <script for="ActiveXPowUpload" event="OnServerError()" language="JavaScript">
	ShowServerResponse();
    </script>

</head>
<body style="background-color: #EAEBEC; margin: 10px;">
    <noscript>
        <!-- This message will be shown if Java Scripts disabled in user browser -->
        This sample requires JavaScript enabled!<br>
        You can allow Scripting (Java Scripts) in your browser at <em>Tools-> Internet options->
            Security tab-> Custom level...-> Scripting-> Active scripting-> Check "Enable"</em>.
        <br>
        <br>
    </noscript>
    <input type="hidden" id="ServerPath" value="127.0.0.1" />
    <div align="center" style="width: 100%; height: 250px;">
        <span class="txtantracite"><strong>Step 1</strong>
            <br />
            Clicca con il tasto destro nell'area sottostante per selezionare la cartella da
            caricare sul server<br />
            La cartella deve contenere il documento salvato in MS Word 2007 nel formato "Pagina
            Web Filtrata" </span>
        <div id="serverresponse" style="visibility: hidden; height: 0px;" class="txtantracite">
            <input type="checkbox" id="UploadStructure" checked="checked" visible="true" />
        </div>
        <object height="200" width="510" id="ActiveXPowUpload" codebase="XpowUploader/ActiveXPowUpload.cab"
            classid="CLSID:FB98CEED-9DE1-4517-B30C-CDA19C6D150B">
            <!--<param name="Script" value="FileProcessingScripts/DirectoryUpload.aspx" />-->
            <param name="Script" value="/XpowUploader/fileProcessingScripts/DirUpload.aspx" />
            <param name="Server" value="localhost:2531" />
            <param name="SerialKey" value="1511652350113254927402510167" />
        </object>
    </div>
    <!-- "Click to activate" workaround script.  -->

    <script type="text/javascript" src="../XpowUploader/scripts/ActivateActiveControlWorkaround.js"></script>

    <form id="insdoc" runat="server">
    <table id="step2" runat="server" width="100%">
        <tr>
            <td align="center">
                <span class="txtantracite"><strong>Step 2</strong>
                    <br />
                    Inserisci il nome del documento caricato nel box seguente </span>
            </td>
             <td align="center">
                <span class="txtantracite"><strong>Step 3</strong>
                    <br />
                    Inserisci il titolo del documento nel box seguente </span>
            </td>
        </tr>
        <tr>
            <td align="center" height="40">
                <span class="txtantracite">Nome File :</span>
                <asp:TextBox CssClass="form" ID="tbxFileName" runat="server" Text="documento.htm"
                    Width="190px" Height="21px"></asp:TextBox>
            </td>
            <td align="center" height="40">
                <span class="txtantracite">Titolo File :</span>
                <asp:TextBox CssClass="form" ID="TextBox1" runat="server" Text="" Width="190px" 
                    Height="21px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table id="step3" runat="server" width="100%">
        <tr>
            <td align="center">
                <span class="txtantracite"><strong>Step 4</strong>
                    <br />
                    Clicca sul pulsante "Conferma" per avviare la compilazione del documento.
                    <br />
                    Attendi il messaggio "Compilazione completata" per chiudere la finestra </span>
            </td>
        </tr>
        <tr>
            <td align="center" height="40">
                <span class="txtantracite">
                    <asp:Button ID="Upload" runat="server" Text="Conferma" OnClick="Upload_Compila" CssClass="formbot"
                        Style="width: 150px;" />
                </span>
            </td>
        </tr>
    </table>
    <table id="step4" runat="server" width="100%">
        <tr>
            <td align="center">
                <span class="txtantracite">
                    <label id="LbAttendi" runat="server" text="">
                    </label>
                </span>
            </td>
        </tr>
        <tr>
            <td align="center" height="40">
                <span class="txtantracite">
                    <asp:Button ID="Close" Visible="false" runat="server" Text="Torna Indietro" CssClass="formbot"
                        Style="width: 150px;" PostBackUrl="~/Default.aspx" />
                </span>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
