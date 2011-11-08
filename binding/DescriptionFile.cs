using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

/// <summary>
/// Ahe Aji PDF Library Namespace
/// see http://www.ajidev.com/iannotate/developers.html for more info
/// </summary>
namespace AlexTouch.AjiPDF
{
	/// <summary>
	/// The driver class for the Aji PDF Library. This must be created and
 	/// initialized before using any other API calls.
 	/// 
 	/// Create this object once per application session, and release it in
 	/// order to clean up the library and free associated resources. After
 	/// releasing the APLibrary object in this way, any calls to API
 	/// methods or use of API objects may fail.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APLibrary 
	{
		/// <summary>
		/// Initialize the Aji PDF Library. Must be called before any other API
		/// calls are made. Release the library to flush any caches and close
		/// any allocated resources; it is highly recommended to release the
		/// library handle before terminating the application.
		/// </summary>
		/// <returns>
		/// NSObject
		/// </returns>
		/// <param name='key'>
		/// The license key as provided per your license
		/// agreement. Please note that your license file must be included in
		/// the main resource bundle of your application, under the name
		/// "license.xml". For evaluation versions, pass null.
		/// </param>
		/// <param name='path'>
		/// The full path to a folder where the library should keep
		/// its data/cache information. This folder must exist, otherwise the
		/// default will be used. Pass null to use the default,
		/// which will be in the Library area.
		/// </param>
		[Export ("initWithLicenseKey:dataFolder:")]
		IntPtr Constructor ([NullAllowed] string key, [NullAllowed] string path);
		
		/// <summary>
		/// Used to access the version of the PDF Library.
		/// </summary>
		/// <returns>
		/// The version number of the PDF Library.
		/// </returns>
		[Export ("versionString")]
		string VersionString ();

	}
	
	/// <summary>
	/// Encapsulates a PDF file, including its "Information" file.
	///
	/// All PDF documents used with the API must be wrapped in an APPDFDocument
	/// object before use. This class includes a reference to the full path
	/// of the PDF file, and also a reference to its "Information", an
	/// instance of the APPDFInformation class.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APPDFDocument 
	{
		/// <summary>
		/// The full path to the PDF file (null if this is an in-memory PDF)
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		[Export ("path")] [NullAllowed]
		string Path { get;  }
		
		/// <summary>
		/// The in-memory bytes for this PDF file (null if this is a disk-based PDF).
		/// </summary>
		/// <value>
		/// The data.
		/// </value>
		[Export ("data")] [NullAllowed]
		NSData Data { get;  }
		
		/// <summary>
		/// The PDF Information file for this PDF file.
		/// </summary>
		/// <value>
		/// The information.
		/// </value>
		[Export ("information")]
		APPDFInformation Information { get;  }
		
		/// <summary>
		/// Constructor designated for path-based PDF.
		/// </summary>
		/// <param name='path'>
		/// The full path to the PDF file.
		/// </param>
		/// <param name='information'>
		/// The APPDFInformation for this PDF file.
		/// </param>
		[Export ("initWithPath:information:")]
		IntPtr Constructor (string path, APPDFInformation information);
		
		/// <summary>
		/// Convenience intializer which looks for the information file at the
		/// same location as the PDF file, but with appending ".metadata" to
		/// the filename. If the file is not found, it is created
		/// uninitialized. Note that the document will have to be processed
		/// before certain functionality will be enabled -- see APPDFProcessor
		/// for more information.
		/// </summary>
		/// <param name='path'>
		/// The full path to the PDF file.
		/// </param>
		[Export ("initWithPath:")]
		IntPtr Constructor (string path);
		
		/// <summary>
		/// Initializer for creating NSData-backed PDF.
		/// 
		/// Note that the corresponding information object will be created
		/// in-memory as well, so that no data is ever written or cached to
		/// disk. In this case, the information object cannot be cached; so the
		/// newly created APPDFDocument will have to be processed before
		/// text-based features will be available.
		/// 
		/// Note that the information object will be destroyed when this
		/// document is destroyed. So, if you wish to persist the annotations,
		/// you'll need to use APPDFProcessor to do the writeback (which will
		/// update the NSMutableData object) before releasing this object.
		/// 
		/// Please use care when using in-memory PDF, as the entire PDF, index
		/// (if processed), and all annotation objects will be loaded in memory
		/// -- the size of PDF you will be able to use will be limited by the
		/// available RAM on the device.
		/// </summary>
		/// <param name='data'>
		/// The PDF data.
		/// </param>
		[Export ("initWithPDFData:")]
		IntPtr Constructor (NSMutableData data);
		
		/// <summary>
		/// Used to determine if the document has encryption. Note that this is
		/// a static property of the PDF file. Also note that it is possible
		/// for a PDF document to be encrypted without a password; in this
		/// case, this method will return YES and APPDFDocument::isDecrypted
		/// will always return YES (and there is no need to explicitly decrypt
		/// the document).
		/// </summary>
		/// <returns>
		/// <c>true</c> If the PDF document is encrypted. otherwise, <c>false</c>.
		/// </returns>
		[Export ("isEncrypted")]
		bool IsEncrypted ();
		
		/// <summary>
		/// Used to determine if an encrypted PDF document has been decrypted
		/// (not applicable if the PDF is not encrypted). If the PDF is
		/// encrypted, this returns True if the password is empty, or if the
		/// document has been successfully decrypted using
		/// APPDFDocument::decryptWithPassword:.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the PDF document is encrypted and either has an
		/// empty password or has been successfully decrypted; otherwise, <c>false</c>.
		/// </returns>
		[Export ("isDecrypted")]
		bool IsDecrypted ();
		
		/// <summary>
		/// Use this method to decrypt an ecrypted PDF document with the
		/// indicated password.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the password was succesfully used to decrypt the Document
		/// </returns>
		/// <param name='password'>
		/// Password of the PDF Document
		/// </param>
		[Export ("decryptWithPassword:")]
		bool DecryptWithPassword (string password);
		
		/// <summary>
		/// The number of pages in the PDF. Note that this method will
		/// return 0 if the PDF is corrupt or if it is an encrypted PDF which
		/// has not yet been decrypted.
		/// </summary>
		/// <returns>
		/// The count of the pages on the PDF Document
		/// </returns>
		[Export ("pageCount")]
		uint PageCount ();
		
		/// <summary>
		/// The Crop Box rectangle for the PDF page (in page space coordinates).
		/// </summary>
		/// <returns>
		/// The Crop Box rectangle for the PDF page (in page space coordinates).
		/// </returns>
		/// <param name='page'>
		/// The PDF Page number.
		/// </param>
		[Export ("displayBoxForPage:")]
		RectangleF DisplayBoxForPage (uint page);

	}
	
	/// <summary>
	/// Extension of APPDFViewController that permits creation and editing of annotations.
	/// </summary>
	[BaseType (typeof (APPDFViewController))]
	interface APAnnotatingPDFViewController 
	{
		/// <summary>
		/// The delegate for callback notifications.
		/// </summary>
		/// <value>
		/// The delegate.
		/// </value>
//		[Wrap ("WeakDelegate")] 
//		APAnnotatingPDFViewDelegate Delegate { get; set; }
//
//		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
//		NSObject WeakDelegate { get; set; }
		
		/// <summary>
		/// Controls whether non-contiguous markup annotations (highlight,
		/// underline, strikeout) are allowed. Default is <c>false</c>, but set this
		/// property to <c>true</c> to disable.
		/// </summary>
		/// <value>
		/// <c>true</c> to disable non-contiguous markup annotations; otherwise, <c>false</c>.
		/// </value>
		[Export ("disableNoncontiguousMarkup", ArgumentSemantic.Assign)]
		bool DisableNoncontiguousMarkup { get; set;  }
		
		/// <summary>
		/// The 'User' to use when creating annotations. 
		/// If unspecified, a generic 'Unknown' author is used.
		/// </summary>
		/// <value>
		/// The annotation author.
		/// </value>
		[Export ("annotationAuthor", ArgumentSemantic.Retain)] [NullAllowed]
		string AnnotationAuthor { get; set;  }
		
		/// <summary>
		/// Used to enter "annotation mode" for the indicated annotation
		/// type. Subsequent touch interaction will be interpreted according to
		/// the type of annotation being created.
		/// </summary>
		/// <param name='type'>
		/// The type of annotation to be created.
		/// </param>
		[Export ("addAnnotationOfType:")]
		void AddAnnotationOfType (APAnnotationType type);
		
		/// <summary>
		/// For location-based annotations (such as notes), this will immediately create an annotation of the indicated type at the specified location.
		/// </summary>
		/// <param name='type'>
		/// The type of annotation to be created.
		/// </param>
		/// <param name='location'>
		/// The location at which the annotation should be created.
		/// </param>
		[Export ("addAnnotationOfType:atViewLocation:")]
		void AddAnnotationOfTypeatViewLocation (APAnnotationType type, PointF location);
		
		/// <summary>
		/// Used to obtain the text that is selected in an in-progress markup
		/// (highlight, underline, strikeout) annotation.
		/// </summary>
		/// <returns>
		/// The text for the current markup annotation, or
		/// null if no markup annotation is in progress or text is
		/// not yet selected.
		/// </returns>
		[Export ("selectedAnnotationText")] [NullAllowed]
		string SelectedAnnotationText ();
		
		/// <summary>
		/// Used to end "annotation mode" for multi-gesture annotations, such as highlights or ink annotations.
		/// </summary>
		[Export ("finishCurrentAnnotation")]
		void FinishCurrentAnnotation ();
		
		/// <summary>
		/// Used to cancel "anotation mode" for any type of annotation. No annotation will be created.
		/// </summary>
		[Export ("cancelAddAnnotation")]
		void CancelAddAnnotation ();
		
		/// <summary>
		/// Used to force all current annotation views to update; call this
		/// after any programmatic changes to existing annotations to have them 
		/// reflected in the PDF view.
		/// </summary>
		[Export ("updateAnnotationViews")]
		void UpdateAnnotationViews ();
		
		/// <summary>
		/// Used to force a programmatic refresh of all annotation views. Use
		/// this method after programmatically adding or removing annotations
		/// from the APPDFInformation associated with this view.
		/// </summary>
		[Export ("reloadAnnotationViews")]
		void ReloadAnnotationViews ();
		
		/// <summary>
		/// Used to permanently remove all user annotations from the PDF view
		/// (and underlying information object).
		/// </summary>
		[Export ("removeAllAnnotations")]
		void RemoveAllAnnotations ();
		
		/// <summary>
		/// Used to permanently remove all user annotations from the indicated
		/// page, in the PDF view and the underlying information object.
		/// </summary>
		/// <param name='pageIndex'>
		/// The 0-based page index of the page from which all
		/// annotations are to be removed.
		/// </param>
		[Export ("removeAnnotationsOnPage:")]
		void RemoveAnnotationsOnPage (uint pageIndex);
	}
	
	/// <summary>
	/// AP annotating PDF view delegate.
	/// </summary>
	[BaseType (typeof (APPDFViewDelegate))]
	[Model]
	interface APAnnotatingPDFViewDelegate 
	{
		/// <summary>
		/// Notification for when annotation mode begins.
		/// </summary>
		/// <param name='controller'>
		/// Controller.
		/// </param>
		/// <param name='type'>
		/// Type.
		/// </param>
		[Export ("pdfController:didEnterAnnotationMode:")]
		void PdfControllerdidEnterAnnotationMode (APAnnotatingPDFViewController controller, APAnnotationType type);
		
		/// <summary>
		/// Used to provide a default color for the annotation about to be created.
		/// </summary>
		/// <returns>
		/// The color to use, or null to use a default color.
		/// </returns>
		/// <param name='controller'>
		/// Controller.
		/// </param>
		/// <param name='annotType'>
		/// Annotation Type
		/// </param>
		[Export ("pdfController:colorForNewAnnotationOfType:")] [NullAllowed]
		UIColor PdfControllercolorForNewAnnotationOfType (APAnnotatingPDFViewController controller, APAnnotationType annotType);
		
		/// <summary>
		/// Notification for when annotation mode is aborted without creating an annotation.
		/// </summary>
		/// <param name='controller'>
		/// Controller.
		/// </param>
		/// <param name='type'>
		/// Type.
		/// </param>
		[Export ("pdfController:didEndAnnotationMode:")]
		void PdfControllerdidEndAnnotationMode (APAnnotatingPDFViewController controller, APAnnotationType type);
		
		/// <summary>
		/// Notification for when part of a Markup (highlight, underline,
		/// strikeout) or Ink annotation was updated by the user -- for
		/// example, a portion of text highlighted, a highlight handlebar
		/// dragged, or a stroke of the Ink added. The rect parameter indicates
		/// the bounding view rectangle of the annotation.
		/// </summary>
		/// <param name='controller'>
		/// Controller.
		/// </param>
		/// <param name='annotType'>
		/// Annot type.
		/// </param>
		/// <param name='rect'>
		/// Rect.
		/// </param>
		[Export ("pdfController:didUpdateNewAnnotationOfType:inRect:")]
		void PdfControllerdidUpdateNewAnnotationOfTypeinRect (APAnnotatingPDFViewController controller, APAnnotationType annotType, RectangleF rect);
		
		/// <summary>
		/// Notification for when an annotation is created.
		/// </summary>
		/// <param name='controller'>
		/// Controller.
		/// </param>
		/// <param name='annotation'>
		/// Annotation.
		/// </param>
		[Export ("pdfController:didCreateAnnotation:")]
		void PdfControllerdidCreateAnnotation (APAnnotatingPDFViewController controller, APAnnotation annotation);
		
		/// <summary>
		/// Notification for when an annotation is modified.
		/// </summary>
		/// <param name='controller'>
		/// Controller.
		/// </param>
		/// <param name='annotation'>
		/// Annotation.
		/// </param>
		[Export ("pdfController:didModifyAnnotation:")]
		void PdfControllerdidModifyAnnotation (APAnnotatingPDFViewController controller, APAnnotation annotation);
		
		/// <summary>
		/// Used to control whether a confirmation dialog is displayed when a
		/// user attempts to delete an anntoation. If this method is not
		/// implemented, the default is <c>true</c>.
		/// </summary>
		/// <returns>
		/// Return <c>true</c> is a confirmation dialog should be displayed, <c>false</c> to immediately delete the annotation.
		/// </returns>
		/// <param name='controller'>
		/// controller.
		/// </param>
		/// <param name='annotation'>
		/// annotation.
		/// </param>
		[Export ("pdfController:shouldDisplayConfirmationBeforeDeletingAnnotation:")]
		bool PdfControllershouldDisplayConfirmationBeforeDeletingAnnotation (APAnnotatingPDFViewController controller, APAnnotation annotation);
		
		/// <summary>
		/// Notification for when an annotation is deleted.
		/// </summary>
		/// <param name='controller'>
		/// Controller.
		/// </param>
		/// <param name='annotation'>
		/// Annotation.
		/// </param>
		[Export ("pdfController:didDeleteAnnotation:")]
		void PdfControllerdidDeleteAnnotation (APAnnotatingPDFViewController controller, APAnnotation annotation);
	}
	
	/// <summary>
	/// Represents a display color in RGBA components.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APRGBColor 
	{
		/// <summary>
		/// A value from 0.0 - 1.0 representing red component.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing red component.
		/// </value>
		[Export ("red")]
		float Red { get;  }
		
		/// <summary>
		/// A value from 0.0 - 1.0 representing green component.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing green component.
		/// </value>
		[Export ("green")]
		float Green { get;  }
		
		/// <summary>
		/// A value from 0.0 - 1.0 representing blue component.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing blue component.
		/// </value>
		[Export ("blue")]
		float Blue { get;  }
		
		/// <summary>
		/// A value from 0.0 - 1.0 representing the transparency.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing the transparency.
		/// </value>
		[Export ("alpha")]
		float Alpha { get;  }

	}
	
	/// <summary>
	/// Represents a display color in CMYK components.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APCMYKColor 
	{
		/// <summary>
		/// A value from 0.0 - 1.0 representing the cyan component.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing the cyan component.
		/// </value>
		[Export ("cyan")]
		float Cyan { get;  }
		
		/// <summary>
		/// A value from 0.0 - 1.0 representing the magenta component.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing the magenta component.
		/// </value>
		[Export ("magenta")]
		float Magenta { get;  }
		
		/// <summary>
		/// A value from 0.0 - 1.0 representing the yellow component.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing the yellow component.
		/// </value>
		[Export ("yellow")]
		float Yellow { get;  }
		
		/// <summary>
		/// A value from 0.0 - 1.0 representing the key.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing the key.
		/// </value>
		[Export ("key")]
		float Key { get;  }

	}
	
	/// <summary>
	/// Represents a grayscale color (optionally with alpha).
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APBWColor 
	{
		/// <summary>
		/// A value from 0.0 - 1.0 representing the white component.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing the white component.
		/// </value>
		[Export ("white")]
		float White { get;  }
		
		/// <summary>
		/// A value from 0.0 - 1.0 representing the transparency.
		/// </summary>
		/// <value>
		/// A value from 0.0 - 1.0 representing the transparency.
		/// </value>
		[Export ("alpha")]
		float Alpha { get;  }
	}
	
	/// <summary>
	/// Represents a color for a PDF object; effectively a union of the three possible color types.s
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APColor 
	{
		/// <summary>
		/// Indicates the type of color being used. In most cases, this will be an APRGBColor.
		/// </summary>
		/// <value>
		/// Indicates the type of color being used. In most cases, this will be an APRGBColor.
		/// </value>
		[Export ("colorType")]
		APColorType ColorType { get;  }
		
		/// <summary>
		/// The RGB color object for this annotation, only valid if the colorType is APColorType.RGB
		/// </summary>
		/// <value>
		/// The RGB color object for this annotation, only valid if the colorType is APColorType.RGB
		/// </value>
		[Export ("rgbColor")]
		APRGBColor RgbColor { get;  }
		
		/// <summary>
		/// The CMYK color object for this annotation, only valid if the colorType is APColorType.CMYK
		/// </summary>
		/// <value>
		/// The CMYK color object for this annotation, only valid if the colorType is APColorType.CMYK
		/// </value>
		[Export ("cmykColor")]
		APCMYKColor CmykColor { get;  }
		
		/// <summary>
		/// The greyscale color object for this annotation, only valid if the colorType is APColorType.BW
		/// </summary>
		/// <value>
		/// The greyscale color object for this annotation, only valid if the colorType is APColorType.BW
		/// </value>
		[Export ("bwColor")]
		APBWColor BwColor { get;  }
		
		[Static]
		[Export ("colorWithUIColor:")]
		APColor ColorWithUIColor (UIColor color);
		
		/// <summary>
		/// Used to obtain the UIColor object from the APColor.
		/// </summary>
		/// <returns>
		/// The color UIColor object from the APColor
		/// </returns>
		[Export ("UIColor")]
		UIColor UIcolor();

	}
	
	/// <summary>
	/// The base class for data objects (annotations and outline elements) associated with a PDF file.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APPDFObject
	{
		/// <summary>
		/// Gets or sets the color of the object; has different meanings in the context of various annotations.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		[Export ("color", ArgumentSemantic.Retain)]
		APColor Color { get; set;  }
	}
	
	/// <summary>
	/// The base class for all annotations in a PDF.
	/// </summary>
	[BaseType (typeof (APPDFObject))]
	interface APAnnotation 
	{	
		/// <summary>
		/// The 0-based PDF page upon which this annotation appears.
		/// This attribute is required when creating new annotation objects.
		/// </summary>
		/// <value>
		/// The 0-based PDF page.
		/// </value>
		[Export ("page", ArgumentSemantic.Assign)]
		uint Page { get; set;  }
			
		/// <summary>
		/// "Page Space" coordinates of the bounding rectangle of the annotation. 
		/// Page Space coordinates are in the domain 0.0 - 1.0 and represent fraction of a page's viewable area, 
		/// with an origin in the top-left corner and positive y-values moving down the page.
		/// 
		/// This attribute is required when creating new annotation objects.
		/// </summary>
		/// <value>
		/// Coordinates of the bounding rectangle of the annotation.
		/// </value>
		[Export ("rect", ArgumentSemantic.Assign)]
		RectangleF Rect { get; set;  }
		
		/// <summary>
		/// Bitmask specifying flags for what you can do to/with this annotation (see section 12.5.3 of the PDF standard). 
		/// </summary>
		/// <value>
		/// Bitmask specifying flags for what you can do to/with this annotation (see section 12.5.3 of the PDF standard). 
		/// </value>
		[Export ("flags")]
		uint Flags { get;  }
		
		/// <summary>
		/// Indication of when this annotation was last modified, in number of seconds since 1970.
		/// </summary>
		/// <value>
		/// Number of seconds since 1970.
		/// </value>
		[Export ("lastModified")]
		uint LastModified { get;  }

	}
	
	/// <summary>
	/// Data object for Bookmark annotations. 
	/// 
	/// Note that Bookmark annotations do not have a clear analog in the PDF annotation model. 
	/// When persisting to the PDF, Bookmarks are added to a special section in the PDF Outline.
	/// </summary>
	[BaseType (typeof (APAnnotation))]
	interface APBookmark 
	{	
		/// <summary>
		/// The Bookmark name.
		/// </summary>
		/// <value>
		/// The Bookmark name.
		/// </value>
		[Export ("name", ArgumentSemantic.Retain)]
		string Name {get; set;}
	}
	
	/// <summary>
	/// Used to represent a destination in the PDF document.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APInternalDestination 
	{
		/// <summary>
		/// See section 12.3.2.2 of the PDF standard for the meaning of fit type.
		/// </summary>
		/// <value>
		/// The type of the fit.
		/// </value>
		[Export ("fitType")]
		APDestinationFitType FitType { get;  }
		
		/// <summary>
		/// The 0-based page index.
		/// </summary>
		/// <value>
		/// The 0-based page index.
		/// </value>
		[Export ("page")]
		uint Page { get;  }
		
		/// <summary>
		/// A point in Page Space indicating the location of the destination.
		/// </summary>
		/// <value>
		/// A point in Page Space indicating the location of the destination.
		/// </value>
		[Export ("topLeftPt")]
		PointF TopLeftPt { get;  }
		
		/// <summary>
		/// A point in Page Space indicating the extent of the destination.
		/// </summary>
		/// <value>
		/// A point in Page Space indicating the extent of the destination.
		/// </value>
		[Export ("bottomRightPt")]
		PointF BottomRightPt { get;  }
	}
	
	/// <summary>
	/// Used to represent the destination of a link; can either be a URL or an internal destination.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APDestination 
	{
		/// <summary>
		/// For specifying 'external' destinations.  Cannot have an internalSpot set.
		/// </summary>
		/// <value>
		/// The URL.
		/// </value>
		[Export ("url")]
		string Url { get;  }
		
		/// <summary>
		/// For specifying document-internal (local) destinations.  Cannot have a URL set.
		/// </summary>
		/// <value>
		/// The internal spot.
		/// </value>
		[Export ("internalSpot")]
		APInternalDestination InternalSpot { get;  }

	}
	
	/// <summary>
	/// Used to represent a link in the PDF file.
	/// </summary>
	[BaseType (typeof (APAnnotation))]
	interface APLink 
	{
		/// <summary>
		/// The destination of the link.
		/// </summary>
		/// <value>
		/// The destination of the link.
		/// </value>
		[Export ("destination")]
		APDestination Destination { get;  }
	}
	
	/// <summary>
	/// Used to represent entries in the PDF Outline tree.
	/// </summary>
	[BaseType (typeof (APPDFObject))]
	interface APOutlineElement 
	{
		/// <summary>
		/// The title of the element.
		/// </summary>
		/// <value>
		/// The title of the element.
		/// </value>
		[Export ("title")]
		string Title { get;  }
		
		/// <summary>
		/// Indicates whether or not this is the (invisible) root Outline element.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is root; otherwise, <c>false</c>.
		/// </value>
		[Export ("isRoot")]
		bool IsRoot { get;  }
		
		/// <summary>
		/// The children of this Outline element.
		/// </summary>
		/// <value>
		/// The children of this Outline element.
		/// </value>
		[Export ("children")]
		NSArray children { get;  }
		
		/// <summary>
		/// The PDF destination of this element.
		/// </summary>
		/// <value>
		/// The PDF destination of this element.
		/// </value>
		[Export ("destination")]
		APDestination Destination { get;  }

		[Export ("childrenCount")]
		uint ChildrenCount ();
		
		//TODO: find a way to implement this, this crashes btout because there is a ReadOnly Prop called Children
//		[Export ("children:")]
//		APOutlineElement Children (uint index);

	}
	
	/// <summary>
	/// Base class for user markup annotations.
	/// </summary>
	[BaseType (typeof (APAnnotation))]
	interface APMarkupAnnotation 
	{
		/// <summary>
		/// The author of the annotation.
		/// 
		/// This attribute is required when creating new markup annotation objects.
		/// </summary>
		/// <value>
		/// The title of the annotation.
		/// </value>
		[Export ("title", ArgumentSemantic.Retain)]
		string Title {get; set;}
		
		/// <summary>
		/// Indication of when this annotation was created, in number of seconds since the reference date (see NSTimeInterval)
		/// </summary>
		/// <value>
		/// Indication of when this annotation was created, in number of seconds since the reference date (see NSTimeInterval)
		/// </value>
		[Export ("creationStamp")]
		uint CreationStamp { get;  }
		
		/// <summary>
		/// Text contents of the associated pop-up note.
		/// 
		/// This attribute is required when creating new markup annotation objects (it may be empty).
		/// </summary>
		/// <value>
		/// The contents of the associated pop-up note.
		/// </value>
		[Export ("contents", ArgumentSemantic.Retain)]
		string Contents { get; set;  }

	}
	
	/// <summary>
	/// Represents a popup (note) annotation.
	/// </summary>
	[BaseType (typeof (APMarkupAnnotation))]
	interface APText 
	{
		
	}
	
	/// <summary>
	/// Represents a point in the path of an ink annotation (in Page Space).
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APLocation 
	{
		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		/// <value>
		/// The location.
		/// </value>
		[Export ("location", ArgumentSemantic.Assign)]
		PointF Location { get; set;  }
	}
	
	/// <summary>
	/// Represents a path component in an ink annotation.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APPath 
	{
		/// <summary>
		/// Gets the list of points making the path.
		/// </summary>
		/// <value>
		/// The list of points making the path.
		/// </value>
		[Export ("points")]
		NSArray Points { get;  }

		[Export ("pointsCount")]
		uint PointsCount ();
		
		//TODO: Find a way to implement this, btouch goes kaboom because there is a prop called "points"
//		[Export ("points:")]
//		APLocation points (uint index);

		[Export ("addToPoints:")]
		void AddToPoints (APLocation points);

		[Export ("insertInPoints:atIndex:")]
		void InsertInPointsatIndex (APLocation points, uint index);

		[Export ("removeFromPoints:")]
		void RemoveFromPoints (APLocation points);

	}
	
	/// <summary>
	/// Represents a rectangular area (in Page Space) of a markup annotation.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APTextArea 
	{
		/// <summary>
		/// Gets or sets the area (in Page Space) of a markup annotation.
		/// </summary>
		/// <value>
		/// The area.
		/// </value>
		[Export ("area", ArgumentSemantic.Assign)]
		RectangleF Area { get; set; }
	}
	
	/// <summary>
	/// Annotation object for highlight, underline, and strike-out annotations.
	/// </summary>
	[BaseType (typeof (APMarkupAnnotation))]
	interface APTextMarkup 
	{
		/// <summary>
		/// Indicates the markup type of this annotation.
		/// 
		/// This attribute is required when creating new markup annotation objects.
		/// </summary>
		/// <value>
		/// The type of the text.
		/// </value>
		[Export ("textType", ArgumentSemantic.Assign)]
		APTextMarkupType TextType { get; set; }
		
		/// <summary>
		/// Rectangular areas, in page space, that are covered by the markup.
		/// 
		/// This attribute is required when creating new markup annotation objects.
		/// </summary>
		/// <value>
		/// The areas.
		/// </value>
		[Export ("areas")]
		NSArray Areas { get;  }
		
		/// <summary>
		/// If available, the text (in the document) that is marked-up.
		/// </summary>
		/// <value>
		/// The markup text.
		/// </value>
		[Export ("markupText", ArgumentSemantic.Retain)]
		string MarkupText { get; set;  }

		[Export ("areasCount")]
		uint AreasCount ();
		
		//TODO: Find a way to implemet this because btouch goes kaboom cuz there is a Prop called areas
//		[Export ("areas:")]
//		APTextArea areas (uint index);

		[Export ("addToAreas:")]
		void AddToAreas (APTextArea areas);

		[Export ("insertInAreas:atIndex:")]
		void InsertInAreasatIndex (APTextArea areas, uint index);

		[Export ("removeFromAreas:")]
		void RemoveFromAreas (APTextArea areas);

	}
	
	/// <summary>
	/// Annotation object for ink annotations (includes "straight-line" annotations).
	/// </summary>
	[BaseType (typeof (APMarkupAnnotation))]
	interface APInk 
	{
		/// <summary>
		/// The paths that compose this annotation.
		/// 
		/// This attribute is required when creating new ink annotation objects.
		/// </summary>
		/// <value>
		/// The paths that compose this annotation.
		/// </value>
		[Export ("paths")]
		NSArray Paths { get; }
		
		/// <summary>
		/// The width of the line used to make this annotation.
		/// 
		/// This attribute is required when creating new ink annotation objects.
		/// </summary>
		/// <value>
		/// The width of the line used to make this annotation.
		/// </value>
		[Export ("penWidth", ArgumentSemantic.Assign)]
		float PenWidth { get; set;  }

		[Export ("pathsCount")]
		uint PathsCount ();
		
		//TODO: Find a way to implement ths because btouch goes kaboom cuz there is a prop named paths
//		[Export ("paths:")]
//		APPath paths (uint index);

		[Export ("addToPaths:")]
		void AddToPaths (APPath paths);

		[Export ("insertInPaths:atIndex:")]
		void InsertInPathsatIndex (APPath paths, uint index);

		[Export ("removeFromPaths:")]
		void RemoveFromPaths (APPath paths);

	}
	
	/// <summary>
	/// Images/photos will be displayed in the reader.  
	/// Other file types will be opened only if there is an app registered that can be used with "Open In" for that type.
	/// </summary>
	[BaseType (typeof (APMarkupAnnotation))]
	interface APFileAttachment 
	{
		/// <summary>
		/// The filename of the attached file.
		/// </summary>
		/// <value>
		/// The name of the attached file.
		/// </value>
		[Export ("fileName", ArgumentSemantic.Retain)]
		string FileName { get; set; }
		
		/// <summary>
		/// Optional string to be used in the UI to identify the file.
		/// </summary>
		/// <value>
		/// The description string to be used in the UI to identify the file.
		/// </value>
		[Export ("description", ArgumentSemantic.Retain)]
		string Description { get; set;  }
		
		/// <summary>
		/// Optional field that will either be a mime type or a proprietary type extension (see Annex E of the ISO PDF standard).
		/// </summary>
		/// <value>
		/// The type of the file.
		/// </value>
		[Export ("fileType", ArgumentSemantic.Retain)] [NullAllowed]
		string FileType { get; set;  }
		
		/// <summary>
		/// The attachment data.
		/// </summary>
		/// <value>
		/// The file attachment data.
		/// </value>
		[Export ("fileData", ArgumentSemantic.Retain)]
		NSData FileData { get; set; }

	}

	[BaseType (typeof (NSObject))]
	interface APSoundDescription 
	{
		[Export ("samplingRate")]
		float SamplingRate { get;  }
		
		[Export ("channels")]
		uint Channels { get;  }

		[Export ("bitsPerSamplePerChannel")]
		uint BitsPerSamplePerChannel { get;  }

		[Export ("encodingFormat")]
		APSoundEncoding EncodingFormat { get;  }

		[Export ("compressionFormat")]
		string CompressionFormat { get;  }

		[Export ("compressionParameters")]
		string CompressionParameters { get;  }

	}
	
	/// <summary>
	/// Represents a Sound annotation.  
	/// 
	/// Note that in the PDF standard, a Sound annotation is technically not a type of Markup annotation, 
	/// but for the purposes of library usage it makes more sense as a Markup annotation.  
	/// 
	/// Sounds exported to PDF will still function as normal.
	/// </summary>
	[BaseType (typeof (APMarkupAnnotation))]
	interface APSound 
	{
		/// <summary>
		/// The sound data.
		/// </summary>
		/// <value>
		/// The sound data.
		/// </value>
		[Export ("soundData")]
		NSData soundData { get; }
		
		/// <summary>
		/// Detailed information about the sound format and encoding.
		/// </summary>
		/// <value>
		/// The sound sound format and encoding.
		/// </value>
		[Export ("soundDescription")]
		APSoundDescription SoundDescription { get; }
	}
	
	/// <summary>
	/// Base class for transformable user markup annotations.
	/// </summary>
	[BaseType (typeof (APMarkupAnnotation))]
	interface APTransformableMarkupAnnotation 
	{
		/// <summary>
		/// Gets or sets the affine transform that was applied to the original annotation.
		/// </summary>
		/// <value>
		/// The affine transform that was applied to the original annotation.
		/// </value>
		[Export ("transform", ArgumentSemantic.Assign)]
		CGAffineTransform transform { get; set; }
		
		/// <summary>
		/// Gets or sets the original rect prior to the application of the transform.
		/// </summary>
		/// <value>
		/// Holds the Rect of the annotation prior to the application of the transform.
		/// </value>
		[Export ("originalRect", ArgumentSemantic.Assign)]
		RectangleF OriginalRect { get; set; }

	}
	
	/// <summary>
	/// A free-text (typewriter) annotation, with text that is embedded directly onto the PDF page content.
	/// </summary>
	[BaseType (typeof (APTransformableMarkupAnnotation))]
	interface APFreeText 
	{
		/// <summary>
		/// Gets or sets the text justification within the rectangle.
		/// </summary>
		/// <value>
		/// The text justification within the rectangle.
		/// </value>
		[Export ("justification", ArgumentSemantic.Assign)]
		APFreeTextJustification Justification { get; set; }
		
		/// <summary>
		/// Gets or sets the font name to be used.
		/// </summary>
		/// <value>
		/// The name of the font.
		/// </value>
		[Export ("fontName", ArgumentSemantic.Retain)]
		string FontName { get; set;  }
		
		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>
		/// The size of the font.
		/// </value>
		[Export ("fontSize", ArgumentSemantic.Assign)]
		float FontSize { get; set;  }

	}
	
	/// <summary>
	/// Data object describing a search result from a query based on an APSearchRequest.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APSearchResult 
	{
		/// <summary>
		/// Gets The actual text from the document that matched the query 
		/// (e.g., could be different from the query text due to case insensitivity or if 
		/// a regular expression search was performed).
		/// </summary>
		/// <value>
		/// The matched text.
		/// </value>
		[Export ("matchedText")]
		string MatchedText { get;  }
		
		/// <summary>
		/// Gets the page number upon which this match was found.
		/// </summary>
		/// <value>
		/// The index of the page.
		/// </value>
		[Export ("pageIndex")]
		uint PageIndex { get;  }
		
		/// <summary>
		/// Gets Additional textual context, if available, found before and after the matched text.
		/// </summary>
		/// <value>
		/// Additional textual context, if available, found before and after the matched text.
		/// </value>
		[Export ("context")]
		string Context { get;  }
		
		/// <summary>
		/// The 0-based index of the matchedText attribute within the context attribuate.
		/// </summary>
		/// <value>
		/// The 0-based index of the matchedText attribute within the context attribuate.
		/// </value>
		[Export ("indexOfMatchInContext")]
		uint IndexOfMatchInContext { get;  }
		
		/// <summary>
		/// One or more rectangles in page space where the characters matching the search result were found.
		/// </summary>
		/// <value>
		/// One or more rectangles in page space where the characters matching the search result were found.
		/// </value>
		[Export ("areas")]
		NSArray Areas { get;  }
		
		/// <summary>
		/// The bounding rectangle of the matched areas.
		/// </summary>
		/// <value>
		/// The bounding rectangle of the matched areas.
		/// </value>
		[Export ("bounds")]
		RectangleF Bounds { get;  }
		
		/// <summary>
		/// If this result was found in an annotation, the annotation object in which the match was found; otherwise null.
		/// </summary>
		/// <value>
		/// If this result was found in an annotation, the annotation object in which the match was found; otherwise null.
		/// </value>
		[Export ("annotation")] [NullAllowed]
		APAnnotation Annotation { get; }

		[Export ("areasCount")]
		uint AreasCount ();
		
		//TODO: Find a way to implement this because btouch goes kaboom because there is a prop named areas
//		[Export ("areas:")]
//		APTextArea areas (uint index);
	}
	
	/// <summary>
	/// Used to describe a search query, which can be passed to APPDFInformation::performSearch: to do a full-text search of an APPDFDocument
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APSearchRequest 
	{
		/// <summary>
		/// Gets or sets the text to use for the search query.
		/// </summary>
		/// <value>
		/// The text to use for the search query.
		/// </value>
		[Export ("query", ArgumentSemantic.Retain)]
		string Query { get; set;  }
		
		/// <summary>
		/// Gets or sets the page upon which to start the search
		/// </summary>
		/// <value>
		/// The page upon which to start the search
		/// </value>
		[Export ("basePage", ArgumentSemantic.Assign)]
		uint BasePage { get; set;  }
		
		/// <summary>
		/// Gets or sets the location on the base page from which to start the search.
		/// </summary>
		/// <value>
		/// The location on the base page from which to start the search.
		/// </value>
		[Export ("baseLocation", ArgumentSemantic.Assign)]
		PointF BaseLocation { get; set;  }
		
		/// <summary>
		/// Indicates whether the query attribute is a plain text search, or a regular expression (see NSRegularExpression)..
		/// </summary>
		/// <value>
		/// <c>true</c> if query is regular expression; otherwise, <c>false</c>.
		/// </value>
		[Export ("queryIsRegularExpression", ArgumentSemantic.Assign)]
		bool QueryIsRegularExpression { get; set;  }
		
		/// <summary>
		/// Indicates whether annotations should also be searched, or just the plain text of the document.
		/// </summary>
		/// <value>
		/// <c>true</c> if search annotations; otherwise, <c>false</c>.
		/// </value>
		[Export ("searchAnnotations", ArgumentSemantic.Assign)]
		bool SearchAnnotations { get; set;  }

		[Export ("cancel", ArgumentSemantic.Assign)]
		bool Cancel { get; set;  }

		[Export ("searchResultsCapped")]
		bool SearchResultsCapped { get;  }

		[Export ("inProgress")]
		bool InProgress { get;  }

		[Export ("numResults")]
		uint NumResults ();
	}
	
	/// <summary>
	/// Provides information about a PDF file. This information is cached
	/// in a disk file, which is available in the path attribute. PDF
	/// Information files are generated by the APPDFProcessor driver class,
	/// and are also used as an on-disk cache for user annotations.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APPDFInformation 
	{
		/// <summary>
		/// Gets the full path to the information file.
		/// </summary>
		/// <value>
		/// The full path to the information file.
		/// </value>
		[Export ("path")]
		string Path { get;  }
		
		/// <summary>
		/// Document permissions. For most documents, this is
		/// APPDFDocumentPermissions.All, unless the document is encrypted and
		/// the User Password is provided.
		/// </summary>
		/// <value>
		/// The permissions.
		/// </value>
		[Export ("permissions")]
		APPDFDocumentPermissions Permissions { get;  }
		
		/// <summary>
		/// Gets the APPDFInformation version of this information file.
		/// </summary>
		/// <value>
		/// The version of PPDFInformation information file.
		/// </value>
		[Export ("version")]
		uint Version { get;  }
		
		/// <summary>
		/// Used to determine if this PDF file has had its information processed.
		/// </summary>
		/// <value>
		/// <c>true</c> if the information has been processed for
		/// this PDF, <c>false</c> if processing is still required.
		/// </value>
		[Export ("isProcessed")]
		bool IsProcessed { get;  }
		
		/// <summary>
		/// Used to determine if the information for this PDF matches the PDF
		/// file on disk. The file is modified whenever any annotations are
		/// added, deleted, or modified.
		/// </summary>
		/// <value>
		/// <c>true</c> if the file is modified, <c>false</c> if the PDF file on disk is up-to-date.
		/// </value>
		[Export ("isModified")]
		bool IsModified { get;  }
		
		/// <summary>
		/// Gets the root APOutlineElement for this document;
		/// null if this document contains no PDF outline.
		/// </summary>
		/// <value>
		/// The root APOutlineElement for this document.
		/// </value>
		[Export ("outlineRoot")] [NullAllowed]
		APOutlineElement OutlineRoot { get;  }
		
		/// <summary>
		/// Use to verify the validity and obtain the version of the
		/// information file at the specified path.
		/// </summary>
		/// <returns>
		/// The version of the information file, or 0 if the file is
		/// not a valid APPDFInformatio file.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		[Static] [Export ("versionOfInformationAtPath:")]
		uint VersionOfInformationAtPath (string path);
		
		/// <summary>
		/// Designated Constructor. The file at basePath must
		/// exist and point to a valid PDF Information file (created using the
		/// APPDFProcessor class).
		/// </summary>
		/// <param name='basePath'>
		/// Base path.
		/// </param>
		[Export ("initWithPath:")]
		IntPtr Constructor (string basePath);
		
		/// <summary>
		/// Used to access all annotations in the document.
		/// <returns>
		/// An array of APAAnnotation objects containing all
		/// user annotations in the document.
		/// </returns>
		[Export ("allUserAnnotations")]
		APAnnotation [] AllUserAnnotations ();
		
		/// <summary>
		/// Used to access all annotations on a single page of the document.
		/// </summary>
		/// <returns>
		/// An Array of APAAnnotation objects for the annotations on
		/// the indicated page of the document.
		/// </returns>
		/// <param name='page'>
		/// The 0-based page index of the PDF document.
		/// </param>
		[Export ("userAnnotationsOnPage:")]
		APAnnotation [] UserAnnotationsOnPage (uint page);
		
		/// <summary>
		/// Used to access all bookmarks in the document.
		/// </summary>
		/// <returns>
		/// An array of APABookmark objects containing all
		/// bookmarks in the document.
		/// </returns>
		[Export ("bookmarks")]
		APBookmark [] Bookmarks ();
		
		/// <summary>
		/// Used to programmatically add a new annotation object to the
		/// document. Note that annotation must be a client-created annotation
		/// object with all required properties set.
		/// </summary>
		/// <returns>
		/// true if the annotation was successfully added to the
		/// document, false otherwise.
		/// </returns>
		/// <param name='annotation'>
		/// Annotation
		/// </param>
		[Export ("addUserAnnotation:")]
		bool AddUserAnnotation (APAnnotation annotation);
		
		/// <summary>
		/// Used to persist programmatic changes to an annotation object. Note
		/// that annotation must be an annotation object returned from this
		/// APPDFInformation object; for adding new annotation objects
		/// use APPDFInformation.addUserAnnotation()
		/// </summary>
		/// <returns>
		/// true if the annotation was successfully updated, false otherwise.
		/// </returns>
		/// <param name='annotation'>
		/// Annotation
		/// </param>
		[Export ("updateUserAnnotation:")]
		bool UpdateUserAnnotation (APAnnotation annotation);
		
		/// <summary>
		/// Used to programmatically delete an annotation. Note that annotation
		/// must be an annotation object returned from this APPDFInformation
		/// object.
		/// </summary>
		/// <returns>
		/// true if the annotation was successfully deleted, false otherwise.
		/// </returns>
		/// <param name='annot'>
		/// Annotation
		/// </param>
		[Export ("removeAnnotation:")]
		bool RemoveAnnotation (APAnnotation annot);
		
		/// <summary>
		/// Used to programmatically remove all annotations from this document.
		/// </summary>
		/// <returns>
		/// true if the annotations were successfully deleted, false otherwise.
		/// </returns>
		[Export ("removeAllAnnotations")]
		bool RemoveAllAnnotations ();
		
		/// <summary>
		/// Used to determine if any text has been processed for this PDF.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the PDF document has been processed and
		/// any text has been successfully extracted. Note that some PDF files
		/// may not have any text if they have been processed; for example, if
		/// a scanned document has not been OCR'd.
		/// </returns>
		[Export ("hasText")]
		bool HasText ();
		
		/// <summary>
		/// Used to determine if there are any annotations for the document.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the document has any annotations.; otherwise, <c>false</c>.
		/// </returns>
		[Export ("hasUserAnnotations")]
		bool HasUserAnnotations ();
		
		/// <summary>
		/// Used to determine if there is any selectable text on a single page
		/// of the PDF.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the PDF document has been processed and
		/// any text on the indicated 0-based page index has been successfully
		/// extracted.
		/// </returns>
		/// <param name='page'>
		/// Page.
		/// </param>
		[Export ("hasTextOnPage:")]
		bool HasTextOnPage (uint page);
		
		/// <summary>
		/// Used to determine if there are any bookmarks in the document.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the document has any bookmarks.
		/// </returns>
		[Export ("hasBookmarks")]
		bool HasBookmarks ();
		
		/// <summary>
		/// Used to determine if the document has a PDF Outline.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the document has a PDF Outline.
		/// </returns>
		[Export ("hasPDFOutline")]
		bool HasPDFOutline ();
		
		/// <summary>
		/// Used to perform a full-text search of the document. The parameter
		/// is an APSearch helper object, which includes an APSearchRequest
		/// object describing the search query and specifying various search
		/// options, and an APPDFSearchDelegate object which is used to return
		/// search result data.
		/// 
		/// A search operation can take some time, especially on large
		/// documents, so it is highly recommended that this method only be
		/// called on a background thread.
		/// </summary>
		/// <returns>
		/// <c>true</c> if one or more results were found that match the query.
		/// </returns>
		/// <param name='query'>
		/// Query
		/// </param>
		[Export ("performSearch:")]
		bool PerformSearch (APSearch query);

	}
	
	/// <summary>
	/// Delegate protocol for document search notifications.
	/// </summary>
	[BaseType (typeof (NSObject))]
	[Model]
	interface APPDFSearchDelegate 
	{
		/// <summary>
		/// Called when the search has found a result.
		/// </summary>
		/// <param name='searchRequest'>
		/// The request object for this search
		/// </param>
		/// <param name='result'>
		/// The result object with information about the match
		/// </param>
		[Export ("search:foundResult:")]
		void SearchfoundResult (APSearchRequest searchRequest, APSearchResult result);
		
		/// <summary>
		/// Called every time a page has finished searching.
		/// </summary>
		/// <param name='searchRequest'>
		/// The request object for this search
		/// </param>
		/// <param name='pageIndex'>
		/// The 0-based index of the page that was searched
		/// </param>
		[Export ("search:didSearchPage:")]
		void SearchdidSearchPage (APSearchRequest searchRequest, uint pageIndex);
		
		/// <summary>
		/// Called when the search is complete
		/// </summary>
		/// <param name='searchRequest'>
		/// The request object for this search
		/// </param>
		[Export ("searchDidFinish:")]
		void SearchDidFinish (APSearchRequest searchRequest);

	}
	
	/// <summary>
	/// Helper object to encapsulate a search request and delegate for callback notifications.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APSearch 
	{
		/// <summary>
		/// Constructor for creating the search object.
		/// </summary>
		/// <param name='request'>
		/// The data object desribing the search to be performed
		/// </param>
		/// <param name='_delegate'>
		/// An APPDFSearchDelegate object for notification callbacks
		/// </param>
		[Export ("initWithSearchRequest:delegate:")]
		IntPtr Constructor (APSearchRequest request, APPDFSearchDelegate _delegate);
	}
	
	/// <summary>
	/// Driver class for processing PDF files and writing back annotations
	/// to PDF files.
	///
	/// In order to support certain reading and annotation features, the
	/// PDF must first be "processed" to determine the locations of
	/// all text on the document pages, as well as other information such
	/// as links, existing annotations, and security information.
	///
	/// This extraction process generally takes a "long" time, so this
	/// information is cached on disk in the form of an APPDFInformation
	/// object. That way, each document only needs to be processed once,
	/// and then subsequent reads can access the information instantly.
	///
	/// This PDF Information file is then also used to store and manage
	/// annotations as the user creates them in real-time. When an
	/// annotated PDF file is required, this driver class can be used to
	/// generate it. (Generating an annotated PDF file can, again, be a
	/// lengthy process, depending on the number, order, and location of
	/// annotations; hence, it is only done on-demand using this driver
	/// class.)
	///
	/// It is highly recommend that this driver class is only used on a
	/// background thread, as these operations can take significant amounts
	/// of time (up to minutes for very large documents) depending on the
	/// size and complexity of the document being processed. The delegate
	/// callbacks in the APPDFProcessorDelegate protocol will be used to
	/// report progress. Note that a delegate is required for encrypted PDF
	/// documents, in order to provide the document password.
	///
	/// Use the delegate to receive notifications of processing progress,
	/// completion, and errors. Note that delegate callbacks will be
	/// performed on a non-main thread, so your delegate may have to
	/// perform selectors on the main thread to do any corresponding
	/// interface updates.
	///
	/// Also note that calls to the methods in this class will be
	/// automatically serialized by the library to avoid over-utilization
	/// of resources.
	///
	/// Implementations may cache information about PDF documents across
	/// processing and writeback requests, to improve performance for some
	/// common scenarios. You may therefore wish to keep one instance of
	/// the processor for the duration of your application, to take
	/// advantage of this potential performance boost. On the other hand,
	/// cache information may require additional memory resources; so at
	/// the very least you should release any idle processor object in the
	/// case of a memory warning. It is always safe to release the
	/// processor object after using it.
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APPDFProcessor 
	{
		/// <summary>
		/// The delegate used for notification and information callbacks.
		/// </summary>
		/// <value>
		/// The delegate.
		/// </value>
		[Wrap ("WeakDelegate")] 
		APPDFProcessorDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)] [NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		/// <summary>
		/// The options to use when processing PDF files. Defaults to
		/// APPDFProcessingOptions.Default
		/// </summary>
		/// <value>
		/// The processing options.
		/// </value>
		[Export ("processingOptions")]
		APPDFProcessingOptions ProcessingOptions { get; set;  }
		
		/// <summary>
		/// Used to process the PDF Information from a PDF file. Use the
		/// APPDFProcessorDelegate callbacks to obtain result or error
		/// information, or notification when processing is complete.
		/// </summary>
		/// <param name='pdf'>
		/// The APPDFDocument to process. Upon success, the
		/// APPDFDocument.Information property of the file will be updated with
		/// the information extracted from the document.
		/// </param>
		[Export ("processPDF:")]
		void ProcessPDF (APPDFDocument pdf);
		
		/// <summary>
		/// Used to sync the PDF file with the associated annotations in the
		/// APPDFInformation file. Use the APPDFProcessorDelegate callbacks to
		/// obtain result or error information, or notification when the
		/// write-back is complete.
		/// 
		/// If using a disk-based PDF, this method updates the PDF file
		/// in-place, which is done via an atomic file replace operation if the
		/// operation is successful. If using a data-based PDF, the
		/// APPDFProcessorDelegate.pdfProcessordidSyncAnnotationsToPDF()
		/// delegate callback will be used to report the updated data.
		/// </summary>
		/// <param name='pdf'>
		/// The APPDFDocument which has annotations in its
		/// APPDFDocument.Information property.
		/// </param>
		[Export ("syncAnnotationsToPDF:")]
		void SyncAnnotationsToPDF (APPDFDocument pdf);
		
		/// <summary>
		/// Used to write the PDF content and annotations to another output PDF
		/// file. Use the options parameter to control the method used to write
		/// the output file. Use the APPDFProcessorDelegate callbacks to obtain
		/// result or error information, or notification when the output is
		/// complete.
		/// 
		/// This method writes the output PDF file at the specified path. Any
		/// existing file is removed and replaced. If the operation fails, the
		/// status of the file at destPath is undefined.
		/// </summary>
		/// <param name='pdf'>
		/// The APPDFDocument indicating the PDF and associated annotations.
		/// </param>
		/// <param name='destPath'>
		/// The full path of the output file.
		/// </param>
		/// <param name='options'>
		/// Options used to control the output PDF.
		/// </param>
		[Export ("writePDFWithAnnotations:toPath:options:")]
		void WritePDFWithAnnotationstoPathoptions (APPDFDocument pdf, [NullAllowed] string destPath, APPDFProcessorWriteOptions options);
	}
	
	/// <summary>
	/// Container class used to hold options when writing out PDF using
	/// APPDFProcessor.writePDFWithAnnotationstoPathoptions()
	/// </summary>
	[BaseType (typeof (NSObject))]
	interface APPDFProcessorWriteOptions 
	{
		[Export ("flags", ArgumentSemantic.Assign)]
		APPDFWriteAnnotationOptions Flags { get; set; }
		
		[Export ("pageRange", ArgumentSemantic.Assign)]
		NSRange PageRange { get; set; }
		
		[Export ("initWithFlags:pageRange:")]
		IntPtr Constructor (APPDFWriteAnnotationOptions flags, NSRange pageRange);

		[Export ("initWithFlags:")]
		IntPtr Constructor (APPDFWriteAnnotationOptions flags);

		[Static]
		[Export ("optionsWithFlags:pageRange:")]
		APPDFProcessorWriteOptions optionsWithFlagspageRange (APPDFWriteAnnotationOptions flags, NSRange pageRange);
		
		[Static]
		[Export ("optionsWithFlags:")]
		APPDFProcessorWriteOptions optionsWithFlags (APPDFWriteAnnotationOptions flags);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface APPDFProcessorDelegate 
	{
		[Export ("pdfProcessor:requiresPasswordOfType:forPDF:")]
		string PdfProcessorrequiresPasswordOfTypeforPDF (APPDFProcessor processor, APPDFPasswordType passwordType, APPDFDocument pdf);

		[Export ("pdfProcessor:validatedPassword:ofType:forPDF:")]
		void PdfProcessorvalidatedPasswordofTypeforPDF (APPDFProcessor processor, string password, APPDFPasswordType passwordType, APPDFDocument pdf);

		[Export ("pdfProcessor:validationFailedForPasswordOfType:forPDF:")]
		void PdfProcessorvalidationFailedForPasswordOfTypeforPDF (APPDFProcessor processor, APPDFPasswordType passwordType, APPDFDocument pdf);

		[Export ("pdfProcessor:willProcessPDF:")]
		void PdfProcessorwillProcessPDF (APPDFProcessor processor, APPDFDocument pdf);

		[Export ("pdfProcessor:didIndexPage:ofPage:ofPDF:shouldCancel:")]
		void PdfProcessordidIndexPageofPageofPDFshouldCancel (APPDFProcessor processor, uint page, uint numPages, APPDFDocument pdf, bool cancel);

		[Export ("pdfProcessor:didProcessAnnotationsOnPage:ofPage:ofPDF:shouldCancel:")]
		void PdfProcessordidProcessAnnotationsOnPageofPageofPDFshouldCancel (APPDFProcessor processor, uint page, uint numPages, APPDFDocument pdf, bool cancel);

		[Export ("pdfProcessor:didProcessPDF:")]
		void PdfProcessordidProcessPDF (APPDFProcessor processor, APPDFDocument pdf);

		[Export ("pdfProcessor:failedToProcessPDF:withError:")]
		void PdfProcessorfailedToProcessPDFwithError (APPDFProcessor processor, APPDFDocument pdf, NSError error);

		[Export ("pdfProcessor:willSyncAnnotationsToPDF:")]
		void PdfProcessorwillSyncAnnotationsToPDF (APPDFProcessor processor, APPDFDocument pdf);

		[Export ("pdfProcessor:didSyncAnnotationsToPDF:")]
		void PdfProcessordidSyncAnnotationsToPDF (APPDFProcessor processor, APPDFDocument pdf);

		[Export ("pdfProcessor:failedToSyncAnnotationsToPDF:withError:")]
		void PdfProcessorfailedToSyncAnnotationsToPDFwithError (APPDFProcessor processor, APPDFDocument pdf, NSError error);

		[Export ("pdfProcessor:willWritePDFWithAnnotations:toPath:")]
		void PdfProcessorwillWritePDFWithAnnotationstoPath (APPDFProcessor processor, APPDFDocument pdf, string path);

		[Export ("pdfProcessor:didWritePDFWithAnnotations:toPath:")]
		void PdfProcessordidWritePDFWithAnnotationstoPath (APPDFProcessor processor, APPDFDocument pdf, string path);

		[Export ("pdfProcessor:failedToWritePDFWithAnnotations:toPath:withError:")]
		void PdfProcessorfailedToWritePDFWithAnnotationstoPathwithError (APPDFProcessor processor, APPDFDocument pdf, string path, NSError error);

		[Export ("pdfProcessor:encounteredNonFatalError:whileProcessingPDF:")]
		void PdfProcessorencounteredNonFatalErrorwhileProcessingPDF (APPDFProcessor processor, NSError error, APPDFDocument pdf);

		[Export ("pdfProcessor:reportProcessingLog:forPDF:")]
		void PdfProcessorreportProcessingLogforPDF (APPDFProcessor processor, string processingLog, APPDFDocument pdf);
	}
	
	[BaseType (typeof (UIViewController))]
	interface APPDFViewController 
	{
		//Detected properties
		[Export ("pageToViewScale")]
		float PageToViewScale { get; set; }
		
//		[Wrap ("WeakDelegate")]
//		APPDFViewDelegate Delegate { get; set; }
//
//		[Export ("delegate", ArgumentSemantic.Assign)] [NullAllowed]
//		NSObject WeakDelegate { get; set; }
				
		[Export ("viewOptions", ArgumentSemantic.Retain)]
		APPDFViewOptions ViewOptions { get; set; }

		[Export ("pdf")]
		APPDFDocument Pdf { get; }

		[Export ("visiblePageRange", ArgumentSemantic.Assign)]
		NSRange VisiblePageRange { get; set; }

		[Export ("initWithPDF:")] [NullAllowed]
		IntPtr Constructor (APPDFDocument pdf);

		[Export ("navigateToPage:animated:")]
		void NavigateToPageanimated (uint pageIndex, bool animated);

		[Export ("navigateToAnnotation:showPopup:animated:")]
		void NavigateToAnnotationshowPopupanimated (APAnnotation annotation, bool showPopup, bool animated);

		[Export ("navigateToBookmark:animated:")]
		void NavigateToBookmarkanimated (APBookmark bookmark, bool animated);

		[Export ("navigateToDestination:animated:")]
		void NavigateToDestinationanimated (APDestination destination, bool animated);

		[Export ("nextPageAnimated:")]
		void NextPageAnimated (bool animated);

		[Export ("hasNextPage")]
		bool HasNextPage ();

		[Export ("previousPageAnimated:")]
		void PreviousPageAnimated (bool animated);

		[Export ("hasPreviousPage")]
		bool HasPreviousPage ();

		[Export ("firstPageAnimated:")]
		void FirstPageAnimated (bool animated);

		[Export ("lastPageAnimated:")]
		void LastPageAnimated (bool animated);

		[Export ("screenUpAnimated:")]
		void ScreenUpAnimated (bool animated);

		[Export ("screenDownAnimated:")]
		void ScreenDownAnimated (bool animated);

		[Export ("screenLeftAnimated:")]
		void ScreenLeftAnimated (bool animated);

		[Export ("screenRightAnimated:")]
		void ScreenRightAnimated (bool animated);

		[Export ("showGoToPageFromRect:inView:permittedArrowDirections:animated:")]
		void ShowGoToPageFromRectinViewpermittedArrowDirectionsanimated (RectangleF rect, UIView view, UIPopoverArrowDirection arrowDirections, bool animated);

		[Export ("showGoToPageFromBarButtonItem:permittedArrowDirections:animated:")]
		void ShowGoToPageFromBarButtonItempermittedArrowDirectionsanimated (UIBarButtonItem item, UIPopoverArrowDirection arrowDirections, bool animated);

		[Export ("showBookmarksFromRect:inView:permittedArrowDirections:animated:")]
		void ShowBookmarksFromRectinViewpermittedArrowDirectionsanimated (RectangleF rect, UIView view, UIPopoverArrowDirection arrowDirections, bool animated);

		[Export ("showBookmarksFromBarButtonItem:permittedArrowDirections:animated:")]
		void ShowBookmarksFromBarButtonItempermittedArrowDirectionsanimated (UIBarButtonItem item, UIPopoverArrowDirection arrowDirections, bool animated);

		[Export ("showPDFOutlineFromRect:inView:permittedArrowDirections:animated:")]
		void ShowPDFOutlineFromRectinViewpermittedArrowDirectionsanimated (RectangleF rect, UIView view, UIPopoverArrowDirection arrowDirections, bool animated);

		[Export ("showPDFOutlineFromBarButtonItem:permittedArrowDirections:animated:")]
		void ShowPDFOutlineFromBarButtonItempermittedArrowDirectionsanimated (UIBarButtonItem item, UIPopoverArrowDirection arrowDirections, bool animated);

		[Export ("showAnnotationListFromRect:inView:permittedArrowDirections:animated:")]
		void ShowAnnotationListFromRectinViewpermittedArrowDirectionsanimated (RectangleF rect, UIView view, UIPopoverArrowDirection arrowDirections, bool animated);

		[Export ("showAnnotationListFromBarButtonItem:permittedArrowDirections:animated:")]
		void ShowAnnotationListFromBarButtonItempermittedArrowDirectionsanimated (UIBarButtonItem item, UIPopoverArrowDirection arrowDirections, bool animated);

		[Export ("dismissActivePopoverAnimated:")]
		void DismissActivePopoverAnimated (bool animated);

		[Export ("showSearchAnimated:")]
		void ShowSearchAnimated (bool animated);

		[Export ("showSearchForTerm:animated:")]
		void ShowSearchForTermanimated (string term, bool animated);

		[Export ("hideSearchAnimated:")]
		void HideSearchAnimated (bool animated);

		[Export ("isSearchVisible")]
		bool IsSearchVisible ();

		[Export ("clearSelection")]
		void ClearSelection ();

		[Export ("enterSelectionMode")]
		void EnterSelectionMode ();

		[Export ("endSelectionMode")]
		void EndSelectionMode ();

		[Export ("isSelectionMode")]
		bool IsSelectionMode ();

		[Export ("selectedText")]
		string SelectedText ();

		[Export ("pageIndexAtMidscreen")]
		uint PageIndexAtMidscreen ();

		[Export ("pageIndexAtContentOffset")]
		uint PageIndexAtContentOffset ();

		[Export ("pageSpaceContentOffset")]
		PointF PageSpaceContentOffset ();

		[Export ("viewSpaceContentOffset")]
		PointF ViewSpaceContentOffset ();

		[Export ("setViewSpaceContentOffset:animated:")]
		void SetViewSpaceContentOffsetanimated (PointF offset, bool animated);

		[Export ("fitToWidth")]
		void FitToWidth ();

		[Export ("fitToHeight")]
		void FitToHeight ();

		[Export ("layout")]
		void Layout ();

		[Export ("setPage:contentOffsetInPageSpace:animated:")]
		void SetPagecontentOffsetInPageSpaceanimated (uint page, PointF offset, bool animated);

		[Export ("gestureView")]
		UIView GestureView ();

		[Export ("hasValidDocument")]
		bool HasValidDocument ();

//		[Export ("didReceiveMemoryWarning")]
//		void DidReceiveMemoryWarning ();

		[Export ("mirroringController")]
		UIViewController MirroringController ();

		[Export ("setMirroringControllerViewSize:")]
		void SetMirroringControllerViewSize (SizeF size);

		[Export ("releaseMirroringController")]
		void ReleaseMirroringController ();
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface APPDFViewDelegate {
		
		[Export ("pdfControllerWillAskUserForDocumentPassword:")]
		void PdfControllerWillAskUserForDocumentPassword (APPDFViewController controller);

		[Export ("pdfController:didUnlockDocumentWithPassword:")]
		void PdfControllerdidUnlockDocumentWithPassword (APPDFViewController controller, string password);
		
		[Export ("pdfControllerDidLoadContentView:")]
		void PdfControllerDidLoadContentView (APPDFViewController controller);
		
		[Export ("pdfController:didTapOnLinkToExternalURL:")]
		void PdfControllerdidTapOnLinkToExternalURL (APPDFViewController controller, NSUrl url);
		
		[Export ("pdfController:willShowSearchAnimated:")]
		void PdfControllerwillShowSearchAnimated (APPDFViewController controller, bool animated);
		
		[Export ("pdfController:willHideSearchAnimated:")]
		void PdfControllerwillHideSearchAnimated (APPDFViewController controller, bool animated);
		
		[Export ("pdfControllerDidEnterSelectionMode:")]
		void PdfControllerDidEnterSelectionMode (APPDFViewController controller);
		
		[Export ("pdfControllerDidEndSelectionMode:")]
		void PdfControllerDidEndSelectionMode (APPDFViewController controller);
		
		[Export ("pdfControllerDidChangePage:")]
		void PdfControllerDidChangePage (APPDFViewController controller);
		
		[Export ("pdfControllerDidChangeLayout:")]
		void PdfControllerDidChangeLayout (APPDFViewController controller);
		
		[Export ("pdfController:didTapOnAnnotation:inRect:")]
		void PdfControllerdidTapOnAnnotationinRect (APPDFViewController controller, APAnnotation annotation, RectangleF rect);
		
		[Export ("pdfController:didTapOnSelectionInRect:")]
		void PdfControllerdidTapOnSelectionInRect (APPDFViewController controller, RectangleF rect);
		
		[Export ("pdfController:shouldShowPopupForAnnotation:")]
		bool PdfControllershouldShowPopupForAnnotation (APPDFViewController controller, APAnnotation annotation);
		
		[Export ("pdfController:didShowPopupForAnnotation:")]
		void PdfControllerdidShowPopupForAnnotation (APPDFViewController controller, APAnnotation annotation);
		
		[Export ("pdfController:didHidePopupForAnnotation:")]
		void PdfControllerdidHidePopupForAnnotation (APPDFViewController controller, APAnnotation annotation);
	}
	
	[BaseType (typeof (NSObject))]
	interface APPDFViewOptions 
	{
		[Export ("showLinks", ArgumentSemantic.Assign)]
		bool ShowLinks { get; set;  }
		
		[Export ("showPageLocator", ArgumentSemantic.Assign)]
		bool ShowPageLocator { get; set;  }

		[Export ("showPageNumber", ArgumentSemantic.Assign)]
		bool ShowPageNumber { get; set;  }

		[Export ("horizontalScrollLocked", ArgumentSemantic.Assign)]
		bool HorizontalScrollLocked { get; set;  }

		[Export ("softScrollLock", ArgumentSemantic.Assign)]
		bool SoftScrollLock { get; set;  }

		[Export ("showAnnotations", ArgumentSemantic.Assign)]
		bool ShowAnnotations { get; set;  }

		[Export ("autoPopup", ArgumentSemantic.Assign)]
		bool AutoPopup { get; set;  }

		[Export ("singlePageMode", ArgumentSemantic.Assign)]
		bool SinglePageMode { get; set;  }

		[Export ("slideToChangePages", ArgumentSemantic.Assign)]
		bool SlideToChangePages { get; set;  }

		[Export ("disableSelectionMenuController", ArgumentSemantic.Assign)]
		bool DisableSelectionMenuController { get; set;  }

		[Export ("minZoomScale", ArgumentSemantic.Assign)]
		float MinZoomScale { get; set;  }

		[Export ("maxZoomScale", ArgumentSemantic.Assign)]
		float MaxZoomScale { get; set;  }
	}	
}