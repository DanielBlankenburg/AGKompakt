using BGH_Kompakt.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace BGH_Kompakt.Services
{
    public class DBUnitBase<ModelName> where ModelName : class
    {
        //private readonly ActivityRequestDBContext _dBContext;
        //private readonly DbSet<ModelName> _dBSet;

        //public DBUnitBase()
        //{
        //    _dBContext = new ActivityRequestDBContext();
        //    _dBSet = _dBContext.Set<ModelName>();
        //}

        //public async Task<ModelName> GetFirstOrDefaultAsync(Expression<Func<ModelName, bool>> filter = null, string inCludproperties = null)
        //{
        //    try
        //    {
        //        IQueryable<ModelName> query = _dBSet;
        //        if (filter != null)
        //        {
        //            query = query.Where(filter);
        //        }

        //        if (inCludproperties != null)
        //        {
        //            var MesIncludeModels = inCludproperties.Split(',');
        //            foreach (var includeModel in MesIncludeModels)
        //            {
        //                query = query.Include(includeModel);
        //            }
        //        }

        //        return await query.FirstOrDefaultAsync();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        }


        //public async Task<IEnumerable<ModelName>> GetAllAsync(Expression<Func<ModelName, bool>> filter = null, string inCludproperties = null)
        //{
        //    try
        //    {
        //        IQueryable<ModelName> query = _dBSet;
        //        if (filter != null) 
        //        {
        //            query = query.Where(filter);
        //        }

        //        if (inCludproperties != null)
        //        {
        //            var MesIncludeModels = inCludproperties.Split(',');
        //            foreach (var includeModel in MesIncludeModels)
        //            {
        //                query = query.Include(includeModel);
        //            }
        //        }

        //        return await query.ToListAsync();
        //    }
        //    catch (Exception)
        //    {

        //        return null;
        //    }
        //}

        //public async Task<DBResponse> AddAsync(ModelName iModel) 
        //{
        //    DBResponse resp = new DBResponse();
        //    try
        //    {
        //        var erg = _dBSet.Add(iModel);
        //        await _dBContext.SaveChangesAsync();
        //        resp.Success = true;

        //        return resp;
        //    }
        //    catch (Exception ex)
        //    {

        //        resp.Message = ex.Message;
        //        return resp;
        //    }
        //}
        //public async Task<DBResponse> DeleteAsync(int iID)
        //{
        //    DBResponse resp = new DBResponse();
        //    try
        //    {
        //        if (iID > 0)
        //        {
        //            ModelName modelName = await GetByIDAsync(iID);
        //            if (modelName is null)
        //            {
        //                resp.Message = $"Das Objekt mit der ID = {iID} konnte nicht gefunden werden.";
        //                return resp;
        //            }
        //            _dBSet.Remove(modelName);
        //            await _dBContext.SaveChangesAsync();
        //            resp.Success = true;
        //            return resp;
        //        }

        //        resp.Message = "Das Objekt mit der ID = 0 wurde nicht gefunden.";
        //        return resp;
        //    }
        //    catch (Exception ex)
        //    {

        //        resp.Message = ex.Message;
        //        return resp;
        //    }
        //}
        //public async Task<DBResponse> UpdateAsync(ModelName iModel) 
        //    {
        //    DBResponse resp = new DBResponse();
        //    try
        //    {
        //        //_dBSet.AddOrUpdate(iModel);
        //        _dBContext.Entry(iModel).State = EntityState.Modified;
        //        await _dBContext.SaveChangesAsync();

        //        resp.Success= true;
        //        return resp;
        //    }
        //    catch (Exception ex)
        //    {

        //        resp.Message = ex.Message;
        //        return resp;
        //    }
        //}
        //public async Task<ModelName> GetByIDAsync(int iID) 
        //{
        //    try
        //    {
        //        if (iID > 0)
        //        {
        //            return await _dBSet.FindAsync(iID);
        //        }
        //        return null;
        //    }
        //    catch (Exception)
        //    {

        //        return null;
        //    }  
        //}


    //}
}
