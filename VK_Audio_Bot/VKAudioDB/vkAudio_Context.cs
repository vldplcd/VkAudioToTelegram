using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace VKAudioDB
{
    class vkAudio_Context : IDisposable
    {
        MongoClient client;
        IMongoDatabase database;
        private bool disposed = false;

        public vkAudio_Context()
        {
            client = new MongoClient();
            database = client.GetDatabase("bot_users");
        }

        public IMongoCollection<ak> aks
        {
            get { return database.GetCollection<ak>("ak"); }
        }

        public IMongoCollection<Track> Tracks
        {
            get { return database.GetCollection<Track>("tracks"); }
        }

        public IMongoCollection<BsonDocument> TracksBson
        {
            get { return database.GetCollection<BsonDocument>("tracks"); }
        }

        public IMongoCollection<User> Users
        {
            get { return database.GetCollection<User>("users"); }
        }

        public IMongoCollection<BsonDocument> UsersBson
        {
            get { return database.GetCollection<BsonDocument>("users"); }
        }

        public IMongoCollection<SavedInfo> SavedInfo
        {
            get { return database.GetCollection<SavedInfo>("savedInfo"); }
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
