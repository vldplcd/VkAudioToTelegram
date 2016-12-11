using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using System.Collections.Generic;

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

        public async void InsertUser(User user)
        {
            if(Users.Find(u => u.chatID == user.chatID).Count() == 0)
                await Users.InsertOneAsync(user);
        }

        public async void InsertTrack(Track track)
        {
            if (Tracks.Find(t => t.dbID == track.dbID).Count() == 0)
                await Tracks.InsertOneAsync(track);
        }

        public async void UpdateUser(long chatID, List<int> tracks)
        {
            var filter = Builders<User>.Filter.Eq((u) => u.chatID, chatID);
            var update = Builders<User>.Update.Set((u) => u.tracks, tracks);
            await Users.UpdateManyAsync(filter, update);
        }

        public async void UpdateSavedInfo (string sID, Dictionary<string, object> info)
        {
            var filter = Builders<SavedInfo>.Filter.Eq((si) => si.sID, sID);
            var update = Builders<SavedInfo>.Update.Set((si) => si.Info, info);
            await SavedInfo.UpdateOneAsync(filter, update);
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
