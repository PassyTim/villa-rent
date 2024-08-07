
# VillaRent

Pet project consisting of API and frontend for villa booking service.



## Features

- CRUD operations on 2 domain models (villa and villa number)
- Redis caching
- Authtantication and authorization via jwt tokens
- Сan be launched in a docker container


## Technologies

**API:** ASP.NET Core 8, Redis, MS SQL Server

**Client:** ASP.NET MVC 




# API Endpoints

## Auth

#### Login

```http
  POST /api/usersAuth/login
```

Request body:
{
  "username": "string",
  "password": "string"
}

#### Registration

```http
  POST /api/usersAuth/register
```

Request body:
{
  "username": "string",
  "name": "string",
  "password": "string",
  "isAdmin": true
}

## Villa

Requires JWT Token in Authentication header

#### Get all villas

```http
  GET /api/villaApi
```

#### Get villa

```http
  GET /api/villaApi/{id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | **Required**. Id of villa |

#### Create villa

```http
  POST /api/villaApi
```
Request body:
{
  "name": "string",
  "details": "string",
  "rate": 0,
  "imageUrl": "string",
  "amenity": "string",
  "occupancy": 0,
  "sqft": 0
}

#### Delete villa

```http
  DELETE /api/villaApi/{id}
```

#### Update villa

```http
  PUT /api/villaApi/{id}
```
Request body:
{
  "name": "string",
  "details": "string",
  "rate": 0,
  "imageUrl": "string",
  "amenity": "string",
  "occupancy": 0,
  "sqft": 0
}

#### Update partial villa

```http
  PATCH /api/villaApi/{id}
```

## Villa Number

Requires JWT Token in Authentication header

#### Get all villa numbers

```http
  GET /api/villaNumberApi
```

#### Get villa number

```http
  GET /api/villaNumberApi/{number}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `number`      | `int` | **Required**. Number of villa number |

#### Create villa number

```http
  POST /api/villaNumberApi/{number}
```
Request body:
{
  "villaNo": 0,
  "villaId": 0,
  "details": "string"
}

#### Delete villa number

```http
  DELETE /api/villaNumberApi/{number}
```

#### Update villa number

```http
  PUT /api/villaNumberApi/{number}
```
Request body:
{
  "villaNo": 0,
  "villaId": 0,
  "details": "string"
}

#### Update partial villa number

```http
  PATCH /api/villaNumberApi/{number}
```


# Installation

    
## Run Frontend

Clone the project

```bash
  git clone https://github.com/PassyTim/villa-rent.git
```

Go to the project directory

```bash
  cd villa-rent/VillaRent.Web
```

Install dependencies

```bash
  dotnet restore
```


Run application
```bash
  dotnet run
```
Then run API 



## Api Docker Run

Make sure you have [Docker](https://docs.docker.com/get-docker/) и [Docker Compose](https://docs.docker.com/compose/install/).

Application Docker image can be downloaded as follows:

```bash
docker push passyexe/villarent.api:v1.1
```

Application docker-compose.yml can be found in the repository

Switch to the folder containing the docker-compose file:

```bash
cd /path/to/your/compose-file
```

To start containers, execute:

```bash
docker-compose up -d
```

To stop and delete containers, use the following command:

```bash
docker-compose down
```
