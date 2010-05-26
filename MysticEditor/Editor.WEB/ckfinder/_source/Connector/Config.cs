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
using System.Collections.Generic;
using System.Text;

namespace CKFinder.Connector
{
	internal class Config
	{
		private static Config _Current;

		private Config()
		{ }

		public static Config Current
		{
			get
			{
				if ( _Current == null )
					_Current = new Config();
				return _Current;
			}
		}

		public bool CheckAuthentication()
		{
			return Settings.ConfigFile.Current.CheckAuthentication();
		}

		public Settings.Thumbnails Thumbnails
		{
			get { return Settings.ConfigFile.Current.Thumbnails; }
		}

		public Settings.Images Images
		{
			get { return Settings.ConfigFile.Current.Images; }
		}

		public string LicenseKey
		{
			get { return Settings.ConfigFile.Current.LicenseKey.PadRight( 32, ' ' ); }
		}

		public string LicenseName
		{
			get { return Settings.ConfigFile.Current.LicenseName; }
		}

		public string DefaultResourceTypes
		{
			get { return Settings.ConfigFile.Current.DefaultResourceTypes; }
		}

		public bool ForceSingleExtension
		{
			get { return Settings.ConfigFile.Current.ForceSingleExtension; }
		}

		public bool SecureImageUploads
		{
			get { return Settings.ConfigFile.Current.SecureImageUploads; }
		}

		public bool CheckSizeAfterScaling
		{
			get { return Settings.ConfigFile.Current.CheckSizeAfterScaling; }
		}

		public string RoleSessionVar
		{
			get { return Settings.ConfigFile.Current.RoleSessionVar; }
		}

		public Settings.ResourceTypeManager ResourceTypes
		{
			get { return Settings.ConfigFile.Current.ResourceType; }
		}

		public Settings.ResourceType GetResourceTypeConfig( string resourceTypeName )
		{
			return Settings.ConfigFile.Current.ResourceType.GetByName( resourceTypeName );
		}

		public Settings.AccessControlManager AccessControl
		{
			get { return Settings.ConfigFile.Current.AccessControl; }
		}

		internal bool CheckIsNonHtmlExtension( string extension )
		{
			string[] htmlExtensions = Settings.ConfigFile.Current.HtmlExtensions;

			return ( htmlExtensions.Length == 0 || !Util.ArrayContains( htmlExtensions, extension, System.Collections.CaseInsensitiveComparer.DefaultInvariant ) );
		}

		internal bool CheckIsHiddenFolder( string folderName )
		{
			return Settings.ConfigFile.Current.HideFoldersRegex.IsMatch( folderName );
		}

		internal bool CheckIsHiddenFile( string fileName )
		{
			return Settings.ConfigFile.Current.HideFilesRegex.IsMatch( fileName );
		}
	}
}
