﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SampleWebApi.Models;

namespace SampleWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ICollection<Device> _devices = new List<Device>
        {
            new Device { Airport = "SYD", Id = "1", Terminal = "1", Type = "Unit" },
            new Device { Airport = "SYD", Id = "2", Terminal = "1", Type = "Unit" },
            new Device { Airport = "SYD", Id = "3", Terminal = "2", Type = "Unit" },
        };

        // GET /device/1
        [HttpGet("{id}")]
        public ActionResult<Device> Get(string id)
        {
            var device = _devices.FirstOrDefault(d => d.Id == id);
            if (device == null)
                return NotFound();

            return device;
        }

        // GET /device/query?Airport=1
        // GET /device/query?Airport=1&Terminal=2
        // GET /device/query?Airport=1&Terminal=2&Type=Unit
        // ... any combination
        [HttpGet("query")]
        public ActionResult<Device[]> Query([FromQuery]Location location)
        {
            const int maxItems = 100;

            var q = _devices.AsQueryable();

            if (!string.IsNullOrEmpty(location.Id)) q = q.Where(d => d.Id == location.Id);
            if (!string.IsNullOrEmpty(location.Airport)) q = q.Where(d => d.Airport == location.Airport);
            if (!string.IsNullOrEmpty(location.Terminal)) q = q.Where(d => d.Terminal == location.Terminal);
            if (!string.IsNullOrEmpty(location.Type)) q = q.Where(d => d.Type == location.Type);

            return q.Take(maxItems).ToArray();
        }
    }
}
