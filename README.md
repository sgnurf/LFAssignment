# LinkFire Assignment

## Running the solution

Prerequistes :
* Dotnet5 runtime
* Elastic Search  `docker run -d -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:7.12.0`

Open a command line prompt in the project folder and execute `dotnet run`

## GoogleApi Key

Add the api key to the appsettings.json file under the key `GoogleApi:ApiKey` 
or use dotnet secrets
`dotnet user-secrets set "GoogleApi:ApiKey" "myKey"`

## Points fo interests

* Each phase (Download, Extract, Read/Load, Index) is decoupled by using in-process messaging with MediatR. It can easily be replaced with a cloud based messaging system
* Access to files has been abstracted. Where the files are stored (local disk, Azure Blob, ...) can be changed through configuration

## Potential Improvements

* Stored the parsed files in a Database instead of keeping them in memory. This would make querying easier and allow scaling by splitting the generation of `Albums` from the `Collections` over several workers. This would also aloow different services to ingest other part of the data.
* Split the app in Micro-Services
  * A `FileRetievalService` service to download and extract the files, quite generic and reusable.
  * A `ParsingService` to load the files in the DB
  * An `AlbumIndexer` service to create the `Album` documents and store them in ElasticSearch
