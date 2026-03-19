# troubleshoot
feel free to extend or add to this document if you encountered a issue or something else

## is not running
1. make sure docker is running.
2. `docker-compose up -d --build` was executed accourding to proper instructions
3. containers all running without any errors

## database problems
1. exceute commands below
```
docker-compose down -v
```
2. run application again. [link](../README.md)
3. see that docker container `webshop.website` crashes
4. execute migrations from visual studio

## wierd build errors
1. in this case check first for any errors in the code.
2. delete the bin folder of the projects in this solution and rebuild.
3. restart the computer you are using and try to run the app again