using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

namespace AlexTouch.AjiPDF
{
	public enum APPDFDocumentPermissions
	{
		/// <summary>
		/// Document can only be read/viewed; no further permissions are granted.
		/// </summary>
	    ReadOnly = 0,
		
		/// <summary>
		/// Document allows text selection (copy/paste).
		/// </summary>
	    AllowsTextSelection = 1 << 0,
	    
	    /// <summary>
	    /// Document allows annotation and inserting bookmarks.
	    /// </summary>
	    AllowsAnnotation = 1 << 1,
	
	    /// <summary>
	    /// Document allows "assembly" (including inserting bookmarks).
	    /// </summary>
		AllowsAssembly = 1 << 2,
	
	    /// <summary>
	    /// Document allows printing (both high and low quality)
	    /// </summary>
	    AllowsPrinting = 1 << 3,
	    
	    /// <summary>
	    /// Grants all permissions on the Document.
	    /// </summary>
	    All = (	AllowsTextSelection | 
				AllowsAnnotation | 
				AllowsAssembly | 
				AllowsPrinting	)
	}
	
	/// <summary>
	/// The password type required for processing or writing annotations to a PDF Document.
	/// </summary>
	public enum APPDFPasswordType
	{
		/// <summary>
		/// The User Password allows restricted access to the PDF. Actual permissions granted depends on the document.
		/// </summary>
	    Any = 1,
	
	    /// <summary>
	    /// The Owner Password allows unrestricted access to the PDF.
	    /// </summary>
	    Owner = 2
	}
	
	/// <summary>
	/// Options controlling PDF processing behavior.
	/// </summary>
	public enum APPDFProcessingOptions
	{
		/// <summary>
		/// Use default options when processing the PDF.
		/// </summary>
	    Default = 0
	}
	
	/// <summary>
	/// Options controlling PDF generation behavior.
	/// </summary>
	public enum APPDFWriteAnnotationOptions
	{
		/// <summary>
		/// The default write options; just makes a copy of the base PDF.
		/// </summary>
	    CopyOriginal = 0,

		/// <summary>
		/// "Flattens" all annotations into the page content. Extra pages
	    /// may be generated and added to the document in order to
	    /// accomodate annotation text. Note that this will embed the
	    /// annotations directly into the document, so that they will not
	    /// be editable or removable.
		/// </summary>
	    Flatten = 1 << 0,
	
	    /// <summary>
	    /// Specifies that only annotated pages should be written.
	    /// </summary>
	    AnnotatedPagesOnly = 1 << 1,
	
		/// <summary>
		/// Specifies that all (user-editable) annotations should be
		/// stripped, leaving only the "original" PDF content.
		/// </summary>
	    StripUserAnnotations = 1 << 2,
		
		/// <summary>
		/// Indicates the the "pageRange" parameter should be used to
		/// control which pages of the PDF should be generated.
		/// </summary>
	    kAPPDFWriteOptionsUsePageRange = 1 << 3
	}
	
	/// <summary>
	/// Controls PDF document search.
	/// </summary>
	public enum APSearchDirection
	{
		/// <summary>
		/// Search for the next occurrence, forwards in the document.
		/// </summary>
	    Forward = 0,
	
		/// <summary>
		/// Search for the previous occurrence, backwards in the document.
		/// </summary>
	    Backward = 1
	}
	
	/// <summary>
	/// Supported annotation types.
	/// </summary>
	public enum APAnnotationType
	{
		/// <summary>
		/// Invalid or unspecified annotation type.
		/// </summary>
	    None = 0,
	
	    /// <summary>
	    /// Note annotation (see APText).
	    /// </summary>
	    Note = 1,
	
	    /// <summary>
	    /// Highlight annotation (see APTextMarkup).
	    /// </summary>
	    Highlight = 2,
		
		/// <summary>
		/// Underline annotation (see APTextMarkup).
		/// </summary>
	    Underline = 3,
	
	    /// <summary>
	    /// Strikeout annotation (see APTextMarkup).
	    /// </summary>
	    Strikeout = 4,
	
	    /// <summary>
	    /// Bookmark annotation (see APBookmark).
	    /// </summary>
	    Bookmark = 5,
	
	    /// <summary>
	    /// Ink annotation (see APInk).
	    /// </summary>
	    Ink = 6,
	
	    /// <summary>
	    /// Straight-line ink annotation (see APInk).
	    /// </summary>
	    StraightLine  = 7,
	
		/// <summary>
		/// Free-text (typewriter) annotation (see APFreeText).
		/// </summary>
		FreeText = 9,
		
		/// <summary>
		/// Photo annotation (see APFileAttachment).
		/// </summary>
		Photo = 11,
		
		/// <summary>
		/// File Attachment annotation (see APFileAttachment).
		/// </summary>
		Attachment = 12,
		
		/// <summary>
		/// Sound annotation (see APSound).
		/// </summary>
		Sound = 13
	}
	
	/// <summary>
	/// APPDFProcessor error codes.
	/// 
 	/// Please note that you may want to implement the [APPDFProcessor
 	/// pdfProcessor:reportProcessingLog:forPDF:] delegate method for more
 	/// details if you are getting one of these error codes. (Please
 	/// include this information if you need to contact support about an
 	/// error.)
	/// </summary>
	public enum APProcessorError
	{
		/// <summary>
		/// kAPProcessorErrorDomain Error Code 1.
		/// 
		/// Invalid processing options were provided to the APPDFProcessor.
		/// </summary>
	    InvalidProcessingOptions = 1,
	
		/// <summary>
		/// kAPProcessorErrorDomain Error Code 2.
		/// 
		/// Invalid write options were provided to the APPDFProcessor.
		/// </summary>
	    InvalidWriteOptions = 2,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 3.
		/// 
		/// Some or all elements of the PDF outline could not be loaded
		/// from the PDF.
		/// </summary>
	    SomeOutlineElementsFailed = 3,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 4.
		/// 
		/// Some or all of the Aji Bookmarks could not be loaded from or
		/// written to the PDF outline.
		/// </summary>
	    SomeBookmarksFailed = 4,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 5.
		/// 
		/// Some or all of the PDF annotations could not be loaded from the
		/// PDF file.
		/// </summary>
	    SomeAnnotationsFailed = 5,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 6.
		/// 
		/// The PDF file appears to be invalid/corrupt.
		/// </summary>
	    InvalidPDF = 6,
	
		/// <summary>
		/// kAPProcessorErrorDomain Error Code 7.
		/// 
		/// There was an error parsing the PDF when attempting to extract
		/// information about the text.
		/// </summary>
	    ProcessingPDFText = 7,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 8.
		/// 
		/// There was an error writing the annotations back to the PDF
		/// document.
		/// </summary>
	    WritingAnnotations = 8,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 9.
		/// 
		/// There was an error updating existing annotations in the PDF
		/// file.
		/// </summary>
	    UpdatingWrittenAnnotations = 9,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 10.
		/// 
		/// The PDF has already been processed.
		/// </summary>
	    AlreadyProcessed = 10,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 11.
		/// 
		/// An internal logic error occurred.
		/// </summary>
	    Internal = 11,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 12.
		/// 
		/// The PDF password provided does not give sufficient permissions
		/// for the requested operation.
		/// </summary>
	    Permissions = 12,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 13.
		/// 
		/// An incorrect PDF password was provided.
		/// </summary>
	    InvalidDocumentPassword = 13,

		/// <summary>
		/// kAPProcessorErrorDomain Error Code 14.
		/// 
		/// The operation failed due to a cancellation request.
		/// </summary>
	    Cancelled = 14
	}

	public enum APAnnotationFlags_e
	{
		None            = 0x000,
		Invisible       = 0x001,
		Hidden          = 0x002, 
		Print           = 0x004, 
		NoZoom          = 0x008,
		NoRotate        = 0x010,
		NoView          = 0x020,
		ReadOnly        = 0x040,
		Locked          = 0x080,
		ToggleNoView    = 0x100,
		LockedContents  = 0x200
	}
	
	public enum APColorType
	{
		RGB,
		CMYK,   
		BW	
	}
	
	public enum APDestinationFitType
	{
		XYZ,
		Fit,
		FitH,
		FitV,
		FitB,
		FitBH,
		FitBV,
		FitR	
	}
	
	public enum APLineEndingStyle
	{
		None,
		Square,
		Circle,
		Diamond,
		OpenArrow,
		ClosedArrow,
		Butt,
		ROpenArrow,
		RClosedArrow,
		Slash
	}
	
	public enum APTextMarkupType
	{
		Highlight,
   		Underline,
		StrikeOut
	}
	
	public enum APSoundEncoding
	{
		Raw,
		Signed,
		MuLaw,
   		ALaw	
	}
	
	public enum APSoundIcon
	{
		Speaker,
   		Mic	
	}
	
	public enum APFreeTextJustification
	{
		Left,
		Right,
		Center	
	}
	
}