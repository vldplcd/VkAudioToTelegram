using System;
using MongoDB.Driver;
using MongoDB.Bson;
namespace VKAudioDB
{
    class vkAudio_Context
    {
        MongoClient client;
        IMongoDatabase database;
        private bool disposed = false;

        public vkAudio_Context()
        {
            client = new MongoClient(new MongoClientSettings
            {

            });
            database = client.GetDatabase("bot_users");
        }

        public IMongoCollection<TransportNode> TransportNodes
        {
            get { return database.GetCollection<TransportNode>("TransportNodes"); }
        }
        public IMongoCollection<BsonDocument> TransportNodesBson
        {
            get { return database.GetCollection<BsonDocument>("TransportNodes"); }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                client = null;
                database = null;
                disposed = true;
            }
        }
    }
}
