# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# structure guidelines
```
.
├── Webshop
│   ├── Webshop.Application ------------------ (Application Layer (Interfaces))
│   │   ├── Repositories
│   │   │   └── interface IProductRepository
│   │   │       ├── method GetAll
│   │   │       ├── method GetById
│   │   │       ├── method Add
│   │   │       ├── method Update
│   │   │       └── method Delete
│   │   └── Services
│   │       ├── interface IProductService
│   │       └── class ProductService : IProductService (uses ProductRepository)
│   │
│   │
│   ├── Webshop.Domain ----------------------- (Domain Layer (No dependencies))
│   │   └── Entities ------------------------- (Database Entities)
│   │       └── class Product
│   │
│   │
│   ├── Webshop.Infrastructure --------------- (Infrastructure Layer (EF Core Implementation))
│   │   ├── Data
│   │   │   └── class AppDbContext
│   │   ├── Repositories
│   │   │   └── class ProductRepository : IProductRepository (uses AppDbContext)
│   │   │       ├── method GetAll
│   │   │       ├── method GetById
│   │   │       ├── method Add
│   │   │       ├── method Update
│   │   │       └── method Delete
│   │   └── Migrations
│   │
│   │
│   ├── Webshop.MVC -------------------------- (Website Layer)
│   │   ├── Controllers
│   │   │   ├── class HomeController
│   │   │   └── class ProductController (uses ProductService)
│   │   │       ├── method List
│   │   │       ├── method Details
│   │   │       ├── method Create
│   │   │       ├── method Update
│   │   │       └── method Delete
│   │   ├── ViewModels --------------------------- (ViewModels)
│   │   │   ├── class ProductListViewModel
│   │   │   ├── class ProductDetailsViewModel
│   │   │   ├── class ProductCreateViewModel
│   │   │   ├── class ProductUpdateViewModel
│   │   │   └── class ProductDeleteViewModel
│   │   └── Views ---------------------------- (All Webpages)
│   │       ├── Home
│   │       │   ├── Index.cshtml
│   │       │   └── Privacy.cshtml
│   │       ├── Product ---------------------- (Product Webpages)
│   │       │   ├── List.cshtml
│   │       │   ├── Details.cshtml
│   │       │   ├── Create.cshtml
│   │       │   ├── Update.cshtml
│   │       │   └── Delete.cshtml
│   │       ├── _ViewImports.cshtml
│   │       └── _ViewStart.cshtml
│   │
│   │
│   ├── appsettings.json (settings for the app)
│   └── class Program (main program)
├── docker-compose.yml (all containers for this C# project)
├── Dockerfile (docker container of this C# project)
└── README.md
```

# Build and Test
## Run app
1. install [docker](https://docs.docker.com/desktop/setup/install/windows-install) if you haven't yet. follow instructions here: https://docs.docker.com/desktop/setup/install/windows-install/#install-docker-desktop-on-windows
2. startup docker desktop app.
3. make sure you are in the right place by using this command in visual studio's "developer powershell":
```ps1
> ls

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----         3/13/2026   8:57 PM                Docs
d-----         2/24/2026  11:39 AM                Tests
d-----          3/7/2026   3:48 PM                Webshop.Application
d-----         3/10/2026   9:44 AM                Webshop.Domain
d-----         3/10/2026   9:44 AM                Webshop.Infrastructure
d-----         3/13/2026   8:30 PM                Webshop.MVC
-a----         3/13/2026   8:30 PM           6578 .gitignore
-a----         2/24/2026  11:39 AM           1003 docker-compose.yml
-a----         2/24/2026  11:39 AM            888 Dockerfile
-a----         3/13/2026   8:49 PM           6367 README.md
-a----         2/24/2026  11:39 AM           3134 Webshop.sln
```
4. start docker services with following command:
```ps1
docker-compose up -d --build
```

## Remove app
1. use following command:
```ps1
docker-compose down --remove-orphans
```

## Debug
1. after u have done the steps under [Run app](#run-app)
2. stop the webapp in docker it has the name `webshop.website`
3. start the project in visual studio

the code in visual studio will connect to the postgress database in docker. And u can just start debugging normaly with visual studio.

## See Database
1. use a external database client: [datagrip](https://www.jetbrains.com/datagrip), [beekeeper studio](https://www.beekeeperstudio.io/), ...
2. credentials needed for connection down here: 
- database-provider: [PostgresSQL](https://www.postgresql.org)-v18
- host: `localhost`
- port: `5331`
- database: `webshop`
- username: `postgres`
- password: `postgres`

# Flow
1. below is the general flow of the program, in this example: a user visits the product list page.
```
Browser
   │
   ▼
==== Webshop.website container =====
ASP.NET Routing
   │
   ▼
ProductController (Website Layer)
   │
   ▼
ProductService (Application Layer)
   │
   ▼
ProductRepository (Infrastructure Layer)
   │
   ▼
AppDbContext (Infrastructure Layer)
====================================
   │
   ▼
==== Webshop.database container ====
Database (postgres SQL)
====================================
```

Then back:
```
==== Webshop.database container ====
Database (postgres SQL)
====================================
   │
   ▼
==== Webshop.website container =====
AppDbContext (Infrastructure Layer)
   │
   ▼
ProductRepository (Infrastructure Layer)
   │
   ▼
ProductService (Application Layer)
   │
   ▼
ProductController (Website Layer)
   │
   ▼
Product/List.cshtml + ProductListViewModel (Website Layer)
====================================
   │
   ▼
Browser (HTML)
```

# Workflow
is here: [workflow](./Docs/Workflow.md)