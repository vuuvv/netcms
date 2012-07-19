using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using vuuvv.utils;

using vuuvv.data.sql.query;
using vuuvv.data.schemas;
using vuuvv.data.schemas.fields;

namespace vuuvv.data.models
{
    public class Manager<T> 
        where T : Model
    {
        public virtual T Get(int pk) 
        {
            return null;
        }

        public virtual void Insert(T model)
        {
            var query = new InsertQuery(model);
            model.Id = (int?)query.DoQuery();
        }

        public virtual void Insert(T model, string[] fields)
        {
            var query = new InsertQuery(model, fields);
            model.Id = (int?)query.DoQuery();
        }

        public virtual void Update(T model)
        {
            var query = new UpdateQuery(model);
            query.DoQuery();
        }

        public virtual void Update(T model, string[] fields)
        {
            var query = new UpdateQuery(model, fields);
            query.DoQuery();
        }

        public virtual int delete()
        {
            return 0;
        }
    }
}
