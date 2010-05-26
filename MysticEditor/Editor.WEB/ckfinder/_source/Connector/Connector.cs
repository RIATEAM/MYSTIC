/*
 * CKFinder
 * ========
 * http://ckfinder.com
 * Copyright (C) 2007-2010, CKSource - Frederico Knabben. All rights reserved.
 *
 * The software, this file and its contents are subject to the CKFinder
 * License. Please read the license.txt file before using, installing, copying,
 * modifying or distribute this file or part of its contents. The contents of
 * this file is part of the Source Code of CKFinder.
 */

// To disabled special debugging features, simply uncomment this line.
// #undef DEBUG

using System;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace CKFinder.Connector
{
	/// <summary>
	/// The class handles all requests sent to the CKFinder connector file,
	/// connector.aspx.
	/// </summary>
	public class Connector : Page
	{
		/// <summary>
		/// The "ConfigFile" object, is the instance of config.ascx, present in
		/// the connector.aspx file. It makes it possible to configure CKFinder
		/// whithout having to compile it.
		/// </summary>
		public Settings.ConfigFile ConfigFile;

		#region Disable ASP.NET features

		/// <summary>
		/// Theming is disabled as it interferes in the connector response data.
		/// </summary>
		public override bool EnableTheming
		{
			get { return false; }
			set { /* Ignore it with no error */ }
		}

		/// <summary>
		/// Master Page is disabled as it interferes in the connector response data.
		/// </summary>
		public override string MasterPageFile
		{
			get { return null; }
			set { /* Ignore it with no error */ }
		}

		/// <summary>
		/// Theming is disabled as it interferes in the connector response data.
		/// </summary>
		public override string Theme
		{
			get { return ""; }
			set { /* Ignore it with no error */ }
		}

		/// <summary>
		/// Theming is disabled as it interferes in the connector response data.
		/// </summary>
		public override string StyleSheetTheme
		{
			get { return ""; }
			set { /* Ignore it with no error */ }
		}

		#endregion

		protected override void OnLoad( EventArgs e )
		{
			// Set the config file instance as the current one (to avoid singleton issues).
			ConfigFile.SetCurrent();

			// Load the config file settings.
			ConfigFile.SetConfig();

#if (DEBUG)
			// For testing purposes, we may force the user to get the Admin role.
			// Session[ "CKFinder_UserRole" ] = "Admin";

			// Simulate slow connections.
			// System.Threading.Thread.Sleep( 2000 );
#endif
			CommandHandlers.CommandHandlerBase commandHandler = null;
			
			try
			{
				// Take the desired command from the querystring.
				string command = Request.QueryString["command"];

				if ( command == null )
					ConnectorException.Throw( Errors.InvalidCommand );
				else
				{
					// Create an instance of the class that handles the
					// requested command.
					switch ( command )
					{
						case "Init":
							commandHandler = new CommandHandlers.InitCommandHandler();
							break;

						case "GetFolders":
							commandHandler = new CommandHandlers.GetFoldersCommandHandler();
							break;

						case "GetFiles":
							commandHandler = new CommandHandlers.GetFilesCommandHandler();
							break;

						case "Thumbnail":
							commandHandler = new CommandHandlers.ThumbnailCommandHandler();
							break;

						case "CreateFolder":
							commandHandler = new CommandHandlers.CreateFolderCommandHandler();
							break;

						case "RenameFolder":
							commandHandler = new CommandHandlers.RenameFolderCommandHandler();
							break;

						case "DeleteFolder":
							commandHandler = new CommandHandlers.DeleteFolderCommandHandler();
							break;

						case "FileUpload":
							commandHandler = new CommandHandlers.FileUploadCommandHandler();
							break;

						case "QuickUpload":
							commandHandler = new CommandHandlers.QuickUploadCommandHandler();
							break;

						case "DownloadFile":
							commandHandler = new CommandHandlers.DownloadFileCommandHandler();
							break;

						case "RenameFile":
							commandHandler = new CommandHandlers.RenameFileCommandHandler();
							break;

						case "DeleteFile":
							commandHandler = new CommandHandlers.DeleteFileCommandHandler();
							break;

						default:
							ConnectorException.Throw( Errors.InvalidCommand );
							break;
					}
				}

				// Send the appropriate response.
				if ( commandHandler != null )
					commandHandler.SendResponse( Response );
			}
			catch ( ConnectorException connectorException )
			{
#if DEBUG
				// While debugging, throwing the error gives us more useful
				// information.
				throw connectorException;
#else
			    commandHandler = new CommandHandlers.ErrorCommandHandler( connectorException );
			    commandHandler.SendResponse( Response );
#endif
			}
		}

		internal static bool CheckFileName( string fileName )
		{
			if ( fileName == null || fileName.Length == 0 || fileName.StartsWith( "." ) || fileName.EndsWith( "." ) || fileName.Contains( ".." ) )
				return false;

			if ( Regex.IsMatch( fileName, @"[/\\:\*\?""\<\>\|\p{C}]" ) )
				return false;

			return true;
		}
	}
}
