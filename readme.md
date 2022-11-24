## Overview
This is a repo that shows a prototype system that is composed of multiple microservices running SQL server and ElasticSearch. It shows how such a system may be composed and how the services may interact with each other.  

### Services/responsibilities
- Vehicle API. This is a REST api that performs some CRUD operations backed by a SQL Server data store. The SQL data store acts as the source of truth for the vehicle entity. Whenever an operation occurs on the data store an event is published that allows subscribers to listen

- Vehicle Search Consumer. This is a hosted process that is listening for VehicleAdded events. If it receives the event the consumer will index the document so that it is searchable.

- Vehicle Search API. This API allows the user to query the elastic index. It has a simple match query and it will return any matching docs and a facet aggregation showing the user counts by make

## How to run
In a terminal navigate to the root directory and run:
```
docker-compose up
```
This will start all the services and dependencies up. 

Once the containers have started there are two main APIs to view:
http://localhost:8748/swagger/index.html - this is the Search API. There should already be some dummy data in the index for you to view

http://localhost:45478/swagger/index.html - this is the Vehicle API. This will allow you to add a new vehicle. 


## Improvements
- Splitting each service into its own repo
- Vehicle schema and API models are very simple. Make and Model inputs are just strings; on a production system these should be vaidated against a set of known Make/Models
- Authentication/Authorisation should be considered
- Secrets handling - the appsettings files contain database strings. In a production system this should be moved to the CI/CD pipeline
- Nuget packages - there are duplicate models across the services that should be moved into shared nuget packages. 
- Number of Unit tests is a bit light



