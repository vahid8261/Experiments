﻿using System.Threading;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Serilog;

namespace Server.Api
{
    [RoutePrefix("tasks")]
    public class TaskController : ApiController
    {
        private IHubContext _context;

        // This can be defined a number of ways
        //
        private string _channel = Constants.TaskChannel;

        public TaskController()
        {
            // Normally we would inject this
            //
            _context = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
        }


        [Route("long")]
        [HttpGet]
        public IHttpActionResult GetLongTask()
        {
            Log.Information("Starting long task");

            double steps = 10;
            var eventName = "longTask.status";

            ExecuteTask(eventName, steps);

            return Ok("Long task complete");
        }



        [Route("short")]
        [HttpGet]
        public IHttpActionResult GetShortTask()
        {
            Log.Information("Starting short task");

            double steps = 5;
            var eventName = "shortTask.status";

            ExecuteTask(eventName, steps);

            return Ok("Short task complete");
        }

        private void ExecuteTask(string eventName, double steps)
        {
            var status = new Status
            {
                State = "starting",
                PercentComplete = 0.0
            };

            PublishEvent(eventName, status);

            for (double i = 0; i < steps; i++)
            {
                // Update the status and publish a new event
                //
                status.State = "working";
                status.PercentComplete = (i / steps) * 100;
                PublishEvent(eventName, status);

                Thread.Sleep(500);
            }

            status.State = "complete";
            status.PercentComplete = 100;
            PublishEvent(eventName, status);
        }

        private void PublishEvent(string eventName, Status status)
        {
        // From .NET code like this we can't invoke the methods that
        //  exist on our actual Hub class...because we only have a proxy
        //  to it. So to publish the event we need to call the method that
        //  the clients will be listening on.
        //
        _context.Clients.Group(_channel).OnEvent(Constants.TaskChannel, new ChannelEvent
        {
            ChannelName = Constants.TaskChannel,
            Name = eventName,
            Data = status
        });
        }
    }


    public class Status
    {
        public string State { get; set; }

        public  double PercentComplete { get; set; }
    }
}
