﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoCRUD.Interfaces
{
    public interface IRepository<T> where T:DbObject
    {
        bool Insert(T entity);
        bool InsertMany(params T[] entities);
        bool Delete(string id);
        bool Update(string id, T entity);
        bool Upsert(T entity);
        List<T> GetAll(BsonDocument filter);
        T GetOne(string id);
        List<T> Search(string value, string column);
        List<T> MultipleColumnSearch(string value, IEnumerable<string> columns);
    }
}