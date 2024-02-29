
namespace BlImplementation;

using BlApi;
using BO;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
                (item.Name, item!.Id, item.UserName, item.Password, item.IsAdmin);
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
        if (doUser == null)
            throw new BlDoesNotExistException($"User with ID={id} does Not exist");
        try
        {
            return new BO.User()
            {
                Name = doUser.Name,
                Id = id,
                UserName = doUser.UserName,
                Password = doUser.Password!,
                IsAdmin = doUser.IsAdmin
            };
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
                    select new BO.User()
                    {
                        Name = doUser.Name,
                        Id = doUser.Id,
                        UserName = doUser.UserName,
                        Password = doUser.Password!,
                        IsAdmin = doUser.IsAdmin
                    });
        else
            return (from DO.User doUser in _dal.User.ReadAll()
                    let boUser = new BO.User()
                    {
                        Name = doUser.Name,
                        Id = doUser.Id,
                        UserName = doUser.UserName,
                        Password = doUser.Password!,
                        IsAdmin = doUser.IsAdmin
                    }
                    where filter(boUser)
                    select boUser);
    }
    public void Update(BO.User? item)
    {
        DO.User doUser = new(item!.Name, item!.Id, item.UserName, item.Password, item.IsAdmin);
        _dal.User.Update(doUser);
    }
    public User ReadByNameAndPassword(string userName, string password)
    {
        try
        {
            IEnumerable<User> users = ReadAll();

            User user = users.FirstOrDefault(u => u.UserName == userName && u.Password == password)! ?? throw new BlDoesNotExistException($"User does not exist"); ;

            if (user.UserName == null || user.Password == null)
            {
                throw new BlDoesNotExistException($"Not all the fields are full ");
            }

            if (user == null)
            {
                throw new BlDoesNotExistException($"User with Name={userName} and Password={password} does not exist");
            }

            return user;
        }
        catch (BlDoesNotExistException )
        { throw new BlDoesNotExistException($"User does not exist"); }
    }

}
