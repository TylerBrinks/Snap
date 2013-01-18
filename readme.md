#SNAP is a Aspect Oriented Programming (AoP) utility for .NET

SNAP works at runtime unlike PostSharp which modifies IL at compile time.  SNAP works in 
tandem with your favorite IoC container.

#IoC Providers

 - StructureMap
 - Autofac
 - Ninject
 - LinFu
 - Castle Windsor

#Quick Start#
Install the pagckage from the NuGet Package Manager or via command line.  We'll use StructureMap for this example.

	Install-Package SNAP.StructureMap

##A basic example:##

	// Configure SNAP to look at all assemblies starting with the "ConsoleApplication1" namespace.
	// Next, tell SNAP to intercept any code decorated with a DemoAttribute by running
	// an instance of the DemoInterceptor class.
	SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
                c.IncludeNamespace("ConsoleApplication1*");
                c.Bind<DemoInterceptor>().To<DemoAttribute>();
            });

	// Configure StructureMap as usual.
	ObjectFactory.Configure(c => c.For<ISampleClass>().Use<SampleClass>());
				
##Tested##
The project has a comprehensive suite of tests.  Currently the tests exercise scenarios for each of the IoC providers.

##FAQ##

 - *Do I have to use an DI (IoC) tool to make SNAP work?*
	Yes.  SNAP works by creating a transparent runtime proxy around classes.  It does so by working in tandem with IoC containers 
	during the object creation process.
 - *Is SNAP limited to the 5 IoC tools listed above?*
	No.  You can extend SNAP to use any IoC container.  MEF is an example of a container that's not yet supported, but is perfectly suitable.  
	Submit a pull request and get your extension added to the project!
 - *Is there documentation?*
	Yes.  Try the wiki 