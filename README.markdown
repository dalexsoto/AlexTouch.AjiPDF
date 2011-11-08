MonoTouch Aji iAnnotate PDF Library
====================================

This is a MonoTouch binding for Aji iAnnotate PDF Library which can be found at:

     http://www.ajidev.com/iannotate/developers.html 	(General Description)
	 http://www.ajidev.com/iannotate/dev-form.php 		(Lib Request Form)

I used version 1.5

Building
========

When you get libAjiPDFLib.a paste it into binding folder.

You Must Have the following files inside binding folder in order to compile your AlexTouch.AjiPDF.dll

-binding
	+ AssemblyInfo.cs		(INCLUDED)
	+ DescriptionFile.cs	(INCLUDED)
	+ enums.cs				(INCLUDED)
	+ extensions.cs			(INCLUDED)
	+ Makefile				(INCLUDED)
	+ libAjiPDFLib.a		(THE ONE YOU MUST GET FROM Ajidev.com*****)

To build the bindings, run the `make` command from within the bindings
directory.


Using AlexTouch.AjiPDF.dll with your own iOS App
=================================================

Simply add AlexTouch.AjiPDF.dll to your project's References in MonoDevelop and you are
good to go!

All kudos for this excelent piece of work "libAjiPDFLib.a" goes to http://www.ajidev.com/team.html also they have some docs ;) http://www.ajidev.com/lib-web/html/index.html


Alex Soto
@dalexsoto
https://github.com/dalexsoto/




