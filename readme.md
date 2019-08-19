# SNAP is a Aspect Oriented Programming (AoP) utility for .NET

SNAP works at runtime unlike PostSharp which modifies IL at compile time.  SNAP works in 
tandem with your favorite IoC container.

# How it Works

SNAP is simple.  You decorate methods with your own custom attributes.  Your attributes derive from SNAP's base attribute. 
SNAP uses your IoC of choice to intercept method calls based on the attribute.  Methods are intercepted with *YOUR* 
own code either before and/or after a method call

# IoC Providers

 - StructureMap
 - Autofac
 - Ninject
 - LinFu
 - Castle Windsor

# Quick Start

Install any of the SNAP provider pagckages from the NuGet Package Manager or via command line.  
We'll use StructureMap for this example.

	Install-Package SNAP.StructureMap

## A Basic C# Example:

```cs
	// Configure SNAP to look at all assemblies starting with the "ConsoleApplication1" namespace.
	// Next, tell SNAP to intercept any code decorated with a DemoLoggingAttribute by running
	// an instance of the DemoLoggingInterceptor class.
	SnapConfiguration.For<StructureMapAspectContainer>(c =>
            {
            	// Tell SNAP to only be concerned with stuff starting with this namespace.
                c.IncludeNamespace("ConsoleApplication1*");
                // Tell SNAP to intercept any method or class decorated with "DemoLoggingAttribute"
                // by wrapping execution in your own DemoInterceptor class.
                c.Bind<DemoLoggingInterceptor>().To<DemoLoggingAttribute>();
            });

	// Configure StructureMap as usual.
	ObjectFactory.Configure(c => c.For<ISampleClass>().Use<SampleClass>());
	
	
	
	// Use your own class to handle interception.  Logging is a good example.
	public class DemoLoggingInterceptor : MethodInterceptor
	{
	    public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
	    {
		// Log something important here.
		
		// Then continue executing your method.
		invocation.Proceed();
	    }
	}
```
	
## Is this a Magic Black Box?
Unlinke tools that use IL weaving, SNAP's runtime approach means 100% of your code is unchanged.  The difference is that 
SNAP creates a proxy object at runtime that wraps your class instances.  This is similar to how the Entity Framework
issues proxies of your POCOs to assis with state and navigation properties.  It's still 100% your code.

## FAQ

 - *Do I have to use an DI (IoC) tool to make SNAP work?*
	Yes.  SNAP works by creating a transparent runtime proxy around classes.  It does so by working in tandem with IoC containers 
	during the object creation process.
 - *Is SNAP limited to the 5 IoC tools listed above?*
	No.  You can extend SNAP to use any IoC container.  MEF is an example of a container that's not yet supported, but is perfectly suitable.  
	Submit a pull request and get your extension added to the project!
 - *Is there documentation?*
	Yes.  Try the wiki 
 - *Pull Requests
 - 	Yes please!  I'd love help with MEF, Unity, or other IoC providers to make this project well rounded.

## Is it Tested?
The project has a comprehensive suite of tests.  Currently the tests exercise scenarios for each of the IoC providers.
