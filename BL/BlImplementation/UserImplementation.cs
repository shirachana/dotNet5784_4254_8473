
namespace BlImplementation;

using BlApi;
using BO;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void Clear()
    {
        _dal.User.Clear();
    }

    public void Create(User item)
    {
        try
        {
            if (Tools.CheckUserHealth(item))//its good
            {
                DO.User doUser = new DO.User
                (item!.Id, item.userName, item.isAdmin);
                _dal.User.Create(doUser);
                // Proceed with other operations if the engineer is healthy
            }
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistException($"User with ID={item!.Id} already exists", ex);
        }
    }

    public void Delete(string id)
    {
        try
        {
            _dal.User.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to delete User with ID={id}", ex);
        }
    }

    public BO.User? Read(string id)
    {
        DO.User? doUser = _dal.User.Read(id);
        if (doUser  == null)
            throw new BlDoesNotExistException($"User with ID={id} does Not exist");
        try
        {
            return new BO.User(id, doUser.userName, doUser.isAdmin);
        }
        catch (DO.DalDoesNotExistException)
        {
            throw new BlDoesNotExistException($"User with ID={id} does Not exist");
        }
    }

    public IEnumerable<User> ReadAll(Func<BO.User, bool>? filter = null)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();
        if (filter == null)//if there no filter
            return (from DO.User doUser in _dal.User.ReadAll()
                    select new BO.User(doUser.Id, doUser.userName, doUser.isAdmin));
        else
            return (from DO.User doUser in _dal.User.ReadAll()
                    let boUser = new BO.User(doUser.Id, doUser.userName, doUser.isAdmin)
                    where filter(boUser)
                    select boUser);
    }
}
