# CDNApplication
## Techology used
1) C# under ASP.NET CORE MVC
2) MS SQL Server

## How to run
1) Open the CDN.API/DBScript folder
2) Run the script from 00 to 02 in MS SQL server
3) Then can run the program to open swagger to test the GET/PUT/POST/DELETE APIs

## Explaination
1) There is HTTP GET API to retrieve the data of all the registered user.
   This API is accepting page size and page number for pagination. It also will return a total record count for the front end to be able to calculate how many pages 
   that it require to display.

2) There is HTTP POST API to add new user data, HTTP DELETE to remove user data, HTTP PUT to update user data which calling the same method. The method will then 
   uses enum to differentiate the action that it require to perform on the stored procedure.
