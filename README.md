# Kore Wireless Redis Session
All the information you need for the Redis learning session

### Prerequisites
* Have Visual Studio Installed with C# support
* Have this repo cloned on your machine

### Notes
* Redis is pronounced 'Red-iss'
* [THIS](https://github.com/StackExchange/StackExchange.Redis) is the client we will be using
* [THIS](https://marketplace.visualstudio.com/items?itemName=MohsenKokabi.RedisExplorer-13503) is a Visual Studio extension that lets you investigate a Redis server with a GUI, if you so desire

### If you want to play with your own Redis instance
* Have the Windows Subsystem for Linux enabled and the Ubuntu terminal installed
* Run the following to install Redis:
    ```
    wget hhttp://download.redis.io/releases/redis-5.0.5.tar.gz
    tar xzf redis-5.0.5.tar.gz
    cd redis-5.0.5
    make
    ```
* Run the following from within redis-5.0.5 to start Redis locally
    ```
    ./src/redis-server redis.conf
    ```
* The complete documentation is [HERE](https://redis.io/documentation)
