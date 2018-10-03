using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PushNotification
{
    public class NotificationComponent
    {
        // Register notification (will add sql dependency)
        public void RegisterNotification(DateTime currentTime)
        {
            // Fetch connexion string from web.config file
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            // Create sql command needed for sql dependency
            string sqlCommand = @"SELECT [ContactId], [ContactName], [ContctNo] from  [dbo].[Contacts] WHERE [AddedOn] > @AddedOn";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if(con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                // Create an sql dependency object with the sql command we just have created for revieved notification from the sql server
                SqlDependency sqlDep = new SqlDependency(cmd);
                // To recieve notifications we will need to subscribe the change OnChange so that when the sql command produces a different result the OnChange event will be fired
                sqlDep.OnChange += sqlDep_OnChange;
                // Execute sql command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        private void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if(e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                // from here we will send notification message to clients
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");

                // re-register notification
                RegisterNotification(DateTime.Now);

            }
        }


        // This is the method to return the changes from the server
        public List<Contact> GetContacts(DateTime afterDate)
        {
            using(MyPushNotificationEntities dc = new MyPushNotificationEntities())
            {
                return dc.Contacts.Where(a => a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();
            }
        }
    }
}