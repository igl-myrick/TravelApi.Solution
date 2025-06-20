# Travel API

#### An API for searching travel destinations.

#### By _**India Lyon-Myrick**_

## Technologies Used

* _C#_
* _.NET_
* _MySQL_
* _MySQL Workbench_
* _Git_

## Description

_An API for searching up travel destinations. The API includes full CRUD functionality for viewing any and all reviews, adding new reviews, and editing or deleting existing reviews. Additionally, there are endpoints for viewing a random review, or viewing destinations by either how many times they've been reviewed, or their aggregated review score. Prior to accessing any travel information endpoints, users must create an account via the ``register`` endpoint. After creating an account and accessing the ``login`` endpoint, users will have access to travel endpoints for 10 minutes before they must sign in again._

_Note: When making API calls through Postman, please copy the token returned by the ``login`` route. To make calls, access the ``Authorization`` tab of your request, set the type to ``Bearer Token``, and paste your token. Once again, tokens expire after 10 minutes. After that, you will need to log in again and update your token._

## Setup/Installation Requirements

* _You will need [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/6.0), [MySQL](https://downloads.mysql.com/archives/get/p/25/file/mysql-installer-web-community-8.0.19.0.msi), and [Git](https://git-scm.com/downloads/) in order to run the program._

_1: Clone the repository to a folder of choice on your machine (by either using the "Code" button on the GitHub page, or in a terminal application using `git clone https://github.com/igl-myrick/TravelApi.Solution`)._

_2: Using a terminal application such as Git Bash or Windows Command Prompt, navigate to the top level of the program folder, then into the `TravelApi` folder._

_3: You will need to create an `appsettings.json` file within the `TravelApi` folder, including the following code:_

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;database=[YOUR-DB-NAME];uid=[YOUR-USER-HERE];pwd=[YOUR-PASSWORD-HERE];"
  },
  "Jwt": {
    "Key": "[YOUR-KEY-HERE]",
    "Issuer": "http://localhost:5000",
    "Audience": "dotnetclient"
  }
}
```

_Insert your own MySQL username in place of [YOUR-USER-HERE], MySQL password in place of [YOUR-PASSWORD-HERE], and the name of your database in place of [YOUR-DB-NAME]. You will additionally need to insert a string of letters and/or numbers at least 32 characters long in place of [YOUR-KEY-HERE]._

_4: Once the `appsettings.json` file has been created, run the command `dotnet ef database update` in your terminal to create the database._

_5: Next, run `dotnet build` in the command line to build the program._

_6: After the program is built, run `dotnet run` to start the program._

_7: Once the program is running, you can make calls to the API through either your own webpage/programs, or through an application like [Postman](https://www.postman.com/downloads/)._

## API Endpoints

### Manage accounts:

_Registration:_

``POST http://localhost:5000/api/auth/register``

_Logging in:_

``POST http://localhost:5000/api/auth/login``

### Travel endpoints:

_Access a list of all reviews:_

``GET http://localhost:5000/api/reviews/``

_Access a specific review by id:_

``GET http://localhost:5000/api/reviews/{id}``

_Add a  new review:_

``POST http://localhost:5000/api/reviews/``

_Edit an existing review:_

``PUT http://localhost:5000/api/reviews/{id}``

_Delete an existing review:_

``DELETE http://localhost:5000/api/reviews/{id}``

_View a random review:_

``GET http://localhost:5000/api/reviews/random``

_View the most reviewed destinations:_

``GET http://localhost:5000/api/reviews/popular``

_View the highest rated destinations:_

``GET http://localhost:5000/api/reviews/highest-rated``

### Endpoint details

_The first GET route, which displays all reviews, has two optional parameters that can be used to narrow the list of reviews:_

| Parameter | Type | Required | Description | Example query |
|---|---|---|---|---|
| country | String | not required | Returns all reviews with a matching country value | GET http://localhost:5000/api/reviews?country=usa |
| city | String | not required | Returns all reviews with a matching city value | GET http://localhost:5000/api/reviews?city=london

_You can also chain these queries together, for example:_ 

``GET http://localhost:5000/api/reviews?country=usa&city=portland``

---

_When making a request to the POST endpoint, you must include a body in JSON format containing the review's information. Here is an example request:_

```
{
  "body": "Lorem ipsum dolor sit amet.",
  "rating": 5,
  "country": "USA",
  "city": "Phoenix",
  "userName": "user@example.com"
}
```

_PUT requests have similar body requirements, but you must additionally specify the id of the review you wish to edit. Here is an example request to ``http://localhost:5000/api/reviews/1``:_

```
{
  "reviewId": 1,
  "body": "Lorem ipsum dolor sit amet.",
  "rating": 3,
  "country": "USA",
  "city": "Portland",
  "userName": "name@email.com"
}
```

## Known Bugs

* _None at the moment_

## License

MIT:

Copyright (c) _6/11/2025_ _India Lyon-Myrick_

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.