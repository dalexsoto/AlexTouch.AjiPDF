using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

	partial class APPDFInformation
	{
		/// <summary>
		/// Unknown APPDFInformation version
		/// </summary>
		public static readonly uint kAPPDFInformationVersionUnknown = 0x00000000;
		
		/// <summary>
		/// APPDFInformation Version 5
		/// </summary>
		public static readonly uint kAPPDFInformationVersion5 = 0x00050000;
		
		/// <summary>
		/// The current version of APPDFInformation files.
		/// </summary>
		public static readonly uint kAPPDFInformationCurrentVersion = kAPPDFInformationVersion5;
	}

	partial class kAPProcessor
	{
		public static readonly string kAPProcessorErrorDomain ="AjiPDFLib_APProcessor";
	}