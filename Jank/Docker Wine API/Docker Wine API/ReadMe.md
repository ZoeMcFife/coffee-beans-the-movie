# Wine API

## Docker Container

**First Build the Docker Container**
- execute `update-database.bat` (note: Connection error might pop up, can be ignored. if test data isn't in database, run this again!)
- execute `build.bat`
- (execute `run.bat` file) (not needed after build, since container is running)

**Afterwards; you only need to run it via the bat file or in docker.**

- execute `run.bat` file

- enjoy!

## Database

- open `http://localhost:8080/` for [Adminer](http://localhost:8080/) 
- Credentials:
    - System : PostgreSQL
    - Server : db
    - Username : wineAdmin
    - Password : 12345678
    - Database : WineDb

## Swagger

- open `https://localhost:80/swagger` for [Swagger](https://localhost:80/swagger)

## How to use the Api

- Login with a user
- Default User:
    - testUser1@gmail.com
    - password123

- You'll get a jwt Token in return
- Use that to authorize further requests! 