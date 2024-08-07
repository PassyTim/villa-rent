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




## API Reference

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
| `id`      | `int` | **Required**. Id of villa to fetch |



    
## Run Locally

Clone the project

```bash
  git clone https://link-to-project
```

Go to the project directory

```bash
  cd my-project
```

Install dependencies

```bash
  npm install
```

Start the server

```bash
  npm run start
```


## Docker Run

Make sure you have [Docker](https://docs.docker.com/get-docker/) и [Docker Compose](https://docs.docker.com/compose/install/).

Application Docker image can be downloaded as follows:

```bash
docker pull yourdockerhubusername/yourappname:latest
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
