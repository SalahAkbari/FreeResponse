# FreeResponse

As we now, the layering of an application's codebase is a widely accepted technique to help reduce complexity and to improve code reusability.

 To achieve a layered architecture in this project, I tried to follow the principles of Domain Driven Design.

There are some fundamental layers in Domain Driven Design (DDD) which I have implemented as well:

 Application Layer: mainly includes Application Services that use domain layer and domain objects (Domain Services, Entities...)
 to perform requested application functionalities. In this solution I have considered the Application project for this purpose, it contains (DTOs,
 application services...) It uses Data Transfer Objects to get data from and return data to our API Controller.
 It can also deal with Logging, Object Mapping and so on...
    
 Domain Layer: The domain layer usually Includes business objects and their rules. This is the heart of the application. I have considered the .Core project
for this purpose, it includes Entities


EntityFramework project is for EF Core integration (abstracts EF Core from other layers).

The Web.Host project which is our main rest controller API and finally two test projects:

FreeResponse.IntegrationTests  integration test including the web layer
and FreeResponse.Tests wich is used for unit tests


As the result, we will see the index page of the Swagger. Actually When consuming a web API, understanding its various methods can be challenging for a developer.
 Swagger, also known as OpenAPI, solves the problem of generating useful documentation and help pages for web APIs. It provides benefits such as 
interactive documentation, client SDK generation, and API discover ability. So testing our API methods with Swagger is pretty straight forward, let's try some of them...

As you have noticed I have used some XML comments to describe and document our API methods, like the summery of the method and the response codes as you can see here,
it will fetch this information from this lines above each method in our controller class. The XML comments can be enabled by 
Right-clicking the Web.Host project in Solution Explorer and select Edit project file, you can see I have set the "GenerateDocumentationFile" to true.

I have then set the comments path for the Swagger JSON and UI in the startup class.

The CreateProject action returns an HTTP 201 states code on success. An HTTP 400 states code is returned when the posted request body is null. 
a 404 code when the System based on the SystemId could not be found and a 409 conflict code, In case Payload Containing Conflicting System / External ID,
Well actually Without proper documentation in the Swagger UI, the consumer lacks knowledge of these expected outcomes. I have Fixed that problem by adding this lines:

     /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the dto is null or the ModelState is invalid</response> 
        /// <response code="404">If the SdlcSystem based on the SdlcSystemId could not be found</response> 
        /// <response code="409">In case Payload Containing Conflicting System / External ID</response> 

        [HttpPost("api/v2/projects")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

As you can see in the both CreateProject and UpdateProject methods, I have used the _projectAppService, which I used the Constructer Injection pattern concept for 
handling dependency Injection which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies
Well, of course the ASP.NET Core supports the dependency injection (DI) software design pattern. it's a framework-provided service registered by default by the framework.
So all that we need to do is this lines in the Startup class:

            services.AddScoped(typeof(ISdlcSystemAppService), typeof(SdlcSystemAppService));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IProjectAppService), typeof(ProjectAppService));

the IProjectAppService service is registered with the concrete type ProjectAppService.

OK let's see what is happening in the project service class, well after passing some validations, the main code of inserting a new record into tables has been written
in the CreateProject method of the projectService class.

As you can see, I have used the AutoMapper for mapping a project entity to a project dto class, as you know, It’s common to map a similar object to another object.

 It's also tedious and repetitive since generally both objects (classes) may have the same/similar properties mapped to each other. We can use a library to automatically handle our mappings. AutoMapper is one of the best libraries for object to object mapping. You can see how I have configured it, in the MappingProfile

class which can be find here in the Application project. In this class we can see that I have used the _projectRepository which you can find its implementation in
Repositories folder in the EF Core project. It contains all necessary methods to deal with our database like Add and save and so on...

Another interesting part of the solution that I am always interested in, is testing. There is two kind of testing in this solution, unit testing and Integration tests.

Unlike the unit tests which are individual modules of an application in isolation (without any interaction with dependencies),
 to confirm that the code is doing things right, as we know, Integration tests ensure that an app's components function correctly at a level that includes the app's 
supporting infrastructure, such as the database.

The next part is testing the API with postman. I have provided a set of tests with postman.

My name is Salah Akbari, and thank you so much for reading this file. Please let me know, in case anything need to be clarified any further. Have a nice day!

