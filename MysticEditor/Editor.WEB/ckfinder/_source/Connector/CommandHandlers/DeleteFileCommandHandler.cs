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

using System;
using System.Web;
using System.Xml;
using System.Globalization;

namespace CKFinder.Connector.CommandHandlers
{
	internal class DeleteFileCommandHandler : XmlCommandHandlerBase
	{
		public DeleteFileCommandHandler()
			: base()
		{
		}

		protected override void BuildXml()
		{
			if ( !this.CurrentFolder.CheckAcl( AccessControlRules.FileDelete ) )
			{
				ConnectorException.Throw( Errors.Unauthorized );
			}

			string fileName = Request["FileName"];

			if ( !Connector.CheckFileName( fileName ) || Config.Current.CheckIsHiddenFile( fileName ) )
			{
				ConnectorException.Throw( Errors.InvalidRequest );
				return;
			}

			if ( !this.CurrentFolder.ResourceTypeInfo.CheckExtension( System.IO.Path.GetExtension( fileName ) ) )
			{
				ConnectorException.Throw( Errors.InvalidRequest );
				return;
			}

			string filePath = System.IO.Path.Combine( this.CurrentFolder.ServerPath, fileName );

			bool bDeleted = false;

			if ( !System.IO.File.Exists( filePath ) )
				ConnectorException.Throw( Errors.FileNotFound );

			try
			{
				System.IO.File.Delete( filePath );
				bDeleted = true;
			}
			catch ( System.UnauthorizedAccessException )
			{
				ConnectorException.Throw( Errors.AccessDenied );
			}
			catch ( System.Security.SecurityException )
			{
				ConnectorException.Throw( Errors.AccessDenied );
			}
			catch (System.ArgumentException)
			{
				ConnectorException.Throw( Errors.FileNotFound );
			}
			catch ( System.IO.PathTooLongException )
			{
				ConnectorException.Throw( Errors.FileNotFound );
			}
			catch ( Exception )
			{
				ConnectorException.Throw( Errors.Unknown );
			}

			try
			{
				string thumbPath = System.IO.Path.Combine( this.CurrentFolder.ThumbsServerPath, fileName );
				System.IO.File.Delete( thumbPath );
			}
			catch { /* No errors if we are not able to delete the thumb. */ }

			if ( bDeleted )
			{
				XmlNode oDeletedFileNode = XmlUtil.AppendElement( this.ConnectorNode, "DeletedFile" );
				XmlUtil.SetAttribute( oDeletedFileNode, "name", fileName );
			}
		}
	}
}
