using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace test_lib
{
    public class Class1
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public Class1()
        {
            var user = new User();

            _client = new MongoClient();
            _database = _client.GetDatabase("test");

            //_database.GetCollection("asdf").fin
                var replaceResult = _database.GetCollection<User>("").ReplaceOne(x => x.Id == user.Id, user, );
        }

        public class User
        {
            public string Id { get; set; }
        }
    }
}
