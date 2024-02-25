using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IUser
{
    void Create(BO.User item); //Creates new entity object in DAL
    BO.User? Read(string id); //Reads entity object by its ID 
    IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null); //stage 1 only, Reads all entity objects
    void Delete(string id); //Deletes an object by its Id
    void Clear();//clear all the enitity
}
