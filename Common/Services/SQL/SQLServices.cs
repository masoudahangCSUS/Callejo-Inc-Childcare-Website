using Common.Models.Data;
using Common.View;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.SQL
{
    public class SQLServices : ISQLServices
    {
        private CallejoSystemDbContext _context;

        public SQLServices(CallejoSystemDbContext context)
        {
            _context = context;
        }
        public ListChildrenGuardianView GetListOfAllChildrenAndGuardians()
        {
            ListChildrenGuardianView listChildren = new ListChildrenGuardianView();

            try
            {
                string connectionString = _context.Database.GetConnectionString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
               select 
                  c.id as childId,
                  ISNULL(c.first_name, '') as childFirstName,
                  ISNULL(c.middle_name, '') as childMiddleName,
                  c.last_name as childLastName,
                  ISNULL(ciu.first_name, '') as guadianFirstName,
                  ISNULL(ciu.middle_name, '') as guardianMiddleName,
                  ciu.last_name as guardianLastName,
                  ciu.address,
                  ciu.city,
                  ciu.zip_code
               from
                  Guardians g
                  inner join Children c
                     on c.id = g.fk_children
                  inner join Callejo_Inc_Users ciu
                     on ciu.id = g.fk_parent
               order by
                  c.id
            ";

                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                    DataSet dataSet = new DataSet();
                    sqlAdapter.Fill(dataSet);

                    ChildrenGuadianView childrenGuadianView = null;
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        childrenGuadianView = new ChildrenGuadianView();
                        childrenGuadianView.childId = long.Parse(dataRow["childId"].ToString());
                        childrenGuadianView.childFirstName = dataRow["childFirstName"].ToString();
                        childrenGuadianView.childMiddleName = dataRow["childMiddleName"].ToString();
                        childrenGuadianView.childLastName = dataRow["childLastName"].ToString();
                        childrenGuadianView.guardianId = long.Parse(dataRow["guardianId"].ToString());
                        childrenGuadianView.guadianFirstName = dataRow["guadianFirstName"].ToString();
                        childrenGuadianView.guardianMiddleName = dataRow["guardianMiddleName"].ToString();
                        childrenGuadianView.guardianLastName = dataRow["guardianLastName"].ToString();
                        childrenGuadianView.address = dataRow["address"].ToString();
                        childrenGuadianView.city = dataRow["city"].ToString();
                        childrenGuadianView.zip_code = dataRow["zip_code"].ToString();

                        listChildren.listChildrenGuardian.Add(childrenGuadianView);
                    }

                    listChildren.Success = true;
                    listChildren.Message = "Retrieved " + listChildren.listChildrenGuardian.Count.ToString() + " number of records";
                }
            }
            catch (Exception ex)
            {
                listChildren.Success = false;
                listChildren.Message = "Problems retrieving children and guardian records. Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }
            return listChildren;
        }
    }

}
