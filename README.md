# Microservices + SPA

Este proyecto implementa una arquitectura basada en microservicios utilizando .NET para el backend y Blazor WebAssembly para el frontend.
La aplicación permite gestionar usuarios y productos con autenticación basada en JWT y control de roles.

## Arquitectura del Proyecto

El sistema está compuesto por:

* **User Microservice**

  * Registro y autenticación de usuarios
  * Generación de tokens JWT

* **Product Microservice**

  * CRUD de productos
  * Autorización basada en roles (Admin / User)

* **SPA (Blazor WebAssembly)**

  * Interfaz de usuario
  * Consumo de los microservicios
  * Manejo de sesión mediante JWT

---

* # 1. Requisitos del entorno

Para ejecutar el proyecto es necesario contar con las siguientes herramientas:

* .NET 8
* SQL Server
* Visual Studio 2022
* Git

---

# 2. Ejecutar los Microservicios y el SPA

Cada microservicio se debe ejecutar en una instancia diferente.
También es posible configurar en Visual Studio la ejecución de múltiples proyectos, haciendo click en el menú "Proyecto > Configurar proyectos de inicio".
Se debe habilitar la opción "Proyectos de inicio múltiples" y se debe marcar con "Iniciar" los siguientes proyectos:
* FrontApplication
* ProductService.Api
* UserService.Api

---

# 3. Configuración de las Bases de Datos

Cada microservicio utiliza su propia base de datos independiente, siguiendo el principio de aislamiento de microservicios.

### Bases de datos requeridas

* UserDb
* ProductDb

### Configuración de conexión

Editar el archivo appsettings.json.

Ejemplo:

```json
"ConnectionStrings": {
  "ProductDb": "Server=localhost;Database=ProductDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Cada microservicio tiene su propia cadena de conexión.

---

# 4. Migraciones de Base de Datos

Para crear las tablas se debe ejecutar los siguientes comandos en cada microservicio en la Consola del Administrador de Paquetes.

### UserService

Update-Database

### ProductService

Update-Database

Esto aplicará las migraciones de Entity Framework Core y generará las tablas necesarias.

---

# 5. Configuración del JWT

El sistema utiliza JWT (JSON Web Tokens) para autenticación.

Se configura en el appsettings.json de ambos microservicio. Ejemplo:

```json
"JwtSettings": {
  "SecretKey": "THIS_IS_A_SUPER_SECRET_KEY_FOR_JWT",
  "Issuer": "ProductApp",
  "Audience": "ProductAppUsers",
  "ExpirationMinutes": 60
}
```

---

# 6. Flujo de uso de la aplicación

1. Registrar un nuevo usuario
2. Iniciar sesión
3. El sistema generará un token JWT
4. El token se almacena en LocalStorage
5. Las peticiones al microservicio de productos incluyen automáticamente el token

---

* # 7. Tecnologías utilizadas

Backend

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication

Frontend

* Blazor WebAssembly
* MudBlazor

---

# 8. Consideraciones de arquitectura

* Cada microservicio posee su propia base de datos
* Autenticación centralizada mediante JWT
* Control de roles en frontend y backend
* Separación por capas (Domain, Application, Infrastructure)

