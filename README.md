# ASP.NET-SignalR-PushNotification

A push notification system with ASP.NET + SignalR for notifying connected clients / users when any database changes happen on the server.

### Prerequisites

* VisualStudio 2017
* Microsoft SQL Manager Studio



## Acknowledgments

* Sourav Mondal: [http://www.dotnetawesome.com/2016/05/push-notification-system-with-signalr.html]

### Steps

* Create New Project.
* Add a SQL Server Database.
* Create a table for store data.
* Enable Service Broker on the database.
* Add Entity Data Model.
* Install SignalR NuGet Package.
* Add an Owin startup file.
* Add a SignalR hub class.
* Add connection string into the web.config file.
* Add an another class file for register notification for data changes in the database.
* Create an MVC Controller.
* Add new action into your controller.
* Add view for your Action.
* Add an another action in your controller (here HomeController) for fetch contact data.
* Update _Layout.cshtml for showing notification.
* Update global.asax.cs for start, stop SQL dependency.
