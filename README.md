This is an unoffical Mongo ORM for c#.
It inculudes a basic generic crud(With IRepository) and a connection manager

NuGet Page : https://www.nuget.org/packages/MongoORM/

For more information please check wiki.

# Welcome to Mongo ORM!

Mongo ORM is a way for you to use MongoDB database without requiring to deal with MongoDB Driver's usual hindrance. Right now this project does not support async connections but it is to be implemented in future updates.

## Getting Started
### Prerequisites
This project requires MongoDB Driver already installed to the project. If you use NuGet, it will automatically install this dependency.
### Installing
You can install our project with NuGet, using this command
```
Install-Package MongoORM -Version 0.0.4.1
```
### Example
#### C#
```
using MongoCRUD;
using MongoCRUD.Interfaces;

namespace MongoCRUDDllTest
{
    class User : DbObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class MongoExample
    {
        static void Main(string[] args)
        {
            MongoDbConnection.InitializeAndStartConnection("CoolDatabase");
            var userCRUD = new Crud<User>();
            User[] userEntities = {
                new User
                {
                    Age = 25,
                    Name = "John"
                },
                new User
                {
                    Age = 26,
                    Name = "Smith"
                }
            };
            userCRUD.InsertMany(userEntities);
        }
    }
}
```

Execution of this code should be resulted in 2 new User classes to be added to the CoolDatabase.

![CurrentDatabase](https://github.com/mustafarumeli/MongoORM-for-C-/blob/master/ExamplePicture.PNG)
