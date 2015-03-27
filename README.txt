FAQ
===

Author: Claudio Rivera
Date: 2012-12-30

How did all begins?
The first version of this project was in 2006, when I was thinking to write a Persistence subsystem using the requirements defined
by Scott Ambler in its essay "The Design of a Robust Persistence Layer For Relational Databases". The section 6 "Requirements for a 
Persitent Layer" enumerates 14 points that a robust persistent layer must meet. While some of the requirements are contradictory or 
objectable for an object-oriented persistent layer (support for sql language and transactions, by example), the remaining are good
conditions for a full-strength persistent sub-system.

The design of this persistence service is inspired by the chapter 37, "Designing a Persistence Framework with Patterns", of 
the excellent book "Applying UML and Pattern"s by Craig Larman. This persistence framework implements two mechanisms. One similar of Larman, that
uses a database mapper (Fowler), when the client make persistent objects that combines persistence with domain data. The other use metadata-
based mappers that reads from an XML file the link between object properties and columns or values of the storage mechanism.


Which persistent mechanism it is supported?
The solution has an implementation for SQL Server, but the main project is an abstraction that can be implemented for other mechanisms.
Also the PersistentServer abstract class encapsulate all specification of the underlying mechanism used.

Is there support for multiple object persistence?
Yes. The main methods Store, Change, Retrieve and Destroy  of the PersistentServer class has overloaded versions that accept multiple
objects.

Is there support for transactions?
No. I believe that transactions are a persistence mechanism element, not an object oriented persistence service feature. 
Some mechanisms (text files, for example) don't permit transactions, like relational databases. In these cases, if you need
transaction support, then use a persistent mechanism that support it.	In SQL, relational databases, we can send all parameters,
extracted from multiple objects, and configure an explicit transaction, for saving data into multiple tables.

When my domain classes changes or I add new classes, do I need to change the persistence service code?
Not at all. All you need is to properly map the object properties, that you want to persist, to the underlying persistence mechanism.
If there's no implementation for the desired persistence mechanism, you have to construct one!

How can I relate an object with the entity in the persistence mechanism?
The project implement the Ambler's object identifier pattern, with an ObjectIdentifer abstract class, and an implementation class
called OID.

Is there support for cursors?
The retrive method in the PersisteServer class can return DataViews (.NET implementation), which is a non-provider class for collections of records. It can 
return one record or millions. In this way the control is in the client that use the method to return the number of entities
it can handles.

Is there any support for reporting tools?
This is an object-oriented Persistence Service for applications. If the reporting tool can work with .NET classes, then it will work
with it. Otherwise, you need to encapsulate the service and make an adapter for your reporting tool. If the tool works with your
persistence mechanism it's recommended then that the reporting tool and your mechanism communicate directly, using the language or
code it supports.

Is there support for multiple architectures?
This is a .NET framework project. If the architecture where you plan to use it, supports .NET, there's no problem to use this service.
But, this persistence service has no knowledge of the environment where it runs.

How many databases systems it supports?
This persistence service has no control or notion of the persistence mechanism or vendor directly, only at runtime. If there's no support for
your persistence mechanism, you have to construct one. Also, the system has no notion of the specific driver you use to connect to
the database or mechanism. It only knows of the connection string. It's the task of the client to configure the proper driver for
.NET.

The SQL Server implementation doesn't specify code with versioning in mind.
The code can be used with any of the actual (2014) SQL Server versions.

Can I open multiple connections?
You can execute any of the persistence commands in parallel with multiple PersistentServer instanes pointing to different or the
same persistence mechanism.

Is there native support for SQL?
No. SQL is the standard language for relational databases. The persistence service is a framework for persistence, that you can implement
as you wish with your persistence mechanism in mind. You could if you want, to extend the code of your implementation to support the
language that use your persistent mechanism. Actually there's an implementation to SQL Server.'

Is this system the same design that Ambler describes?
No. There are similar methods for persistence for CRUD operations, of course, but the way he solves the persistence is quite 
different than ours.





