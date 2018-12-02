﻿using MongoCRUD.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MongoCrudExceptionHandling.Exceptions;
using MongoCrudExceptionHandling.Exceptions.DeleteFailed;
using MongoCrudExceptionHandling.Interfaces;

namespace MongoCRUD
{
    public  class Crud<T> : IRepository<T> where T : DbObject
    {
        private readonly IMongoDatabase _database;
        protected IMongoCollection<BsonDocument> Table;

        /// <summary>
        /// Tries to initialize the database and finds or creates a MongoDB Collection.
        /// </summary>
        public Crud()
        {
            _database = MongoDbConnection.Database;
            Table = _database.GetCollection<BsonDocument>(typeof(T).Name);
        }
        /// <summary>
        /// Finds the object given by the parameter "_id" and updates it. If not found, inserts that object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Upsert(T entity)
        {
            try
            {
                var filter = new BsonDocument { { "_id", entity._id } };
                var updateOption = new UpdateOptions { IsUpsert = true };
                Table.ReplaceOne(filter, entity.ToBsonDocument(), updateOption);
                return true;
            }
            catch (Exception e)
            {
                throw new UpsertFailedException();
            }
        }

        private bool SoftDelete(string id)
        {
            try
            {
                var filter = new BsonDocument { { "_id", id } };
                var builder = Builders<BsonDocument>.Update.Set("IsDeleted", 1);

                Table.UpdateOne(filter, builder);
                return true;
            }
            catch
            {
                throw new SoftDeleteFailedException(id);
            }
        }
        /// <summary>
        /// Deletes the given object by the "id" parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(string id)
        {
            try
            {
                if (typeof(T) == typeof(DbObjectSD))
                {
                    return SoftDelete(id);
                }
                var filter = new BsonDocument { { "_id", id } };
                Table.DeleteOne(filter);
                return true;
            }
            catch
            {
                throw new HardDeleteFailedException(id);
            }
        }
        /// <summary>
        /// Returns all the object in the MongoDB Collection.
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetAll()
        {
            try
            {
                var filter = new BsonDocument { { "IsDeleted", 0 } };
                var results = new List<T>();
                var found = Table.FindSync(filter);
                while (found.MoveNext())
                {
                    var batch = found.Current;
                    results.AddRange(batch.Select(item => BsonSerializer.Deserialize<T>(item)));
                }
                return results;
            }
            catch
            {

                return new List<T>();
            }

        }
        /// <summary>
        /// Removes all content from collection.
        /// </summary>
        public void ClearColection()
        {
            _database.DropCollection(typeof(T).Name);
        }
        /// <summary>
        /// Returns number of rows from collection which is not marked as deleted.
        /// </summary>
        public long Count => Table.Count(new BsonDocument { { "IsDeleted", 0 } });
        /// <summary>
        /// Returns number of rows from collection.
        /// </summary>
        public long CountAll => Table.Count(new BsonDocument());
        /// <summary>
        /// Returns all data based on given filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<T> GetAll(BsonDocument filter)
        {
            try
            {
                if (filter == null) filter = new BsonDocument { { "IsDeleted", 0 } };
                var results = new List<T>();
                var found = Table.FindSync(filter);
                while (found.MoveNext())
                {
                    var batch = found.Current;
                    results.AddRange(batch.Select(item => BsonSerializer.Deserialize<T>(item)));
                }
                return results;
            }
            catch
            {

                return new List<T>();
            }

        }

        private bool FieldCheck(string field)
        {
            var fieldExists = Table.Find(Builders<BsonDocument>.Filter.Exists(field));
            return fieldExists.CountDocuments() > 0;
        }
        #region GetOne
        /// <summary>
        /// Gets the object given by the "id" parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetOne(string id)
        {

            BsonDocument filter;
            if (typeof(T) == typeof(DbObjectSD))
            {
                filter = new BsonDocument { { "_id", id }, { "IsDeleted", 0 } };
            }
            else if(typeof(T) == typeof(DbObject))
            {
                filter = new BsonDocument { { "_id", id }};
            }
            else
            {
                throw new TypeAccessException(); //todo throw new UnsupportedInheritanceException()
            }

            if (!FieldCheck("_id") || !FieldCheck("IsDeleted"))
            {
                throw new GetOneColumnNotFoundException();
            }
            var cursor = Table.FindSync(filter);
            cursor.MoveNext();
            try
            {
                var batch = cursor.Current;
                if (batch == null) return null;
                return BsonSerializer.Deserialize<T>(batch.FirstOrDefault());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Searches given value in given Field. If available, SoftDeletes are included.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<T> Search(string field, string value)
        {
            BsonDocument GetFilter()
            {
                return new BsonDocument { { field, new BsonDocument { { "$regex", "(?i)" + value + "(?-i)" } } } };
            }

            FieldCheckWithException(field);
            var cursor = Table.FindSync(GetFilter());
            cursor.MoveNext();
            var batch = cursor.Current;
            List<T> results = new List<T>();
            results.AddRange(batch.Select(item => BsonSerializer.Deserialize<T>(item)));
            return results;
        }
        /// <summary>
        /// Searches by given value by multiple given Fields.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<T> MultipleFieldSearch(string value,params string[] fields)
        {
            if (!fields.Any())
            {
                throw new MultipleColumnSearchEmptyColumsException();
            }
            BsonDocument GetFilter(string column)
            {
                return new BsonDocument { { column, new BsonDocument { { "$regex", "(?i)" + value + "(?-i)" } } } };
            }
            var filterArray = new BsonArray();
            foreach (var field in fields)
            {
                FieldCheckWithException(field);
                filterArray.Add(GetFilter(field));
            }
            var bsonOr = new BsonDocument{
            {
                "$or",
                filterArray
            }};

            //var bsonResult = new BsonDocument{{"$match",bsonOr}};
            var cursor = Table.FindSync(bsonOr);
            cursor.MoveNext();
            var batch = cursor.Current;
            List<T> results = new List<T>();
            results.AddRange(batch.Select(item => BsonSerializer.Deserialize<T>(item)));
            return results;
        }

        private void FieldCheckWithException(string field)
        {
            if (FieldCheck(field) == false)
            {
                throw new GetOneColumnNotFoundException(field);
            }
        }
        /// <summary>
        /// Returns the object given by field and value. If can't be found, returns null.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value">Must Support ToString()</param>
        /// <returns></returns>
        public virtual T GetOne<T1>(string fieldName, T1 value) //todo T1 must be supporting ToString()
        {
            FieldCheckWithException(fieldName);
            try
            {
                BsonDocument filter;
                if (typeof(T) == typeof(DbObjectSD))
                {
                    filter = new BsonDocument { { fieldName, value.ToString() }, { "IsDeleted", 0 } };
                }
                else if (typeof(T) == typeof(DbObject))
                {
                    filter = new BsonDocument { { fieldName, value.ToString() } };
                }
                else
                {
                    throw new TypeAccessException(); //todo throw new UnsupportedInheritanceException()
                }
                var cursor = Table.FindSync(filter);
                cursor.MoveNext();
                var batch = cursor.Current;
                if (batch == null) return null;
                return BsonSerializer.Deserialize<T>(batch.FirstOrDefault());

            }
            catch
            {
                throw new GetOneFailedException();
            }

        }

        #endregion

        /// <summary>
        /// Marks rows as un-deleted if consistent with the filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool BringBack(BsonDocument filter)
        {
            try
            {
                var builder = Builders<BsonDocument>.Update.Set("IsDeleted", 0);
                Table.UpdateMany(filter, builder);
                return true;
            }
            catch
            {

                return false; //todo BringBackFailed || UpdateFilterFailed
            }
        }
        /// <summary>
        /// Inserts a given entity into the MongoDB Collection.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Insert(T entity)
        {
            try
            {
                Table.InsertOne(entity.ToBsonDocument());
                return true;
            }
            catch
            {
                throw new InsertFailedException(entity);
            }
        }

        /// <summary>
        /// Inserts given entities into the MongoDB Collection.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual bool InsertMany(params T[] entities)
        {
            try
            {
                Table.InsertMany(entities.Select(x => x.ToBsonDocument()));
                return true;
            }
            catch
            {
                return false;// todo InsertManyFailedException
            }


        }
        /// <summary>
        /// Updates a chosen entity. Note that it can also change _id property of entity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Update(string id, T entity)
        {
            try
            {
                var filter = new BsonDocument { { "_id", id } };
                var bsonEntity = entity.ToBsonDocument();
                var update = new BsonDocument { { "$set", bsonEntity } };
                Table.UpdateOne(filter, update);
                return true;
            }
            catch
            {
                throw new UpdateFailedException(id,entity);
            }
        }
    }
}
