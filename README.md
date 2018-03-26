# AutoWire
## Introduction
AutoWire is a utility project that allows you to register classes within Microsoft's Dependency Injection (DI) framework by applying attributes to your class rather than wiring them up seperately within your application's Startup class (or appropriate alternative).

## Why?
AutoWire was created firstly just to see if I could create a small open-source project (for fun basically), but also to solve an actual problem I had which was that I kept creating classes within my code and then forgetting to have to write the Dependency Injection code to regiter the types.

## Can I have an example?
Sure, suppose you have a class that implements an interface:
```csharp
public class MyClass : MyInterface
{
}
```

Typically you would have the following code in your Startup.cs (in ASP.NET core at least):
```csharp
public void ConfigureServices(IServiceCollection serviceCollection)
{
  serviceCollection.AddSingleton<MyInterface, MyClass>();
}
```

With AutoWire this is replaced by applying an AutoService attribute on your class:
```csharp
[AutoService]
public class MyClass : MyInterface
{
}
```

When you have your classes decorated you need to then make this call in your Startup.cs:
```csharp
public void ConfigureServices(IServiceCollection serviceCollection)
{
  serviceCollection.AutoWire();
}
```

OK, so it doesn't look much when one class is used, but when your project grows and you have many interfaces and types then AutoWire makes registering these types pretty simple, and you can set the type registration up right there with your class.

## Can I mix AutoWire with standard registration?
Yes you can, AutoWire will find classes that have the AutoService attribute applied, and you can continue to register any other types that do not have this attribute just as you do now

## Do you have full documentation/
Not yet, but I will do - soon
