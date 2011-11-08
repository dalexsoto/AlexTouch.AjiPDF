BTOUCH=/Developer/MonoTouch/usr/bin/btouch
VERSION=1.5

all: AlexTouch.AjiPDF.dll

libAjiPDFLib.a: libAjiPDFLib.a

AlexTouch.AjiPDF.dll: Makefile AssemblyInfo.cs DescriptionFile.cs enums.cs extensions.cs libAjiPDFLib.a
	$(BTOUCH) -e DescriptionFile.cs AssemblyInfo.cs enums.cs extensions.cs --out=$@ --link-with=libAjiPDFLib.a,libAjiPDFLib.a

clean:
	-rm -f *.a *.dll
