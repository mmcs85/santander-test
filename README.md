# Santander Coding Test

Implementation of Senior Backend Developer Coding Test.
Using ASP.NET Core 2.2, implement a RESTful API to retrieve the details of the first 20 "best
stories" from the Hacker News API.

### assumptions

This implementation uses two physical processes, Publisher and API (Subscriber) to represent a logical microservice.
Both communicate using NetMQ (based on ZeroMQ) to send messages in a publisher / subscribers pattern.

The Publisher fetches in a pooling manner the Hacker News API, transforms it and publish's to the message queue.
The API (Subscriber) receives the message with the stories and caches it in a MemoryCache.
The REST API then reads the memoryCache and returns to the client on request

### Possible improvements



### Running

##### Running the publisher

```
cd SantanderTest.Publisher
dotnet run
```

##### Running the API (subscriber)

```
cd SantanderTest.API
dotnet run
```

##### Testing API

https://localhost:5001/api/stories

