using Common.Models.Data;
using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Role
{
    public class RoleService : IRoleService
    {
        // DbContext class provides access to database
        private CallejoSystemDbContext _context;

        // Constructor uses dependency injection
        public RoleService(CallejoSystemDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Deletes role record from database
        /// NOTE: Deletion will fail if role is referred to in any other table
        /// </summary>
        /// <param name="id">Primary key of the record to delete</param>
        /// <returns></returns>
        public APIResponse DeleteRole(long id)
        {
            APIResponse response = new APIResponse();

            try
            {
                var roleRecord = _context.Roles.Where(r => r.Id == id).FirstOrDefault();
                if (roleRecord != null)
                {
                    response.Message = "Role record " + roleRecord.Id.ToString() + " : " + roleRecord.Description + " has been deleted";
                    _context.Roles.Remove(roleRecord);
                    _context.SaveChanges();
                }
                else
                {
                    response.Message = "No record with the id of " + id.ToString() + " was not found";
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Problems deleting role record. Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return response;
        }
        /// <summary>
        /// Get all roles in the role table
        /// </summary>
        /// <returns></returns>
        public ListRoles GetAllRoles()
        {
            ListRoles listRoles = new ListRoles();

            try
            {
                var roleRecs = _context.Roles.ToList();
                RoleView roleViewRec = null;
                foreach (Models.Data.Role roleRec in roleRecs)
                {
                    roleViewRec = new RoleView();
                    roleViewRec.Id = roleRec.Id;
                    roleViewRec.Description = roleRec.Description;

                    listRoles.roles.Add(roleViewRec);
                }

                listRoles.Success = true;
                listRoles.Message = "Retrieved " + listRoles.roles.Count.ToString() + " role records";
            }
            catch (Exception ex)
            {
                listRoles.Success = false;
                listRoles.Message = "Problems retrieving all role record. Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return listRoles;
        }
        /// <summary>
        /// Retrieve a role record
        /// </summary>
        /// <param name="id">Primary key for role record</param>
        /// <returns></returns>
        public ListRoles GetRole(long id)
        {
            ListRoles listRoles = new ListRoles();

            try
            {
                var roleRec = _context.Roles.Where(r => r.Id == id).FirstOrDefault();

                if (roleRec != null)
                {
                    RoleView roleViewRec = new RoleView();
                    roleViewRec.Id = roleRec.Id;
                    roleViewRec.Description = roleRec.Description;

                    listRoles.roles.Add(roleViewRec);
                    listRoles.Message = "Retrieved role record that matched id " + id.ToString();
                }
                else
                {
                    listRoles.Status = "No record matching id " + id.ToString() + " would found";
                }
                listRoles.Success = true;
            }
            catch (Exception ex)
            {
                listRoles.Success = false;
                listRoles.Message = "Problems retrieve role record " + id.ToString() + ". Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return listRoles;
        }
        /// <summary>
        /// Creates a role reccord in role database
        /// </summary>
        /// <param name="roleInfo">Data to be saved.  Since the id field is an auto increment field we will not retrieve that value</param>
        /// <returns></returns>
        public APIResponse InsertRole(RoleView roleInfo)
        {
            APIResponse response = new APIResponse();

            try
            {
                Models.Data.Role role = new Models.Data.Role();
                role.Description = roleInfo.Description;
                _context.Roles.Add(role);
                _context.SaveChanges();
                response.Message = "Role record with description " + roleInfo.Description + " was saved to database";

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Problems saving role record " + roleInfo.Description + ". Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return response;
        }
        /// <summary>
        /// Will update role record
        /// </summary>
        /// <param name="roleView">Contains id of record to update and description</param>
        /// <returns></returns>
        public APIResponse UpdateRole(RoleView roleView)
        {
            APIResponse response = new APIResponse();

            try
            {
                var roleRec = _context.Roles.Where(r => r.Id == roleView.Id).FirstOrDefault();

                if (roleRec != null)
                {
                    roleRec.Description = roleView.Description;
                    _context.SaveChanges();
                    response.Message = "Role record with id " + roleView.Id.ToString() + " was updated to database";
                }
                else
                {
                    response.Message = "No Role record with id " + roleView.Id.ToString() + " was found in database";
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Problems updating role record " + roleView.Id + ". Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return response;
        }
    }

}
