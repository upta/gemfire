using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Gemfire
{
    public class MongoRepository : IRepository
    {
        private readonly MongoDatabase db;

        public MongoRepository( string connectionString, string database )
        {
            this.db = new MongoClient( connectionString ).GetServer().GetDatabase( database );
            
        }

        public void Delete<T>( string id )
        {
            this.Collection<T>().Remove( Query.EQ( "_id", ObjectId.Parse( id ) ) );
        }

        public IQueryable<T> Find<T>()
        {
            return this.Collection<T>().AsQueryable<T>();
        }

        public T Save<T>( T entity )
        {
            var collection = this.Collection<T>();
            collection.Save( entity );

            return entity;
        }


        private IMongoQuery MongoQuery<T>( IQueryable<T> q )
        {
            return ( (MongoQueryable<T>) q ).GetMongoQuery();
        }

        private MongoCollection<T> Collection<T>()
        {
            return this.db.GetCollection<T>( typeof( T ).Name.ToLower() );
        }
    }
}